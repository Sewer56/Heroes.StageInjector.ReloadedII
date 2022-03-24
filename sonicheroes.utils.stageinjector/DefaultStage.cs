using Heroes.SDK.Definitions.Enums;
using Heroes.SDK.Definitions.Structures.Stage.Spawn;
using Reloaded.Memory.Interop;
using SonicHeroes.Utils.StageInjector.Heroes;

namespace SonicHeroes.Utils.StageInjector;

public unsafe class DefaultStage : StageBase 
{
    /* Default positions at 0,0,0 if null for any stage. */
    private static Pinnable<PositionStart> _defaultStartPositions = new(new[] { new PositionStart(), new PositionStart(), new PositionStart(), new PositionStart(), new PositionStart() } );
    private static Pinnable<PositionEnd>   _defaultEndPositions   = new(new[] { new PositionEnd(), new PositionEnd(), new PositionEnd(), new PositionEnd(), new PositionEnd() });
    private Pinnable<PositionStart> _startPositionsSingleInMultiplayer;

    public DefaultStage(Stage stageId)
    {
        StageId = stageId;
        var stageInfo = StageInfo.FromStageId(stageId);

        if (stageInfo.MultiplayerStartPositions != null)
        {
            // For Rose and Chaotix copy 1P and 2P spawns in multiplayer.
            _startPositionsSingleInMultiplayer = new Pinnable<PositionStart>(new[]
            {
                stageInfo.MultiplayerStartPositions->Player1Start,
                stageInfo.MultiplayerStartPositions->Player2Start,
                stageInfo.MultiplayerStartPositions->Player1Start,
                stageInfo.MultiplayerStartPositions->Player2Start
            });

            StartPositions = _startPositionsSingleInMultiplayer.Pointer;
        }
        else
        {
            if (stageInfo.StartPositions != null)
            {
                StartPositions = &stageInfo.StartPositions->SonicStart;
            }
        }

        if (stageInfo.MultiplayerBragPositions != null)
            BragPositions = &stageInfo.EndPositions->SonicEnd;

        if (stageInfo.EndPositions != null)
            EndPositions = &stageInfo.EndPositions->SonicEnd;

        /* Replace any nulls with defaults. */
        if (StartPositions == null)
            StartPositions = _defaultStartPositions.Pointer;

        if (EndPositions == null)
            EndPositions = _defaultEndPositions.Pointer;

        if (BragPositions == null)
            BragPositions = _defaultEndPositions.Pointer;
    }
}