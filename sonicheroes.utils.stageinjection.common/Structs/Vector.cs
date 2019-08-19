using System;
using System.Runtime.CompilerServices;

namespace SonicHeroes.Utils.StageInjector.Common.Structs
{
    /// <summary>
    /// Represents a standard vertex in three dimensional space.
    /// </summary>
    public struct Vector
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Vector(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>
        /// Obtains the distance between two vertices.
        /// </summary>
        /// <param name="vertex1">The first vertex.</param>
        /// <param name="vertex2">The second vertex.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetDistance(Vector vertex1, Vector vertex2)
        {
            Vector delta = vertex1 - vertex2;
            return (float)Math.Sqrt(Math.Pow(delta.X, 2) + Math.Pow(delta.Y, 2) + Math.Pow(delta.Z, 2));
        }

        /// <summary>
        /// Obtains the distance between two vertices.
        /// </summary>
        /// <param name="other">The other vertex.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float GetDistance(Vector other)
        {
            return GetDistance(this, other);
        }

        /* Operator Overrides */
        public static Vector operator +(Vector vector1, Vector vector2)
        {
            return new Vector
            {
                X = vector1.X + vector2.X,
                Y = vector1.Y + vector2.Y,
                Z = vector1.Z + vector2.Z
            };
        }

        public static Vector operator -(Vector vector1, Vector vector2)
        {
            return new Vector
            {
                X = vector1.X - vector2.X,
                Y = vector1.Y - vector2.Y,
                Z = vector1.Z - vector2.Z
            };
        }


    }
}