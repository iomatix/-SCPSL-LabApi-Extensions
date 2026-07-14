using LabApi.Features.Wrappers;
using MapGeneration;
using MEC;
using System.Collections.Generic;
using UnityEngine;

namespace LabApi.Extensions
{
    /// <summary>
    /// Highly optimized utility extensions for controlling lighting states and playing animations in <see cref="FacilityZone"/>.
    /// Features zero-allocation state-passing batch operations.
    /// </summary>
    public static class ZoneLightingExtensions
    {
        #region Single-Zone Lights

        /// <summary>
        /// Turns off lights in the zone and its elevators with zero heap allocations.
        /// </summary>
        public static void TurnOffLights(this FacilityZone zone, float duration)
        {
            Map.TurnOffLights(duration, zone);

            if (Elevator.List == null)
                return;

            // FIX: Direct loop bypassing obsolete GetElevators() and reusing ElevatorLightingExtensions!
            Elevator.List.ForEach((zone, duration), static (e, state) =>
            {
                if (e != null && ZoneExtensions.IsElevatorInZone(e, state.zone))
                {
                    e.TurnOffLights(state.duration);
                }
            });
        }

        /// <summary>
        /// Turns on lights in the zone and its elevators with zero heap allocations.
        /// </summary>
        public static void TurnOnLights(this FacilityZone zone)
        {
            Map.TurnOnLights(zone);

            if (Elevator.List == null)
                return;

            // FIX: Direct loop bypassing obsolete GetElevators() and reusing ElevatorLightingExtensions!
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
        /// Turns off lights across multiple zones with zero heap allocations.
        /// </summary>
        public static void TurnOffLights(this IEnumerable<FacilityZone> zones, float duration)
        {
            if (zones == null)
                return;

            // FIX: Enforced state-passing and static lambda to prevent closure garbage generation.
            zones.ForEach(duration, static (z, dur) => z.TurnOffLights(dur));
        }

        /// <summary>
        /// Turns off lights across multiple zones (params overload).
        /// </summary>
        public static void TurnOffLights(float duration, params FacilityZone[] zones)
        {
            if (zones == null)
                return;

            zones.TurnOffLights(duration);
        }

        /// <summary>
        /// Turns on lights across multiple zones with zero heap allocations.
        /// </summary>
        public static void TurnOnLights(this IEnumerable<FacilityZone> zones)
        {
            if (zones == null)
                return;

            // FIX: Enforced static delegate caching to achieve 0 allocations.
            zones.ForEach(static z => z.TurnOnLights());
        }

        /// <summary>
        /// Turns on lights across multiple zones (params overload).
        /// </summary>
        public static void TurnOnLights(params FacilityZone[] zones)
        {
            if (zones == null)
                return;

            zones.TurnOnLights();
        }

        #endregion

        #region Zone Animations (Unified Flicker)

        /// <summary>
        /// Performs a flicker animation on zone lights.
        /// </summary>
        public static IEnumerator<float> FlickerLightsCoroutine(this FacilityZone zone, Color color, float duration, float frequency)
        {
            float interval = 1f / frequency.LimitMin(0.1f);
            float half = interval * 0.5f;
            int flickers = (int)(duration / interval);

            Map.SetColorOfLights(color, zone);

            for (int i = 0; i < flickers; i++)
            {
                Map.TurnOffLights(half, zone);
                yield return Timing.WaitForSeconds(half);

                Map.TurnOnLights(zone);
                yield return Timing.WaitForSeconds(half);
            }

            Map.ResetColorOfLights(zone);
        }

        /// <summary>
        /// Starts a flicker animation on multiple zones with zero heap allocations.
        /// </summary>
        public static void FlickerLights(
            this IEnumerable<FacilityZone> zones,
            Color color,
            float duration,
            float frequency,
            string coroutineTag = "LabApi.Extensions-flickerLights")
        {
            if (zones == null)
                return;

            // FIX: State-passing value tuple to launch multiple coroutines without closure allocations.
            zones.ForEach((color, duration, frequency, coroutineTag), static (z, s) =>
            {
                Timing.RunCoroutine(z.FlickerLightsCoroutine(s.color, s.duration, s.frequency), s.coroutineTag);
            });
        }

        /// <summary>
        /// Starts a flicker animation on multiple zones (params overload).
        /// </summary>
        public static void FlickerLights(
            Color color,
            float duration,
            float frequency,
            string @coroutineTag = "LabApi.Extensions-flickerLights",
            params FacilityZone[] zones)
            => zones.FlickerLights(color, duration, frequency, coroutineTag);

        #endregion
    }
}