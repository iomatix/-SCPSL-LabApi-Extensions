using LabApi.Extensions.Misc;
using System;

namespace LabApi.Extensions
{
    /// <summary>
    /// Simple helpers for working with enums.
    /// </summary>
    public static class EnumExtensions
    {
        private static class EnumCache<T> where T : struct, Enum
        {
            public static readonly T[] Values = (T[])Enum.GetValues(typeof(T));
            public static readonly int Length = Values.Length;
        }

        /// <summary>
        /// Returns the enum value as a lowercase string.
        /// </summary>
        public static string ToAudioKey(this Enum value)
            => value?.ToString().ToLowerInvariant() ?? string.Empty;

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
        /// Returns a random value from the enum.
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