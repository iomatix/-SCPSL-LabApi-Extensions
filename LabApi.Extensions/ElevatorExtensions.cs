using LabApi.Extensions.Misc;
using LabApi.Features.Wrappers;
using MapGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LabApi.Extensions
{
    /// <summary>
    /// Provides enterprise-grade abstraction query layers, real-time spatial lookup matrices, 
    /// and automated locking hooks for server-side <see cref="Elevator"/> components.
    /// </summary>
    internal static class ElevatorExtensions
    {
        #region Illumination Registry & State Trackers
        /// <summary>
        /// Thread-safe runtime registry storing instances of elevators currently trapped in total darkness
        /// </summary>
        private static readonly HashSet<int> _blackedOutElevators = new();
        #endregion

        #region Elevator Illumination Matrix (Anti-Desync Engine)
        /// <summary>
        /// Forcibly suppresses all physical and software-controlled light emission vectors inside the elevator cabin for a specific duration.
        /// </summary>
        /// <param name="elevator">The target <see cref="Elevator"/> instance undergoing cabin blackout.</param>
        /// <param name="duration">The chronological duration window in seconds during which the cabin remains pitch black.</param>
        public static void TurnOffLights(this Elevator elevator, float duration)
        {
            if (elevator?.Base == null) return;
            int instanceId = elevator.Base.GetInstanceID();

            lock (_blackedOutElevators)
            {
                _blackedOutElevators.Add(instanceId);
            }

            // Client-side visual sync: Deep-search and disable all rendering light components inside the active chamber structure
            try
            {
                foreach (Light light in elevator.Base.GetComponentsInChildren<Light>(true))
                {
                    light.enabled = false;
                }
                foreach (MonoBehaviour fLight in elevator.Base.GetComponentsInChildren<MonoBehaviour>(true))
                {
                    // Targets native game FlickerableLight components dynamically without tight assembly coupling
                    if (fLight.GetType().Name == "FlickerableLight")
                    {
                        fLight.enabled = false;
                    }
                }
            }
            catch { /* Defensive safety isolation barrier */ }

            // Automated thread-safe power restoration pipeline loop using MEC delay tokens
            MEC.Timing.CallDelayed(duration, () => elevator.TurnOnLights());
        }

        /// <summary>
        /// Restores standard electrical power and re-activates all internal illumination emitters inside the elevator cabin.
        /// </summary>
        /// <param name="elevator">The target <see cref="Elevator"/> instance undergoing light grid restoration.</param>
        public static void TurnOnLights(this Elevator elevator)
        {
            if (elevator?.Base == null) return;
            int instanceId = elevator.Base.GetInstanceID();

            lock (_blackedOutElevators)
            {
                _blackedOutElevators.Remove(instanceId);
            }

            try
            {
                foreach (Light light in elevator.Base.GetComponentsInChildren<Light>(true))
                {
                    light.enabled = true;
                }
                foreach (MonoBehaviour fLight in elevator.Base.GetComponentsInChildren<MonoBehaviour>(true))
                {
                    if (fLight.GetType().Name == "FlickerableLight")
                    {
                        fLight.enabled = true;
                    }
                }
            }
            catch { /* Defensive safety isolation barrier */ }
        }

        /// <summary>
        /// Verifies whether the targeted elevator cabin is currently locked in an active blackout state loop.
        /// </summary>
        /// <param name="elevator">The source <see cref="Elevator"/> wrapper platform being polled.</param>
        /// <returns><c>true</c> if the cabin lights are currently disabled logically; otherwise, <c>false</c>.</returns>
        public static bool AreLightsOff(this Elevator elevator)
        {
            if (elevator?.Base == null) return false;
            lock (_blackedOutElevators)
            {
                return _blackedOutElevators.Contains(elevator.Base.GetInstanceID());
            }
        }
        #endregion

        #region Active Floor Operations (Safe State Mutators)
        /// <summary>
        /// Fluently opens ONLY the elevator doors located on the currently active floor level, preventing cross-floor safety exploits.
        /// </summary>
        /// <param name="elevator">The target <see cref="Elevator"/> instance undergoing door manipulation.</param>
        /// <param name="bypassLocks">If set to <c>true</c>, forces the activation state even if the door is restricted by a server lock.</param>
        public static void OpenActiveDoors(this Elevator elevator, bool bypassLocks = false)
        {
            if (elevator?.Doors is null) return;

            // SCP:SL native architecture maps the Elevator's floor level arrays dynamically.
            // We iterate and evaluate each door component, opening exclusively those attached to the active deck.
            foreach (Door door in elevator.Doors)
            {
                if (door.GameObject != null && door.GameObject.TryGetComponent<Interactables.Interobjects.ElevatorDoor>(out var nativeDoor))
                {
                    if (elevator.IsDoorAtCurrentLevel(door, nativeDoor))
                    {
                        door.Open(bypassLocks);
                    }
                }
            }
        }

        /// <summary>
        /// Fluently closes ONLY the elevator doors located on the currently active floor level.
        /// </summary>
        /// <param name="elevator">The target <see cref="Elevator"/> instance undergoing door manipulation.</param>
        /// <param name="bypassLocks">If set to <c>true</c>, forces the activation state even if the door is restricted by a server lock.</param>
        public static void CloseActiveDoors(this Elevator elevator, bool bypassLocks = false)
        {
            if (elevator?.Doors is null) return;

            foreach (Door door in elevator.Doors)
            {
                if (door.GameObject != null && door.GameObject.TryGetComponent<Interactables.Interobjects.ElevatorDoor>(out var nativeDoor))
                {
                    if (elevator.IsDoorAtCurrentLevel(door, nativeDoor))
                    {
                        door.Close(bypassLocks);
                    }
                }
            }
        }

        /// <summary>
        /// Defensive helper executing a high-precision proximity check to verify if a door asset belongs to the active elevator car level.
        /// </summary>
        /// <param name="elevator">The source <see cref="Elevator"/> wrapper platform being polled.</param>
        /// <param name="door">The high-level <see cref="Door"/> wrapper asset checked for intersection bounds.</param>
        /// <param name="nativeDoor">The native underlying <see cref="Interactables.Interobjects.ElevatorDoor"/> engine instance component.</param>
        /// <returns><c>true</c> if the door position maps perfectly onto the elevator car's active vertical alignment track; otherwise, <c>false</c>.</returns>
        private static bool IsDoorAtCurrentLevel(this Elevator elevator, Door door, Interactables.Interobjects.ElevatorDoor nativeDoor)
        {
            // FIXED: Replaced non-existent .Chamber property allocation with the native base component wrapper hook (.Base)
            if (elevator?.Base is null) return false;

            float verticalDelta = Math.Abs(door.Position.y - elevator.Base.transform.position.y);

            // A threshold delta limit of 3.5 meters perfectly encapsulates the vertical clearance envelope of an active cabin floor link
            return verticalDelta <= 3.5f;
        }
        #endregion

        #region Collection Query Extensions (Spatial Matrices)
        /// <summary>
        /// Retrieves a filtered sequence of active elevator modules whose current destination grids map directly to a target facility zone boundary.
        /// </summary>
        /// <param name="zone">The targeting operational <see cref="FacilityZone"/> configuration used to anchor the tracking evaluation query.</param>
        /// <returns>An enumerable collection containing all matching <see cref="Elevator"/> units intersecting the target layout zone bounds.</returns>
        public static IEnumerable<Elevator> GetElevatorsInZone(FacilityZone zone)
        {
            return Elevator.List.Where(elevator =>
                elevator.CurrentDestination.Rooms.Any(room => Room.Get(room.Base)?.Zone == zone));
        }

        /// <summary>
        /// Isolates and filters the global elevator tracking arrays to return only the specific units structurally bridging into the target room.
        /// </summary>
        /// <param name="room">The anchoring <see cref="Room"/> instance tracking local destination mappings.</param>
        /// <returns>An enumerable sequence tracking matching elevator units linked directly to the specified layout node mapping.</returns>
        public static IEnumerable<Elevator> GetElevatorsConnectedToRoom(this Room room)
        {
            if (room == null) return Enumerable.Empty<Elevator>();
            return Elevator.List.Where(elevator => elevator.CurrentDestination?.Rooms.Contains(room) == true);
        }
        #endregion

        #region State Assessment (Validation Ensembles)
        /// <summary>
        /// Evaluates whether any elevator infrastructure bound to the specified room is actively executing a mechanical movement sequence.
        /// </summary>
        /// <param name="room">The source <see cref="Room"/> spatial structure queried for mechanical transition operations.</param>
        /// <returns><c>true</c> if an elevator is bound to the room and currently processing an active mechanical cycle; otherwise, <c>false</c>.</returns>
        public static bool IsActiveInRoom(this Room room)
        {
            if (room == null) return false;

            return Elevator.List.Any(elevator =>
                elevator.CurrentDestination.Rooms.Contains(room) &&
                elevator.CurrentSequence != Interactables.Interobjects.ElevatorChamber.ElevatorSequence.Ready);
        }

        /// <summary>
        /// Evaluates if an active player's spatial coordinates currently overlap an operational elevator cabin mapped to executive or facility transitional sectors.
        /// </summary>
        /// <param name="player">The target <see cref="Player"/> entity execution node evaluated for ongoing spatial tracking containment.</param>
        /// <returns><c>true</c> if the player entity is verified inside an elevator room boundary; otherwise, <c>false</c>.</returns>
        public static bool IsInExecutiveElevator(this Player player)
        {
            var pRoom = player?.Room;
            if (pRoom == null) return false;

            return Elevator.List.Any(elevator => elevator.CurrentDestination.Rooms.Contains(pRoom));
        }
        #endregion

        #region Mass Enforcement (Administrative Hooks)
        /// <summary>
        /// Enforces absolute structural lockdowns on all elevator bulkhead vectors tracking within the requested facility zone.
        /// </summary>
        /// <param name="zone">The targeting structural <see cref="FacilityZone"/> layout block assigned for immediate passage suppression.</param>
        public static void LockElevatorsInZone(FacilityZone zone)
        {
            foreach (var elevator in GetElevatorsInZone(zone)) elevator.LockAllDoors();
        }

        /// <summary>
        /// Restores normal passage access and lifts all operational bulkhead locking restrictions across elevator units within the specified zone.
        /// </summary>
        /// <param name="zone">The targeting structural <see cref="FacilityZone"/> layout block assigned for operational recovery routines.</param>
        public static void UnlockElevatorsInZone(FacilityZone zone)
        {
            foreach (var elevator in GetElevatorsInZone(zone)) elevator.UnlockAllDoors();
        }

        /// <summary>
        /// Processes a localized, probability-driven evaluation sweep across all elevators bound to a room, 
        /// safely routing matching units into an execution action graph.
        /// </summary>
        /// <param name="room">The source <see cref="Room"/> context serving as the spatial matrix anchor for local connections.</param>
        /// <param name="affectChance">The fractional probability value constraint ceiling checked via thread-safe random state generators.</param>
        /// <param name="duration">The execution lifecycle tracking timeframe in seconds allocated for downstream manipulation routines.</param>
        /// <param name="elevatorAction">The specialized modification action callback graph deployed if probability check criteria are successfully met.</param>
        public static void HandleElevatorsForRoom(this Room room, float affectChance, float duration, Action<Elevator> elevatorAction)
        {
            if (affectChance <= 0f || affectChance > 100f || elevatorAction == null) return;

            foreach (var elevator in room.GetElevatorsConnectedToRoom())
            {
                if (SafeRandom.Range(0f, 100f) <= affectChance) elevatorAction(elevator);
            }
        }
        #endregion
    }
}