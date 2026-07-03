using System;
using System.Collections.Generic;
using System.Linq;
using MapGeneration;
using LabApi.Features.Wrappers;

namespace LabApi.Extensions
{
    /// <summary>
    /// Provides enterprise-grade abstraction query layers, real-time spatial lookup matrices, 
    /// and automated locking hooks for server-side <see cref="Elevator"/> components.
    /// </summary>
    internal static class ElevatorExtensions
    {
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
        /// Evaluates whether any elevator infrastructure bound to the specified room is actively executing a mechanical movement sequence.
        /// </summary>
        /// <param name="room">The source <see cref="Room"/> spatial structure queried for mechanical transition operations.</param>
        /// <returns><c>true</c> if an elevator is bound to the room and currently processing an active mechanical cycle; otherwise, <c>false</c>.</returns>
        public static bool IsElevatorActiveInRoom(this Room room)
        {
            if (room == null) return false;

            return Elevator.List.Any(elevator =>
                elevator.CurrentDestination.Rooms.Contains(room) &&
                elevator.CurrentSequence != Interactables.Interobjects.ElevatorChamber.ElevatorSequence.Ready);
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
        /// Evaluates if an active player's spatial coordinates currently overlap an operational elevator cabin mapped to executive or facility transitional sectors.
        /// </summary>
        /// <param name="player">The target <see cref="Player"/> entity execution node evaluated for ongoing spatial tracking containment.</param>
        /// <returns><c>true</c> if the player entity is verified inside an elevator room boundary; otherwise, <c>false</c>.</returns>
        public static bool IsPlayerInExecutiveElevator(this Player player)
        {
            var pRoom = player?.Room;
            if (pRoom == null) return false;

            return Elevator.List.Any(elevator => elevator.CurrentDestination.Rooms.Contains(pRoom));
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
    }
}