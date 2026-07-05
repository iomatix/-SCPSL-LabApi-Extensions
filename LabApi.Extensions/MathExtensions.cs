using System;
using UnityEngine;

namespace LabApi.Extensions
{
    /// <summary>
    /// Provides high-performance fluent primitives for mathematical validations and mutations across primitive numeric types.
    /// Optimized for allocation-free real-time DSP pipelines and hot-path execution loop transformations.
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

        #region Absolute & Sign Mechanics
        /// <summary>
        /// Fluently resolves the absolute magnitude of a single-precision floating-point sample coefficient.
        /// </summary>
        public static float Abs(this float value) => Mathf.Abs(value);

        /// <summary>
        /// Fluently resolves the absolute magnitude of a double-precision floating-point coefficient.
        /// </summary>
        public static double Abs(this double value) => Math.Abs(value);

        /// <summary>
        /// Returns an integer indicating the sign (+1, 0, or -1) of the single-precision floating-point value.
        /// </summary>
        public static float Sign(this float value) => value is 0f ? 0f : (value > 0f ? 1f : -1f);
        #endregion

        #region Linear Interpolation (Lerp)
        /// <summary>
        /// Performs a high-performance fluent linear interpolation between two float boundaries.
        /// </summary>
        public static float Lerp(this float from, float to, float t) => Mathf.Lerp(from, to, t);

        /// <summary>
        /// Performs an un-clamped linear interpolation, vital for extreme out-of-bounds modular sound transformation tracks.
        /// </summary>
        public static float LerpUnclamped(this float from, float to, float t) => Mathf.LerpUnclamped(from, to, t);
        #endregion

        #region Step Quantization & Truncation
        /// <summary>
        /// Fluently truncates a single-precision floating-point value down to the nearest mathematical integer step boundary.
        /// </summary>
        public static float Floor(this float value) => Mathf.Floor(value);

        /// <summary>
        /// Fluently truncates a single-precision floating-point value up to the nearest mathematical integer step boundary.
        /// </summary>
        public static float Ceil(this float value) => Mathf.Ceil(value);
        #endregion

        #region Logarithmic & Acoustic Decibel Transforms
        /// <summary>
        /// Converts a raw logarithmic decibel amplitude value (-96dB..0dB) safely into a float-native linear coefficient scalar (0.0f..1.0f).
        /// </summary>
        public static float DbToLinear(this float db) => db <= -96f ? 0f : Mathf.Pow(10f, db / 20f);

        /// <summary>
        /// Converts a float-native linear coefficient scalar safely into a raw logarithmic decibel amplitude value.
        /// </summary>
        public static float LinearToDb(this float linear) => linear <= 0.00001f ? -96f : 20f * Mathf.Log10(linear);
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
        public static int RoundToInt(this float value) => Mathf.RoundToInt(value);

        /// <summary>
        /// Rounds a double-precision floating-point value to the nearest 32-bit signed integer using standard mathematical truncation loops.
        /// </summary>
        public static int RoundToInt(this double value) => (int)Math.Round(value);
        #endregion
    }
}