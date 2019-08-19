using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using SonicHeroes.Utils.StageInjector.Common.Shared;
using SonicHeroes.Utils.StageInjector.Common.Shared.Enums;
using SonicHeroes.Utils.StageInjector.Common.Structs;
using SonicHeroes.Utils.StageInjector.Common.Structs.Positions.Substructures;
using SonicHeroes.Utils.StageInjector.Heroes;

namespace SonicHeroes.Utils.StageInjector
{
    public unsafe class DefaultStage : Stage, IDisposable 
    {
        /* Default positions at 0,0,0 if null for any stage. */
        private static PinnedManagedObject<PositionStart[]> _defaultStartPositions = new PinnedManagedObject<PositionStart[]>(new[] { new PositionStart(), new PositionStart(), new PositionStart(), new PositionStart(), new PositionStart() } );
        private static PinnedManagedObject<PositionEnd[]>   _defaultEndPositions   = new PinnedManagedObject<PositionEnd[]>(new[] { new PositionEnd(), new PositionEnd(), new PositionEnd(), new PositionEnd(), new PositionEnd() });

        private PinnedManagedObject<PositionStart[]> _startPositionsSingleInMultiplayer;

        public DefaultStage(StageId stageId)
        {
            StageId = stageId;
            var stageInfo = StageInfo.FromStageId(stageId);

            if (stageInfo.MultiplayerStartPositions != null)
            {
                /* For Rose and Chaotix copy 1P and 2P spawns. */
                _startPositionsSingleInMultiplayer = new PinnedManagedObject<PositionStart[]>(new[]
                {
                    stageInfo.MultiplayerStartPositions->Player1Start,
                    stageInfo.MultiplayerStartPositions->Player2Start,
                    stageInfo.MultiplayerStartPositions->Player1Start,
                    stageInfo.MultiplayerStartPositions->Player2Start
                });
                StartPositions = (PositionStart*) _startPositionsSingleInMultiplayer.ObjectPtr;
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
                StartPositions = _defaultStartPositions.AsPointer<PositionStart>();

            if (EndPositions == null)
                EndPositions = _defaultEndPositions.AsPointer<PositionEnd>();

            if (BragPositions == null)
                BragPositions = _defaultEndPositions.AsPointer<PositionEnd>();
        }

        public void Dispose()
        {

        }
    }
}
