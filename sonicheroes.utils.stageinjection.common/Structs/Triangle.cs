namespace SonicHeroes.Utils.StageInjector.Common.Structs
{
    /// <summary>
    /// Represents a standard vertex in three dimentional space.
    /// </summary>
    public struct Triangle
    {
        public Vector Vertex1 { get; set; }
        public Vector Vertex2 { get; set; }
        public Vector Vertex3 { get; set; }

        public Triangle(Vector vertex1, Vector vertex2, Vector vertex3)
        {
            this.Vertex1 = vertex1;
            this.Vertex2 = vertex2;
            this.Vertex3 = vertex3;
        }
    }
}