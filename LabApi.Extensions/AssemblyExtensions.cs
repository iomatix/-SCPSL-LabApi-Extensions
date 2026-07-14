using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace LabApi.Extensions
{
    /// <summary>
    /// Helpers for working with embedded resources inside assemblies.
    /// </summary>
    public static class AssemblyExtensions
    {
        // Thread-safe cache to avoid redundant and expensive reflection calls.
        private static readonly ConcurrentDictionary<Assembly, string[]> ResourceManifestCache = new();

        /// <summary>
        /// Finds an embedded resource inside the assembly.
        /// Looks for: primaryKey + extension, primaryKey with dots replaced by underscores, or any fallback token.
        /// </summary>
        /// <param name="assembly">Assembly that contains the embedded resources.</param>
        /// <param name="primaryKey">Main name to search for (without extension).</param>
        /// <param name="fileExtension">File extension (e.g. ".wav" or "wav").</param>
        /// <param name="alternativeTokens">Optional fallback names to try if the primary key fails.</param>
        /// <returns>
        /// Full manifest resource path if found; otherwise, <c>null</c>.
        /// </returns>
        public static string FindEmbeddedAsset(
            this Assembly assembly,
            string primaryKey,
            string fileExtension = ".wav",
            params string[] alternativeTokens)
        {
            if (assembly == null || string.IsNullOrWhiteSpace(primaryKey))
                return null;

            // Load or retrieve cached manifest names atomically.
            string[] resourceNames = ResourceManifestCache.GetOrAdd(
                assembly,
                static ass => ass.GetManifestResourceNames()
            );

            // Normalize the extension to handle both ".ext" and "ext" gracefully.
            string normalizedExtension = fileExtension.StartsWith(".")
                ? fileExtension
                : $".{fileExtension}";

            // Prepare primary search patterns BEFORE entering the resource loop.
            // This prevents redundant allocations on the heap.
            string sanitizedKey = primaryKey.Contains(".")
                ? primaryKey.Replace(".", "_")
                : primaryKey;

            string primaryMatch = $"{primaryKey}{normalizedExtension}";
            string underscoreMatch = $"{sanitizedKey}{normalizedExtension}";

            // Pre-allocate and pre-format fallback search strings to achieve O(1) allocation path in the search loop.
            int fallbackCount = alternativeTokens?.Length ?? 0;
            string[] fallbackMatches = null;

            if (fallbackCount > 0)
            {
                fallbackMatches = new string[fallbackCount];
                int validFallbackCount = 0;

                for (int i = 0; i < fallbackCount; i++)
                {
                    string token = alternativeTokens[i];
                    if (!string.IsNullOrWhiteSpace(token))
                    {
                        fallbackMatches[validFallbackCount++] = $"{token}{normalizedExtension}";
                    }
                }

                fallbackCount = validFallbackCount;
            }

            int resourceCount = resourceNames.Length;
            for (int i = 0; i < resourceCount; i++)
            {
                string resource = resourceNames[i];
                if (resource == null)
                    continue;

                // 1. Check primary matches
                if (resource.EndsWith(primaryMatch, StringComparison.OrdinalIgnoreCase) ||
                    resource.EndsWith(underscoreMatch, StringComparison.OrdinalIgnoreCase))
                {
                    return resource;
                }

                // 2. Check fallback matches (zero runtime allocation inside this nested block)
                for (int j = 0; j < fallbackCount; j++)
                {
                    if (resource.EndsWith(fallbackMatches[j], StringComparison.OrdinalIgnoreCase))
                    {
                        return resource;
                    }
                }
            }

            return null;
        }
    }
}