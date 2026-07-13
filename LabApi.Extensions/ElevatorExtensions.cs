using LabApi.Extensions.Misc;
using LabApi.Features.Wrappers;
using MapGeneration;
using System;
using System.Collections.Generic;

namespace LabApi.Extensions
{
    /// <summary>
    /// Provides extension methods for batch elevator operations, door control, and spatial queries.
    /// </summary>
    public static class ElevatorExtensions
    {
        #region Active Floor Operations (Single Target State Mutators)
        /// <summary>
        /// Opens elevator doors exclusively on the currently active floor level.
        /// </summary>
        public static void OpenActiveDoors(this Elevator elevator, bool bypassLocks = false)
        {
            if (elevator?.Doors is null) return;

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
        /// Closes elevator doors exclusively on the currently active floor level.
        /// </summary>
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

        private static bool IsDoorAtCurrentLevel(this Elevator elevator, Door door, Interactables.Interobjects.ElevatorDoor nativeDoor)
        {
            if (elevator?.Base is null) return false;

            float verticalDelta = Math.Abs(door.Position.y - elevator.Base.transform.position.y);
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
        /// Gets all elevators connected to the specified room.
        /// </summary>
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
        /// Checks if any elevator connected to the room is currently moving.
        /// </summary>
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
        /// Checks if the player is currently inside an elevator cabin.
        /// </summary>
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

        #region Zone Enforcement (Fluent Administrative Hooks)
        /// <summary>
        /// Locks all elevator doors within the specified facility zone.
        /// </summary>
        public static void LockElevators(this FacilityZone zone)
        {
            foreach (var elevator in zone.GetElevators()) elevator.LockAllDoors();
        }

        /// <summary>
        /// Unlocks all elevator doors within the specified facility zone.
        /// </summary>
        public static void UnlockElevators(this FacilityZone zone)
        {
            foreach (var elevator in zone.GetElevators()) elevator.UnlockAllDoors();
        }

        /// <summary>
        /// Executes a probabilistic action on all elevators connected to the specified room.
        /// </summary>
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