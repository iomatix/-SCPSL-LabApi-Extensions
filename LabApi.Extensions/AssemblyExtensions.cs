using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace LabApi.Extensions
{
    /// <summary>
    /// Reflection and manifest tracking utilities for managing embedded assembly assets fluently.
    /// </summary>
    public static class AssemblyExtensions
    {
        // Thread-safe repository map isolating assembly references from iterative reflection lookups
        private static readonly ConcurrentDictionary<Assembly, string[]> ResourceManifestCache = new ConcurrentDictionary<Assembly, string[]>();

        /// <summary>
        /// Dynamically resolves an embedded manifest resource pathway matching primary identifiers or structural fallback names.
        /// </summary>
        /// <param name="assembly">The targeting assembly domain containing the embedded assets.</param>
        /// <param name="primaryKey">The core identifier string tracking the asset.</param>
        /// <param name="fileExtension">The target file extension including the leading dot (e.g., '.wav'). Defaults to '.wav'.</param>
        /// <param name="alternativeTokens">Optional fallback identifiers or enum-based naming keys to validate against.</param>
        /// <returns>The fully qualified manifest resource path if located cleanly; otherwise, null.</returns>
        public static string FindEmbeddedAsset(this Assembly assembly, string primaryKey, string fileExtension = ".wav", params string[] alternativeTokens)
        {
            if (assembly == null || string.IsNullOrWhiteSpace(primaryKey))
                return null;

            // Performance Optimization: Cache extraction layout prevents repeated reflection array generation loops
            if (!ResourceManifestCache.TryGetValue(assembly, out string[] resourceNames))
            {
                resourceNames = assembly.GetManifestResourceNames();
                ResourceManifestCache.TryAdd(assembly, resourceNames);
            }

            // Performance Optimization: Bypass allocation sweep if dot mapping notation isn't detected
            string sanitizedUnderscoreKey = primaryKey.Contains(".") ? primaryKey.Replace(".", "_") : primaryKey;

            int resourceCount = resourceNames.Length;
            int alternativeCount = alternativeTokens?.Length ?? 0;

            string primaryTargetMatch = $"{primaryKey}{fileExtension}";
            string underscoreTargetMatch = $"{sanitizedUnderscoreKey}{fileExtension}";

            // High-Performance Pipeline: Replaced allocation-heavy LINQ (FirstOrDefault/Any) with zero-alloc indexed loops
            for (int i = 0; i < resourceCount; i++)
            {
                string resource = resourceNames[i];
                if (resource == null) continue;

                if (resource.EndsWith(primaryTargetMatch, StringComparison.OrdinalIgnoreCase) ||
                    resource.EndsWith(underscoreTargetMatch, StringComparison.OrdinalIgnoreCase))
                {
                    return resource;
                }

                for (int j = 0; j < alternativeCount; j++)
                {
                    string token = alternativeTokens[j];
                    if (!string.IsNullOrWhiteSpace(token) && resource.EndsWith($"{token}{fileExtension}", StringComparison.OrdinalIgnoreCase))
                    {
                        return resource;
                    }
                }
            }

            return null;
        }
    }
}