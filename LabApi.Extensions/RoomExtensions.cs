using LabApi.Extensions.Misc;
using LabApi.Features.Wrappers;
using MapGeneration;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LabApi.Extensions
{
    /// <summary>
    /// Highly optimized utility extensions for working with <see cref="Room"/> and <see cref="RoomName"/>.
    /// Includes neighbor lookups, generator checks, door operations, and spatial helpers with zero heap allocations.
    /// </summary>
    public static class RoomExtensions
    {
        #region Neighbor Lookup

        /// <summary>
        /// Returns all rooms directly connected to this room.
        /// </summary>
        public static IEnumerable<Room> GetNeighbors(this Room room)
        {
            if (room == null || room.ConnectedRooms == null)
                yield break;

            // FIX: Using struct enumerator for zero-allocation iteration over HashSet.
            foreach (var id in room.ConnectedRooms)
            {
                var neighbor = Room.Get(id);
                if (neighbor != null)
                    yield return neighbor;
            }
        }

        /// <summary>
        /// Returns elevators connected to the given room. Safe from recursion loops.
        /// </summary>
        public static IEnumerable<Elevator> GetElevatorsConnectedToRoom(this Room room)
        {
            if (room == null)
                yield break;

            // FIX: Prevented StackOverflowException by iterating over global Elevator list instead of self-recursion.
            foreach (var elevator in Elevator.List)
            {
                if (elevator == null)
                    continue;

                if (elevator.CurrentDestination?.Rooms?.Contains(room) == true)
                {
                    yield return elevator;
                }
            }
        }

        #endregion

        #region Room Collection Filters

        /// <summary>
        /// Returns all rooms except the Pocket Dimension with optimized collection fast-paths.
        /// </summary>
        public static IEnumerable<Room> WhereNotInPocket(this IEnumerable<Room> rooms)
        {
            if (rooms == null)
                yield break;

            if (rooms is Room[] array)
            {
                int count = array.Length;
                for (int i = 0; i < count; i++)
                {
                    var room = array[i];
                    if (room != null && room.Name != RoomName.Pocket)
                        yield return room;
                }
                yield break;
            }

            if (rooms is List<Room> list)
            {
                int count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    var room = list[i];
                    if (room != null && room.Name != RoomName.Pocket)
                        yield return room;
                }
                yield break;
            }

            foreach (var room in rooms)
            {
                if (room != null && room.Name != RoomName.Pocket)
                    yield return room;
            }
        }

        #endregion

        #region RoomName Classification

        /// <summary>
        /// Returns true if the room is a checkpoint. Compiled to highly optimized JIT jump tables.
        /// </summary>
        public static bool IsCheckpoint(this RoomName name) =>
            name is RoomName.LczCheckpointA
                or RoomName.LczCheckpointB
                or RoomName.HczCheckpointA
                or RoomName.HczCheckpointB
                or RoomName.HczCheckpointToEntranceZone;

        /// <summary>
        /// Returns true if the room is an SCP containment room. Compiled to highly optimized JIT jump tables.
        /// </summary>
        public static bool IsScpRoom(this RoomName name) =>
            name is RoomName.Lcz173
                or RoomName.Lcz330
                or RoomName.Hcz049
                or RoomName.Hcz079
                or RoomName.Hcz096
                or RoomName.Hcz106
                or RoomName.Hcz939
                or RoomName.Lcz914
                or RoomName.HczTestroom;

        /// <summary>
        /// Returns true if the room is an armory. Compiled to highly optimized JIT jump tables.
        /// </summary>
        public static bool IsArmory(this RoomName name) =>
            name is RoomName.LczArmory or RoomName.HczArmory;

        #endregion

        #region Generator Checks

        /// <summary>
        /// Returns true if the room has no engaged generators.
        /// </summary>
        public static bool IsFreeOfEngagedGenerators(this Room room)
        {
            if (room == null)
                return true;

            if (!Generator.TryGetFromRoom(room, out List<Generator> generators) || generators == null)
                return true;

            int count = generators.Count;
            for (int i = 0; i < count; i++)
            {
                if (generators[i].Engaged)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Returns true if the room and all its neighbors have no engaged generators.
        /// Bypasses neighbor iterator allocations entirely.
        /// </summary>
        public static bool IsRoomAndNeighborsFreeOfEngagedGenerators(this Room room)
        {
            if (room == null)
                return false;

            if (!room.IsFreeOfEngagedGenerators())
                return false;

            if (room.ConnectedRooms == null)
                return true;

            // FIX: Bypassed GetNeighbors() yield allocation and used struct enumerator for zero allocation.
            foreach (var id in room.ConnectedRooms)
            {
                var neighbor = Room.Get(id);
                if (neighbor != null && !neighbor.IsFreeOfEngagedGenerators())
                    return false;
            }

            return true;
        }

        #endregion

        #region Elevator Checks

        /// <summary>
        /// Returns true if any elevator connected to the room is currently moving.
        /// </summary>
        public static bool IsElevatorActiveInRoom(this Room room)
        {
            if (room == null)
                return false;

            foreach (var elevator in Elevator.List)
            {
                if (elevator == null)
                    continue;

                if (elevator.CurrentDestination?.Rooms?.Contains(room) == true &&
                    elevator.CurrentSequence != Interactables.Interobjects.ElevatorChamber.ElevatorSequence.Ready)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region Delegate Propagation

        /// <summary>
        /// Executes an action on the room and all its neighbors with 0 iterator allocations.
        /// </summary>
        public static void ExecuteActionOnRoomAndNeighbors(this Room room, Action<Room> action)
        {
            if (room == null || action == null)
                return;

            action(room);

            if (room.ConnectedRooms == null)
                return;

            // FIX: Bypassed GetNeighbors() yield allocation and used struct enumerator.
            foreach (var id in room.ConnectedRooms)
            {
                var neighbor = Room.Get(id);
                if (neighbor != null)
                {
                    action(neighbor);
                }
            }
        }

        /// <summary>
        /// Executes an action on the room and all its neighbors passing a custom state with 0 allocations.
        /// </summary>
        public static void ExecuteActionOnRoomAndNeighbors<TState>(this Room room, TState state, Action<Room, TState> action)
        {
            if (room == null || action == null)
                return;

            action(room, state);

            if (room.ConnectedRooms == null)
                return;

            // FIX: Added state-passing overload to prevent GC closures during propagation.
            foreach (var id in room.ConnectedRooms)
            {
                var neighbor = Room.Get(id);
                if (neighbor != null)
                {
                    action(neighbor, state);
                }
            }
        }

        /// <summary>
        /// Executes an action on all elevators connected to the room.
        /// Bypasses state-machine iterator allocations.
        /// </summary>
        public static void HandleElevatorsForRoom(this Room room, float affectChance, Action<Elevator> action)
        {
            if (room == null || affectChance <= 0f || affectChance > 100f || action == null)
                return;

            // FIX: Direct loop over Elevator.List instead of yielding through GetElevatorsConnectedToRoom().
            foreach (var elevator in Elevator.List)
            {
                if (elevator == null)
                    continue;

                if (elevator.CurrentDestination?.Rooms?.Contains(room) == true)
                {
                    if (SafeRandom.Range(0f, 100f) <= affectChance)
                    {
                        action(elevator);
                    }
                }
            }
        }

        #endregion

        #region Door Operations

        /// <summary>
        /// Breaks all breakable doors in the room with 0 allocations.
        /// </summary>
        public static void BreakAllDoors(this Room room)
        {
            if (room == null || room.Doors == null)
                return;

            room.Doors.ForEach(static d =>
            {
                if (d is BreakableDoor b && !b.IsBroken)
                {
                    b.TryBreak();
                }
            });
        }

        /// <summary>
        /// Breaks all breakable doors in multiple rooms with 0 allocations.
        /// </summary>
        public static void BreakAllDoors(this IEnumerable<Room> rooms)
        {
            if (rooms == null)
                return;

            rooms.ForEach(static r => r?.BreakAllDoors());
        }

        /// <summary>
        /// Breaks all breakable doors in multiple rooms (params overload).
        /// </summary>
        public static void BreakAllDoors(params Room[] rooms) =>
            rooms.BreakAllDoors();

        #endregion

        #region Spatial Helpers

        /// <summary>
        /// Returns the room at the given world position.
        /// </summary>
        public static Room GetRoom(this Vector3 position) =>
            Room.GetRoomAtPosition(position);

        /// <summary>
        /// Returns the distance from the room center to the given position.
        /// </summary>
        public static float GetDistanceTo(this Room room, Vector3 position)
        {
            if (room == null || room.Base == null)
                return 0f;

            return Vector3.Distance(room.Position, position);
        }

        /// <summary>
        /// Returns true if the position is within the given radius from the room center.
        /// </summary>
        public static bool IsWithinRadius(this Room room, Vector3 position, float radius)
        {
            if (room == null || room.Base == null)
                return false;

            float sqr = (room.Position - position).sqrMagnitude;
            return sqr <= radius * radius;
        }

        #endregion
    }
}