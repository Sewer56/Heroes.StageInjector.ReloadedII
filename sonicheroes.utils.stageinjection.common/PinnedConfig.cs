using System;
using SonicHeroes.Utils.StageInjector.Common.Shared;
using SonicHeroes.Utils.StageInjector.Common.Shared.Enums;
using SonicHeroes.Utils.StageInjector.Common.Structs.Positions.Substructures;

namespace SonicHeroes.Utils.StageInjector.Common
{
    /// <summary>
    /// Pins an individual <see cref="Config"/> for access from native code.
    /// </summary>
    public class PinnedConfig : IDisposable
    {
        public StageId StageId { get; private set; }

        /// <summary>
        /// If in single player mode, contains the 5 teams.
        /// If in multiplayer mode, contains P1 and P2 entries.
        /// </summary>
        public PinnedManagedObject<PositionStart[]> StartPositions { get; private set; }

        /// <summary>
        /// Contains 5 entries for each of the teams. (4 + 1 unused)
        /// </summary>
        public PinnedManagedObject<PositionEnd[]>   EndPositions { get; private set; }

        /// <summary>
        /// Contains 4 entries for each of the used teams.
        /// </summary>
        public PinnedManagedObject<PositionEnd[]>   BragPositions { get; private set; }

        public PinnedConfig(Config config)
        {
            StartPositions = new PinnedManagedObject<PositionStart[]>(config.StartPositions);
            EndPositions   = new PinnedManagedObject<PositionEnd[]>  (config.EndPositions);
            BragPositions  = new PinnedManagedObject<PositionEnd[]>  (config.BragPositions);
            StageId = config.StageId;
        }

        public void Dispose()
        {
            StartPositions?.Dispose();
            EndPositions?.Dispose();
            BragPositions?.Dispose();
        }
    }
}
