using Interactables.Interobjects.DoorUtils;
using LabApi.Features.Enums;
using LabApi.Features.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LabApi.Extensions
{
    /// <summary>
    /// Extensions for working with doors. Clear intent, predictable behavior, zero allocation in batch operations.
    /// </summary>
    public static class DoorExtensions
    {

        #region Queries

        /// <summary>
        /// Returns true if the door is open.
        /// </summary>
        public static bool IsOpen(this Door door) =>
            door != null && door.IsOpened;

        /// <summary>
        /// Returns only doors whose DoorName matches any of the provided names.
        /// </summary>
        public static IEnumerable<Door> WhereNameIn(this IEnumerable<Door> doors, params DoorName[] names)
        {
            if (doors == null || names == null || names.Length == 0)
                yield break;

            int count = names.Length;

            foreach (var door in doors)
            {
                if (door == null) continue;

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
        /// Toggles the door open/closed state.
        /// </summary>
        public static void Toggle(this Door door)
        {
            if (door != null)
                door.SetOpenState(!door.IsOpened, bypassLocks: true);
        }

        /// <summary>
        /// Sets or removes a lock reason on the door.
        /// </summary>
        public static void SetLockState(this Door door, DoorLockReason reason, bool locked = true)
        {
            if (door == null) return;

            door.Lock(reason, locked);

            if (!locked)
                door.CheckAndRestoreElevatorDoorState();
        }

        /// <summary>
        /// Sets the open/closed state of the door. This is the single source of truth for door state changes.
        /// </summary>
        public static void SetOpenState(this Door door, bool opened, bool bypassLocks = false)
        {
            if (door == null) return;
            if (door.IsLocked && !bypassLocks) return;

            if (opened && door.IsElevatorDoor() && !door.IsElevatorAtDoorLevel())
                return;

            door.IsOpened = opened;
        }

        /// <summary>
        /// Opens the door (if safe) and immediately applies a lock reason.
        /// </summary>
        public static void OpenAndLock(this Door door, DoorLockReason reason, bool playSound = true)
        {
            if (door == null) return;

            bool safeToOpen = !door.IsElevatorDoor() || door.IsElevatorAtDoorLevel();

            door.SetOpenState(safeToOpen, bypassLocks: true);

            if (safeToOpen && playSound)
                door.PlayLockBypassDeniedSound();

            door.Lock(reason, true);
        }

        #endregion

        #region Batch Operations (Lambda-Based, Zero Allocation)

        /// <summary>
        /// Opens all doors.
        /// </summary>
        public static void Open(this IEnumerable<Door> doors, bool bypassLocks = false) =>
            doors.ForEach(d => d?.Open(bypassLocks));

        /// <summary>
        /// Opens all provided doors.
        /// </summary>
        public static void Open(bool bypassLocks, params Door[] doors) =>
            ((IEnumerable<Door>)doors).Open(bypassLocks);

        /// <summary>
        /// Closes all doors.
        /// </summary>
        public static void Close(this IEnumerable<Door> doors, bool bypassLocks = false) =>
            doors.ForEach(d => d?.Close(bypassLocks));

        /// <summary>
        /// Closes all provided doors.
        /// </summary>
        public static void Close(bool bypassLocks, params Door[] doors) =>
            ((IEnumerable<Door>)doors).Close(bypassLocks);

        /// <summary>
        /// Sets lock state for all doors.
        /// </summary>
        public static void SetLockState(this IEnumerable<Door> doors, DoorLockReason reason, bool locked = true) =>
            doors.ForEach(d => d?.SetLockState(reason, locked));

        /// <summary>
        /// Sets lock state for all provided doors.
        /// </summary>
        public static void SetLockState(DoorLockReason reason, bool locked, params Door[] doors) =>
            ((IEnumerable<Door>)doors).SetLockState(reason, locked);

        /// <summary>
        /// Sets open/closed state for all doors.
        /// </summary>
        public static void SetOpenState(this IEnumerable<Door> doors, bool opened, bool bypassLocks = false) =>
            doors.ForEach(d => d?.SetOpenState(opened, bypassLocks));

        /// <summary>
        /// Sets open/closed state for all provided doors.
        /// </summary>
        public static void SetOpenState(bool opened, bool bypassLocks, params Door[] doors) =>
            ((IEnumerable<Door>)doors).SetOpenState(opened, bypassLocks);

        /// <summary>
        /// Opens all doors and applies a lock reason.
        /// </summary>
        public static void OpenAndLock(this IEnumerable<Door> doors, DoorLockReason reason, bool playSound = true) =>
            doors.ForEach(d => d?.OpenAndLock(reason, playSound));

        /// <summary>
        /// Opens all provided doors and applies a lock reason.
        /// </summary>
        public static void OpenAndLock(DoorLockReason reason, bool playSound, params Door[] doors) =>
            ((IEnumerable<Door>)doors).OpenAndLock(reason, playSound);

        #endregion

        #region Elevator Identity

        /// <summary>
        /// Returns the elevator associated with this door.
        /// </summary>
        public static Elevator GetElevator(this Door door)
        {
            if (door == null)
                return null;

            if (door is ElevatorDoor ed)
                return ed.Elevator;

            if (door.GameObject != null &&
                door.GameObject.TryGetComponent<Interactables.Interobjects.ElevatorDoor>(out var nativeDoor))
            {
                return Elevator.GetByGroup(nativeDoor.Group)?.FirstOrDefault();
            }

            return null;
        }

        /// <summary>
        /// Returns true if the door is an elevator door.
        /// </summary>
        public static bool IsElevatorDoor(this Door door)
        {
            if (door == null) return false;
            if (door is ElevatorDoor) return true;

            return door.GameObject != null &&
                   door.GameObject.GetComponent<Interactables.Interobjects.ElevatorDoor>() != null;
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
            if (door == null) return;

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
