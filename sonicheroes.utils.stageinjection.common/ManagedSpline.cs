using SonicHeroes.Utils.StageInjector.Common.Structs.Enums;
using SonicHeroes.Utils.StageInjector.Common.Structs.Splines;
using SonicHeroes.Utils.StageInjector.Common.Utilities;

namespace SonicHeroes.Utils.StageInjector.Common
{
    /// <summary>
    /// Represents an individual spline.
    /// </summary>
    public class ManagedSpline : JsonSerializable<ManagedSpline>
    {
        public SplineType       SplineType  { get; set; }
        public SplineVertex[]   Vertices    { get; set; }

        public ManagedSpline() { }
        public ManagedSpline(SplineType splineType, SplineVertex[] vertices)
        {
            SplineType = splineType;
            Vertices = vertices;
        }
    }
}
