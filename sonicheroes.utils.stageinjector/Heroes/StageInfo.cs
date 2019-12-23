using Heroes.SDK.Classes.PseudoNativeClasses;
using Heroes.SDK.Definitions.Enums;
using Heroes.SDK.Definitions.Structures.Stage.Spawn.Collections;
using Heroes.SDK.Utilities.Tagger;
using Heroes.SDK.Utilities.Tagger.Enums;

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
        public static StageInfo FromStageId(Stage stageId)
        {
            // Categorize the individual stage and decide if to take 1P or 2P path.
            var tags = Tagger.GetStageTags(stageId);
            var info = new StageInfo();

            // Check if 2 player to perform or 1 player.
            if (tags.HasFlag(StageTag.TwoPlayer))
            {
                for (int x = 0; x < StageFunctions.MultiplayerStart.Count; x++)
                {
                    if (StageFunctions.MultiplayerStart[x].StageId == stageId)
                    {
                        info.MultiplayerStartPositions = &StageFunctions.MultiplayerStart.Pointer[x];
                        break;
                    }
                }

                for (int x = 0; x < StageFunctions.MultiPlayerBrag.Count; x++)
                {
                    if (StageFunctions.MultiPlayerBrag[x].StageId == stageId)
                    {
                        info.MultiplayerBragPositions = &StageFunctions.MultiPlayerBrag.Pointer[x];
                        break;
                    }
                }
            }
            else
            {
                for (int x = 0; x < StageFunctions.SinglePlayerStart.Count; x++)
                {
                    if (StageFunctions.SinglePlayerStart[x].StageId == stageId)
                    {
                        info.StartPositions = &StageFunctions.SinglePlayerStart.Pointer[x];
                        break;
                    }
                }
            }

            for (int x = 0; x < StageFunctions.BothPlayerEnd.Count; x++)
            {
                if (StageFunctions.BothPlayerEnd[x].StageId == stageId)
                {
                    info.EndPositions = &StageFunctions.BothPlayerEnd.Pointer[x];
                    break;
                }
            }

            return info;
        }
    }
}
