using LabApi.Features.Wrappers;
using System.Collections.Generic;

namespace LabApi.Extensions
{
    /// <summary>
    /// Provides high-performance utility abstraction layers for manipulating Tesla gate components across the facility floor topology map.
    /// </summary>
    internal static class TeslaExtensions
    {
        /// <summary>
        /// Forcibly puts a specific Tesla gate into an inactive operational cooldown state for a designated time window.
        /// </summary>
        /// <param name="tesla">The source <see cref="Tesla"/> gate asset component model manipulated defensively.</param>
        /// <param name="duration">The total temporal delay window in seconds during which the weapon array remains inert.</param>
        /// <param name="forceTrigger">If set to <c>true</c>, forcibly shocks the gate logic blocks to discharge current arcs before applying the timer envelope.</param>
        public static void DisableFor(this Tesla tesla, float duration, bool forceTrigger = true)
        {
            if (tesla == null) return;

            if (forceTrigger)
            {
                tesla.Trigger();
            }
            tesla.InactiveTime = duration;
        }

        /// <summary>
        /// Iterates over a collection layout of Tesla gates to reset their active cooldown timers or force a temporary tactical shutdown loop.
        /// </summary>
        /// <param name="teslas">The target enumerable collection layout containing active Tesla subsystem nodes.</param>
        /// <param name="duration">The core baseline operational inactive timer tracking value committed to the target assets.</param>
        /// <param name="forceTrigger">If set to <c>true</c>, forcibly shocks the gate logic blocks to discharge current arcs before applying the timer envelope.</param>
        public static void SetInactiveTimeForAll(this IEnumerable<Tesla> teslas, float duration, bool forceTrigger = true)
        {
            if (teslas == null) return;

            foreach (Tesla tesla in teslas)
            {
                tesla.DisableFor(duration, forceTrigger);
            }
        }

        /// <summary>
        /// High-level semantic shortcut to instantly deactivate an entire collection profile of Tesla gates with an optional chronological duration envelope.
        /// Seamlessly delegates execution to the underlying structural state engine to maintain absolute single responsibility.
        /// </summary>
        /// <param name="teslas">The target enumerable profile of Tesla units undergoing technical suppression.</param>
        /// <param name="duration">The target operational standby window in seconds. Defaults to 5.0 seconds matching standard facility protocols.</param>
        /// <param name="forceTrigger">If set to <c>true</c>, forcibly shocks the gate logic blocks to discharge current arcs before applying the timer envelope.</param>
        public static void Disable(this IEnumerable<Tesla> teslas, float duration = 5.0f, bool forceTrigger = false)
        {
            teslas.SetInactiveTimeForAll(duration, forceTrigger: forceTrigger);
        }

    }
}