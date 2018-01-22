using System.Collections.Generic;
using FuzzyString;

namespace HopfieldNetworkOCR.Core.Logic
{
    static class FuzzyStringComparer
    {
        private static readonly List<FuzzyStringComparisonOptions> Options = new List<FuzzyStringComparisonOptions>
        {
            FuzzyStringComparisonOptions.UseHammingDistance,
            FuzzyStringComparisonOptions.UseOverlapCoefficient,
            FuzzyStringComparisonOptions.UseLongestCommonSubsequence,
            FuzzyStringComparisonOptions.UseLongestCommonSubstring,
            FuzzyStringComparisonOptions.UseRatcliffObershelpSimilarity
        };

        private static readonly FuzzyStringComparisonTolerance Tolerance = FuzzyStringComparisonTolerance.Normal;

        public static bool Equals(string vector1, string vector2)
        {
            return vector1.ApproximatelyEquals(vector2, Options, Tolerance);
        }
    }
}
