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
        // Cache to avoid repeated reflection calls.
        private static readonly ConcurrentDictionary<Assembly, string[]> ResourceManifestCache
            = new ConcurrentDictionary<Assembly, string[]>();

        /// <summary>
        /// Finds an embedded resource inside the assembly.
        /// Looks for: primaryKey + extension, primaryKey with dots replaced by underscores, or any fallback token.
        /// </summary>
        /// <param name="assembly">Assembly that contains the embedded resources.</param>
        /// <param name="primaryKey">Main name to search for (without extension).</param>
        /// <param name="fileExtension">File extension including the dot (e.g. ".wav").</param>
        /// <param name="alternativeTokens">Optional fallback names to try if the primary key fails.</param>
        /// <returns>
        /// Full manifest resource path if found; otherwise <c>null</c>.
        /// </returns>
        public static string FindEmbeddedAsset(
            this Assembly assembly,
            string primaryKey,
            string fileExtension = ".wav",
            params string[] alternativeTokens)
        {
            if (assembly == null || string.IsNullOrWhiteSpace(primaryKey))
                return null;

            // Load or retrieve cached manifest names.
            if (!ResourceManifestCache.TryGetValue(assembly, out var resourceNames))
            {
                resourceNames = assembly.GetManifestResourceNames();
                ResourceManifestCache.TryAdd(assembly, resourceNames);
            }

            string sanitizedKey = primaryKey.Contains(".")
                ? primaryKey.Replace(".", "_")
                : primaryKey;

            string primaryMatch = $"{primaryKey}{fileExtension}";
            string underscoreMatch = $"{sanitizedKey}{fileExtension}";

            int resourceCount = resourceNames.Length;
            int fallbackCount = alternativeTokens?.Length ?? 0;

            for (int i = 0; i < resourceCount; i++)
            {
                string resource = resourceNames[i];
                if (resource == null)
                    continue;

                // Primary key match
                if (resource.EndsWith(primaryMatch, StringComparison.OrdinalIgnoreCase) ||
                    resource.EndsWith(underscoreMatch, StringComparison.OrdinalIgnoreCase))
                {
                    return resource;
                }

                // Fallback tokens
                for (int j = 0; j < fallbackCount; j++)
                {
                    string token = alternativeTokens[j];
                    if (!string.IsNullOrWhiteSpace(token) &&
                        resource.EndsWith($"{token}{fileExtension}", StringComparison.OrdinalIgnoreCase))
                    {
                        return resource;
                    }
                }
            }

            return null;
        }
    }
}