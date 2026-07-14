using Interactables.Interobjects.DoorUtils;
using LabApi.Features.Enums;
using LabApi.Features.Wrappers;
using System;
using System.Collections.Generic;

namespace LabApi.Extensions
{
    /// <summary>
    /// Highly optimized extensions for working with doors and elevators. 
    /// Features JIT-cached state-passing batch operations and fast-path Unity component caching.
    /// </summary>
    public static class DoorExtensions
    {
        // High-performance cache maps using Instance IDs to avoid heavy Unity GC and GetComponent calls.
        private static readonly Dictionary<int, bool> IsElevatorCache = new();
        private static readonly Dictionary<int, Elevator> ElevatorCache = new();

        /// <summary>
        /// Explicitly clears the internal door and elevator caches.
        /// Must be called on round restart to prevent critical memory leaks of stale Unity objects.
        /// </summary>
        public static void ClearCache()
        {
            IsElevatorCache.Clear();
            ElevatorCache.Clear();
        }

        #region Queries

        /// <summary>
        /// Returns true if the door is open.
        /// </summary>
        public static bool IsOpen(this Door door) =>
            door != null && door.IsOpened;

        /// <summary>
        /// Returns only doors whose DoorName matches any of the provided names.
        /// </summary>
        /// <remarks>
        /// This is a query operation and will allocate an iterator state-machine under the hood.
        /// </remarks>
        public static IEnumerable<Door> WhereNameIn(this IEnumerable<Door> doors, params DoorName[] names)
        {
            if (doors == null || names == null || names.Length == 0)
                yield break;

            int count = names.Length;

            foreach (var door in doors)
            {
                if (door == null)
                    continue;

                for (int i = 0; i < count; i++)
                {
                    if (door.DoorName == names[i])
                    {
                        yield return door;
                        break;
                    }
                }
            }
        }

        #endregion

        #region Single Door Operations

        /// <summary>
        /// Opens the door.
        /// </summary>
        public static void Open(this Door door, bool bypassLocks = false) =>
            door.SetOpenState(true, bypassLocks);

        /// <summary>
        /// Closes the door.
        /// </summary>
        public static void Close(this Door door, bool bypassLocks = false) =>
            door.SetOpenState(false, bypassLocks);

        /// <summary>
        /// Toggles the door open/closed state. Respects locks by default.
        /// </summary>
        public static void Toggle(this Door door, bool bypassLocks = false)
        {
            if (door != null)
                door.SetOpenState(!door.IsOpened, bypassLocks);
        }

        /// <summary>
        /// Sets or removes a lock reason on the door.
        /// </summary>
        public static void SetLockState(this Door door, DoorLockReason reason, bool locked = true)
        {
            if (door == null)
                return;

            door.Lock(reason, locked);

            if (!locked)
                door.CheckAndRestoreElevatorDoorState();
        }

        /// <summary>
        /// Sets the open/closed state of the door. This is the single source of truth for door state changes.
        /// </summary>
        public static void SetOpenState(this Door door, bool opened, bool bypassLocks = false)
        {
            if (door == null)
                return;

            if (door.IsLocked && !bypassLocks)
                return;

            if (opened && door.IsElevatorDoor() && !door.IsElevatorAtDoorLevel())
                return;

            door.IsOpened = opened;
        }

        /// <summary>
        /// Opens the door (if safe) and immediately applies a lock reason.
        /// </summary>
        public static void OpenAndLock(this Door door, DoorLockReason reason = DoorLockReason.AdminCommand, bool playSound = true)
        {
            if (door == null)
                return;

            bool safeToOpen = !door.IsElevatorDoor() || door.IsElevatorAtDoorLevel();

            door.SetOpenState(safeToOpen, bypassLocks: true);

            if (safeToOpen && playSound)
            {
                door.PlayLockBypassDeniedSound();
            }

            door.Lock(reason, true);
        }

        #endregion

        #region Batch Operations (True Zero-Allocation Paths via State-Passing)

        /// <summary>
        /// Opens all doors in the collection with zero heap allocations.
        /// </summary>
        public static void Open(this IEnumerable<Door> doors, bool bypassLocks = false)
        {
            if (doors == null)
                return;

            doors.ForEach(bypassLocks, static (d, bypass) => d?.Open(bypass));
        }

        /// <summary>
        /// Opens all provided doors.
        /// </summary>
        public static void Open(bool bypassLocks, params Door[] doors) =>
            doors.Open(bypassLocks);

        /// <summary>
        /// Closes all doors in the collection with zero heap allocations.
        /// </summary>
        public static void Close(this IEnumerable<Door> doors, bool bypassLocks = false)
        {
            if (doors == null)
                return;

            doors.ForEach(bypassLocks, static (d, bypass) => d?.Close(bypass));
        }

        /// <summary>
        /// Closes all provided doors.
        /// </summary>
        public static void Close(bool bypassLocks, params Door[] doors) =>
            doors.Close(bypassLocks);

