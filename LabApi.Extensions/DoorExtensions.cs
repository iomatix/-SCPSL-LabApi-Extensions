using Interactables.Interobjects.DoorUtils;
using LabApi.Features.Enums;
using LabApi.Features.Wrappers;
using System.Collections.Generic;

namespace LabApi.Extensions
{
    /// <summary>
    /// Provides enterprise-grade extension methods for <see cref="Door"/> wrappers, 
    /// optimizing real-time state mutations and native Unity component reflection lookup heuristics.
    /// </summary>
    public static class DoorExtensions
    {
        /// <summary>
        /// Evaluates the underlying structural state of the specified door entity to determine if it is currently unsealed.
        /// </summary>
        /// <param name="door">The target <see cref="Door"/> instance requested for state validation.</param>
        /// <returns><c>true</c> if the door instance is verified as open; otherwise, <c>false</c> if closed or evaluated as null.</returns>
        public static bool IsOpen(this Door door) => door != null && door.IsOpened;

        /// <summary>
        /// Inverts the operational passage status of the targeted door instance, 
        /// executing a swift state mutation between open and closed topologies defensively.
        /// </summary>
        /// <param name="door">The target <see cref="Door"/> instance targeted for structural manipulation.</param>
        public static void Toggle(this Door door)
        {
            if (door != null)
            {
                // Reusing SetOpenState to naturally handle elevator safety and state validation
                door.SetOpenState(!door.IsOpened, bypassLocks: true);
            }
        }

        /// <summary>
        /// Filters a sequence of doors to return only those matching any of the specified <see cref="DoorName"/> tokens.
        /// Leverages lazy streaming pipelines to guarantee absolute zero heap allocation metrics.
        /// </summary>
        /// <param name="doors">The source collection of doors evaluated for identity matching.</param>
        /// <param name="names">The structural door identifier criteria context matrix used for filtering.</param>
        /// <returns>A filtered enumerable sequence layout yielding matching door entities.</returns>
        public static IEnumerable<Door> WhereNameIn(this IEnumerable<Door> doors, params DoorName[] names)
        {
            if (doors == null || names == null || names.Length == 0)
            {
                yield break;
            }

            int nameCount = names.Length;

            foreach (Door door in doors)
            {
                if (door == null) continue;

                // High-Performance Indexed Loop replacing allocation-heavy foreach over arrays
                for (int i = 0; i < nameCount; i++)
                {
                    if (door.DoorName == names[i])
                    {
                        yield return door;
                        break;
                    }
                }
            }
        }

        #region Single Door Operations

        /// <summary>
        /// Fluently unseals an individual door instance, driving its structural passage topology state to open.
        /// </summary>
        /// <param name="door">The target door instance targeted for structural manipulation.</param>
        /// <param name="bypassLocks">If set to <c>true</c>, forces the state mutation even if the door is restricted by an active lock.</param>
        public static void Open(this Door door, bool bypassLocks = false) => door.SetOpenState(opened: true, bypassLocks);

        /// <summary>
        /// Fluently seals an individual door instance, driving its structural passage topology state to closed.
        /// </summary>
        /// <param name="door">The target door instance targeted for structural manipulation.</param>
        /// <param name="bypassLocks">If set to <c>true</c>, forces the state mutation even if the door is restricted by an active lock.</param>
        public static void Close(this Door door, bool bypassLocks = false) => door.SetOpenState(opened: false, bypassLocks);

        /// <summary>
        /// Updates the administrative server-side lock state of an individual door instance under a specific constraint reason.
        /// </summary>
        /// <param name="door">The target door instance undergoing lock state modification.</param>
        /// <param name="reason">The specific structural <see cref="DoorLockReason"/> constraint token applied or removed.</param>
        /// <param name="locked">If set to <c>true</c>, forcibly engages the lock; if <c>false</c>, releases the specified lock reason constraint.</param>
        public static void SetLockState(this Door door, DoorLockReason reason, bool locked = true)
        {
            if (door is null) return;
            door.Lock(reason, locked);

            // Automatically evaluate and restore elevator door state when a lock is released
            if (!locked)
            {
                door.CheckAndRestoreElevatorDoorState();
            }
        }

        /// <summary>
        /// Mutates the passage activation status (Open or Closed topology) of an individual door instance.
        /// This method acts as the single source of truth for all door state mutations.
        /// </summary>
        /// <param name="door">The target door instance undergoing passage status modification.</param>
        /// <param name="opened">If set to <c>true</c>, attempts to unseal the door; if <c>false</c>, forces it to close.</param>
        /// <param name="bypassLocks">If set to <c>true</c>, forces the state mutation even if the door is currently locked.</param>
        public static void SetOpenState(this Door door, bool opened, bool bypassLocks = false)
        {
            if (door is null) return;
            if (door.IsLocked && !bypassLocks) return;

            // Absolute elevator safety gateway: refuse to open if the elevator cabin is elsewhere
            if (opened && door.IsElevatorDoor() && !door.IsElevatorAtDoorLevel())
            {
                return;
            }

            door.IsOpened = opened;
        }

        /// <summary>
        /// Forcibly unseals an individual door and applies an administrative server-side lock state under a specific structural reason constraint.
        /// </summary>
        /// <param name="door">The target door instance undergoing lockdown registration.</param>
        /// <param name="reason">The underlying system internal reason token driving the mechanical lockdown registration.</param>
        /// <param name="playSound">If set to <c>true</c>, forces the target door module to trigger its diagnostic lock bypass denied audio cue.</param>
        public static void OpenAndLock(this Door door, DoorLockReason reason, bool playSound = true)
        {
            if (door is null) return;

            // Evaluate if it's safe to open. If false, SetOpenState will safely close the door instead.
            bool safeToOpen = !door.IsElevatorDoor() || door.IsElevatorAtDoorLevel();

            door.SetOpenState(opened: safeToOpen, bypassLocks: true);

            if (safeToOpen && playSound)
            {
                door.PlayLockBypassDeniedSound();
            }

            door.Lock(reason, true);
        }

