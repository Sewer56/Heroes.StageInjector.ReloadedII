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
        /// BAMS 0 - 65535. Pitch is clockwise, i.e. rotating 90 degrees causes the characters' legs to point AWAY from the camera.
        /// </summary>
        public ushort Pitch   { get; set; }
        public ushort Unknown { get; set; }

        public int Null { get; set; }
    }
}