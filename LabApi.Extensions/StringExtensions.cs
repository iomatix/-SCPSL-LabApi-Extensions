using System;

namespace LabApi.Extensions
{
    /// <summary>
    /// Simple helpers for working with strings.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Returns the user ID in lowercase, or an empty string if null.
        /// </summary>
        public static string NormalizeUserId(this string userId)
            => userId?.ToLowerInvariant() ?? string.Empty;

        /// <summary>
        /// Returns the Levenshtein distance between two strings.
        /// </summary>
        public static int ComputeLevenshteinDistance(this string source, string target)
        {
            if (source == null || target == null)
                return int.MaxValue;

            if (source == target)
                return 0;

            int n = source.Length;
            int m = target.Length;

            if (n == 0) return m;
            if (m == 0) return n;

            // Rolling array: only two rows instead of full matrix
            int[] prev = new int[m + 1];
            int[] curr = new int[m + 1];

            for (int j = 0; j <= m; j++)
                prev[j] = j;

            for (int i = 1; i <= n; i++)
            {
                curr[0] = i;

                for (int j = 1; j <= m; j++)
                {
                    int cost = source[i - 1] == target[j - 1] ? 0 : 1;

                    curr[j] = Math.Min(
                        Math.Min(curr[j - 1] + 1, prev[j] + 1),
                        prev[j - 1] + cost);
                }

                // Swap rows
                var temp = prev;
                prev = curr;
                curr = temp;
            }

            return prev[m];
        }
    }
}