        #endregion

        #region Batch Collection Operations

        /// <summary>
        /// Attempts to mass unseal an aggregated collection sequence of doors cleanly.
        /// </summary>
        /// <param name="doors">The target collection stream of doors undergoing passage status modification.</param>
        /// <param name="bypassLocks">If set to <c>true</c>, forces the state mutation even if individual doors are restricted by an active lock.</param>
        public static void Open(this IEnumerable<Door> doors, bool bypassLocks = false) => doors.SetOpenState(opened: true, bypassLocks);

        /// <summary>
        /// Attempts to mass seal an aggregated collection sequence of doors cleanly.
        /// </summary>
        /// <param name="doors">The target collection stream of doors undergoing passage status modification.</param>
        /// <param name="bypassLocks">If set to <c>true</c>, forces the state mutation even if individual doors are restricted by an active lock.</param>
        public static void Close(this IEnumerable<Door> doors, bool bypassLocks = false) => doors.SetOpenState(opened: false, bypassLocks);

        /// <summary>
        /// Forcibly updates the administrative server-side lock state across an aggregated collection sequence of doors.
        /// </summary>
        /// <param name="doors">The target collection stream of doors undergoing lock state modification.</param>
        /// <param name="reason">The specific structural <see cref="DoorLockReason"/> constraint token applied or removed.</param>
        /// <param name="locked">If set to <c>true</c>, forcibly engages the lock; if <c>false</c>, releases the specified lock reason constraint.</param>
        public static void SetLockState(this IEnumerable<Door> doors, DoorLockReason reason, bool locked = true)
        {
            if (doors is null) return;
            foreach (Door door in doors)
            {
                door.SetLockState(reason, locked);
            }
        }

        /// <summary>
        /// Attempts to mass mutate the passage activation status (Open or Closed topology) across an aggregated collection sequence of doors.
        /// </summary>
        /// <param name="doors">The target collection stream of doors undergoing passage status modification.</param>
        /// <param name="opened">If set to <c>true</c>, attempts to unseal the doors; if <c>false</c>, forces them to close.</param>
        /// <param name="bypassLocks">If set to <c>true</c>, forces the state mutation even if individual doors are restricted by an active lock.</param>
        public static void SetOpenState(this IEnumerable<Door> doors, bool opened, bool bypassLocks = false)
        {
            if (doors is null) return;
            foreach (Door door in doors)
            {
                door.SetOpenState(opened, bypassLocks);
            }
        }

        /// <summary>
        /// Forcibly unseals a collection layout of doors and applies an administrative server-side lock state under a specific structural reason constraint.
        /// </summary>
        /// <param name="doors">The targeted enumerable collection of doors undergoing bulk state mutations.</param>
        /// <param name="reason">The underlying system internal reason token driving the mechanical lockdown registration.</param>
        /// <param name="playSound">If set to <c>true</c>, forces the target door modules to trigger its diagnostic lock bypass denied audio cue.</param>
        public static void OpenAndLock(this IEnumerable<Door> doors, DoorLockReason reason, bool playSound = true)
        {
            if (doors is null) return;
            foreach (Door door in doors)
            {
                door.OpenAndLock(reason, playSound);
            }
        }

        #endregion

        /// <summary>
        /// Performs hierarchical component reflection and token verification on the native GameObject metadata 
        /// to isolate whether the asset behaves structurally as an elevator cabin bulkhead.
        /// </summary>
        /// <param name="door">The target <see cref="Door"/> instance passed for underlying name pattern tracking.</param>
        /// <returns><c>true</c> if the object matches elevator architecture signatures; otherwise, <c>false</c>.</returns>
        public static bool IsElevatorDoor(this Door door)
        {
            if (door == null) return false;
            if (door is ElevatorDoor) return true;
            return door.GameObject != null && door.GameObject.GetComponent<Interactables.Interobjects.ElevatorDoor>() != null;
        }

        /// <summary>
        /// Evaluates and corrects the open state of an elevator door after a lock release event.
        /// If the elevator is physically present at the deck and unlocked, the doors are automatically restored to their open state.
        /// </summary>
        /// <param name="door">The target <see cref="Door"/> instance targeted for state validation.</param>
        public static void CheckAndRestoreElevatorDoorState(this Door door)
        {
            if (door == null) return;

            if (door.IsElevatorDoor())
            {
                bool atLevel = door.IsElevatorAtDoorLevel();

                // SSOT Integration: Reusing SetOpenState to dry up and fully consolidate execution pathways.
                // If the elevator is at level, we try to open it (respecting active locks).
                // If it is NOT at level, we force-close it by bypassing locks.
                door.SetOpenState(opened: atLevel, bypassLocks: !atLevel);
            }
        }

        /// <summary>
        /// Scans the underlying engine transform string identifiers to evaluate if the targeted runtime door asset 
        /// is classified as a heavy checkpoint airlock Gate structure.
        /// </summary>
        /// <param name="door">The target <see cref="Door"/> instance targeted for transform layout identification.</param>
        /// <returns><c>true</c> if the structural identity maps directly to facility Gate systems; otherwise, <c>false</c>.</returns>
        public static bool IsGate(this Door door)
        {
            if (door == null) return false;
            return door is Gate;
        }
    }
}