using LabApi.Extensions.Misc;
using LabApi.Features.Wrappers;
using MapGeneration;
using MEC;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LabApi.Extensions
{
    /// <summary>
    /// Utility extensions for working with <see cref="Room"/> and <see cref="RoomName"/>.
    /// Includes adjacency lookup, generator checks, lighting control, door operations and spatial helpers.
    /// </summary>
    public static class RoomExtensions
    {
        #region Neighbor Lookup

        /// <summary>
        /// Returns all rooms directly connected to this room.
        /// </summary>
        public static IEnumerable<Room> GetNeighbors(this Room room)
        {
            if (room?.ConnectedRooms is null)
                yield break;

            foreach (var id in room.ConnectedRooms)
            {
                var neighbor = Room.Get(id);
                if (neighbor != null)
                    yield return neighbor;
            }
        }

        /// <summary>
        /// Returns elevators connected to the given room.
        /// </summary>
        public static IEnumerable<Elevator> GetElevatorsConnectedToRoom(this Room room)
        {
            if (room is null)
                yield break;

            foreach (var elevator in room.GetElevatorsConnectedToRoom())
            {
                if (elevator?.CurrentDestination?.Rooms?.Contains(room) == true)
                    yield return elevator;
            }
        }

        #endregion

        #region Room Collection Filters

        /// <summary>
        /// Returns all rooms except the Pocket Dimension.
        /// </summary>
        public static IEnumerable<Room> WhereNotInPocket(this IEnumerable<Room> rooms)
        {
            if (rooms is null)
                yield break;

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
        /// Returns true if the room is a checkpoint.
        /// </summary>
        public static bool IsCheckpoint(this RoomName name) =>
            name is RoomName.LczCheckpointA
                or RoomName.LczCheckpointB
                or RoomName.HczCheckpointA
                or RoomName.HczCheckpointB
                or RoomName.HczCheckpointToEntranceZone;

        /// <summary>
        /// Returns true if the room is an SCP containment room.
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
        /// Returns true if the room is an armory.
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
        /// </summary>
        public static bool IsRoomAndNeighborsFreeOfEngagedGenerators(this Room room)
        {
            if (room == null)
                return false;

            if (!room.IsFreeOfEngagedGenerators())
                return false;

            foreach (var neighbor in room.GetNeighbors())
            {
                if (!neighbor.IsFreeOfEngagedGenerators())
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
            if (room is null)
                return false;

            foreach (var elevator in Elevator.List)
            {
                if (elevator?.CurrentDestination?.Rooms?.Contains(room) == true &&
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
        /// Executes an action on the room and all its neighbors.
        /// </summary>
        public static void ExecuteActionOnRoomAndNeighbors(this Room room, Action<Room> action)
        {
            if (room == null || action == null)
                return;

            action(room);

            foreach (var neighbor in room.GetNeighbors())
                action(neighbor);
        }

        /// <summary>
        /// Executes an action on all elevators connected to the room.
        /// </summary>s
        public static void HandleElevatorsForRoom(this Room room, float affectChance, Action<Elevator> action)
        {
            if (affectChance <= 0f || affectChance > 100f || action is null) return;

            room.GetElevatorsConnectedToRoom()?.ForEach(e =>
            {
                if (SafeRandom.Range(0f, 100f) <= affectChance) action(e);
            });
        }

        #endregion

        #region Door Operations

        /// <summary>
        /// Breaks all breakable doors in the room.
        /// </summary>
        public static void BreakAllDoors(this Room room)
        {
            room?.Doors?.ForEach(d =>
            {
                if (d is BreakableDoor b && !b.IsBroken)
                    b.TryBreak();
            });
        }

        #endregion

        #region Lighting (Single Room)

        /// <summary>
        /// Turns off lights in the room for a given duration.
        /// </summary>
        public static void TurnOffLights(this Room room, float duration)
        => room?.AllLightControllers?.ForEach(lc => lc.FlickerLights(duration));


        /// <summary>
        /// Turns on lights in the room and optionally flickers them.
        /// </summary>
        public static void TurnOnLights(this Room room, float flickerDuration = 0f)
            => room?.AllLightControllers?.ForEach(c => c.FlickerLights(flickerDuration));

        /// <summary>
        /// Turns off lights in the room and its neighbors.
        /// </summary>
        public static void TurnOffRoomAndNeighborLights(this Room room, float duration)
            => room.ExecuteActionOnRoomAndNeighbors(r => r.TurnOffLights(duration));

        /// <summary>
        /// Turns on lights in the room and its neighbors.
        /// </summary>
        public static void TurnOnRoomAndNeighborLights(this Room room, float duration = 0f)
            => room.ExecuteActionOnRoomAndNeighbors(r => r.TurnOnLights(duration));

        /// <summary>
        /// Sets the light color for the room.
        /// </summary>
        public static void SetLightsColor(this Room room, Color color)
        {
            if (room?.LightController != null)
                room.LightController.OverrideLightsColor = color;
        }

        #endregion

        #region Lighting (Batch Operations)

        /// <summary>
        /// Sets light color for multiple rooms.
        /// </summary>
        public static void SetLightsColor(this IEnumerable<Room> rooms, Color color)
        => rooms.ForEach(r => r?.SetLightsColor(color));

        /// <summary>
        /// Sets light color for multiple rooms (params).
        /// </summary>
        public static void SetLightsColor(Color color, params Room[] rooms)
        => ((IEnumerable<Room>)rooms).SetLightsColor(color);

        /// <summary>
        /// Turns off lights for multiple rooms.
        /// </summary>
        public static void TurnOffLights(this IEnumerable<Room> rooms, float duration)
        => rooms.ForEach(r => r?.TurnOffLights(duration));

        /// <summary>
        /// Turns off lights for multiple rooms (params).
        /// </summary>
        public static void TurnOffLights(float duration, params Room[] rooms)
        => ((IEnumerable<Room>)rooms).TurnOffLights(duration);

        /// <summary>
        /// Turns on lights for multiple rooms.
        /// </summary>
        public static void TurnOnLights(this IEnumerable<Room> rooms, float flickerDuration = 0f)
        => rooms.ForEach(r => r?.TurnOnLights(flickerDuration));

        /// <summary>
        /// Turns on lights for multiple rooms (params).
        /// </summary>
        public static void TurnOnLights(float flickerDuration, params Room[] rooms)
        => ((IEnumerable<Room>)rooms).TurnOnLights(flickerDuration);

        /// <summary>
        /// Breaks all breakable doors in multiple rooms.
        /// </summary>
        public static void BreakAllDoors(this IEnumerable<Room> rooms)
        => rooms.ForEach(r => r?.BreakAllDoors());

        /// <summary>
        /// Breaks all breakable doors in multiple rooms (params).
        /// </summary>
        public static void BreakAllDoors(params Room[] rooms)
        => ((IEnumerable<Room>)rooms).BreakAllDoors();

        #endregion

        #region Spatial Helpers

        /// <summary>
        /// Returns the room at the given world position.
        /// </summary>
        public static Room GetRoom(this Vector3 position)
            => Room.GetRoomAtPosition(position);

        /// <summary>
        /// Returns the distance from the room center to the given position.
        /// </summary>
        public static float GetDistanceTo(this Room room, Vector3 position)
        {
            if (room?.Base == null)
                return 0f;

            return Vector3.Distance(room.Position, position);
        }

        /// <summary>
        /// Returns true if the position is within the given radius from the room center.
        /// </summary>
        public static bool IsWithinRadius(this Room room, Vector3 position, float radius)
        {
            if (room?.Base == null)
                return false;

            float sqr = (room.Position - position).sqrMagnitude;
            return sqr <= radius * radius;
        }

        #endregion

        #region Flicker Animations

        /// <summary>
        /// Executes a flicker animation on the room lights.
        /// </summary>
        public static IEnumerator<float> FlickerLightsCoroutine(this Room room, Color color, float duration, float frequency)
        {
            if (room?.AllLightControllers is null)
                yield break;

            float interval = 1f / frequency.LimitMin(0.1f);
            float half = interval * 0.5f;
            int flickers = (int)(duration / interval);

            room.SetLightsColor(color);

            var controllers = room.AllLightControllers is List<LightsController> list
                ? list.ToArray()
                : new List<LightsController>(room.AllLightControllers).ToArray();

            int count = controllers.Length;

            for (int i = 0; i < flickers; i++)
            {
                for (int c = 0; c < count; c++)
                    controllers[c].LightsEnabled = false;

                yield return Timing.WaitForSeconds(half);

                for (int c = 0; c < count; c++)
                    controllers[c].LightsEnabled = true;

                yield return Timing.WaitForSeconds(half);
            }

            room.SetLightsColor(Color.clear);
        }

        #endregion
    }
}
