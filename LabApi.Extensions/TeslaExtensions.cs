using LabApi.Features.Wrappers;
using System.Collections.Generic;

namespace LabApi.Extensions
{
    /// <summary>
    /// Highly optimized utility extensions for controlling Tesla gates.
    /// Features zero-allocation state-passing batch operations and unified null-safety.
    /// </summary>
    public static class TeslaExtensions
    {
        #region Single Tesla

        /// <summary>
        /// Disables a Tesla gate for a given duration.
        /// Use <paramref name="forceTrigger"/> to trigger a discharge.
        /// </summary>
        public static void DisableFor(this Tesla tesla, float duration, bool forceTrigger = true)
        {
            // FIX: Unified null-checking standard (== null instead of is null) to prevent Unity lifetime bypass.
            if (tesla == null)
                return;

            if (forceTrigger)
            {
                tesla.Trigger();
            }

            tesla.InactiveTime = duration;
        }

        #endregion

        #region Batch Tesla (Zero-Allocation via State-Passing)

        /// <summary>
        /// Disables multiple Tesla gates for a given duration with zero heap allocations.
        /// </summary>
        public static void DisableFor(this IEnumerable<Tesla> teslas, float duration, bool forceTrigger = true)
        {
            if (teslas == null)
                return;

            // FIX: Enforced state-passing and static lambda to completely eliminate closure allocations.
            teslas.ForEach((duration, forceTrigger), static (t, state) => t?.DisableFor(state.duration, state.forceTrigger));
        }

        /// <summary>
        /// Disables multiple Tesla gates (params overload).
        /// </summary>
        public static void DisableFor(float duration, bool forceTrigger, params Tesla[] teslas)
        {
            if (teslas == null)
                return;

            teslas.DisableFor(duration, forceTrigger);
        }

        #endregion
    }
}