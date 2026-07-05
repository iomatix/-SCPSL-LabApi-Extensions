using System;
using System.Collections.Generic;
using LabApi.Features.Wrappers;
using MapGeneration;
using MEC;
using UnityEngine;

namespace LabApi.Extensions
{
    /// <summary>
    /// Provides enterprise-grade extension methods for <see cref="FacilityZone"/> structures,
    /// enabling zone-wide electrical grid illumination overrides and asynchronous strobe animation cascades.
    /// </summary>
    internal static class ZoneExtensions
    {
        #region Zone Illumination Overrides
        /// <summary>
        /// Forcibly suppresses all layout light controllers and connected elevator cabins across an entire facility zone for a designated duration.
        /// </summary>
        /// <param name="zone">The target <see cref="FacilityZone"/> sector dropping power grid connection.</param>
        /// <param name="duration">The chronological tracking timeline window in seconds during which the darkness remains active.</param>
        public static void TurnOffZoneLights(this FacilityZone zone, float duration)
        {
            // Suppress standard room controllers within the targeting zone
            Map.TurnOffLights(duration, zone);

            // DRY Pipeline Integration: Cascade the blackout state straight into all matching elevator cabins inside this zone
            foreach (Elevator elevator in ElevatorExtensions.GetElevatorsInZone(zone))
            {
                elevator.TurnOffLights(duration);
            }
        }

        /// <summary>
        /// Instantly restores electrical power to all light controllers and connected elevator cabins across a specific facility zone.
        /// </summary>
        /// <param name="zone">The target <see cref="FacilityZone"/> sector undergoing electrical grid restoration.</param>
        public static void TurnOnZoneLights(this FacilityZone zone)
        {
            Map.TurnOnLights(zone);

            foreach (Elevator elevator in ElevatorExtensions.GetElevatorsInZone(zone))
            {
                elevator.TurnOnLights();
            }
        }
        #endregion

        #region Zone Animation Pipelines
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
        #endregion
    }
}