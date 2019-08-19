namespace SonicHeroes.Utils.StageInjector.Common.Structs.Positions.Substructures
{
    /// <summary>
    /// Represents a coordinate whereby an individual team performs their ending poses after finishing a stage.
    /// </summary>
    public struct PositionEnd
    {
        /// <summary>
        /// Position where the animation is performed.
        /// </summary>
        public Vector Position { get; set; }

        /// <summary>
        /// The vertical angle of their poses expressed in BAMS.
        /// </summary>
        public int Pitch { get; set; }

        public int Null { get; set; }
    }
}