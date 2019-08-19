using SonicHeroes.Utils.StageInjector.Common;
using SonicHeroes.Utils.StageInjector.Common.Shared.Enums;
using SonicHeroes.Utils.StageInjector.Common.Structs.Positions;

namespace SonicHeroes.Utils.StageInjector.Heroes
{

    public unsafe struct StageInfo
    {
        public SingleplayerStart*   StartPositions;
        public SingleplayerEnd*     EndPositions;
        public MultiplayerStart*    MultiplayerStartPositions;
        public MultiplayerBrag*     MultiplayerBragPositions;

        /// <summary>
        /// Obtains the pointers for a specific stage's start/ending/bragging positions for the given stage.
        /// </summary>
        public static StageInfo FromStageId(StageId stageId)
        {
            // Categorize the individual stage and decide if to take 1P or 2P path.
            StageTags.StageTag stageTags = StageTags.CategorizeStage((int)stageId);
            StageInfo stageInfo = new StageInfo();

            // Check if 2 player to perform or 1 player.
            if (stageTags.HasFlag(StageTags.StageTag.TwoPlayer))
            {
                for (int x = 0; x < Pointers.MultiPlayerStartPointer.Count; x++)
                {
                    if (Pointers.MultiPlayerStartPointer[x].StageId == stageId)
                    {
                        stageInfo.MultiplayerStartPositions = (MultiplayerStart*) Pointers.MultiPlayerStartPointer.GetPointerToElement(x);
                        break;
                    }
                }

                for (int x = 0; x < Pointers.MultiPlayerBragPointer.Count; x++)
                {
                    if (Pointers.MultiPlayerBragPointer[x].StageId == stageId)
                    {
                        stageInfo.MultiplayerBragPositions = (MultiplayerBrag*) Pointers.MultiPlayerBragPointer.GetPointerToElement(x);
                        break;
                    }
                }
            }
            else
            {
                for (int x = 0; x < Pointers.SinglePlayerStartPointer.Count; x++)
                {
                    if (Pointers.SinglePlayerStartPointer[x].StageId == stageId)
                    {
                        stageInfo.StartPositions = (SingleplayerStart*) Pointers.SinglePlayerStartPointer.GetPointerToElement(x);
                        break;
                    }
                }
            }

            for (int x = 0; x < Pointers.BothPlayerEndPointer.Count; x++)
            {
                if (Pointers.BothPlayerEndPointer[x].StageId == stageId)
                {
                    stageInfo.EndPositions = (SingleplayerEnd*) Pointers.BothPlayerEndPointer.GetPointerToElement(x);
                    break;
                }
            }

            return stageInfo;
        }
    }
}
