using System;

namespace GildedRose.Console.Helpers
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    internal sealed class QualityConstraintAttribute : Attribute
    {
        // Defaults mapped to the project's constants.
        public int Min { get; set; } = QualityConstants.MinQuality;
        public int Max { get; set; } = QualityConstants.MaxQuality;

        // If Exact != int.MinValue, the quality must equal Exact.
        public int Exact { get; set; } = int.MinValue;
    }
}