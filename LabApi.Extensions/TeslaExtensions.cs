using LabApi.Features.Wrappers;
using System;
using System.Collections.Generic;

namespace LabApi.Extensions
{
    /// <summary>
    /// Utility extensions for controlling Tesla gates.
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
            if (tesla is null)
                return;

            if (forceTrigger)
                tesla.Trigger();

            tesla.InactiveTime = duration;
        }

        #endregion

        #region Batch Tesla (IEnumerable + params)

        /// <summary>
        /// Disables multiple Tesla gates for a given duration.
        /// </summary>
        public static void DisableFor(this IEnumerable<Tesla> teslas, float duration, bool forceTrigger = true)
        => teslas.ForEach(t => t?.DisableFor(duration, forceTrigger));
        

        /// <summary>
        /// Disables multiple Tesla gates (params overload).
        /// </summary>
        public static void DisableFor(float duration, bool forceTrigger, params Tesla[] teslas)
            => ((IEnumerable<Tesla>)teslas).DisableFor(duration, forceTrigger);

        #endregion
    }
}
