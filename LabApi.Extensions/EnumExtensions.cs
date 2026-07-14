using LabApi.Extensions.Misc;
using System;
using System.Collections.Generic;

namespace LabApi.Extensions
{
    /// <summary>
    /// Highly optimized extension methods for working with enums.
    /// Features compile-time zero-allocation paths and JIT-cached lowercase string representations.
    /// </summary>
    public static class EnumExtensions
    {
        // Thread-safe, JIT-compiled static metadata cache per enum type.
        private static class EnumCache<T> where T : struct, Enum
        {
            public static readonly T[] Values = (T[])Enum.GetValues(typeof(T));
            public static readonly int Length = Values.Length;

            // Stores pre-calculated lowercase string values to completely eliminate runtime string allocations.
            public static readonly Dictionary<T, string> LowercaseNames = new();

            static EnumCache()
            {
                int len = Values.Length;
                for (int i = 0; i < len; i++)
                {
                    T val = Values[i];
                    LowercaseNames[val] = val.ToString().ToLowerInvariant();
                }
            }
        }

        /// <summary>
        /// Returns the enum value as a lowercase string without heap allocations or boxing.
        /// Reuses JIT-cached string instances.
        /// </summary>
        public static string ToAudioKey<T>(this T value) where T : struct, Enum
        {
            // FIX: Zero-allocation and no boxing by using generic T constraint and Dictionary lookup.
            return EnumCache<T>.LowercaseNames.TryGetValue(value, out var name)
                ? name
                : value.ToString().ToLowerInvariant(); // Fallback for safety (should never be reached)
        }

        /// <summary>
        /// Parses the string into an enum value or returns the fallback.
        /// </summary>
        public static T ParseOrDefault<T>(this string value, T defaultValue = default, bool ignoreCase = true)
            where T : struct, Enum
        {
            if (string.IsNullOrWhiteSpace(value))
                return defaultValue;

            return Enum.TryParse<T>(value, ignoreCase, out var result)
                ? result
                : defaultValue;
        }

        /// <summary>
        /// Returns a random value from the enum with zero allocations.
        /// </summary>
        public static T GetRandomValue<T>() where T : struct, Enum
        {
            int length = EnumCache<T>.Length;
            if (length == 0)
                return default;

            int index = SafeRandom.Next(0, length);
            return EnumCache<T>.Values[index];
        }
    }
}