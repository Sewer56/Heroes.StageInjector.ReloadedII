using Reloaded.Memory.Pointers;
using SonicHeroes.Utils.StageInjector.Common.Structs.Positions;

namespace SonicHeroes.Utils.StageInjector.Heroes
{
    /// <summary>
    /// Only to be used inside an injected DLL.
    /// Contains the pointers which point to the start of individual sections.
    /// </summary>
    public static unsafe class Pointers
    {
        private const int SinglePlayerStartEntryLength = 27;
        private const int BothPlayerEndEntryLength = 60;
        private const int MultiPlayerStartEntryLength = 23;
        private const int MultiPlayerBragEntryLength = 21;

        /// <summary>
        /// Pointer to the current stage's singleplayer start positions for each team.
        /// </summary>
        public static FixedArrayPtr<SingleplayerStart> SinglePlayerStartPointer = new FixedArrayPtr<SingleplayerStart>(0x7C2FC8, SinglePlayerStartEntryLength);

        /// <summary>
        /// Pointer to the current stage's end positions for each team.
        /// </summary>
        public static FixedArrayPtr<SingleplayerEnd>  BothPlayerEndPointer = new FixedArrayPtr<SingleplayerEnd>(0x7C45B8, BothPlayerEndEntryLength);

        /// <summary>
        /// Pointer to the current stage's multiplayer start positions for each team.
        /// </summary>
        public static FixedArrayPtr<MultiplayerStart> MultiPlayerStartPointer = new FixedArrayPtr<MultiplayerStart>(0x7C5E18, MultiPlayerStartEntryLength);

        /// <summary>
        /// Pointer to the current stage's multiplayer end positions for each team.
        /// </summary>
        public static FixedArrayPtr<MultiplayerBrag>  MultiPlayerBragPointer = new FixedArrayPtr<MultiplayerBrag>(0x7C6380, MultiPlayerBragEntryLength);
    }
}
