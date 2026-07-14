using LabApi.Features.Wrappers;
using MEC;
using System.Collections.Generic;
using UnityEngine;

namespace LabApi.Extensions
{
    /// <summary>
    /// Highly optimized utility extensions for global facility operations: doors, lights, and generators.
    /// Employs early-exit (short-circuiting) logic and nested state-passing loops to guarantee 0 heap allocations.
    /// </summary>
    public static class MapExtensions
    {
        #region Doors

        /// <summary>
        /// Breaks all breakable doors in the entire facility with zero heap allocations.
        /// </summary>
        public static void BreakAllFacilityDoors()
        {
            if (Room.List == null)
                return;

            Room.List.ForEach(static r => r?.BreakAllDoors());
        }

        #endregion

        #region Lights & Global Animations

        /// <summary>
        /// Enables or disables all lights in the facility with zero heap allocations.
        /// </summary>
        public static void SetAllLightsEnabled(bool enabled)
        {
            if (Room.List == null)
                return;

            Room.List.ForEach(enabled, static (room, stateEnabled) =>
            {
                if (room != null && room.AllLightControllers != null)
                {
                    room.AllLightControllers.ForEach(stateEnabled, static (c, innerEnabled) =>
                    {
                        if (c != null)
                        {
                            c.LightsEnabled = innerEnabled;
                        }
                    });
                }
            });
        }

        /// <summary>
        /// Private lightweight coroutine that drives the global strobe sequence using a single state loop on the stack.
        /// Completely avoids heap-allocation scheduling overhead.
        /// </summary>
        private static IEnumerator<float> EmergencyStrobeCoroutine(float totalDuration, float pulseInterval, Color alertColor)
        {
            float elapsed = 0f;
            float phase1Duration = pulseInterval * 0.25f;
            float phase2Duration = pulseInterval * 0.38f; // (0.63 - 0.25)
            float phase3Duration = pulseInterval * 0.37f; // (1.00 - 0.63)

            while (elapsed < totalDuration)
            {
                Map.SetColorOfLights(alertColor);
                yield return Timing.WaitForSeconds(phase1Duration);

                Map.TurnOffLights();
                yield return Timing.WaitForSeconds(phase2Duration);

                Map.TurnOnLights();
                Map.SetColorOfLights(Color.black);
                yield return Timing.WaitForSeconds(phase3Duration);

                elapsed += pulseInterval;
            }

            Map.ResetColorOfLights();
        }

        /// <summary>
        /// Launches a highly optimized global facility-wide emergency strobe light sequence using a single lightweight coroutine loop.
        /// </summary>
        public static CoroutineHandle StartEmergencyStrobe(
            float totalDuration,
            float pulseInterval,
            Color alertColor,
            string coroutineTag = "LabApi.MapExtensions-emergencyStrobe")
        {
            if (pulseInterval <= 0.05f)
                pulseInterval = 0.1f; // Prevent infinite yield loops

            // FIX: This lives now in MapExtensions as a native, ultra-clean global utility.
            return string.IsNullOrEmpty(coroutineTag)
                ? Timing.RunCoroutine(EmergencyStrobeCoroutine(totalDuration, pulseInterval, alertColor))
                : Timing.RunCoroutine(EmergencyStrobeCoroutine(totalDuration, pulseInterval, alertColor), coroutineTag);
        }

        #endregion

        #region Generators

        /// <summary>
        /// Returns the number of engaged generators.
        /// </summary>
        public static int GetEngagedGeneratorsCount()
        {
            var generators = Generator.List;
            if (generators == null)
                return 0;

            if (generators is List<Generator> list)
            {
                int count = 0;
                int len = list.Count;
                for (int i = 0; i < len; i++)
                {
                    var generator = list[i];
                    if (generator != null && generator.Engaged)
                    {
                        count++;
                    }
                }
                return count;
            }

            int fallbackCount = 0;
            foreach (var generator in generators)
            {
                if (generator != null && generator.Engaged)
                {
                    fallbackCount++;
                }
            }

            return fallbackCount;
        }

        /// <summary>
        /// Returns true if all generators are engaged and the count meets the required minimum.
        /// </summary>
        public static bool AreAllGeneratorsEngaged(int minimumRequiredCount = 3)
        {
            var generators = Generator.List;
            if (generators == null)
                return false;

            int total = generators.Count;
            if (total < minimumRequiredCount)
                return false;

            if (generators is List<Generator> list)
            {
                for (int i = 0; i < total; i++)
                {
                    var generator = list[i];
                    if (generator == null || !generator.Engaged)
                        return false;
                }
                return true;
            }

            foreach (var generator in generators)
            {
                if (generator == null || !generator.Engaged)
                    return false;
            }

            return true;
        }

        #endregion
    }
}