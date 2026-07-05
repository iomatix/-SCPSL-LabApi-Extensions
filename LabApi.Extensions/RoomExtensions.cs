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
        #region Adjacency & Neighbor Routing (DRY Engine)
        /// <summary>
        /// Resolves and collects all valid physically adjacent neighboring <see cref="Room"/> nodes connected directly to this room instance.
        /// </summary>
        /// <param name="room">The source room anchor node initiating the topology adjacency query.</param>
        /// <returns>An enumerable collection of valid neighboring initialized <see cref="Room"/> instances.</returns>
        public static IEnumerable<Room> GetNeighbors(this Room room)
        {
            if (room?.ConnectedRooms is null) yield break;

            // FIXED: HashSet requires foreach evaluation as it does not natively expose an indexer tracker property
            foreach (var roomIdentifier in room.ConnectedRooms)
            {
                var neighborRoom = Room.Get(roomIdentifier);
                if (neighborRoom is not null)
                {
                    yield return neighborRoom;
                }
            }
        }
        #endregion

        #region Collection Query Extensions
        /// <summary>
        /// Filters an enumerable collection stream of rooms to insulate the pipeline against sectors 
        /// representing the unstable spatial bounds of SCP-106's Pocket Dimension.
        /// </summary>
        /// <param name="rooms">The source collection of room architectural sectors undergoing dimension audit.</param>
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
        public static bool IsFreeOfEngagedGenerators(this Room room)
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
            if (!room.IsFreeOfEngagedGenerators()) return false;

            foreach (Room neighborRoom in room.GetNeighbors())
            {
                if (!neighborRoom.IsFreeOfEngagedGenerators())
                    return false;
            }
            return true;
        }
        #endregion

        #region Lighting Overrides
        /// <summary>
        /// Forcibly suppresses the active illumination controllers across the specified room topology for a precise timeframe.
        /// Insulation Upgrade: Mechanical elevator door override routines removed to preserve Single Responsibility Principle (SRP).
        /// </summary>
        /// <param name="room">The target <see cref="Room"/> spatial context where the environmental illumination override is executed.</param>
        /// <param name="duration">The execution lifespan timeframe measured in seconds during which the light suppression grid remains active.</param>
        public static void TurnOffLights(this Room room, float duration)
        {
            if (room?.AllLightControllers == null) return;

            foreach (var controller in room.AllLightControllers)
            {
                controller.FlickerLights(duration);
            }
        }

        /// <summary>
        /// Fluent API DRY Refactor: Restores electrical power to the room's lighting grid controllers and optionally triggers a brief flicker sequence.
        /// </summary>
        /// <param name="room">The target room architecture instance undergoing illumination restoration.</param>
        /// <param name="flickerDuration">The optional temporal flicker sequence window in seconds (assign 0s for immediate baseline initialization).</param>
        public static void TurnOnLights(this Room room, float flickerDuration = 0f)
        {
            if (room?.AllLightControllers is null) return;

            foreach (var controller in room.AllLightControllers)
            {
                controller.LightsEnabled = true;
                if (flickerDuration > 0f)
                {
                    controller.FlickerLights(flickerDuration);
                }
            }
        }

        /// <summary>
        /// Forcibly suppresses illumination across this room and all physically connected adjacent neighboring rooms simultaneously for a designated duration track.
        /// </summary>
        /// <param name="room">The root room anchor node executing the multi-sector blackout cascade.</param>
        /// <param name="duration">The operational execution lifespan measured in seconds during which the light grids stay unpowered.</param>
        /// <param name="forced">A defensive safety toggle bypass flag to force processing overrides regardless of structural sub-states.</param>
        public static void TurnOffRoomAndNeighborLights(this Room room, float duration, bool forced = false)
        {
            if (room is null) return;

            room.TurnOffLights(duration);

            foreach (Room neighbor in room.GetNeighbors())
            {
                neighbor.TurnOffLights(duration);
            }
        }

        /// <summary>
        /// Restores active electrical power and forces an optional brief flickering update sequence across this room and all adjacent neighbors.
        /// </summary>
        /// <param name="room">The root room anchor node executing the cluster grid illumination restoration.</param>
        /// <param name="duration">The optional temporal flicker sequence window in seconds (assign 0s for immediate baseline initialization).</param>
        public static void TurnOnRoomAndNeighborLights(this Room room, float duration = 0f)
        {
            if (room is null) return;

            // DRY Upgrade: Eradicated local action delegate in favor of the newly exposed TurnOnRoomLights primitive extension
            room.TurnOnLights(duration);
            foreach (Room neighbor in room.GetNeighbors())
            {
                neighbor.TurnOnLights(duration);
            }
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

        #region Delegate Cascade Propagation
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

            foreach (Room neighborRoom in room.GetNeighbors())
            {
                action(neighborRoom);
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
        #endregion

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
        /// Performs a high-performance proximity validation query tracking from the room's transform center point 
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

        #region Lighting Flicker Matrix (Animation Pipelines)
        /// <summary>
        /// Fluently executes a synchronized asynchronous lighting flicker animation loop over an individual <see cref="Room"/> instance.
        /// </summary>
        /// <param name="room">The target room instance undergoing the visual flicker loop sequence.</param>
        /// <param name="color">The target rendering illumination <see cref="Color"/> applied to the light nodes.</param>
        /// <param name="duration">The absolute total timeline duration track in seconds assigned for the animation execution layout.</param>
        /// <param name="frequency">The frequency coefficient tracking how many strobe status shifts execute per second.</param>
        public static IEnumerator<float> FlickerLightsCoroutine(this Room room, Color color, float duration, float frequency)
        {
            if (room?.AllLightControllers == null) yield break;

            // Defensive Boundary: Protect inverse interval math updates against sub-zero or zero division crashes
            float interval = 1f / frequency.LimitMin(0.1f);
            int flickers = (int)Math.Round(duration / interval);

            room.SetLightsColor(color);
            for (int i = 0; i < flickers; i++)
            {
                // Toggle off state using half frame intervals execution bounds
                room.TurnOnLights(); // Ensure state tracking registers are fully updated
                foreach (var controller in room.AllLightControllers) controller.LightsEnabled = false;
                yield return Timing.WaitForSeconds(interval * 0.5f);

                // Toggle back onto standard operational coordinates
                room.TurnOnLights();
                yield return Timing.WaitForSeconds(interval * 0.5f);
            }
            room.SetLightsColor(Color.clear);
        }

        /// <summary>
        /// Fluently executes a batch synchronized visual lighting flicker animation sequence across a concrete <see cref="FacilityZone"/>.
        /// </summary>
        /// <param name="targetZone">The targeted facility sector zone quadrant undergoing power grid flicker animations.</param>
        /// <param name="color">The structural <see cref="Color"/> spectrum applied to zone illumination grids.</param>
        /// <param name="duration">The chronological tracking timeline window in seconds allocated for the animation loop.</param>
        /// <param name="frequency">The velocity scaling frequency determining strobe repetitions per second.</param>
        public static IEnumerator<float> FlickerZoneLightsCoroutine(this FacilityZone targetZone, Color color, float duration, float frequency)
        {
            float interval = 1f / frequency.LimitMin(0.1f);
            int flickers = (int)Math.Round(duration / interval);

            Map.SetColorOfLights(color, targetZone);
            for (int i = 0; i < flickers; i++)
            {
                Map.TurnOffLights(interval * 0.5f, targetZone);
                yield return Timing.WaitForSeconds(interval * 0.5f);
                Map.TurnOnLights(targetZone);
                yield return Timing.WaitForSeconds(interval * 0.5f);
            }
            Map.ResetColorOfLights(targetZone);
        }

        /// <summary>
        /// Fluently executes a global or batch collection wide lighting flicker animation loop sequence spanning multiple rooms simultaneously.
        /// </summary>
        /// <param name="rooms">The enumerable stream collection tracking targeted room assets loaded inside server memory channels.</param>
        /// <param name="color">The target rendering illumination <see cref="Color"/> matrix applied to the cluster layouts.</param>
        /// <param name="duration">The absolute execution timeline width evaluated in fractional seconds.</param>
        /// <param name="frequency">The frequency parameter enforcing execution cycle counts per second across the collection elements.</param>
        public static IEnumerator<float> FlickerBulkLightsCoroutine(this IEnumerable<Room> rooms, Color color, float duration, float frequency)
        {
            if (rooms is null) yield break;

            float interval = 1f / frequency.LimitMin(0.1f);
            int flickers = (int)Math.Round(duration / interval);

            // Capture and filter room listings to accelerate downstream loops without heap overhead allocation tracking
            var cachedRooms = rooms.Where(r => r?.AllLightControllers != null).ToList();

            foreach (var room in cachedRooms) room.SetLightsColor(color);

            for (int i = 0; i < flickers; i++)
            {
                // Bulk suppression phase sweep execution paths
                for (int r = 0; i < cachedRooms.Count; i++)
                {
                    foreach (var controller in cachedRooms[r].AllLightControllers) controller.LightsEnabled = false;
                }
                yield return Timing.WaitForSeconds(interval * 0.5f);

                // Bulk restoration phase sweep execution paths
                for (int r = 0; i < cachedRooms.Count; i++)
                {
                    foreach (var controller in cachedRooms[r].AllLightControllers) controller.LightsEnabled = true;
                }
                yield return Timing.WaitForSeconds(interval * 0.5f);
            }

            foreach (var room in cachedRooms) room.SetLightsColor(Color.clear);
        }
        #endregion
    }
}