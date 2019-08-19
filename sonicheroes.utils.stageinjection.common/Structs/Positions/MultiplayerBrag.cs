using SonicHeroes.Utils.StageInjector.Common.Shared.Enums;
using SonicHeroes.Utils.StageInjector.Common.Structs.Positions.Substructures;

namespace SonicHeroes.Utils.StageInjector.Common.Structs.Positions
{
    /// <summary>
    /// Describes a Multiplayer Bragging Position structure for an individual action stage.
    /// Bragging just describes the short comment a team makes to another at the start of a 2P stage.
    /// </summary>
    public struct MultiplayerBrag
    {
        public StageId StageId;
        public PositionEnd Sonic;
        public PositionEnd Dark;
        public PositionEnd Rose;
        public PositionEnd Chaotix;
    }
}
