using MEC;
using System;
using System.Collections.Generic;

namespace LabApi.Extensions
{
    /// <summary>
    /// Provides utility extension methods for Movement Evacuation Coroutines (MEC).
    /// </summary>
    public static class TimingExtensions
    {
        #region Delayed Spawners
        /// <summary>
        /// Executes an action after a delay if the specified condition evaluates to true.
        /// </summary>
        public static CoroutineHandle CallDelayedIf(float delay, Func<bool> condition, Action action, string coroutineTag = null)
        {
            if (action == null || condition == null) return default;

            CoroutineHandle handle = Timing.CallDelayed(delay, () =>
            {
                if (condition.Invoke())
                {
                    action.Invoke();
                }
            });

            if (!string.IsNullOrEmpty(coroutineTag))
            {
                handle.Tag = coroutineTag;
            }

            return handle;
        }
        #endregion

        #region Single Target Kill Operations
        /// <summary>
        /// Kills all coroutines bound to the specified string tag.
        /// </summary>
        public static void KillCoroutine(this string tag)
        {
            if (!string.IsNullOrEmpty(tag))
            {
                Timing.KillCoroutines(tag);
            }
        }

        /// <summary>
        /// Kills the coroutine mapped to the specified handle if it is running.
        /// </summary>
        public static void Kill(this CoroutineHandle handle)
        {
            if (handle.IsRunning)
            {
                Timing.KillCoroutines(handle);
            }
        }
        #endregion

        #region Batch & Params Operations
        /// <summary>
        /// Kills all coroutines bound to the collection of string tags.
        /// </summary>
        public static void KillCoroutines(this IEnumerable<string> tags)
        {
            if (tags == null) return;

            if (tags is List<string> concreteList)
            {
                int count = concreteList.Count;
                for (int i = 0; i < count; i++) concreteList[i].KillCoroutine();
                return;
            }

            foreach (string tag in tags)
            {
                tag.KillCoroutine();
            }
        }

        /// <summary>
        /// Kills all coroutines bound to the inline array of string tags.
        /// </summary>
        public static void KillCoroutines(params string[] tags)
        {
            if (tags is null) return;

            int count = tags.Length;
            for (int i = 0; i < count; i++) tags[i].KillCoroutine();
        }

        /// <summary>
        /// Kills all coroutines associated with the collection of handles.
        /// </summary>
        public static void Kill(this IEnumerable<CoroutineHandle> handles)
        {
            if (handles is null) return;

            if (handles is List<CoroutineHandle> concreteList)
            {
                int count = concreteList.Count;
                for (int i = 0; i < count; i++) concreteList[i].Kill();
                return;
            }

            foreach (CoroutineHandle handle in handles)
            {
                handle.Kill();
            }
        }

        /// <summary>
        /// Kills all coroutines associated with the inline array of handles.
        /// </summary>
        public static void Kill(params CoroutineHandle[] handles)
        {
            if (handles is null) return;

            int count = handles.Length;
            for (int i = 0; i < count; i++) handles[i].Kill();
        }

        /// <summary>
        /// Kills all coroutines in the list using a zero-allocation loop and clears the list.
        /// </summary>
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