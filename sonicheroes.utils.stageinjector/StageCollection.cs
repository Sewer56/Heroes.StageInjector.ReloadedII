using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Reloaded.Hooks.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;
using Reloaded.Universal.Redirector.Interfaces;
using SonicHeroes.Utils.StageInjector.Common.Shared.Enums;
using SonicHeroes.Utils.StageInjector.Common.Shared.Splines;
using SonicHeroes.Utils.StageInjector.Common.Structs.Positions.Substructures;
using SonicHeroes.Utils.StageInjector.Structures;

namespace SonicHeroes.Utils.StageInjector
{
    public unsafe class StageCollection
    {
        private Dictionary<string, IEnumerable<Stage>> _idToStages = new Dictionary<string, IEnumerable<Stage>>();
        private List<Stage> _allStages  = new List<Stage>(); // Ordered list of all stages.

        private WeakReference<IRedirectorController> _redirectorController;
        private IModLoader      _modLoader;
        private ILogger         _logger;
        private Hooks.Hooks     _hooks;

        private StageId*        _stageId    = (StageId*)    0x8D6710;
        private ModeSwitch*     _modeSwitch = (ModeSwitch*) 0x00A777E4;
        private TeamTop*        _teamTop    = (TeamTop*)    0x00A4C268;

        public StageCollection(IModLoader loader, IReloadedHooks hooks)
        {
            _modLoader = loader;
            _logger    = (ILogger) _modLoader.GetLogger();
            _redirectorController   = _modLoader.GetController<IRedirectorController>();
            _hooks = new Hooks.Hooks(hooks, InitSplineImpl, GetStartPositionImpl, GetEndPositionImpl, GetBragPositionImpl);

            // Populate Default Stages
            foreach (StageId stageId in (StageId[])Enum.GetValues(typeof(StageId)))
            {
                _allStages.Add(new DefaultStage(stageId));
            }
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
            var stageCollection = stages.Select(x => new CustomStage(x, _redirectorController));
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
            if (TryGetCurrentStage(out Stage stage))
            {
                if (stage.Splines != null)
                    return _hooks.InitPathHook.OriginalFunction(stage.Splines);
            }

            return _hooks.InitPathHook.OriginalFunction(splinePointerArray);
        }

        /// <summary>
        /// See <see cref="SonicHeroes.Utils.StageInjector.Hooks.Hooks.GetEndPosition"/>
        /// </summary>
        private PositionEnd* GetEndPositionImpl(Teams team)
        {
            if (TryGetCurrentStage(out Stage stage))
                return &stage.EndPositions[(int) team];

            return _hooks.GetEndPositionHook.OriginalFunction(team);
        }

        /// <summary>
        /// See <see cref="SonicHeroes.Utils.StageInjector.Hooks.Hooks.GetBragPosition"/>
        /// </summary>
        private PositionEnd* GetBragPositionImpl(Teams team)
        {
            if (TryGetCurrentStage(out Stage stage))
                return &stage.BragPositions[(int)team];

            return _hooks.GetBragPositionHook.OriginalFunction(team);
        }

        /// <summary>
        /// See <see cref="SonicHeroes.Utils.StageInjector.Hooks.Hooks.GetStartPosition"/>
        /// </summary>
        private PositionStart* GetStartPositionImpl(Teams team)
        {
            if (TryGetCurrentStage(out Stage stage))
                return GetStartPositionForStage(stage, team);

            return _hooks.GetStartPositionHook.OriginalFunction(team);
        }

        private PositionStart* GetStartPositionForStage(Stage stage, Teams team)
        {
            if (IsTwoPlayer())
            {
                // Bug: Setting the same team multiple times will always use start position of first player to use that team.
                // This bug is inherited from the game, because the game only uses the team parameter to determine
                // spawn location, assuming players are separate teams.
                // If one player uses a team used by an earlier player, they will get earlier player's spawn.

                // Support 4 players.
                int playerNumber = 0;
                for (; playerNumber <= 3; playerNumber++)
                {
                    if (_teamTop[playerNumber].CurrentTeam == team)
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
        /// Attempts to obtain a <see cref="Stage"/> object from this class' collection for the current stage.
        /// </summary>
        private bool TryGetCurrentStage(out Stage stage)
        {
            for (int x = _allStages.Count - 1; x >= 0; x--)
            {
                var currentStage = _allStages[x];
                if (currentStage.StageId == *_stageId)
                {
                    stage = currentStage;
                    return true;
                }
            }

            stage = null;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool IsTwoPlayer()
        {
            return _modeSwitch->Flags.twoPlayerMode;
        }
    }
}
