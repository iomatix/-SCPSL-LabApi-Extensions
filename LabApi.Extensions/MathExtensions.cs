using System;
using UnityEngine;

namespace LabApi.Extensions
{
    /// <summary>
    /// Provides high-performance fluent primitives for mathematical validations and mutations across primitive numeric types.
    /// </summary>
    public static class MathExtensions
    {
        #region Clamping Primaries
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
        #endregion

        #region Boundary Limitations
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
        #endregion

        #region Anomalous Validations
        /// <summary>
        /// Verifies whether the specified single-precision floating-point value evaluates to NaN or Infinity.
        /// </summary>
        public static bool IsNanOrInfinity(this float value) => float.IsNaN(value) || float.IsInfinity(value);

        /// <summary>
        /// Verifies whether the specified double-precision floating-point value evaluates to NaN or Infinity.
        /// </summary>
        public static bool IsNanOrInfinity(this double value) => double.IsNaN(value) || double.IsInfinity(value);

        /// <summary>
        /// Baseline safety fallback for integer primitives. Always returns false as integral types cannot represent NaN or Infinity states.
        /// </summary>
        public static bool IsNanOrInfinity(this int value) => false;
        #endregion

        #region Integer Rounding Conversions
        /// <summary>
        /// Rounds a single-precision floating-point value to the nearest 32-bit signed integer using high-performance engine math routines.
        /// </summary>
        /// <param name="value">The source float value to convert.</param>
        /// <returns>The rounded 32-bit signed integer.</returns>
        public static int RoundToInt(this float value) => Mathf.RoundToInt(value);

        /// <summary>
        /// Rounds a double-precision floating-point value to the nearest 32-bit signed integer using standard mathematical truncation loops.
        /// </summary>
        /// <param name="value">The source double value to convert.</param>
        /// <returns>The rounded 32-bit signed integer.</returns>
        public static int RoundToInt(this double value) => (int)Math.Round(value);
        #endregion
    }
}