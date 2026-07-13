using System;
using UnityEngine;

namespace LabApi.Extensions
{
    /// <summary>
    /// Provides extension methods for common mathematical operations and primitive numeric types.
    /// </summary>
    public static class MathExtensions
    {
        #region Clamping
        /// <summary>
        /// Clamps a float value between a minimum and maximum range.
        /// </summary>
        public static float Clamp(this float value, float min, float max) => Mathf.Clamp(value, min, max);

        /// <summary>
        /// Clamps an int value between a minimum and maximum range.
        /// </summary>
        public static int Clamp(this int value, int min, int max) => Mathf.Clamp(value, min, max);

        /// <summary>
        /// Clamps a double value between a minimum and maximum range.
        /// </summary>
        public static double Clamp(this double value, double min, double max)
            => value < min ? min : (value > max ? max : value);
        #endregion

        #region Boundary Limitations
        /// <summary>
        /// Ensures a float value does not fall below the specified minimum baseline.
        /// </summary>
        public static float LimitMin(this float value, float minBaseline) => Mathf.Max(minBaseline, value);

        /// <summary>
        /// Ensures an int value does not fall below the specified minimum baseline.
        /// </summary>
        public static int LimitMin(this int value, int minBaseline) => Mathf.Max(minBaseline, value);

        /// <summary>
        /// Ensures a double value does not fall below the specified minimum baseline.
        /// </summary>
        public static double LimitMin(this double value, double minBaseline) => value < minBaseline ? minBaseline : value;

        /// <summary>
        /// Ensures a float value does not exceed the specified maximum baseline.
        /// </summary>
        public static float LimitMax(this float value, float maxBaseline) => Mathf.Min(maxBaseline, value);

        /// <summary>
        /// Ensures an int value does not exceed the specified maximum baseline.
        /// </summary>
        public static int LimitMax(this int value, int maxBaseline) => Mathf.Min(maxBaseline, value);

        /// <summary>
        /// Ensures a double value does not exceed the specified maximum baseline.
        /// </summary>
        public static double LimitMax(this double value, double maxBaseline) => value > maxBaseline ? maxBaseline : value;
        #endregion

        #region Absolute & Sign Mechanics
        /// <summary>
        /// Returns the absolute value of an int.
        /// </summary>
        public static int Abs(this int value) => Math.Abs(value);

        /// <summary>
        /// Returns the absolute value of a float.
        /// </summary>
        public static float Abs(this float value) => Mathf.Abs(value);

        /// <summary>
        /// Returns the absolute value of a double.
        /// </summary>
        public static double Abs(this double value) => Math.Abs(value);

        /// <summary>
        /// Returns the sign of an int value (-1, 0, or 1).
        /// </summary>
        public static int Sign(this int value) => value == 0 ? 0 : (value > 0 ? 1 : -1);

        /// <summary>
        /// Returns the sign of a float value (-1.0, 0.0, or 1.0).
        /// </summary>
        public static float Sign(this float value) => value == 0f ? 0f : (value > 0f ? 1f : -1f);

        /// <summary>
        /// Returns the sign of a double value (-1.0, 0.0, or 1.0).
        /// </summary>
        public static double Sign(this double value) => value == 0.0 ? 0.0 : (value > 0.0 ? 1.0 : -1.0);
        #endregion

        #region Linear Interpolation (Lerp)
        /// <summary>
        /// Linearly interpolates between two float values based on the specified alpha.
        /// </summary>
        public static float Lerp(this float from, float to, float t) => Mathf.Lerp(from, to, t);

        /// <summary>
        /// Linearly interpolates between two float values, allowing out-of-bounds alpha values.
        /// </summary>
        public static float LerpUnclamped(this float from, float to, float t) => Mathf.LerpUnclamped(from, to, t);
        #endregion

        #region Step Quantization & Truncation
        /// <summary>
        /// Returns the largest integer smaller than or equal to the float value.
        /// </summary>
        public static float Floor(this float value) => Mathf.Floor(value);

        /// <summary>
        /// Returns the largest integer smaller than or equal to the double value.
        /// </summary>
        public static double Floor(this double value) => Math.Floor(value);

        /// <summary>
        /// Returns the smallest integer greater than or equal to the float value.
        /// </summary>
        public static float Ceil(this float value) => Mathf.Ceil(value);

        /// <summary>
        /// Returns the smallest integer greater than or equal to the double value.
        /// </summary>
        public static double Ceil(this double value) => Math.Ceiling(value);
        #endregion

        #region Logarithmic & Acoustic Decibel Transforms
        /// <summary>
        /// Converts a decibel amplitude value (-96dB to 0dB) to a linear coefficient scale (0.0 to 1.0).
        /// </summary>
        public static float DbToLinear(this float db) => db <= -96f ? 0f : Mathf.Pow(10f, db / 20f);

        /// <summary>
        /// Converts a linear coefficient scalar (0.0 to 1.0) to a logarithmic decibel scale.
        /// </summary>
        public static float LinearToDb(this float linear) => linear <= 0.00001f ? -96f : 20f * Mathf.Log10(linear);
        #endregion

        #region Validations
        /// <summary>
        /// Checks if a float value is NaN or Infinity.
        /// </summary>
        public static bool IsNanOrInfinity(this float value) => float.IsNaN(value) || float.IsInfinity(value);

        /// <summary>
        /// Checks if a double value is NaN or Infinity.
        /// </summary>
        public static bool IsNanOrInfinity(this double value) => double.IsNaN(value) || double.IsInfinity(value);

        /// <summary>
        /// Always returns false, as integral types cannot represent NaN or Infinity.
        /// </summary>
        public static bool IsNanOrInfinity(this int value) => false;
        #endregion

        #region Integer Rounding Conversions
        /// <summary>
        /// Rounds a float value to the nearest integer.
        /// </summary>
        public static int RoundToInt(this float value) => Mathf.RoundToInt(value);

        /// <summary>
        /// Rounds a double value to the nearest integer.
        /// </summary>
        public static int RoundToInt(this double value) => (int)Math.Round(value);
        #endregion
    }
}