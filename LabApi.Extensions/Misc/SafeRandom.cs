using System;

namespace LabApi.Extensions.Misc
{
    /// <summary>
    /// Provides an enterprise-grade, high-performance pseudo-random number generator (PRNG) engine.
    /// Utilizes <see cref="ThreadStaticAttribute"/> isolation to guarantee absolute thread-safety and eliminate 
    /// concurrency lock contention across asynchronous server coroutines and parallel execution threads.
    /// </summary>
    internal static class SafeRandom
    {
        [ThreadStatic]
        private static Random _localRandom;

        /// <summary>
        /// Gets the thread-isolated <see cref="Random"/> instance allocated specifically to the executing thread's local storage block.
        /// Lazily initializes a unique seed mapping utilizing a distinct structural GUID hash sequence to prevent seed synchronization anomalies.
        /// </summary>
        private static Random ThreadRandom => _localRandom ??= new(Guid.NewGuid().GetHashCode());

        /// <summary>
        /// Generates a thread-safe pseudo-random signed 32-bit integer within a concrete range boundary.
        /// </summary>
        /// <param name="min">The inclusive lower historical bound constraint of the generated integer value.</param>
        /// <param name="max">The exclusive upper historical bound constraint of the generated integer value.</param>
        /// <returns>A signed 32-bit integer greater than or equal to <paramref name="min"/> and strictly less than <paramref name="max"/>.</returns>
        public static int Next(int min, int max) => ThreadRandom.Next(min, max);

        /// <summary>
        /// Generates a thread-safe pseudo-random double-precision floating-point scalar token tracking between 0.0 and 1.0.
        /// </summary>
        /// <returns>A double-precision floating-point scalar value greater than or equal to 0.0, and strictly less than 1.0.</returns>
        public static double NextDouble() => ThreadRandom.NextDouble();

        /// <summary>
        /// Computes a single-precision random floating-point scalar value mapped across a localized linear scaling envelope 
        /// bridging the specified minimum and maximum mathematical limits.
        /// </summary>
        /// <param name="min">The inclusive lower boundary limit coefficient assigned for range calculations.</param>
        /// <param name="max">The inclusive upper boundary limit coefficient assigned for range calculations.</param>
        /// <returns>A single-precision floating-point scalar value falling within the explicit tracking boundaries of <paramref name="min"/> and <paramref name="max"/>.</returns>
        public static float Range(float min, float max) => (float)(NextDouble() * (max - min) + min);

        /// <summary>
        /// Performs a thread-safe probability evaluation roll against a specified percentage value (0.0 to 100.0).
        /// </summary>
        /// <param name="chancePercentage">The success chance threshold expressed as a percentage value between 0 and 100.</param>
        /// <returns><c>true</c> if the generated random matrix falls within the success threshold boundary; otherwise, <c>false</c>.</returns>
        public static bool RollSuccess(this float chancePercentage)
        {
            if (chancePercentage <= 0f) return false;
            if (chancePercentage >= 100f) return true;

            // Leverage our core thread-isolated high-performance double generator map
            return (NextDouble() * 100.0) <= chancePercentage;
        }
    }
}