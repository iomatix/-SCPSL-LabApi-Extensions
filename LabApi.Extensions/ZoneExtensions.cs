using Interactables.Interobjects.DoorUtils;
using LabApi.Features.Wrappers;
using MapGeneration;
using System;
using System.Collections.Generic;

namespace LabApi.Extensions
{
    /// <summary>
    /// Highly optimized utility extensions for working with <see cref="FacilityZone"/>:
    /// doors, elevator locks and zone registry.
    /// Employs direct state-passing loops to bypass iterator allocations entirely.
    /// </summary>
    public static class ZoneExtensions
    {
        #region Zone Registry

        /// <summary>
        /// Cached array of all facility zones (zero enum allocation).
        /// </summary>
        public static readonly FacilityZone[] All = (FacilityZone[])Enum.GetValues(typeof(FacilityZone));

        #endregion

        #region Helper Detection (Internal & Zero-Allocation)

        /// <summary>
        /// Helper to check if an elevator belongs to a specific zone without generating heap garbage.
        /// Shared internally with ZoneLightingExtensions to maintain DRY.
        /// </summary>
        internal static bool IsElevatorInZone(Elevator elevator, FacilityZone zone)
        {
            if (elevator == null || elevator.CurrentDestination?.Rooms == null)
                return false;

            // FIX: Using struct enumerator over Rooms collection to achieve true 0-allocation detection.
            foreach (var r in elevator.CurrentDestination.Rooms)
            {
                if (r != null && Room.Get(r.Base)?.Zone == zone)
                    return true;
            }

            return false;
        }

        #endregion

        #region Door Operations

        /// <summary>
        /// Returns all doors located inside the zone.
        /// </summary>
        [Obsolete("GetDoors() allocates heap memory due to iterator state machines. For batch operations, use ZoneExtensions.OpenDoors(), CloseDoors(), or SetDoorsLockState() directly to achieve zero allocations.", false)]
        public static IEnumerable<Door> GetDoors(this FacilityZone zone)
        {
            if (Room.List == null)
                yield break;

            foreach (var room in Room.List)
            {
                if (room == null || room.Zone != zone || room.Doors == null)
                    continue;

                foreach (var door in room.Doors)
                {
                    if (door != null)
                        yield return door;
                }
            }
        }

        /// <summary>
        /// Opens all doors in the zone with zero heap allocations.
        /// </summary>
        public static void OpenDoors(this FacilityZone zone, bool bypassLocks = false)
        {
            if (Room.List == null)
                return;

            // FIX: Direct loop over Room.List bypassing GetDoors() iterator allocation entirely.
            Room.List.ForEach((zone, bypassLocks), static (room, state) =>
            {
                if (room == null || room.Zone != state.zone || room.Doors == null)
                    return;

                room.Doors.ForEach(state.bypassLocks, static (door, bypass) =>
                {
                    if (door != null)
                    {
                        door.Open(bypass);
                    }
                });
            });
        }

        /// <summary>
        /// Closes all doors in the zone with zero heap allocations.
        /// </summary>
        public static void CloseDoors(this FacilityZone zone, bool bypassLocks = false)
        {
            if (Room.List == null)
                return;

            // FIX: Direct loop over Room.List bypassing GetDoors() iterator allocation entirely.
            Room.List.ForEach((zone, bypassLocks), static (room, state) =>
            {
                if (room == null || room.Zone != state.zone || room.Doors == null)
                    return;

                room.Doors.ForEach(state.bypassLocks, static (door, bypass) =>
                {
                    if (door != null)
                    {
                        door.Close(bypass);
                    }
                });
            });
        }

        /// <summary>
        /// Sets the lock state of all doors in the zone with zero heap allocations.
        /// Use <paramref name="locked"/> = false to unlock them.
        /// </summary>
        public static void SetDoorsLockState(this FacilityZone zone, DoorLockReason reason, bool locked = true)
        {
            if (Room.List == null)
                return;

            // FIX: Direct loop over Room.List bypassing GetDoors() iterator allocation entirely.
            Room.List.ForEach((zone, reason, locked), static (room, state) =>
            {
                if (room == null || room.Zone != state.zone || room.Doors == null)
                    return;

                room.Doors.ForEach((state.reason, state.locked), static (door, s) =>
                {
                    if (door != null)
                    {
                        door.SetLockState(s.reason, s.locked);
                    }
                });
            });
        }

        /// <summary>
        /// Locks all elevators in the zone with zero heap allocations.
        /// </summary>
        public static void LockElevators(this FacilityZone zone)
        {
            if (Elevator.List == null)
                return;

            // FIX: Zero-allocation direct lookup and lock invocation.
            Elevator.List.ForEach(zone, static (e, z) =>
            {
                if (e != null && IsElevatorInZone(e, z))
                {
                    e.LockAllDoors();
                }
            });
        }

        public static void LockElevatorsInZone(FacilityZone zone)
            => zone.LockElevators();

        /// <summary>
        /// Unlocks all elevators in the zone with zero heap allocations.
        /// </summary>
        public static void UnlockElevators(this FacilityZone zone)
        {
            if (Elevator.List == null)
                return;

            // FIX: Zero-allocation direct lookup and unlock invocation.
            Elevator.List.ForEach(zone, static (e, z) =>
            {
                if (e != null && IsElevatorInZone(e, z))
                {
                    e.UnlockAllDoors();
                }
            });
        }

        public static void UnlockElevatorsInZone(FacilityZone zone)
            => zone.UnlockElevators();

        #endregion

        #region Elevator Operations

        /// <summary>
        /// Returns all elevators whose destination rooms belong to the given zone.
        /// </summary>
        [Obsolete("GetElevators() allocates heap memory due to iterator state machines. For batch operations, use ZoneExtensions.LockElevators() or ZoneLightingExtensions.TurnOffLights() directly to achieve zero allocations.", false)]
        public static IEnumerable<Elevator> GetElevators(this FacilityZone zone)
        {
            if (Elevator.List == null)
                yield break;

            foreach (var elevator in Elevator.List)
            {
                if (elevator != null && IsElevatorInZone(elevator, zone))
                {
                    yield return elevator;
                }
            }
        }

        /// <summary>
        /// Returns all elevators whose destination rooms belong to the given zone.
        /// </summary>
        [Obsolete("GetElevatorsInZone() allocates heap memory. Use ZoneExtensions.LockElevators() or ZoneLightingExtensions.TurnOffLights() directly to achieve zero allocations.", false)]
        public static IEnumerable<Elevator> GetElevatorsInZone(FacilityZone zone)
            => zone.GetElevators();

        #endregion
    }
}