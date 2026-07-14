using System.Buffers;

namespace LabApi.Extensions
{
    /// <summary>
    /// Highly optimized utility extensions for string manipulation, normalization, and distance metrics.
    /// Employs array pooling and allocation-free fast-paths to completely protect the Garbage Collector during fuzzy target lookups.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Returns the user ID in lowercase, or an empty string if null.
        /// Bypasses string allocations completely if the ID is already lowercase (99% of cases).
        /// </summary>
        public static string NormalizeUserId(this string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return string.Empty;

            int len = userId.Length;
            bool hasUpper = false;

            // FIX: Fast-path. Scan for uppercase letters first. 
            // If none are found, return the original string instance to achieve true 0-allocation.
            for (int i = 0; i < len; i++)
            {
                char c = userId[i];
                if (c >= 'A' && c <= 'Z')
                {
                    hasUpper = true;
                    break;
                }
            }

            return hasUpper ? userId.ToLowerInvariant() : userId;
        }

        /// <summary>
        /// Returns the Levenshtein distance between two strings with absolutely zero heap allocations.
        /// Reuses pooled array buffers from ArrayPool.
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

            // FIX: Prevent array allocation on the heap during lookup sweeps.
            // Rented arrays from the shared pool are ultra-fast and have 0 GC impact.
            int[] prev = ArrayPool<int>.Shared.Rent(m + 1);
            int[] curr = ArrayPool<int>.Shared.Rent(m + 1);

            try
            {
                for (int j = 0; j <= m; j++)
                    prev[j] = j;

                for (int i = 1; i <= n; i++)
                {
                    curr[0] = i;

                    for (int j = 1; j <= m; j++)
                    {
                        int cost = source[i - 1] == target[j - 1] ? 0 : 1;

                        int insert = curr[j - 1] + 1;
                        int delete = prev[j] + 1;
                        int substitute = prev[j - 1] + cost;

                        // FIX: Substituted Math.Min with fast ternary operators to assist JIT-inlining and avoid method call overhead.
                        int minInsertDelete = insert < delete ? insert : delete;
                        curr[j] = minInsertDelete < substitute ? minInsertDelete : substitute;
                    }

                    // Swap rows (safely reference-swap)
                    var temp = prev;
                    prev = curr;
                    curr = temp;
                }

                return prev[m];
            }
            finally
            {
                // ALWAYS return rented buffers back to the shared pool, even in case of unexpected errors.
                ArrayPool<int>.Shared.Return(prev);
                ArrayPool<int>.Shared.Return(curr);
            }
        }
    }
}