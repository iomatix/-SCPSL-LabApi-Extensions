using LabApi.Extensions.Misc;
using LabApi.Features.Wrappers;
using MapGeneration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LabApi.Extensions
{
    /// <summary>
    /// Provides enterprise-grade abstraction query layers, real-time spatial lookup matrices, 
    /// and automated locking hooks for server-side <see cref="Elevator"/> components.
    /// </summary>
    public static class ElevatorExtensions
    {
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
            if (elevator?.Base is null) return false;

            float verticalDelta = Math.Abs(door.Position.y - elevator.Base.transform.position.y);

            // A threshold delta limit of 3.5 meters perfectly encapsulates the vertical clearance envelope of an active cabin floor link
            return verticalDelta <= 3.5f;
        }

        #endregion

        #region Batch Operations (Zero-Allocation High-Performance Overloads)

        /// <summary>
        /// Opens active floor doors for a collection of elevators.
        /// </summary>
        public static void OpenActiveDoors(this IEnumerable<Elevator> elevators, bool bypassLocks = false)
        {
            if (elevators is null) return;

            if (elevators is List<Elevator> concreteList)
            {
                int count = concreteList.Count;
                for (int i = 0; i < count; i++) concreteList[i].OpenActiveDoors(bypassLocks);
                return;
            }

            foreach (Elevator elevator in elevators) elevator.OpenActiveDoors(bypassLocks);
        }

        /// <summary>
        /// Opens active floor doors for an inline array of elevators.
        /// </summary>
        public static void OpenActiveDoors(bool bypassLocks, params Elevator[] elevators)
        {
            if (elevators is null) return;

            int count = elevators.Length;
            for (int i = 0; i < count; i++) elevators[i].OpenActiveDoors(bypassLocks);
        }

        /// <summary>
        /// Closes active floor doors for a collection of elevators.
        /// </summary>
        public static void CloseActiveDoors(this IEnumerable<Elevator> elevators, bool bypassLocks = false)
        {
            if (elevators is null) return;

            if (elevators is List<Elevator> concreteList)
            {
                int count = concreteList.Count;
                for (int i = 0; i < count; i++) concreteList[i].CloseActiveDoors(bypassLocks);
                return;
            }

            foreach (Elevator elevator in elevators) elevator.CloseActiveDoors(bypassLocks);
        }

        /// <summary>
        /// Closes active floor doors for an inline array of elevators.
        /// </summary>
        public static void CloseActiveDoors(bool bypassLocks, params Elevator[] elevators)
        {
            if (elevators is null) return;

            int count = elevators.Length;
            for (int i = 0; i < count; i++) elevators[i].CloseActiveDoors(bypassLocks);
        }

        /// <summary>
        /// Turns off lights for a collection of elevators for the specified duration.
        /// </summary>
        public static void TurnOffLights(this IEnumerable<Elevator> elevators, float duration)
        {
            if (elevators is null) return;

            if (elevators is List<Elevator> concreteList)
            {
                int count = concreteList.Count;
                for (int i = 0; i < count; i++) concreteList[i].TurnOffLights(duration);
                return;
            }

            foreach (Elevator elevator in elevators) elevator.TurnOffLights(duration);
        }

        /// <summary>
        /// Turns off lights for an inline array of elevators for the specified duration.
        /// </summary>
        public static void TurnOffLights(float duration, params Elevator[] elevators)
        {
            if (elevators is null) return;

            int count = elevators.Length;
            for (int i = 0; i < count; i++) elevators[i].TurnOffLights(duration);
        }

        /// <summary>
        /// Turns on lights for a collection of elevators.
        /// </summary>
        public static void TurnOnLights(this IEnumerable<Elevator> elevators)
        {
            if (elevators is null) return;

            if (elevators is List<Elevator> concreteList)
            {
                int count = concreteList.Count;
                for (int i = 0; i < count; i++) concreteList[i].TurnOnLights();
                return;
            }

            foreach (Elevator elevator in elevators) elevator.TurnOnLights();
        }

        /// <summary>
        /// Turns on lights for an inline array of elevators.
        /// </summary>
        public static void TurnOnLights(params Elevator[] elevators)
        {
            if (elevators is null) return;

            int count = elevators.Length;
            for (int i = 0; i < count; i++) elevators[i].TurnOnLights();
        }

        #endregion

        #region Collection Query Extensions (Spatial Matrices)

        /// <summary>
        /// Resolves the high-level <see cref="Elevator"/> wrapper associated with this specific door instance.
        /// Supports both wrapper casting and direct native component resolution fallbacks.
        /// </summary>
        /// <param name="door">The target <see cref="Door"/> instance checked for associated elevator linkages.</param>
        /// <returns>The resolved <see cref="Elevator"/> wrapper if successfully mapped; otherwise, null.</returns>
        public static Elevator GetElevator(this Door door)
        {
            if (door == null) return null;

            if (door is ElevatorDoor elevatorDoor)
            {
                return elevatorDoor.Elevator;
            }

            if (door.GameObject != null && door.GameObject.TryGetComponent<Interactables.Interobjects.ElevatorDoor>(out var nativeDoor))
            {
                return Elevator.GetByGroup(nativeDoor.Group)?.FirstOrDefault();
            }

            return null;
        }

        /// <summary>
        /// Gets all active elevators associated with the specified facility zone destination.
        /// </summary>
        public static IEnumerable<Elevator> GetElevators(this FacilityZone zone)
        {
            foreach (Elevator elevator in Elevator.List)
            {
                var destinationRooms = elevator?.CurrentDestination?.Rooms;
                if (destinationRooms is null) continue;

                bool zoneMatched = false;
                foreach (var r in destinationRooms)
                {
                    if (Room.Get(r.Base)?.Zone == zone)
                    {
                        zoneMatched = true;
                        break;
                    }
                }

                if (zoneMatched) yield return elevator;
            }
        }

        /// <summary>
        /// Retrieves a filtered sequence of active elevator modules whose current destination grids map directly to a target facility zone boundary.
        /// </summary>
        /// <param name="zone">The targeting operational <see cref="FacilityZone"/> configuration used to anchor the tracking evaluation query.</param>
        /// <returns>An enumerable collection containing all matching <see cref="Elevator"/> units intersecting the target layout zone bounds.</returns>
        public static IEnumerable<Elevator> GetElevatorsInZone(FacilityZone zone) => zone.GetElevators();

        /// <summary>
        /// Isolates and filters the global elevator tracking arrays to return only the specific units structurally bridging into the target room.
        /// </summary>
        /// <param name="room">The anchoring <see cref="Room"/> instance tracking local destination mappings.</param>
        /// <returns>An enumerable sequence tracking matching elevator units linked directly to the specified layout node mapping.</returns>
        public static IEnumerable<Elevator> GetElevatorsConnectedToRoom(this Room room)
        {
            if (room is null) yield break;

            foreach (Elevator elevator in Elevator.List)
            {
                if (elevator?.CurrentDestination?.Rooms?.Contains(room) == true)
                {
                    yield return elevator;
                }
            }
        }

        #endregion

        #region State Assessment (Validation Ensembles)

        /// <summary>
        /// Verifies whether the associated elevator cabin is currently physically aligned with this specific door's vertical deck level.
        /// Used comprehensively across door interaction layers to prevent void access exploits.
        /// </summary>
        /// <param name="door">The target <see cref="Door"/> instance checked against cabin proximity envelopes.</param>
        /// <returns><c>true</c> if the cabin is present at the door's floor level; otherwise, <c>false</c>.</returns>
        public static bool IsElevatorAtDoorLevel(this Door door)
        {
            var elevator = door.GetElevator();
            if (elevator?.Base == null) return false;

            float verticalDelta = Math.Abs(door.Position.y - elevator.Base.transform.position.y);
            return verticalDelta <= 3.5f;
        }

        /// <summary>
        /// Evaluates whether any elevator infrastructure bound to the specified room is actively executing a mechanical movement sequence.
        /// </summary>
        /// <param name="room">The source <see cref="Room"/> spatial structure queried for mechanical transition operations.</param>
        /// <returns><c>true</c> if an elevator is bound to the room and currently processing an active mechanical cycle; otherwise, <c>false</c>.</returns>
        public static bool IsActiveInRoom(this Room room)
        {
            if (room is null) return false;

            foreach (Elevator elevator in Elevator.List)
            {
                if (elevator?.CurrentDestination?.Rooms?.Contains(room) == true &&
                    elevator.CurrentSequence != Interactables.Interobjects.ElevatorChamber.ElevatorSequence.Ready)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Evaluates if an active player's spatial coordinates currently overlap an operational elevator cabin mapped to executive or facility transitional sectors.
        /// </summary>
        /// <param name="player">The target <see cref="Player"/> entity execution node evaluated for ongoing spatial tracking containment.</param>
        /// <returns><c>true</c> if the player entity is verified inside an elevator room boundary; otherwise, <c>false</c>.</returns>
        public static bool IsInExecutiveElevator(this Player player)
        {
            Room pRoom = player?.Room;
            if (pRoom is null) return false;

            foreach (Elevator elevator in Elevator.List)
            {
                if (elevator?.CurrentDestination?.Rooms?.Contains(pRoom) == true)
                    return true;
            }
            return false;
        }

        #endregion

        #region Mass Enforcement (Administrative Hooks)

        /// <summary>
        /// Enforces absolute structural lockdowns on all elevator bulkhead vectors tracking within the requested facility zone.
        /// </summary>
        /// <param name="zone">The targeting structural <see cref="FacilityZone"/> layout block assigned for immediate passage suppression.</param>
        public static void LockElevators(this FacilityZone zone)
        {
            foreach (var elevator in zone.GetElevators()) elevator.LockAllDoors();
        }

        /// <summary>
        /// Enforces absolute structural lockdowns on all elevator bulkhead vectors tracking within the requested facility zone.
        /// Retained for backward compatibility.
        /// </summary>
        public static void LockElevatorsInZone(FacilityZone zone) => zone.LockElevators();

        /// <summary>
        /// Restores normal passage access and lifts all operational bulkhead locking restrictions across elevator units within the specified zone.
        /// </summary>
        /// <param name="zone">The targeting structural <see cref="FacilityZone"/> layout block assigned for operational recovery routines.</param>
        public static void UnlockElevators(this FacilityZone zone)
        {
            foreach (var elevator in zone.GetElevators()) elevator.UnlockAllDoors();
        }

        /// <summary>
        /// Restores normal passage access and lifts all operational bulkhead locking restrictions across elevator units within the specified zone.
        /// Retained for backward compatibility.
        /// </summary>
        public static void UnlockElevatorsInZone(FacilityZone zone) => zone.UnlockElevators();

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