using LabApi.Features.Wrappers;
using MapGeneration;
using System.Collections.Generic;

namespace LabApi.Extensions
{
    /// <summary>
    /// Utility extensions for elevator door control and basic cabin state checks.
    /// Reuses core Door extensions to maintain a single source of truth and leverage O(1) instance caching.
    /// </summary>
    public static class ElevatorExtensions
    {
        #region Resolution
        /// <summary>
        /// Instantly resolves the target destination <see cref="FacilityZone"/> of this elevator sequence.
        /// </summary>
        /// <param name="elevator">The active elevator wrapper instance.</param>
        /// <returns>The destination <see cref="FacilityZone"/>; otherwise, <see cref="FacilityZone.None"/> if unresolved.</returns>
        public static FacilityZone GetZone(this Elevator elevator)
        {
            // FIX: One-liner optimization using null-conditional routing to bypass all loop and array allocations.
            return elevator?.CurrentDestination?.Zone ?? FacilityZone.None;
        }

        #endregion

        #region Active Floor Operations

        /// <summary>
        /// Opens only the doors located on the elevator's current floor.
        /// </summary>
        public static void OpenActiveDoors(this Elevator elevator, bool bypassLocks = false)
        {
            // FIX: Unified Unity lifetime check. Prevent operations on null or destroyed objects.
            if (elevator == null || elevator.Doors == null)
                return;

            // FIX: DRY & Zero-Allocation loop using state-passing and pre-cached Door extensions.
            elevator.Doors.ForEach(bypassLocks, static (door, bypass) =>
            {
                if (door != null && door.IsElevatorAtDoorLevel())
                {
                    door.Open(bypass);
                }
            });
        }

        /// <summary>
        /// Closes only the doors located on the elevator's current floor.
        /// </summary>
        public static void CloseActiveDoors(this Elevator elevator, bool bypassLocks = false)
        {
            if (elevator == null || elevator.Doors == null)
                return;

            // FIX: DRY & Zero-Allocation loop using state-passing and pre-cached Door extensions.
            elevator.Doors.ForEach(bypassLocks, static (door, bypass) =>
            {
                if (door != null && door.IsElevatorAtDoorLevel())
                {
                    door.Close(bypass);
                }
            });
        }

        #endregion

        #region Batch Operations (Zero-Allocation via State-Passing)

        /// <summary>
        /// Opens active-floor doors for multiple elevators.
        /// </summary>
        public static void OpenActiveDoors(this IEnumerable<Elevator> elevators, bool bypassLocks = false)
        {
            if (elevators == null)
                return;

            elevators.ForEach(bypassLocks, static (e, bypass) => e?.OpenActiveDoors(bypass));
        }

        /// <summary>
        /// Opens active-floor doors for multiple elevators (params overload).
        /// </summary>
        public static void OpenActiveDoors(bool bypassLocks, params Elevator[] elevators) =>
            elevators.OpenActiveDoors(bypassLocks);

        /// <summary>
        /// Closes active-floor doors for multiple elevators.
        /// </summary>
        public static void CloseActiveDoors(this IEnumerable<Elevator> elevators, bool bypassLocks = false)
        {
            if (elevators == null)
                return;

            elevators.ForEach(bypassLocks, static (e, bypass) => e?.CloseActiveDoors(bypass));
        }

        /// <summary>
        /// Closes active-floor doors for multiple elevators (params overload).
        /// </summary>
        public static void CloseActiveDoors(bool bypassLocks, params Elevator[] elevators) =>
            elevators.CloseActiveDoors(bypassLocks);

        #endregion
    }
}