using System;
using System.Linq;
using System.Runtime.InteropServices;
using Reloaded.Memory;
using Reloaded.Memory.Sources;
using SonicHeroes.Utils.StageInjector.Common.Structs;
using SonicHeroes.Utils.StageInjector.Common.Structs.Enums;
using SonicHeroes.Utils.StageInjector.Common.Structs.Splines;

namespace SonicHeroes.Utils.StageInjector.Common.Shared.Splines
{
    /// <summary>
    /// Struct that defines a spline header.
    /// </summary>
    public unsafe struct Spline : IDisposable
    {
        /// <summary>
        /// Always 1
        /// </summary>
        public ushort Enabler;
        
        /// <summary>
        /// Amount of vertices in spline
        /// </summary>
        public ushort NumberOfVertices;
        
        /// <summary>
        /// Purpose Unknown, Set 1000F
        /// </summary>
        public float TotalSplineLength;

        /// <summary>
        /// Points to the vertex list for the current individual spline.
        /// </summary>
        public SplineVertex* VertexList;

        /// <summary>
        /// Cast Spline_Type
        /// </summary>
        public SplineType SplineType;

        /// <summary>
        /// Creates a <see cref="Spline"/> given a deserialized spline file.
        /// </summary>
        public Spline(ManagedSpline managedSpline)
        {
            Enabler = 1;
            TotalSplineLength = 0;
            NumberOfVertices = 0;
            VertexList = (SplineVertex*) 0;
            SplineType = SplineType.Loop;

            FromSplineJson(managedSpline);
        }

        private void FromSplineJson(ManagedSpline splineFile)
        {
            SplineType       = splineFile.SplineType;
            NumberOfVertices = (ushort)splineFile.Vertices.Length;

            foreach (var vertex in splineFile.Vertices)
                TotalSplineLength += vertex.DistanceToNextVertex;
            
            CopyVertices(splineFile);
        }

        public void Dispose()
        {
            var memory = Memory.Instance;
            memory.Free((IntPtr)VertexList);
        }

        /* Construction Helpers */

        private void CopyVertices(ManagedSpline splineFile)
        {
            var memory      = Memory.Instance;
            var vertices    = splineFile.Vertices;

            int structSize  = StructArray.GetSize<SplineVertex>(vertices.Length);
            VertexList      = (SplineVertex*)memory.Allocate(structSize);
            StructArray.ToPtr((IntPtr)VertexList, vertices);
        }
    }
}
