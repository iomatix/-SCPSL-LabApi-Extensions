using System;
using System.Collections.Generic;
using MEC;

namespace LabApi.Extensions
{
    /// <summary>
    /// Provides advanced high-performance utility abstraction layers and shortcut overloads 
    /// for the Movement Evacuation Coroutines (MEC) execution pipeline.
    /// </summary>
    internal static class TimingExtensions
    {
        #region Independent Sub-System Delayed Spawners

        /// <summary>
        /// Executes a specified action after a structural delay window, but only if a designated 
        /// state validation condition evaluates to true at the exact moment of execution.
        /// </summary>
        /// <param name="delay">The temporal delay duration in seconds to standby before invoking the predicate loop.</param>
        /// <param name="condition">The state verification delegate evaluated defensively before running the core logic block.</param>
        /// <param name="action">The core operational logic block executed seamlessly upon successful validation query.</param>
        /// <param name="coroutineTag">An optional custom tracking token assigned automatically to the spawned MEC coroutine node.</param>
        /// <returns>The native <see cref="CoroutineHandle"/> instance mapping the underlying execution lifetime track.</returns>
        public static CoroutineHandle CallDelayedIf(float delay, Func<bool> condition, Action action, string coroutineTag = null)
        {
            if (action == null || condition == null)
            {
                return default;
            }

            // Spawn the underlying delayed worker node seamlessly
            CoroutineHandle handle = Timing.CallDelayed(delay, () =>
            {
                // Defensive verification boundary evaluation right before invoking the execution payload
                if (condition.Invoke())
                {
                    action.Invoke();
                }
            });

            // Handle optional isolated tracking tag synchronization dynamically
            if (!string.IsNullOrEmpty(coroutineTag))
            {
                handle.Tag = coroutineTag;
            }

            return handle;
        }
        #endregion

        #region Single Target Operations (Single Source of Truth)

        /// <summary>
        /// Forcibly terminates the active coroutine tracking instance bound to this specific string token identifier safely.
        /// </summary>
        /// <param name="tag">The source string tracking token mapping the active MEC coroutine layer to target for destruction.</param>
        public static void KillCoroutine(this string tag)
        {
            if (!string.IsNullOrEmpty(tag))
            {
                Timing.KillCoroutines(tag);
            }
        }

        /// <summary>
        /// Defensively verifies whether the individual coroutine handle is actively running and forcibly terminates its lifetime thread.
        /// </summary>
        /// <param name="handle">The targeting live <see cref="CoroutineHandle"/> tracker focused for destruction.</param>
        public static void Kill(this CoroutineHandle handle)
        {
            if (handle.IsRunning)
            {
                Timing.KillCoroutines(handle);
            }
        }

        #endregion

        #region Batch Collection Operations (Pure DRY Wrappers)

        /// <summary>
        /// Systematically terminates all active running coroutine execution tracks mapped to an aggregated collection layout of string tokens.
        /// </summary>
        /// <param name="tags">The source enumerable stream tracking the explicit target MEC handles cleared out from memory spaces.</param>
        public static void KillCoroutines(this IEnumerable<string> tags)
        {
            if (tags == null) return;

            foreach (string tag in tags)
            {
                tag.KillCoroutine();
            }
        }

        /// <summary>
        /// Systematically iterates over a collection stream of coroutine handles to terminate each active executing thread safely.
        /// </summary>
        /// <param name="handles">The targeted enumerable collection sequence of coroutine trackers.</param>
        public static void Kill(this IEnumerable<CoroutineHandle> handles)
        {
            if (handles is null) return;

            foreach (CoroutineHandle handle in handles)
            {
                handle.Kill();
            }
        }

        /// <summary>
        /// Systematically terminates all running coroutine handles stored within the list layout utilizing zero heap allocation indexing loops, and flushes the cache.
        /// </summary>
        /// <param name="coroutines">The backing collection layout tracking active executing MEC coroutine handles inside memory spaces.</param>
        public static void KillAndClear(this List<CoroutineHandle> coroutines)
        {
            if (coroutines is null) return;

            int count = coroutines.Count;
            for (int i = 0; i < count; i++)
            {
                coroutines[i].Kill();
            }

            coroutines.Clear();
        }

        #endregion
    }
}