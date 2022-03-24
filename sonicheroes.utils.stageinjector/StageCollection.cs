using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Heroes.SDK;
using Heroes.SDK.API;
using Reloaded.Mod.Interfaces;
using Reloaded.Universal.Redirector.Interfaces;
using Heroes.SDK.Definitions.Enums;
using Heroes.SDK.Definitions.Structures.Stage.Spawn;
using Heroes.SDK.Definitions.Structures.Stage.Splines;
using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.Definitions.Enums;
using Stage = Heroes.SDK.Definitions.Enums.Stage;
using static Heroes.SDK.Classes.PseudoNativeClasses.StageFunctions;

namespace SonicHeroes.Utils.StageInjector;

public unsafe class StageCollection
{
    private Dictionary<string, IEnumerable<StageBase>> _idToStages = new Dictionary<string, IEnumerable<StageBase>>();
    private List<StageBase> _allStages  = new List<StageBase>(); // Ordered list of all stages.

    private WeakReference<IRedirectorController> _redirectorController;
    private IModLoader      _modLoader;
    private ILogger         _logger;
    private IHook<InitPath> _initPathHook;
    private IHook<SearchGoalStageLocator> _getEndPositionHook;
    private IHook<SearchIntroStageLocator> _getBragPositionHook;
    private IHook<SearchStartStageLocator> _getStartPositionHook;

    /// <summary>
    /// Workaround to odd "off by one" error (in game code?) causing first byte of pointer to list of objects at A2CF70
    /// to be overwritten.
    /// </summary>
    private IAsmHook _setLimitCrashWorkaround;

    public StageCollection(IModLoader loader)
    {
        _modLoader = loader;
        _logger    = (ILogger) _modLoader.GetLogger();
        _redirectorController   = _modLoader.GetController<IRedirectorController>();
        _initPathHook = Fun_InitializeSplines.Hook(InitSplineImpl).Activate();
        _getEndPositionHook = Fun_GetEndPosition.Hook(GetEndPositionImpl).Activate();
        _getBragPositionHook = Fun_GetIntroPosition.Hook(GetBragPositionImpl).Activate();
        _getStartPositionHook = Fun_GetStartPosition.Hook(GetStartPositionImpl).Activate();
            
        // Populate Default Stages
        foreach (var stageId in (Stage[])Enum.GetValues(typeof(Stage)))
        {
            // Rail Canyon for Chaotix
            var stage = stageId != Stage.RailCanyonChaotix
                ? new DefaultStage(stageId)
                : new DefaultStage(Stage.RailCanyon);

            _allStages.Add(stage);
        }

        // Temporary crash workaround for SET objects beyond default limits.
        var crashWorkaround = new[]
        {
            "use32",
            "mov edi, dword [esp + 0x58]", // Take pointer to object list from function parameters.
            "mov [0xA2CF70], dword edi"
        };
        _setLimitCrashWorkaround = SDK.ReloadedHooks.CreateAsmHook(crashWorkaround, 0x43D1EA, AsmHookBehaviour.ExecuteFirst).Activate();
    }

    /// <summary>
    /// Adds a mod to the watch list.
    /// </summary>
    public void AddMod(string modId)
    {
        if (!CheckRedirectorController())
            return;

        var modDirectory    = _modLoader.GetDirectoryForModId(modId);
        var stagesDirectory = $"{modDirectory}\\Stages";
        var stages          = Directory.GetDirectories(stagesDirectory);
        var stageCollection = stages.Select(x => new CustomStage(x, _redirectorController)).ToArray();
        _idToStages[modId]  = stageCollection;
        _allStages.AddRange(stageCollection);
    }

    /// <summary>
    /// Removes a mod from the watch list.
    /// </summary>
    public void RemoveMod(string modId)
    {
        if (_idToStages.ContainsKey(modId))
        {
            var stages = _idToStages[modId];

            foreach (var stage in stages)
            {
                _allStages.Remove(stage);
                if (stage is IDisposable disposableStage)
                    disposableStage.Dispose();
            }
        }

        _idToStages.Remove(modId);
    }

    private bool CheckRedirectorController()
    {
        if (_redirectorController.TryGetTarget(out var controller))
            return true;
            
        _redirectorController = _modLoader.GetController<IRedirectorController>();
        if (_redirectorController.TryGetTarget(out var newController))
            return true;

        _logger.WriteLine("[StageInjector] Unable to obtain Redirector controller. This indicates the universal file redirector is unloaded or a bug. Stage loading will be aborted.", _logger.ColorRedLight);
        return false;
    }

    /// <summary>
    /// <see cref="SonicHeroes.Utils.StageInjector.Hooks.Hooks.InitPath"/>
    /// </summary>
    private bool InitSplineImpl(Spline** splinePointerArray)
    {
        if (TryGetCurrentStage(out StageBase stage))
        {
            if (stage.Splines != null)
                return _initPathHook.OriginalFunction(stage.Splines);
        }

        return _initPathHook.OriginalFunction(splinePointerArray);
    }

    /// <summary>
    /// See <see cref="SonicHeroes.Utils.StageInjector.Hooks.Hooks.GetEndPosition"/>
    /// </summary>
    private PositionEnd* GetEndPositionImpl(Team team)
    {
        if (TryGetCurrentStage(out StageBase stage))
            return &stage.EndPositions[(int) team];

        return _getEndPositionHook.OriginalFunction(team);
    }

    /// <summary>
    /// See <see cref="SonicHeroes.Utils.StageInjector.Hooks.Hooks.GetBragPosition"/>
    /// </summary>
    private PositionEnd* GetBragPositionImpl(Team team)
    {
        if (TryGetCurrentStage(out StageBase stage))
            return &stage.BragPositions[(int)team];

        return _getBragPositionHook.OriginalFunction(team);
    }

    /// <summary>
    /// See <see cref="SonicHeroes.Utils.StageInjector.Hooks.Hooks.GetStartPosition"/>
    /// </summary>
    private PositionStart* GetStartPositionImpl(Team team)
    {
        if (TryGetCurrentStage(out StageBase stage))
            return GetStartPositionForStage(stage, team);

        return _getStartPositionHook.OriginalFunction(team);
    }

    private PositionStart* GetStartPositionForStage(StageBase stage, Team team)
    {
        if (State.IsMultiplayerMode())
        {
            // Bug: Setting the same team multiple times will always use start position of first player to use that team.
            // This bug is inherited from the game, because the game only uses the team parameter to determine
            // spawn location, assuming players are separate teams.
            // If one player uses a team used by an earlier player, they will get earlier player's spawn.

            // Support 4 players.
            int playerNumber = 0;
            for (; playerNumber <= 3; playerNumber++)
            {
                if (Player.TeamTop[playerNumber].AsReference().Team == team)
                    break;
            }

            // If all numbers fail, use P1's spawn
            if (playerNumber == 4)
                playerNumber = 1;

            // Get spawn position.
            return &stage.StartPositions[playerNumber];
        }

        return &stage.StartPositions[(int) team];
    }

    /// <summary>
    /// Attempts to obtain a <see cref="StageBase"/> object from this class' collection for the current stage.
    /// </summary>
    private bool TryGetCurrentStage(out StageBase stage)
    {
        for (int x = _allStages.Count - 1; x >= 0; x--)
        {
            var currentStage = _allStages[x];
            if (currentStage.StageId == State.CurrentStage)
            {
                stage = currentStage;
                return true;
            }
        }

        stage = null;
        return false;
    }
}