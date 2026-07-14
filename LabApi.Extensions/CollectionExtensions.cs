using System;
using System.Buffers;
using System.Collections.Generic;

namespace LabApi.Extensions
{
    /// <summary>
    /// Simple helpers for working with collections and cooldown dictionaries.
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Runs the action for every item in the collection.
        /// Uses a zero‑allocation fast‑path for List<T>.
        /// </summary>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null || action == null)
                return;

            if (source is List<T> list)
            {
                int count = list.Count;
                for (int i = 0; i < count; i++)
                    action(list[i]);
                return;
            }

            foreach (var item in source)
                action(item);
        }

        /// <summary>
        /// Executes the action if the cooldown for the key has elapsed.
        /// Updates the timestamp and returns true if executed.
        /// </summary>
        public static bool ExecuteThrottled<TKey>(this IDictionary<TKey, DateTime> cooldownMap, TKey key, TimeSpan window, Action throttleAction)
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
        /// Removes all entries whose timestamps are older than the comparison time.
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
                if (dictionary is Dictionary<TKey, DateTime> concrete)
                {
                    foreach (var kvp in concrete)
                    {
                        if (comparisonTime >= kvp.Value)
                            buffer[expired++] = kvp.Key;
                    }
                }
                else
                {
                    foreach (var kvp in dictionary)
                    {
                        if (comparisonTime >= kvp.Value)
                            buffer[expired++] = kvp.Key;
                    }
                }

                for (int i = 0; i < expired; i++)
                    dictionary.Remove(buffer[i]);
            }
            finally
            {
                ArrayPool<TKey>.Shared.Return(buffer, clearArray: true);
            }
        }

        /// <summary>
        /// Returns true if the key exists and its cooldown has not yet expired.
        /// </summary>
        public static bool IsCooldownActive<TKey>(this IDictionary<TKey, DateTime> cooldownMap, TKey key)
        {
            if (cooldownMap is null || cooldownMap.Count == 0)
                return false;

            return cooldownMap.TryGetValue(key, out var expiry) &&
                   DateTime.UtcNow < expiry;
        }

        /// <summary>
        /// Returns true if the cooldown has elapsed and commits a new expiration timestamp.
        /// </summary>
        public static bool TryAcquireLock<TKey>(this IDictionary<TKey, DateTime> cooldownMap, TKey key, TimeSpan lockWindow)
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
    }
}
