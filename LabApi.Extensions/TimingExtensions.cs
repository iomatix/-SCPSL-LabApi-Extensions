using MEC;
using System;
using System.Collections.Generic;

namespace LabApi.Extensions
{
    /// <summary>
    /// Utility extensions for working with MEC coroutines.
    /// </summary>
    public static class TimingExtensions
    {
        #region Delayed

        /// <summary>
        /// Calls <paramref name="action"/> after a delay if <paramref name="condition"/> returns true.
        /// </summary>
        public static CoroutineHandle CallDelayedIf(float delay, Func<bool> condition, Action action, string coroutineTag = null)
        {
            if (action is null || condition is null)
                return default;

            var handle = Timing.CallDelayed(delay, () =>
            {
                if (condition())
                    action();
            });

            if (!string.IsNullOrEmpty(coroutineTag))
                handle.Tag = coroutineTag;

            return handle;
        }

        #endregion

        #region Single Kill

        /// <summary>
        /// Kills all coroutines bound to this tag.
        /// </summary>
        public static void Kill(this string tag)
        {
            if (!string.IsNullOrEmpty(tag))
                Timing.KillCoroutines(tag);
        }

        /// <summary>
        /// Kills this coroutine handle if it is running.
        /// </summary>
        public static void Kill(this CoroutineHandle handle)
        {
            if (handle.IsRunning)
                Timing.KillCoroutines(handle);
        }

        #endregion

        #region Batch Kill (tags)

        /// <summary>
        /// Kills all coroutines bound to the given tags.
        /// </summary>
        public static void Kill(this IEnumerable<string> tags)
            => tags.ForEach(t => t?.Kill());

        /// <summary>
        /// Kills all coroutines bound to the given tags (params overload).
        /// </summary>
        public static void Kill(params string[] tags)
            => ((IEnumerable<string>)tags).Kill();

        #endregion

        #region Batch Kill (handles)

        /// <summary>
        /// Kills all coroutines associated with the given handles.
        /// </summary>
        public static void KillAll(this IEnumerable<CoroutineHandle> handles)
            => handles.ForEach(h => h.Kill());

        /// <summary>
        /// Kills all coroutines associated with the given handles (params overload).
        /// </summary>
        public static void KillAll(params CoroutineHandle[] handles)
            => ((IEnumerable<CoroutineHandle>)handles).KillAll();

        /// <summary>
        /// Kills all coroutines in the list and clears it.
        /// </summary>
        public static void KillAllAndClear(this List<CoroutineHandle> handles)
        {
            handles?.ForEach(h => h.Kill());
            handles.Clear();
        }

        #endregion
    }
}
