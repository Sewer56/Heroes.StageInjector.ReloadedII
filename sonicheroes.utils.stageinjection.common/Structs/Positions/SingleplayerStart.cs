using System.Runtime.InteropServices;
using SonicHeroes.Utils.StageInjector.Common.Shared.Enums;
using SonicHeroes.Utils.StageInjector.Common.Structs.Positions.Substructures;

namespace SonicHeroes.Utils.StageInjector.Common.Structs.Positions
{
    /// <summary>
    /// Describes a Singleplayer Start Position structure for an individual action stage.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SingleplayerStart
    {
        /// <summary>
        /// The stage the following start position is for,
        /// </summary>
        public StageId StageId;

        /// <summary>
        /// Team Sonic's starting position.
        /// </summary>
        public PositionStart SonicStart;

        /// <summary>
        /// Team Dark's starting position.
        /// </summary>
        public PositionStart DarkStart;

        /// <summary>
        /// Team Rose starting position.
        /// </summary>
        public PositionStart RoseStart;

        /// <summary>
        /// Team Chaotix' starting position.
        /// </summary>
        public PositionStart ChaotixStart;

        /// <summary>
        /// Unused Team
        /// </summary>
        public PositionStart ForeditStart;
    }
}
