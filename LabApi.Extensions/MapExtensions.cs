using LabApi.Features.Wrappers;
using System.Collections.Generic;

namespace LabApi.Extensions
{
    /// <summary>
    /// Highly optimized utility extensions for global facility operations: doors, lights, and generators.
    /// Employs early-exit (short-circuiting) logic and nested state-passing loops to guarantee 0 heap allocations.
    /// </summary>
    public static class MapExtensions
    {
        #region Doors

        /// <summary>
        /// Breaks all breakable doors in the entire facility with zero heap allocations.
        /// Reuses highly optimized room door extension.
        /// </summary>
        public static void BreakAllFacilityDoors()
        {
            if (Room.List == null)
                return;

            // FIX: DRY & Zero-Allocation loop utilizing compiled static delegate.
            Room.List.ForEach(static r => r?.BreakAllDoors());
        }

        #endregion

        #region Lights

        /// <summary>
        /// Enables or disables all lights in the facility with zero heap allocations.
        /// Uses nested state-passing to completely prevent double-closure garbage generation.
        /// </summary>
        public static void SetAllLightsEnabled(bool enabled)
        {
            if (Room.List == null)
                return;

            // FIX: Nested state-passing loops over the stack (no closures allocated).
            Room.List.ForEach(enabled, static (room, stateEnabled) =>
            {
                if (room != null && room.AllLightControllers != null)
                {
                    room.AllLightControllers.ForEach(stateEnabled, static (c, innerEnabled) =>
                    {
                        if (c != null)
                        {
                            c.LightsEnabled = innerEnabled;
                        }
                    });
                }
            });
        }

        #endregion

        #region Generators

        /// <summary>
        /// Returns the number of engaged generators.
        /// Optimized with list index lookup fast-path to prevent struct-boxing.
        /// </summary>
        public static int GetEngagedGeneratorsCount()
        {
            var generators = Generator.List;
            if (generators == null)
                return 0;

            // FIX: Zero-allocation fast-path for standard Lists.
            if (generators is List<Generator> list)
            {
                int count = 0;
                int len = list.Count;
                for (int i = 0; i < len; i++)
                {
                    var generator = list[i];
                    if (generator != null && generator.Engaged)
                    {
                        count++;
                    }
                }
                return count;
            }

            // Fallback for general collections
            int fallbackCount = 0;
            foreach (var generator in generators)
            {
                if (generator != null && generator.Engaged)
                {
                    fallbackCount++;
                }
            }

            return fallbackCount;
        }

        /// <summary>
        /// Returns true if all generators are engaged and the count meets the required minimum.
        /// Features highly optimized short-circuit logic to immediately exit on the first non-engaged generator.
        /// </summary>
        public static bool AreAllGeneratorsEngaged(int minimumRequiredCount = 3)
        {
            var generators = Generator.List;
            if (generators == null)
                return false;

            int total = generators.Count;

            // FIX: Instant early-exit if total generator count is below the minimum required.
            if (total < minimumRequiredCount)
                return false;

            // FIX: Direct index loop with early-exit. Stop scanning instantly upon hitting a disabled generator.
            if (generators is List<Generator> list)
            {
                for (int i = 0; i < total; i++)
                {
                    var generator = list[i];
                    if (generator == null || !generator.Engaged)
                        return false;
                }
                return true;
            }

            // Fallback for general collections with early-exit
            foreach (var generator in generators)
            {
                if (generator == null || !generator.Engaged)
                    return false;
            }

            return true;
        }

        #endregion
    }
}