using LabApi.Features.Wrappers;

namespace LabApi.Extensions
{
    /// <summary>
    /// Provides enterprise-grade extension methods for <see cref="Door"/> wrappers, 
    /// optimizing real-time state mutations and native Unity component reflection lookup heuristics.
    /// </summary>
    internal static class DoorExtensions
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
                door.IsOpened = !door.IsOpened;
            }
        }

        /// <summary>
        /// Filters a sequence of doors to return only those matching any of the specified <see cref="DoorName"/> layout tokens.
        /// </summary>
        /// <param name="doors">The source collection of doors evaluated for identity matching.</param>
        /// <param name="names">The structural door identifier criteria context matrix used for filtering.</param>
        /// <returns>A filtered enumerable sequence layout yielding matching door entities.</returns>
        public static System.Collections.Generic.IEnumerable<Door> WhereNameIn(this System.Collections.Generic.IEnumerable<Door> doors, params DoorName[] names)
        {
            if (doors == null || names == null || names.Length == 0)
            {
                return System.Linq.Enumerable.Empty<Door>();
            }

            System.Collections.Generic.List<Door> filtered = new System.Collections.Generic.List<Door>();
            foreach (Door door in doors)
            {
                if (door == null) continue;

                foreach (DoorName name in names)
                {
                    if (door.DoorName == name)
                    {
                        filtered.Add(door);
                        break;
                    }
                }
            }
            return filtered;
        }

        #region Single Door Operations

        /// <summary>
        /// Updates the administrative server-side lock state of an individual door instance under a specific constraint reason.
        /// </summary>
        /// <param name="door">The target door instance undergoing lock state modification.</param>
        /// <param name="reason">The specific structural <see cref="DoorLockReason"/> constraint token applied or removed.</param>
        /// <param name="locked">If set to <c>true</c>, forcibly engages the lock; if <c>false</c>, releases the specified lock reason constraint.</param>
        public static void SetLockState(this Door door, DoorLockReason reason, bool locked = true)
        {
            door?.Lock(reason, locked);
        }

        /// <summary>
        /// Mutates the passage activation status (Open or Closed topology) of an individual door instance.
        /// </summary>
        /// <param name="door">The target door instance undergoing passage status modification.</param>
        /// <param name="opened">If set to <c>true</c>, attempts to unseal the door; if <c>false</c>, forces it to close.</param>
        /// <param name="bypassLocks">If set to <c>true</c>, forces the state mutation even if the door is currently locked.</param>
        public static void SetOpenState(this Door door, bool opened, bool bypassLocks = false)
        {
            if (door is null) return;

            // Corrected logic: If the door is locked and we are NOT bypassing locks, abort execution.
            if (door.IsLocked && !bypassLocks) return;

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

            door.IsOpened = true;
            if (playSound)
            {
                door.PlayLockBypassDeniedSound();
            }
            door.Lock(reason, true);
        }

        #endregion

        #region Batch Collection Operations

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
                // Reusing the single-target execution logic to prevent code duplication
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
                // Reusing the single-target execution logic to guarantee structural consistency
                door.SetOpenState(opened, bypassLocks);
            }
        }

        /// <summary>
        /// Forcibly unseals a collection layout of doors and applies an administrative server-side lock state under a specific structural reason constraint.
        /// </summary>
        /// <param name="doors">The targeted enumerable collection of doors undergoing bulk state mutations.</param>
        /// <param name="reason">The underlying system internal reason token driving the mechanical lockdown registration.</param>
        /// <param name="playSound">If set to <c>true</c>, forces the target door modules to trigger their diagnostic lock bypass denied audio cue.</param>
        public static void OpenAndLock(this IEnumerable<Door> doors, DoorLockReason reason, bool playSound = true)
        {
            if (doors is null) return;

            foreach (Door door in doors)
            {
                // Reusing the single-target structural framework layer natively
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
            if (door?.GameObject == null) return false;

            return door.GameObject.name.Contains("Elevator") ||
                   door.GameObject.GetComponentInParent<Interactables.Interobjects.ElevatorDoor>() != null;
        }

        /// <summary>
        /// Scans the underlying engine transform string identifiers to evaluate if the targeted runtime door asset 
        /// is classified as a heavy checkpoint airlock Gate structure.
        /// </summary>
        /// <param name="door">The target <see cref="Door"/> instance targeted for transform layout identification.</param>
        /// <returns><c>true</c> if the structural identity maps directly to facility Gate systems; otherwise, <c>false</c>.</returns>
        public static bool IsGate(this Door door)
        {
            if (door?.GameObject == null) return false;

            return door.GameObject.name.Contains("Gate");
        }
    }
}