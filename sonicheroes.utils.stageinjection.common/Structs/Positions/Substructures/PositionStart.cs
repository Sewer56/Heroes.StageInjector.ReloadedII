using System.Runtime.InteropServices;
using SonicHeroes.Utils.StageInjector.Common.Structs.Enums;

namespace SonicHeroes.Utils.StageInjector.Common.Structs.Positions.Substructures
{
    /// <summary>
    /// Describes the position and how the player starts off the stage.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PositionStart
    {
        /// <summary>
        /// The starting position of the player.
        /// </summary>
        public Vector Position { get; set; }

        /// <summary>
        /// The vertical direction of the position expressed in BAMS.
        /// </summary>
        public int Pitch { get; set; }

        public int UnknownUnused { get; set; }

        /// <summary>
        /// Describes how the player starts off the stage.
        /// </summary>
        public StartPositionMode Mode { get; set; }

        /// <summary>
        /// Time spent running in Running mode or without controller control, in frames.
        /// </summary>
        public int HoldTime { get; set; }
    }
}
