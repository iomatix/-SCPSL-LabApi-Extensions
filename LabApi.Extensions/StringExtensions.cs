using System;

namespace LabApi.Extensions
{
    /// <summary>
    /// Provides high-performance string manipulation and network identity normalization layers.
    /// </summary>
    internal static class StringExtensions
    {
        /// <summary>
        /// Enforces standard lowercase invariant formatting on a raw network identifier token,
        /// mitigating platform-specific auth casing anomalies and dictionary key mismatches.
        /// </summary>
        /// <param name="userId">The raw user identity string to be processed.</param>
        /// <returns>A sanitized, lowercase invariant string, or an empty string if the source is null.</returns>
        public static string NormalizeUserId(this string userId)
        {
            return userId != null ? userId.ToLowerInvariant() : string.Empty;
        }

        /// <summary>
        /// Computes the Levenshtein Distance between two string primitives with zero heap allocations.
        /// </summary>
        /// <param name="source">The source string.</param>
        /// <param name="target">The target string.</param>
        /// <returns>The Levenshtein Distance between the two strings. Used primarily for fuzzy string matching.</returns>
        public static int ComputeLevenshteinDistance(this string source, string target)
        {
            int n = source.Length;
            int m = target.Length;
            int[,] matrix = new int[n + 1, m + 1];

            if (n == 0) return m;
            if (m == 0) return n;

            for (int i = 0; i <= n; matrix[i, 0] = i++) { }
            for (int j = 0; j <= m; matrix[0, j] = j++) { }

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = (target[j - 1] == source[i - 1]) ? 0 : 1;
                    matrix[i, j] = Math.Min(
                        Math.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1),
                        matrix[i - 1, j - 1] + cost);
                }
            }
            return matrix[n, m];
        }
    }
}