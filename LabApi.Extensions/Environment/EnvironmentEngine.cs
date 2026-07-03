using System.Collections.Generic;
using UnityEngine;
using MEC;
using LabApi.Features.Wrappers;

namespace LabApi.Extensions.Environment
{
    /// <summary>
    /// Provides standalone, high-performance asynchronous orchestration engines designed to manipulate 
    /// global environmental states, structural alarms, and multi-stage facility strobe light sequences.
    /// </summary>
    internal static class EnvironmentEngine
    {
        /// <summary>
        /// Launches a non-blocking background asynchronous coroutine loop that flashes the global facility lighting grid 
        /// between a targeted alert color configuration and a total blackout pulse state.
        /// </summary>
        /// <param name="totalDuration">The total lifespan tracking execution timeframe in seconds for the strobe event sequence.</param>
        /// <param name="pulseInterval">The temporal delay spacing index in seconds indicating how rapidly the strobe oscillations occur.</param>
        /// <param name="alertColor">The target <see cref="Color"/> vector applied to the lighting controllers during the active phase pulse.</param>
        /// <param name="masterTag">A unique tracking string token assigned to tag the underlying MEC coroutine layers for clean structural memory cleanup.</param>
        public static void StartEmergencyStrobe(float totalDuration, float pulseInterval, Color alertColor, string masterTag = "Emergency-Strobe")
        {
            for (float t = 0f; t < totalDuration; t += pulseInterval)
            {
                float initialDelay = t;
                var masterCoroutine = Timing.CallDelayed(initialDelay, () =>
                {
                    // Phase 1: Force alert illumination vector color mapping
                    Map.SetColorOfLights(alertColor);

                    // Phase 2: Intermediary collapse into temporary darkness
                    var subCoroutineOff = Timing.CallDelayed(pulseInterval * 0.25f, () => Map.TurnOffLights());
                    subCoroutineOff.Tag = masterTag;

                    // Phase 3: Restore illumination grid and transition into deep ambient shadows
                    var subCoroutineOn = Timing.CallDelayed(pulseInterval * 0.63f, () =>
                    {
                        Map.TurnOnLights();
                        Map.SetColorOfLights(Color.black);
                    });
                    subCoroutineOn.Tag = masterTag;
                });

                masterCoroutine.Tag = masterTag;
            }
        }

        /// <summary>
        /// Launches a non-blocking background asynchronous coroutine loop that flickers the illumination grid 
        /// of a specific facility zone at a given frequency baseline before reverting to pristine spectrum maps.
        /// </summary>
        /// <param name="zone">The targeted structural <see cref="MapGeneration.FacilityZone"/> boundary context undergoing illumination modulation.</param>
        /// <param name="duration">The total active operational execution lifespan tracking window in seconds for the strobe loop.</param>
        /// <param name="frequency">The frequency parameter coefficient determining how many flash cycles execute per second layer.</param>
        /// <param name="color">The structural <see cref="UnityEngine.Color"/> vector mapping layout applied during the active illumination state pulses.</param>
        /// <param name="coroutineTag">An optional custom tracking token string assigned to bound the underlying MEC handle context safely.</param>
        public static void StartZoneFlicker(MapGeneration.FacilityZone zone, float duration, float frequency, UnityEngine.Color color, string coroutineTag = "Zone-Flicker")
        {
            MEC.Timing.RunCoroutine(ExecuteZoneFlickerRoutine(zone, duration, frequency, color), coroutineTag);
        }

        private static System.Collections.Generic.IEnumerator<float> ExecuteZoneFlickerRoutine(MapGeneration.FacilityZone zone, float duration, float frequency, UnityEngine.Color color)
        {
            float interval = 1f / frequency;
            int flickers = UnityEngine.Mathf.RoundToInt(duration / interval);

            LabApi.Features.Wrappers.Map.SetColorOfLights(color, zone);

            for (int i = 0; i < flickers; i++)
            {
                LabApi.Features.Wrappers.Map.TurnOffLights(interval * 0.5f, zone);
                yield return MEC.Timing.WaitForSeconds(interval * 0.5f);
                LabApi.Features.Wrappers.Map.TurnOnLights(zone);
                yield return MEC.Timing.WaitForSeconds(interval * 0.5f);
            }

            LabApi.Features.Wrappers.Map.ResetColorOfLights(zone);
        }
    }
}