        /// <summary>
        /// Sets lock state for all doors in the collection with zero heap allocations.
        /// </summary>
        public static void SetLockState(this IEnumerable<Door> doors, DoorLockReason reason = DoorLockReason.AdminCommand, bool locked = true)
        {
            if (doors == null)
                return;

            doors.ForEach((reason, locked), static (d, state) => d?.SetLockState(state.reason, state.locked));
        }

        /// <summary>
        /// Sets lock state for all provided doors.
        /// </summary>
        public static void SetLockState(DoorLockReason reason, bool locked, params Door[] doors) =>
            doors.SetLockState(reason, locked);

        /// <summary>
        /// Sets open/closed state for all doors in the collection with zero heap allocations.
        /// </summary>
        public static void SetOpenState(this IEnumerable<Door> doors, bool opened, bool bypassLocks = false)
        {
            if (doors == null)
                return;

            doors.ForEach((opened, bypassLocks), static (d, state) => d?.SetOpenState(state.opened, state.bypassLocks));
        }

        /// <summary>
        /// Sets open/closed state for all provided doors.
        /// </summary>
        public static void SetOpenState(bool opened, bool bypassLocks, params Door[] doors) =>
            doors.SetOpenState(opened, bypassLocks);

        /// <summary>
        /// Opens all doors and applies a lock reason with zero heap allocations.
        /// </summary>
        public static void OpenAndLock(this IEnumerable<Door> doors, DoorLockReason reason, bool playSound = true)
        {
            if (doors == null)
                return;

            doors.ForEach((reason, playSound), static (d, state) => d?.OpenAndLock(state.reason, state.playSound));
        }

        /// <summary>
        /// Opens all provided doors and applies a lock reason.
        /// </summary>
        public static void OpenAndLock(DoorLockReason reason, bool playSound, params Door[] doors) =>
            doors.OpenAndLock(reason, playSound);

        #endregion

        #region Elevator Identity

        /// <summary>
        /// Returns the elevator associated with this door. Optimizes search paths and caches results.
        /// </summary>
        public static Elevator GetElevator(this Door door)
        {
            if (door == null)
                return null;

            if (door is ElevatorDoor ed)
                return ed.Elevator;

            if (door.GameObject == null)
                return null;

            int instanceId = door.GameObject.GetInstanceID();

            if (ElevatorCache.TryGetValue(instanceId, out var cachedElevator))
                return cachedElevator;

            Elevator resolved = null;
            if (door.GameObject.TryGetComponent<Interactables.Interobjects.ElevatorDoor>(out var nativeDoor))
            {
                var elevators = Elevator.GetByGroup(nativeDoor.Group);
                if (elevators != null)
                {
                    if (elevators is List<Elevator> list && list.Count > 0)
                    {
                        resolved = list[0];
                    }
                    else if (elevators is Elevator[] array && array.Length > 0)
                    {
                        resolved = array[0];
                    }
                    else
                    {
                        foreach (var el in elevators)
                        {
                            resolved = el;
                            break;
                        }
                    }
                }
            }

            ElevatorCache[instanceId] = resolved;
            return resolved;
        }

        /// <summary>
        /// Returns true if the door is an elevator door. Caches the result to bypass native Unity calls.
        /// </summary>
        public static bool IsElevatorDoor(this Door door)
        {
            if (door == null)
                return false;

            if (door is ElevatorDoor)
                return true;

            if (door.GameObject == null)
                return false;

            int instanceId = door.GameObject.GetInstanceID();

            if (IsElevatorCache.TryGetValue(instanceId, out bool isElevator))
                return isElevator;

            bool resolved = door.GameObject.GetComponent<Interactables.Interobjects.ElevatorDoor>() != null;
            IsElevatorCache[instanceId] = resolved;
            return resolved;
        }

        /// <summary>
        /// Returns true if the elevator cabin is aligned with the door's floor level.
        /// </summary>
        public static bool IsElevatorAtDoorLevel(this Door door)
        {
            var elevator = door.GetElevator();
            if (elevator?.Base == null)
                return false;

            float delta = Math.Abs(door.Position.y - elevator.Base.transform.position.y);
            return delta <= 3.5f;
        }

        /// <summary>
        /// Restores elevator door state after lock removal.
        /// </summary>
        public static void CheckAndRestoreElevatorDoorState(this Door door)
        {
            if (door == null)
                return;

            if (door.IsElevatorDoor())
            {
                bool atLevel = door.IsElevatorAtDoorLevel();
                door.SetOpenState(atLevel, bypassLocks: !atLevel);
            }
        }

        #endregion

        #region Gate Identity
        /// <summary>
        /// Returns true if the door is a heavy gate.
        /// </summary>
        public static bool IsGate(this Door door) =>
            door is Gate;

        #endregion
    }
}