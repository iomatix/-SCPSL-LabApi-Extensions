using LabApi.Features.Wrappers;
using MapGeneration;
using MEC;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LabApi.Extensions
{
    /// <summary>
    /// Highly optimized utility extensions for controlling lighting states and executing synchronized animations in <see cref="FacilityZone"/>.
    /// Leverages underlying room pipeline extensions and native engine controllers with zero closure garbage.
    /// </summary>
    public static class ZoneLightingExtensions
    {
        #region Single-Zone Lights

        /// <summary>
        /// Turns off lights across the specified zone and its connected elevator shafts with zero heap allocations.
        /// </summary>
        /// <param name="zone">The target facility zone.</param>
        /// <param name="duration">The duration in seconds during which lights remain deactivated.</param>
        public static void TurnOffLights(this FacilityZone zone, float duration)
        {
            Map.TurnOffLights(duration, zone);

            if (Elevator.List == null)
                return;

            Elevator.List.ForEach((zone, duration), static (e, state) =>
            {
                if (e != null && ZoneExtensions.IsElevatorInZone(e, state.zone))
                {
                    e.TurnOffLights(state.duration);
                }
            });
        }

        /// <summary>
        /// Restores lights across the specified zone and its connected elevator shafts with zero heap allocations.
        /// </summary>
        /// <param name="zone">The target facility zone.</param>
        public static void TurnOnLights(this FacilityZone zone)
        {
            Map.TurnOnLights(zone);

            if (Elevator.List == null)
                return;

            Elevator.List.ForEach(zone, static (e, z) =>
            {
                if (e != null && ZoneExtensions.IsElevatorInZone(e, z))
                {
                    e.TurnOnLights();
                }
            });
        }

        #endregion

        #region Multi-Zone Lights (IEnumerable + params)

        /// <summary>
        /// Turns off lights across multiple facility zones with zero heap allocations.
        /// </summary>
        /// <param name="zones">The collection of target zones.</param>
        /// <param name="duration">The duration in seconds for which lights remain deactivated.</param>
        public static void TurnOffLights(this IEnumerable<FacilityZone> zones, float duration)
        {
            if (zones == null)
                return;

            zones.ForEach(duration, static (z, dur) => z.TurnOffLights(dur));
        }

        /// <summary>
        /// Turns off lights across multiple facility zones supplied via parameter array.
        /// </summary>
        /// <param name="duration">The duration in seconds for which lights remain deactivated.</param>
        /// <param name="zones">The array of target zones.</param>
        public static void TurnOffLights(float duration, params FacilityZone[] zones)
        {
            if (zones == null)
                return;

            zones.TurnOffLights(duration);
        }

        /// <summary>
        /// Restores lights across multiple facility zones with zero heap allocations.
        /// </summary>
        /// <param name="zones">The collection of target zones.</param>
        public static void TurnOnLights(this IEnumerable<FacilityZone> zones)
        {
            if (zones == null)
                return;

            zones.ForEach(static z => z.TurnOnLights());
        }

        /// <summary>
        /// Restores lights across multiple facility zones supplied via parameter array.
        /// </summary>
        /// <param name="zones">The array of target zones.</param>
        public static void TurnOnLights(params FacilityZone[] zones)
        {
            if (zones == null)
                return;

            zones.TurnOnLights();
        }

        #endregion

        #region Zone Animations (Unified Flicker)

        /// <summary>
        /// Executes a synchronized native pulse flicker animation across all rooms within a specified zone.
        /// Reuses <see cref="RoomLightingExtensions.FlickerLightsBatchCoroutine"/> to enforce DRY and guarantee network safety.
        /// </summary>
        /// <param name="zone">The target facility zone.</param>
        /// <param name="color">The override color applied during the animation window.</param>
        /// <param name="duration">Total animation lifetime in seconds.</param>
        /// <param name="frequency">Cadence frequency in Hertz. Clamped internally to prevent Mirror network buffer overflow.</param>
        /// <returns>A coroutine enumerator controlling the zone-wide pulse progression.</returns>
        public static IEnumerator<float> FlickerLightsCoroutine(this FacilityZone zone, Color color, float duration, float frequency)
        {
            List<Room> zoneRooms = Room.List.Where(r => r != null && r.Zone == zone).ToList();
            if (zoneRooms.Count == 0)
                yield break;

            yield return Timing.WaitUntilDone(Timing.RunCoroutine(zoneRooms.FlickerLightsBatchCoroutine(color, duration, frequency)));
        }

        /// <summary>
        /// Initiates a synchronized, low-overhead native flicker animation across multiple facility zones using a single centralized coroutine.
        /// </summary>
        /// <param name="zones">The collection of target zones undergoing lockdown flicker disruption.</param>
        /// <param name="color">The override color applied during the animation sequence.</param>
        /// <param name="duration">The duration in seconds for the active animation window.</param>
        /// <param name="frequency">The cadence frequency in Hertz. Automatically clamped to network-safe bounds.</param>
        /// <param name="coroutineTag">MEC tracking tag used to cancel or manage active animation coroutines.</param>
        public static void FlickerLights(
            this IEnumerable<FacilityZone> zones,
            Color color,
            float duration,
            float frequency,
            string coroutineTag = RoomLightingExtensions.DefaultFlickerTag)
        {
            if (zones == null)
                return;

            HashSet<FacilityZone> zoneSet = zones as HashSet<FacilityZone> ?? new HashSet<FacilityZone>(zones);
            List<Room> targetRooms = Room.List.Where(r => r != null && zoneSet.Contains(r.Zone)).ToList();

            if (targetRooms.Count > 0)
            {
                targetRooms.FlickerLights(color, duration, frequency, coroutineTag);
            }
        }

        /// <summary>
        /// Initiates a synchronized native flicker animation across multiple facility zones supplied via parameter array.
        /// </summary>
        /// <param name="color">The override color applied during the animation sequence.</param>
        /// <param name="duration">The duration in seconds for the active animation window.</param>
        /// <param name="frequency">The cadence frequency in Hertz.</param>
        /// <param name="coroutineTag">MEC tracking tag used to cancel or manage active animation coroutines.</param>
        /// <param name="zones">The parameter array of target zones.</param>
        public static void FlickerLights(
            Color color,
            float duration,
            float frequency,
            string coroutineTag = RoomLightingExtensions.DefaultFlickerTag,
            params FacilityZone[] zones) =>
            zones.FlickerLights(color, duration, frequency, coroutineTag);

        #endregion
    }
}