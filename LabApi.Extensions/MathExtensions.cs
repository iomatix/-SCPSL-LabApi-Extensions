using LabApi.Extensions.Misc;
using System;

namespace LabApi.Extensions
{
    /// <summary>
    /// Thread-safe, high-performance math and probability extensions for float, int, and double values.
    /// Bypasses UnityEngine.Mathf to guarantee compatibility with background worker threads and maximize JIT inlining.
    /// </summary>
    public static class MathExtensions
    {
        #region Clamping (Thread-Safe)

        /// <summary>
        /// Clamps the value between the given minimum and maximum.
        /// </summary>
        public static float Clamp(this float value, float min, float max)
            => value < min ? min : (value > max ? max : value);

        /// <summary>
        /// Clamps the value between the given minimum and maximum.
        /// </summary>
        public static int Clamp(this int value, int min, int max)
            => value < min ? min : (value > max ? max : value);

        /// <summary>
        /// Clamps the value between the given minimum and maximum.
        /// </summary>
        public static double Clamp(this double value, double min, double max)
            => value < min ? min : (value > max ? max : value);

        #endregion

        #region Min / Max Limits (Thread-Safe)

        /// <summary>
        /// Ensures the value is not lower than the given minimumBaseline.
        /// </summary>
        public static float LimitMin(this float value, float minBaseline)
            => value < minBaseline ? minBaseline : value;

        /// <summary>
        /// Ensures the value is not lower than the given minimumBaseline.
        /// </summary>
        public static int LimitMin(this int value, int minBaseline)
            => value < minBaseline ? minBaseline : value;

        /// <summary>
        /// Ensures the value is not lower than the given minimumBaseline.
        /// </summary>
        public static double LimitMin(this double value, double minBaseline)
            => value < minBaseline ? minBaseline : value;

        /// <summary>
        /// Ensures the value is not higher than the given maximumBaseline.
        /// </summary>
        public static float LimitMax(this float value, float maxBaseline)
            => value > maxBaseline ? maxBaseline : value;

        /// <summary>
        /// Ensures the value is not higher than the given maximumBaseline.
        /// </summary>
        public static int LimitMax(this int value, int maxBaseline)
            => value > maxBaseline ? maxBaseline : value;

        /// <summary>
        /// Ensures the value is not higher than the given maximumBaseline.
        /// </summary>
        public static double LimitMax(this double value, double maxBaseline)
            => value > maxBaseline ? maxBaseline : value;

        #endregion

        #region Absolute & Sign (Thread-Safe)

        /// <summary>
        /// Returns the absolute value.
        /// </summary>
        public static int Abs(this int value)
            => value < 0 ? -value : value;

        /// <summary>
        /// Returns the absolute value.
        /// </summary>
        public static float Abs(this float value)
            => value < 0f ? -value : value;

        /// <summary>
        /// Returns the absolute value.
        /// </summary>
        public static double Abs(this double value)
            => value < 0.0 ? -value : value;

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

        #region Lerp (Thread-Safe)

        /// <summary>
        /// Linearly interpolates between two values, clamping t between 0 and 1.
        /// </summary>
        public static float Lerp(this float from, float to, float t)
        {
            float clampT = t < 0f ? 0f : (t > 1f ? 1f : t);
            return from + (to - from) * clampT;
        }

        /// <summary>
        /// Linearly interpolates without clamping t.
        /// </summary>
        public static float LerpUnclamped(this float from, float to, float t)
            => from + (to - from) * t;

        #endregion

        #region Floor / Ceil (Thread-Safe)

        /// <summary>
        /// Floors the value.
        /// </summary>
        public static float Floor(this float value)
            => (float)Math.Floor(value);

        /// <summary>
        /// Floors the value.
        /// </summary>
        public static double Floor(this double value)
            => Math.Floor(value);

        /// <summary>
        /// Ceils the value.
        /// </summary>
        public static float Ceil(this float value)
            => (float)Math.Ceiling(value);

        /// <summary>
        /// Ceils the value.
        /// </summary>
        public static double Ceil(this double value)
            => Math.Ceiling(value);

        #endregion

        #region Decibels (Thread-Safe)

        /// <summary>
        /// Converts decibels to a linear 0–1 value.
        /// </summary>
        public static float DbToLinear(this float db)
            => db <= -96f ? 0f : (float)Math.Pow(10.0, db / 20.0);

        /// <summary>
        /// Converts a linear 0–1 value to decibels.
        /// </summary>
        public static float LinearToDb(this float linear)
            => linear <= 0.00001f ? -96f : 20f * (float)Math.Log10(linear);

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

        #region Rounding (Thread-Safe)

        /// <summary>
        /// Rounds the value to the nearest integer.
        /// </summary>
        public static int RoundToInt(this float value)
            => (int)Math.Round(value);

        /// <summary>
        /// Rounds the value to the nearest integer.
        /// </summary>
        public static int RoundToInt(this double value)
            => (int)Math.Round(value);

        #endregion

        #region Probability & Smart Randoms

        /// <summary>
        /// Rolls a percentage-based chance (0% to 100%). Returns true if the roll succeeds.
        /// </summary>
        public static bool RollChance(this float percentageChance)
        {
            if (percentageChance <= 0f) return false;
            if (percentageChance >= 100f) return true;

            return SafeRandom.Range(0f, 100f) <= percentageChance;
        }

        /// <summary>
        /// Rolls an integer percentage-based chance (0% to 100%). Returns true if the roll succeeds.
        /// </summary>
        public static bool RollChance(this int percentageChance)
        {
            if (percentageChance <= 0) return false;
            if (percentageChance >= 100) return true;

            return SafeRandom.Next(0, 100) < percentageChance;
        }

        /// <summary>
        /// Rolls a normalized-based chance (0.0 to 1.0). Returns true if the roll succeeds.
        /// </summary>
        public static bool RollChanceNormalized(this float normalizedChance)
        {
            if (normalizedChance <= 0f) return false;
            if (normalizedChance >= 1f) return true;

            return SafeRandom.Range(0f, 1f) <= normalizedChance;
        }

        /// <summary>
        /// Returns a random float between this minimum and the specified maximum.
        /// </summary>
        public static float RandomTo(this float min, float max)
            => SafeRandom.Range(min, max);

        /// <summary>
        /// Returns a random integer between this minimum (inclusive) and the specified maximum (exclusive).
        /// </summary>
        public static int RandomTo(this int min, int max)
            => SafeRandom.Next(min, max);

        #endregion
    }
}