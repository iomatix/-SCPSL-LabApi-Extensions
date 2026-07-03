using UnityEngine;

namespace LabApi.Extensions
{
    /// <summary>
    /// Provides high-performance fluent mathematical extensions and boundary filters for primitive data types.
    /// </summary>
    internal static class MathExtensions
    {
        /// <summary>
        /// Fluently clamps a single-precision floating-point value within specified structural minimum and maximum bounds.
        /// </summary>
        public static float Clamp(this float value, float min, float max) => Mathf.Clamp(value, min, max);

        /// <summary>
        /// Fluently clamps a 32-bit signed integer value within specified structural minimum and maximum bounds.
        /// </summary>
        public static int Clamp(this int value, int min, int max) => Mathf.Clamp(value, min, max);

        /// <summary>
        /// Fluently clamps a double-precision floating-point value within specified structural minimum and maximum bounds.
        /// </summary>
        public static double Clamp(this double value, double min, double max) => value < min ? min : (value > max ? max : value);

        /// <summary>
        /// Enforces a strict minimum baseline constraint on a single-precision floating-point value.
        /// </summary>
        public static float LimitMin(this float value, float minBaseline) => Mathf.Max(minBaseline, value);

        /// <summary>
        /// Enforces a strict minimum baseline constraint on a 32-bit signed integer value.
        /// </summary>
        public static int LimitMin(this int value, int minBaseline) => Mathf.Max(minBaseline, value);

        /// <summary>
        /// Enforces a strict minimum baseline constraint on a double-precision floating-point value.
        /// </summary>
        public static double LimitMin(this double value, double minBaseline) => value < minBaseline ? minBaseline : value;


    }
}