using System.Collections.Generic;

namespace LabApi.Extensions
{
    /// <summary>
    /// Provides structural extensions for high-performance data deconstruction.
    /// </summary>
    internal static class DictionaryExtensions
    {
        /// <summary>
        /// Deconstructs a key-value pair into separate architectural components (C# 9.0 Pattern Matcher support).
        /// </summary>
        public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> kvp, out TKey key, out TValue value)
        {
            key = kvp.Key;
            value = kvp.Value;
        }
    }
}