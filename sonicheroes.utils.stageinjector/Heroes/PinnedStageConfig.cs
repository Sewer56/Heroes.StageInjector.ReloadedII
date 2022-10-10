using Heroes.SDK.Definitions.Enums;
using Heroes.SDK.Definitions.Structures.Stage.Spawn;
using Heroes.SDK.Parsers.Custom;
using Reloaded.Memory.Interop;

namespace SonicHeroes.Utils.StageInjector.Heroes;

/// <summary>
/// Pins an individual <see cref="StageConfig"/> for access from native code.
/// </summary>
public class PinnedStageConfig : IDisposable
{
    public Stage StageId { get; private set; }

    /// <summary>
    /// If in single player mode, contains the 5 teams.
    /// If in multiplayer mode, contains P1 and P2 entries.
    /// </summary>
    public Pinnable<PositionStart> StartPositions { get; private set; }

    /// <summary>
    /// Contains 5 entries for each of the teams. (4 + 1 unused)
    /// </summary>
    public Pinnable<PositionEnd> EndPositions { get; private set; }

    /// <summary>
    /// Contains 4 entries for each of the used teams.
    /// </summary>
    public Pinnable<PositionEnd> BragPositions { get; private set; }

    public PinnedStageConfig(StageConfig config)
    {
        StartPositions = new Pinnable<PositionStart>(config.StartPositions);
        EndPositions = new Pinnable<PositionEnd>(config.EndPositions);
        BragPositions = new Pinnable<PositionEnd>(config.BragPositions);
        StageId = config.StageId;
    }

    public void Dispose()
    {
        StartPositions?.Dispose();
        EndPositions?.Dispose();
        BragPositions?.Dispose();
    }
}