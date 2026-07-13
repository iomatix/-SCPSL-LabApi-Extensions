using LabApi.Features.Wrappers;
using System.Collections.Generic;

namespace LabApi.Extensions
{
    /// <summary>
    /// Provides extension methods for controlling and manipulating Tesla gates.
    /// </summary>
    public static class TeslaExtensions
    {
        #region Single Target Operations
        /// <summary>
        /// Disables a single Tesla gate for a specified duration with an optional discharge trigger.
        /// </summary>
        public static void DisableFor(this Tesla tesla, float duration, bool forceTrigger = true)
        {
            if (tesla == null) return;

            if (forceTrigger)
            {
                tesla.Trigger();
            }
            tesla.InactiveTime = duration;
        }
        #endregion

        #region Batch & Params Operations (Added for API Consistency)
        /// <summary>
        /// Disables a collection of Tesla gates for a specified duration with an optional discharge trigger.
        /// </summary>
        public static void DisableFor(this IEnumerable<Tesla> teslas, float duration, bool forceTrigger = true)
        {
            if (teslas == null) return;

            if (teslas is List<Tesla> concreteList)
            {
                int count = concreteList.Count;
                for (int i = 0; i < count; i++) concreteList[i].DisableFor(duration, forceTrigger);
                return;
            }

            foreach (Tesla tesla in teslas)
            {
                tesla.DisableFor(duration, forceTrigger);
            }
        }

        /// <summary>
        /// Disables an inline array of Tesla gates for a specified duration with an optional discharge trigger.
        /// </summary>
        public static void DisableFor(float duration, bool forceTrigger, params Tesla[] teslas)
        {
            if (teslas is null) return;

            int count = teslas.Length;
            for (int i = 0; i < count; i++) teslas[i].DisableFor(duration, forceTrigger);
        }

        /// <summary>
        /// Sets the inactive cooldown time for a collection of Tesla gates.
        /// </summary>
        public static void SetInactiveTimeForAll(this IEnumerable<Tesla> teslas, float duration, bool forceTrigger = true)
            => teslas.DisableFor(duration, forceTrigger);

        /// <summary>
        /// Deactivates a collection of Tesla gates using a default shorthand configuration.
        /// </summary>
        public static void Disable(this IEnumerable<Tesla> teslas, float duration = 5.0f, bool forceTrigger = false)
            => teslas.DisableFor(duration, forceTrigger);
        #endregion
    }
}