using System.Collections.Generic;
using LabApi.Features.Wrappers;

namespace LabApi.Extensions
{
    /// <summary>
    /// Provides global enterprise-grade abstraction layers for macro infrastructure mutation, 
    /// facility-wide door structural degradation, and centralized lighting grid overrides.
    /// </summary>
    internal static class MapExtensions
    {
        /// <summary>
        /// Performs a comprehensive, high-performance iteration sweep across all active rooms to forcibly 
        /// shatter and destroy every structural <see cref="BreakableDoor"/> instance that is not currently broken.
        /// </summary>
        public static void BreakAllFacilityDoors()
        {
            foreach (Room room in Room.List)
            {
                if (room == null) continue;

                room.BreakAllDoors();
            }
        }

        /// <summary>
        /// Global shortcut matrix to instantly toggle the active illumination status field of every 
        /// light controller component deployed across the entire facility map topology.
        /// </summary>
        /// <param name="enabled">The target state value where <c>true</c> restores standard grid power and <c>false</c> triggers total facility blackout.</param>
        public static void SetAllLightsEnabled(bool enabled)
        {
            foreach (Room room in Room.List)
            {
                if (room?.AllLightControllers == null) continue;

                foreach (var lightController in room.AllLightControllers)
                {
                    if (lightController != null)
                    {
                        lightController.LightsEnabled = enabled;
                    }
                }
            }
        }

        /// <summary>
        /// Computes the exact real-time operational volume load metrics of facility power generators currently sitting in a fully engaged state.
        /// </summary>
        /// <returns>An integer scalar value indicating the total volume of active, fully engaged generator nodes across the facility floor grid.</returns>
        public static int GetEngagedGeneratorsCount()
        {
            int count = 0;
            foreach (var generator in Generator.List)
            {
                if (generator != null && generator.Engaged)
                {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// Evaluates with zero heap allocations whether all currently deployed power generator sub-units are fully engaged, 
        /// verifying metrics against a minimum structural compliance count.
        /// </summary>
        /// <param name="minimumRequiredCount">The baseline count of generator assets that must exist and be validated as engaged.</param>
        /// <returns><c>true</c> if the total engaged generator volume meets or exceeds the required count and no unengaged units are found; otherwise, <c>false</c>.</returns>
        public static bool AreAllGeneratorsEngaged(int minimumRequiredCount = 3)
        {
            var generators = Generator.List;
            if (generators == null || generators.Count < minimumRequiredCount)
            {
                return false;
            }

            foreach (var gen in generators)
            {
                if (gen == null || !gen.Engaged)
                {
                    return false;
                }
            }

            return true;
        }
    }
}