using LabApi.Features.Wrappers;
using MapGeneration;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LabApi.Extensions
{
    /// <summary>
    /// Provides enterprise-grade extension methods for <see cref="Room"/> and <see cref="RoomName"/> structures,
    /// enabling high-performance map routing lookup, safe generator state validation, and environmental grid illumination overrides.
    /// </summary>
    internal static class RoomExtensions
    {

        #region Collection Query Extensions
        /// <summary>
        /// Filters an enumerable collection stream of rooms to insulate the pipeline against sectors 
        /// representing the unstable spatial bounds of SCP-106's Pocket Dimension.
        /// </summary>
        /// <param name="rooms">The source collection of room architectural sectors undergo dimension audit.</param>
        /// <returns>A filtered enumerable sequence layout containing rooms outside the pocket dimension zone.</returns>
        public static IEnumerable<Room> WhereNotInPocket(this IEnumerable<Room> rooms)
        {
            if (rooms is null) return Enumerable.Empty<Room>();

            List<Room> filtered = new();
            foreach (Room room in rooms)
            {
                if (room is not null && room.Name != RoomName.Pocket)
                {
                    filtered.Add(room);
                }
            }
            return filtered;
        }
        #endregion

        #region Room Name Extensions
        /// <summary>
        /// Evaluates if a specific structural <see cref="RoomName"/> token corresponds directly to a tactical zone checkpoint airlock node.
        /// </summary>
        /// <param name="roomName">The source <see cref="RoomName"/> enumeration literal target requested for evaluation.</param>
        /// <returns><c>true</c> if the room layout represents a checkpoint security node; otherwise, <c>false</c>.</returns>
        public static bool IsCheckpoint(this RoomName roomName) =>
            roomName is RoomName.LczCheckpointA
                      or RoomName.LczCheckpointB
                      or RoomName.HczCheckpointA
                      or RoomName.HczCheckpointB
                      or RoomName.HczCheckpointToEntranceZone;

        /// <summary>
        /// Determines whether the designated <see cref="RoomName"/> spatial layout topology represents a secure containment sector for anomalous entities.
        /// </summary>
        /// <param name="roomName">The source <see cref="RoomName"/> enumeration literal target requested for evaluation.</param>
        /// <returns><c>true</c> if the layout maps to an anomalous entity containment vault zone; otherwise, <c>false</c>.</returns>
        public static bool IsScpRoom(this RoomName roomName) =>
            roomName is RoomName.Lcz173
                      or RoomName.Lcz330
                      or RoomName.Hcz049
                      or RoomName.Hcz079
                      or RoomName.Hcz096
                      or RoomName.Hcz106
                      or RoomName.Hcz939
                      or RoomName.Lcz914
                      or RoomName.HczTestroom;

        /// <summary>
        /// Checks if the designated <see cref="RoomName"/> structural context is classified as a high-security tactical weapons or munitions armory depot.
        /// </summary>
        /// <param name="roomName">The source <see cref="RoomName"/> enumeration literal target requested for evaluation.</param>
        /// <returns><c>true</c> if the zone signature represents a facility armory vault; otherwise, <c>false</c>.</returns>
        public static bool IsArmory(this RoomName roomName) =>
            roomName is RoomName.LczArmory or RoomName.HczArmory;

        #endregion

        #region Spatial Validation
        /// <summary>
        /// Verifies defensively if the target <see cref="Room"/> spatial zone is completely free of any power generators that are currently in an active, fully engaged state.
        /// </summary>
        /// <param name="room">The target <see cref="Room"/> grid instance verified for active generator properties.</param>
        /// <returns><c>true</c> if the zone contains zero engaged power components or if no generators are registered inside the topology; otherwise, <c>false</c>.</returns>
        public static bool IsRoomFreeOfEngagedGenerators(this Room room)
        {
            if (!Generator.TryGetFromRoom(room, out List<Generator> generators) || generators == null)
                return true;

            return !generators.Any(gen => gen.Engaged);
        }

        /// <summary>
        /// Performs an aggregated spatial validation sweep across the target <see cref="Room"/> and all physically connected adjacent neighbor zones 
        /// to confirm absolute grid isolation from any active, fully engaged power generators.
        /// </summary>
        /// <param name="room">The anchoring root <see cref="Room"/> layout node initiating the collective neighbor sweep routine.</param>
        /// <returns><c>true</c> if the local node cluster is verified as completely clear of active operational generators; otherwise, <c>false</c>.</returns>
        public static bool IsRoomAndNeighborsFreeOfEngagedGenerators(this Room room)
        {
            if (room == null) return false;
            if (!room.IsRoomFreeOfEngagedGenerators()) return false;

            foreach (var neighborIdentifier in room.ConnectedRooms)
            {
                var neighborRoom = Room.Get(neighborIdentifier);
                if (neighborRoom != null && !neighborRoom.IsRoomFreeOfEngagedGenerators())
                    return false;
            }
            return true;
        }
        #endregion

        #region Lighting Overrides
        /// <summary>
        /// Forcibly suppresses the active illumination controllers across the specified room topology for a precise timeframe, 
        /// and applies a localized probability-driven execution sweep to lock adjoining elevator pathway vectors.
        /// </summary>
        /// <param name="room">The target <see cref="Room"/> spatial context where the environmental illumination override is executed.</param>
        /// <param name="duration">The execution lifespan timeframe measured in seconds during which the light suppression grid remains active.</param>
        /// <param name="elevatorAffectChance">The fractional probability value constraint percentage checked prior to executing elevator bulkhead passage suppression.</param>
        public static void TurnOffRoomLights(this Room room, float duration, float elevatorAffectChance = 0f)
        {
            if (room == null) return;

            foreach (var controller in room.AllLightControllers)
            {
                controller.FlickerLights(duration);
            }

            room.HandleElevatorsForRoom(elevatorAffectChance, duration, elevator =>
            {
                elevator.LockAllDoors();

                // FIXED: Assigned a context-compliant structural tracking tag matching the elevator domain instead of the leaked audio reference.
                var coroutine = Timing.CallDelayed(duration, () => elevator.UnlockAllDoors());
                coroutine.Tag = "LabApiExtensions-ElevatorUnlock";
            });
        }

        /// <summary>
        /// Fluently overrides the active rendering illumination color spectrum channel variables for a specific room.
        /// </summary>
        /// <param name="room">The target room architecture instance undergoing visual state modifications.</param>
        /// <param name="color">The target <see cref="Color"/> layout spectrum applied to the room lighting controllers.</param>
        public static void SetLightsColor(this Room room, Color color)
        {
            if (room?.LightController is not null)
            {
                room.LightController.OverrideLightsColor = color;
            }
        }

        /// <summary>
        /// Systematically executes a batch color spectrum override sweep across an aggregated collection sequence of rooms.
        /// </summary>
        /// <param name="rooms">The collection layout tracking targeted room assets inside server memory.</param>
        /// <param name="color">The target <see cref="Color"/> layout spectrum applied to all light nodes within the collection matrix.</param>
        public static void SetLightsColor(this IEnumerable<Room> rooms, Color color)
        {
            if (rooms is null) return;

            foreach (Room room in rooms)
            {
                if (room is not null)
                {
                    room.SetLightsColor(color);
                }
            }
        }
        #endregion

        /// <summary>
        /// Executes a specified procedural action delegate graph across a localized room anchor point 
        /// and seamlessly propagates the delegate pattern execution out into all adjacent physical room nodes safely.
        /// </summary>
        /// <param name="room">The structural core <see cref="Room"/> node serving as the root origin point for the iteration cascade.</param>
        /// <param name="action">The modification action callback graph deployed sequentially against each room structure inside the tracking cluster bounds.</param>
        public static void ExecuteActionOnRoomAndNeighbors(this Room room, Action<Room> action)
        {
            if (room == null || action == null) return;
            action(room);
            foreach (var neighborIdentifier in room.ConnectedRooms)
            {
                var neighborRoom = Room.Get(neighborIdentifier);
                if (neighborRoom != null) action(neighborRoom);
            }
        }

        /// <summary>
        /// Iterates over all door sub-components bound to the target room context to safely locate 
        /// and fracture any breakable barriers (<see cref="BreakableDoor"/>) that remain intact.
        /// </summary>
        /// <param name="room">The source <see cref="Room"/> architectural node whose interior doors are targeted for destruction.</param>
        public static void BreakAllDoors(this Room room)
        {
            if (room?.Doors == null) return;

            foreach (var door in room.Doors)
            {
                if (door is BreakableDoor breakable)
                {
                    if (!breakable.IsBroken)
                    {
                        breakable.TryBreak();
                    }
                }
            }
        }

        #region Vector Spatial Intersections
        /// <summary>
        /// Extension method on <see cref="Vector3"/> to seamlessly resolve and fetch the live <see cref="Room"/> instance 
        /// encompassing the targeted coordinates layer directly from the underlying map topology grid.
        /// </summary>
        /// <param name="position">The source <see cref="Vector3"/> coordinates sequence queried within the active workspace simulation.</param>
        /// <returns>The concrete structural <see cref="Room"/> asset containing the vector position; otherwise, <see langword="null"/>.</returns>
        public static Room GetRoom(this Vector3 position)
        {
            return Room.GetRoomAtPosition(position);
        }

        /// <summary>
        /// Computes the precise linear Euclidean distance between the structural transform center position of the room asset 
        /// and a targeted raw 3D position vector coordinate.
        /// </summary>
        /// <param name="room">The source <see cref="Room"/> instance serving as the origin coordinate spatial anchor.</param>
        /// <param name="position">The target destination <see cref="Vector3"/> spatial position checked against the room center.</param>
        /// <returns>A single-precision floating-point scalar value indicating the physical displacement distance in meters.</returns>
        public static float GetDistanceTo(this Room room, Vector3 position)
        {
            if (room?.Base == null)
            {
                return 0f;
            }

            return Vector3.Distance(room.Position, position);
        }

        /// <summary>
        /// Performs a high-performance proximity validation query tracking from the room's transform center center point 
        /// utilizing underlying Unity vector squaring math (<c>sqrMagnitude</c>) to avoid high overhead calculation paths.
        /// </summary>
        /// <param name="room">The source <see cref="Room"/> instance serving as the origin coordinate spatial anchor.</param>
        /// <param name="position">The target destination <see cref="Vector3"/> coordinates evaluated for boundary intersections.</param>
        /// <param name="radiusSize">The maximum range limitation value constraint in meters allowed for positive validation.</param>
        /// <returns><c>true</c> if the target coordinates reside inside the computed room radial envelope boundary; otherwise, <c>false</c>.</returns>
        public static bool IsWithinRadius(this Room room, Vector3 position, float radiusSize)
        {
            if (room?.Base == null)
            {
                return false;
            }

            float sqrDistance = (room.Position - position).sqrMagnitude;
            return sqrDistance <= (radiusSize * radiusSize);
        }
        #endregion
    }
}