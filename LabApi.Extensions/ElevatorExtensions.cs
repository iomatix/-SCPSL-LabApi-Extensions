using LabApi.Extensions.Misc;
using LabApi.Features.Wrappers;
using MapGeneration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LabApi.Extensions
{
    /// <summary>
    /// Utility extensions for elevator door control and basic cabin state checks.
    /// </summary>
    public static class ElevatorExtensions
    {
        #region Active Floor Operations

        /// <summary>
        /// Opens only the doors located on the elevator's current floor.
        /// </summary>
        public static void OpenActiveDoors(this Elevator elevator, bool bypassLocks = false)
        {
            if (elevator?.Doors is null)
                return;

            foreach (var door in elevator.Doors)
            {
                if (door?.GameObject != null &&
                    door.GameObject.TryGetComponent<Interactables.Interobjects.ElevatorDoor>(out var nativeDoor) &&
                    elevator.IsDoorAtCurrentLevel(door, nativeDoor))
                {
                    door.Open(bypassLocks);
                }
            }
        }

        /// <summary>
        /// Closes only the doors located on the elevator's current floor.
        /// </summary>
        public static void CloseActiveDoors(this Elevator elevator, bool bypassLocks = false)
        {
            if (elevator?.Doors is null)
                return;

            foreach (var door in elevator.Doors)
            {
                if (door?.GameObject != null &&
                    door.GameObject.TryGetComponent<Interactables.Interobjects.ElevatorDoor>(out var nativeDoor) &&
                    elevator.IsDoorAtCurrentLevel(door, nativeDoor))
                {
                    door.Close(bypassLocks);
                }
            }
        }

        /// <summary>
        /// Returns true if the door belongs to the elevator's current floor.
        /// </summary>
        private static bool IsDoorAtCurrentLevel(this Elevator elevator, Door door, Interactables.Interobjects.ElevatorDoor nativeDoor)
        {
            if (elevator?.Base is null)
                return false;

            float delta = Math.Abs(door.Position.y - elevator.Base.transform.position.y);
            return delta <= 3.5f;
        }

        #endregion

        #region Batch Operations (IEnumerable + params)

        /// <summary>
        /// Opens active-floor doors for multiple elevators.
        /// </summary>
         public static void OpenActiveDoors(this IEnumerable<Elevator> elevators, bool bypassLocks = false)
            => elevators.ForEach(e => e?.OpenActiveDoors(bypassLocks));

        /// <summary>
        /// Opens active-floor doors for multiple elevators (params overload).
        /// </summary>
        public static void OpenActiveDoors(bool bypassLocks, params Elevator[] elevators)
            => ((IEnumerable<Elevator>)elevators).OpenActiveDoors(bypassLocks);

        /// <summary>
        /// Closes active-floor doors for multiple elevators.
        /// </summary>
        public static void CloseActiveDoors(this IEnumerable<Elevator> elevators, bool bypassLocks = false)
            => elevators.ForEach(e => e?.CloseActiveDoors(bypassLocks));

        /// <summary>
        /// Closes active-floor doors for multiple elevators (params overload).
        /// </summary>
        public static void CloseActiveDoors(bool bypassLocks, params Elevator[] elevators)
            => ((IEnumerable<Elevator>)elevators).CloseActiveDoors(bypassLocks);

        #endregion

    }
}
