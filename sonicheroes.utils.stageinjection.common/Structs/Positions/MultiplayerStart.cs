using SonicHeroes.Utils.StageInjector.Common.Shared.Enums;
using SonicHeroes.Utils.StageInjector.Common.Structs.Positions.Substructures;

namespace SonicHeroes.Utils.StageInjector.Common.Structs.Positions
{
    /// <summary>
    /// Describes a Multiplayer Start Position structure for an individual action stage.
    /// </summary>
    public struct MultiplayerStart
    {
        /// <summary>
        /// The stage the following start position is for.
        /// </summary>
        public StageId StageId;

        /// <summary>
        /// Player 1 starting position.
        /// </summary>
        public PositionStart Player1Start;

        /// <summary>
        /// Player 2 starting position.
        /// </summary>
        public PositionStart Player2Start;
    }
}
