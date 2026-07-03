using System;

namespace LabApi.Extensions
{
    /// <summary>
    /// Provides enterprise-grade extension methods for <see cref="Enum"/> structures,
    /// enabling optimized string serialization, defensive layout parsing, and thread-isolated random state generation.
    /// </summary>
    internal static class EnumExtensions
    {
        /// <summary>
        /// Automatically transforms an execution enum field token into a standardized lowercase string key identifier.
        /// Eliminates redundant switch-case evaluation structures across independent sub-system audio pipeline configurations.
        /// </summary>
        /// <param name="value">The source enum metadata token target requested for serialization.</param>
        /// <returns>A normalized, culture-invariant lowercase string literal representation of the field identifier; otherwise, an empty string block.</returns>
        public static string ToAudioKey(this Enum value)
        {
            if (value == null) return string.Empty;
            return value.ToString().ToLowerInvariant();
        }

        /// <summary>
        /// Defensively converts a raw string literal target into its verified strongly-typed structural enum representation.
        /// Safely insulates execution pipelines against human typographical initialization anomalies located within deployment configuration matrices.
        /// </summary>
        /// <typeparam name="T">The concrete structural value constraint matching a standard system <see cref="Enum"/> layout topology.</typeparam>
        /// <param name="value">The raw input string character sequence extracted from the target server configuration profile.</param>
        /// <param name="defaultValue">The emergency structural fallback value returned if the initialization string data fails parsing validation.</param>
        /// <param name="ignoreCase">Indicates whether character capitalization matrix variances should be discarded during parsing comparison routines.</param>
        /// <returns>The fully synchronized enum state configuration matching the input identifier; otherwise, the specified fallback state context.</returns>
        public static T ParseOrDefault<T>(this string value, T defaultValue = default, bool ignoreCase = true) where T : struct, Enum
        {
            if (string.IsNullOrWhiteSpace(value)) return defaultValue;

            // Utilizing high-performance SDK-style parser engine avoiding allocation grid leaks
            return Enum.TryParse<T>(value, ignoreCase, out T result) ? result : defaultValue;
        }

        /// <summary>
        /// Aggregates enumeration boundaries and retrieves a structurally random field token from the targeted enum reflection array space.
        /// Seamlessly targets the thread-isolated, safe data abstraction framework to guarantee performance scaling.
        /// </summary>
        /// <typeparam name="T">The targeting assembly constraint layout matching a core active system enumeration signature.</typeparam>
        /// <returns>A randomly evaluated structural option field choice evaluated and fetched securely from the target array bounds.</returns>
        public static T GetRandomValue<T>() where T : struct, Enum
        {
            Array values = Enum.GetValues(typeof(T));

            // Querying structural data bounds via the thread-safe local isolation tracker pipeline
            int randomIndex = SafeRandom.Next(0, values.Length);
            return (T)values.GetValue(randomIndex);
        }
    }
}