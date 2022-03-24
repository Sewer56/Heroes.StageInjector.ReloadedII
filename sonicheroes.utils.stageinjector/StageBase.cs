using Heroes.SDK.Definitions.Enums;
using Heroes.SDK.Definitions.Structures.Stage.Spawn;
using Heroes.SDK.Definitions.Structures.Stage.Splines;

namespace SonicHeroes.Utils.StageInjector;

public abstract unsafe class StageBase
{
    public Stage          StageId           { get; protected set; }
    public PositionStart* StartPositions    { get; protected set; }
    public PositionEnd*   EndPositions      { get; protected set; }
    public PositionEnd*   BragPositions     { get; protected set; }
    public Spline**       Splines           { get; protected set; }


    /* Autoimplemented by R# */
    protected bool Equals(StageBase other)
    {
        return StageId == other.StageId &&
               StartPositions == other.StartPositions &&
               EndPositions == other.EndPositions &&
               BragPositions == other.BragPositions &&
               Splines == other.Splines;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((StageBase)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (int)StageId;
            hashCode = (hashCode * 397) ^ unchecked((int)(long)StartPositions);
            hashCode = (hashCode * 397) ^ unchecked((int)(long)EndPositions);
            hashCode = (hashCode * 397) ^ unchecked((int)(long)BragPositions);
            hashCode = (hashCode * 397) ^ unchecked((int)(long)Splines);
            return hashCode;
        }
    }
}