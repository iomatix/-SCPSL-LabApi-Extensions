using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace LabApi.Extensions
{
    /// <summary>
    /// Highly optimized extension methods for working with collections, arrays, and cooldown/throttling maps.
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Executes the specified action for every item in the collection.
        /// Contains optimized zero-allocation fast-paths for Arrays and Lists.
        /// </summary>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null || action == null)
                return;

            // Fast path for arrays - 0 allocations, highly optimized by JIT (Bounds Check Elimination)
            if (source is T[] array)
            {
                int count = array.Length;
                for (int i = 0; i < count; i++)
                {
                    action(array[i]);
                }
                return;
            }

            // Fast path for Lists - 0 allocations, avoids Enumerator boxing
            if (source is List<T> list)
            {
                int count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    action(list[i]);
                }
                return;
            }

            // Fallback for general collections
            foreach (var item in source)
            {
                action(item);
            }
        }

        #region Throttle Model (Stores Last Execution Time)

        /// <summary>
        /// Executes the action if the cooldown (based on last execution timestamp) has elapsed.
        /// Updates the timestamp to current time and returns true if executed.
        /// </summary>
        /// <remarks>
        /// This method uses the "Throttle Model" (stores the last execution time). 
        /// Do not mix this with absolute lock methods like <see cref="IsCooldownActive{TKey}"/>.
        /// </remarks>
        public static bool ExecuteThrottled<TKey>(
            this IDictionary<TKey, DateTime> cooldownMap,
            TKey key,
            TimeSpan window,
            Action throttleAction)
        {
            if (cooldownMap is null || throttleAction is null)
                return false;

            DateTime now = DateTime.UtcNow;

            if (cooldownMap.TryGetValue(key, out var lastTime))
            {
                if (now - lastTime < window)
                    return false;
            }

            throttleAction();
            cooldownMap[key] = now;
            return true;
        }

        /// <summary>
        /// Checks if a throttle-based cooldown is currently active for the specified key.
        /// </summary>
        /// <remarks>
        /// Use this method specifically when your map is managed via <see cref="ExecuteThrottled{TKey}"/>.
        /// </remarks>
        public static bool IsThrottleActive<TKey>(
            this IDictionary<TKey, DateTime> cooldownMap,
            TKey key,
            TimeSpan window)
        {
            if (cooldownMap is null || cooldownMap.Count == 0)
                return false;

            return cooldownMap.TryGetValue(key, out var lastTime) &&
                   (DateTime.UtcNow - lastTime) < window;
        }

        #endregion

        #region Lock Model (Stores Absolute Expiry Time)

        /// <summary>
        /// Checks if a lock-based cooldown is active (where the stored value represents the absolute expiry time).
        /// </summary>
        /// <remarks>
        /// Use this method specifically when your map is managed via <see cref="TryAcquireLock{TKey}"/>.
        /// </remarks>
        public static bool IsCooldownActive<TKey>(this IDictionary<TKey, DateTime> cooldownMap, TKey key)
        {
            if (cooldownMap is null || cooldownMap.Count == 0)
                return false;

            return cooldownMap.TryGetValue(key, out var expiry) &&
                   DateTime.UtcNow < expiry;
        }

        /// <summary>
        /// Attempts to acquire a lock for the specified key. If the lock has elapsed or does not exist,
        /// commits a new absolute expiration timestamp (current time + lockWindow) and returns true.
        /// </summary>
        /// <remarks>
        /// This method uses the "Lock Model" (stores the absolute expiration time).
        /// </remarks>
        public static bool TryAcquireLock<TKey>(
            this IDictionary<TKey, DateTime> cooldownMap,
            TKey key,
            TimeSpan lockWindow)
        {
            if (cooldownMap is null)
                return false;

            DateTime now = DateTime.UtcNow;

            if (cooldownMap.TryGetValue(key, out var nextAllowed))
            {
                if (now < nextAllowed)
                    return false;
            }

            cooldownMap[key] = now + lockWindow;
            return true;
        }

        #endregion

        #region Pruning

        /// <summary>
        /// Removes all expired entries from the dictionary with zero GC heap allocations.
        /// Works seamlessly with both Throttle and Lock models.
        /// </summary>
        public static void PruneExpired<TKey>(this IDictionary<TKey, DateTime> dictionary, DateTime comparisonTime)
        {
            if (dictionary is null || dictionary.Count == 0)
                return;

            int total = dictionary.Count;
            TKey[] buffer = ArrayPool<TKey>.Shared.Rent(total);
            int expired = 0;

            try
            {
                // Fast path for standard Dictionary - struct enumerator, 0 allocations
                if (dictionary is Dictionary<TKey, DateTime> concrete)
                {
                    foreach (var kvp in concrete)
                    {
                        if (comparisonTime >= kvp.Value)
                            buffer[expired++] = kvp.Key;
                    }
                }
                // Fast path for ConcurrentDictionary (extremely common in multi-threaded environments)
                else if (dictionary is ConcurrentDictionary<TKey, DateTime> concurrent)
                {
                    foreach (var kvp in concurrent)
                    {
                        if (comparisonTime >= kvp.Value)
                            buffer[expired++] = kvp.Key;
                    }
                }
                // Fallback for general IDictionary implementations
                else
                {
                    foreach (var kvp in dictionary)
                    {
                        if (comparisonTime >= kvp.Value)
                            buffer[expired++] = kvp.Key;
                    }
                }

                for (int i = 0; i < expired; i++)
                {
                    dictionary.Remove(buffer[i]);
                }
            }
            finally
            {
                // Note: clearArray is set to true to prevent reference-type memory leaks (holding dead objects in pool).
                ArrayPool<TKey>.Shared.Return(buffer, clearArray: true);
            }
        }

        #endregion
    }
}