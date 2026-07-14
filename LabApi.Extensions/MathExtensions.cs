using System;
using UnityEngine;

namespace LabApi.Extensions
{
    /// <summary>
    /// Helpers for common math operations on float, int and double values.
    /// </summary>
    public static class MathExtensions
    {
        #region Clamping

        /// <summary>
        /// Clamps the value between the given minimum and maximum.
        /// </summary>
        public static float Clamp(this float value, float min, float max)
            => Mathf.Clamp(value, min, max);

        /// <summary>
        /// Clamps the value between the given minimum and maximum.
        /// </summary>
        public static int Clamp(this int value, int min, int max)
            => Mathf.Clamp(value, min, max);

        /// <summary>
        /// Clamps the value between the given minimum and maximum.
        /// </summary>
        public static double Clamp(this double value, double min, double max)
            => value < min ? min : (value > max ? max : value);

        #endregion

        #region Min / Max Limits

        /// <summary>
        /// Ensures the value is not lower than the given minimum.
        /// </summary>
        public static float LimitMin(this float value, float minBaseline)
            => Mathf.Max(minBaseline, value);

        /// <summary>
        /// Ensures the value is not lower than the given minimum.
        /// </summary>
        public static int LimitMin(this int value, int minBaseline)
            => Mathf.Max(minBaseline, value);

        /// <summary>
        /// Ensures the value is not lower than the given minimum.
        /// </summary>
        public static double LimitMin(this double value, double minBaseline)
            => value < minBaseline ? minBaseline : value;

        /// <summary>
        /// Ensures the value is not higher than the given maximum.
        /// </summary>
        public static float LimitMax(this float value, float maxBaseline)
            => Mathf.Min(maxBaseline, value);

        /// <summary>
        /// Ensures the value is not higher than the given maximum.
        /// </summary>
        public static int LimitMax(this int value, int maxBaseline)
            => Mathf.Min(maxBaseline, value);

        /// <summary>
        /// Ensures the value is not higher than the given maximum.
        /// </summary>
        public static double LimitMax(this double value, double maxBaseline)
            => value > maxBaseline ? maxBaseline : value;

        #endregion

        #region Absolute & Sign

        /// <summary>
        /// Returns the absolute value.
        /// </summary>
        public static int Abs(this int value)
            => Math.Abs(value);

        /// <summary>
        /// Returns the absolute value.
        /// </summary>
        public static float Abs(this float value)
            => Mathf.Abs(value);

        /// <summary>
        /// Returns the absolute value.
        /// </summary>
        public static double Abs(this double value)
            => Math.Abs(value);

        /// <summary>
        /// Returns -1, 0 or 1 depending on the value.
        /// </summary>
        public static int Sign(this int value)
            => value == 0 ? 0 : (value > 0 ? 1 : -1);

        /// <summary>
        /// Returns -1, 0 or 1 depending on the value.
        /// </summary>
        public static float Sign(this float value)
            => value == 0f ? 0f : (value > 0f ? 1f : -1f);

        /// <summary>
        /// Returns -1, 0 or 1 depending on the value.
        /// </summary>
        public static double Sign(this double value)
            => value == 0.0 ? 0.0 : (value > 0.0 ? 1.0 : -1.0);

        #endregion

        #region Lerp

        /// <summary>
        /// Linearly interpolates between two values.
        /// </summary>
        public static float Lerp(this float from, float to, float t)
            => Mathf.Lerp(from, to, t);

        /// <summary>
        /// Linearly interpolates without clamping the alpha.
        /// </summary>
        public static float LerpUnclamped(this float from, float to, float t)
            => Mathf.LerpUnclamped(from, to, t);

        #endregion

        #region Floor / Ceil

        /// <summary>
        /// Floors the value.
        /// </summary>
        public static float Floor(this float value)
            => Mathf.Floor(value);

        /// <summary>
        /// Floors the value.
        /// </summary>
        public static double Floor(this double value)
            => Math.Floor(value);

        /// <summary>
        /// Ceils the value.
        /// </summary>
        public static float Ceil(this float value)
            => Mathf.Ceil(value);

        /// <summary>
        /// Ceils the value.
        /// </summary>
        public static double Ceil(this double value)
            => Math.Ceiling(value);

        #endregion

        #region Decibels

        /// <summary>
        /// Converts decibels to a linear 0–1 value.
        /// </summary>
        public static float DbToLinear(this float db)
            => db <= -96f ? 0f : Mathf.Pow(10f, db / 20f);

        /// <summary>
        /// Converts a linear 0–1 value to decibels.
        /// </summary>
        public static float LinearToDb(this float linear)
            => linear <= 0.00001f ? -96f : 20f * Mathf.Log10(linear);

        #endregion

        #region NaN / Infinity Checks

        /// <summary>
        /// Returns true if the value is NaN or Infinity.
        /// </summary>
        public static bool IsNanOrInfinity(this float value)
            => float.IsNaN(value) || float.IsInfinity(value);

        /// <summary>
        /// Returns true if the value is NaN or Infinity.
        /// </summary>
        public static bool IsNanOrInfinity(this double value)
            => double.IsNaN(value) || double.IsInfinity(value);

        /// <summary>
        /// Always false for integers.
        /// </summary>
        public static bool IsNanOrInfinity(this int value)
            => false;

        #endregion

        #region Rounding

        /// <summary>
        /// Rounds the value to the nearest integer.
        /// </summary>
        public static int RoundToInt(this float value)
            => Mathf.RoundToInt(value);

        /// <summary>
        /// Rounds the value to the nearest integer.
        /// </summary>
        public static int RoundToInt(this double value)
            => (int)Math.Round(value);

        #endregion
    }
}