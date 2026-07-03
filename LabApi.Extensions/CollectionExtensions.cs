using System;
using System.Collections.Generic;

namespace LabApi.Extensions
{
    /// <summary>
    /// Provides high-performance fluent utility extensions for advanced collection mappings and dictionary state engines.
    /// </summary>
    internal static class CollectionExtensions
    {
        /// <summary>
        /// Evaluates a temporal cooldown window for a specific key profile. 
        /// If the tracking node is ready, executes the payload and automatically commits the new timestamp.
        /// </summary>
        /// <typeparam name="TKey">The structural identity lookup key tracking type.</typeparam>
        /// <param name="cooldownMap">The backing storage dictionary tracking execution milestones.</param>
        /// <param name="key">The specific instance identity key target checked for rate limiting.</param>
        /// <param name="window">The required minimum structural <see cref="TimeSpan"/> lock window before the action can rerun.</param>
        /// <param name="throttleAction">The operational delegate invoked seamlessly upon a successful readiness check.</param>
        /// <returns><c>true</c> if the cooldown has successfully elapsed and the action executed; otherwise, <c>false</c>.</returns>
        public static bool ExecuteThrottled<TKey>(this IDictionary<TKey, DateTime> cooldownMap, TKey key, TimeSpan window, Action throttleAction)
        {
            if (cooldownMap == null || throttleAction == null) return false;

            DateTime now = DateTime.UtcNow;

            if (cooldownMap.TryGetValue(key, out DateTime lastExecutionTime))
            {
                if (now - lastExecutionTime < window)
                {
                    return false; // Operational window is locked. Suppress invocation.
                }
            }

            // Fire the targeted behavioral payload directly
            throttleAction.Invoke();

            // Commit the structural milestone record to the state map
            cooldownMap[key] = now;
            return true;
        }

        /// <summary>
        /// Systematically purges all historical mapping entries whose tracking timestamps have safely elapsed past a specific temporal comparison index.
        /// Mitigates collection modification exceptions by isolating target records dynamically.
        /// </summary>
        /// <typeparam name="TKey">The structural identity lookup key tracking type.</typeparam>
        /// <param name="dictionary">The target backing storage dictionary undergoing reference cleanup.</param>
        /// <param name="comparisonTime">The core current <see cref="System.DateTime"/> index acting as the expiration threshold criteria.</param>
        public static void PruneExpired<TKey>(this System.Collections.Generic.IDictionary<TKey, System.DateTime> dictionary, System.DateTime comparisonTime)
        {
            if (dictionary is null || dictionary.Count == 0) return;

            System.Collections.Generic.List<TKey> expiredKeys = new();

            foreach (System.Collections.Generic.KeyValuePair<TKey, System.DateTime> kvp in dictionary)
            {
                if (comparisonTime >= kvp.Value)
                {
                    expiredKeys.Add(kvp.Key);
                }
            }

            int count = expiredKeys.Count;
            for (int i = 0; i < count; i++)
            {
                dictionary.Remove(expiredKeys[i]);
            }
        }

        /// <summary>
        /// Evaluates defensively whether a specific key profile is currently locked within an active temporal cooldown window.
        /// </summary>
        /// <typeparam name="TKey">The structural identity lookup key tracking type.</typeparam>
        /// <param name="cooldownMap">The target backing storage dictionary tracking execution milestones.</param>
        /// <param name="key">The specific instance identity key target checked for active cooldown metrics.</param>
        /// <returns><c>true</c> if the tracking node exists and its registered expiration timestamp sits in the future; otherwise, <c>false</c>.</returns>
        public static bool IsCooldownActive<TKey>(this System.Collections.Generic.IDictionary<TKey, System.DateTime> cooldownMap, TKey key)
        {
            if (cooldownMap is null || cooldownMap.Count == 0) return false;

            return cooldownMap.TryGetValue(key, out System.DateTime expiryTime) && System.DateTime.UtcNow < expiryTime;
        }

        /// <summary>
        /// Atomically evaluates a temporal gate check for a specific key target. 
        /// If the lock window has elapsed, automatically commits a new future expiration milestone and grants authorization.
        /// </summary>
        /// <typeparam name="TKey">The structural identity lookup key tracking type.</typeparam>
        /// <param name="cooldownMap">The backing storage dictionary tracking execution milestones.</param>
        /// <param name="key">The specific instance identity key target checked for rate limiting.</param>
        /// <param name="lockWindow">The chronological execution lock window duration applied if authorization is granted.</param>
        /// <returns><c>true</c> if the gate was open, authorization was granted, and the new lock was committed; otherwise, <c>false</c>.</returns>
        public static bool TryAcquireLock<TKey>(this System.Collections.Generic.IDictionary<TKey, System.DateTime> cooldownMap, TKey key, System.TimeSpan lockWindow)
        {
            if (cooldownMap is null) return false;

            System.DateTime now = System.DateTime.UtcNow;

            if (cooldownMap.TryGetValue(key, out System.DateTime nextAllowedTime))
            {
                if (now < nextAllowedTime)
                {
                    return false; // Spatial gate is firmly locked. Abort transaction.
                }
            }

            // Commit the future chronological boundary directly and grant passage
            cooldownMap[key] = now + lockWindow;
            return true;
        }
    }
}