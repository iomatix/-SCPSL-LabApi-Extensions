using LabApi.Features.Wrappers;
using System.Collections.Generic;
using Logger = LabApi.Extensions.Misc.iLogger;

namespace LabApi.Extensions
{
    /// <summary>
    /// Utility extensions for controlling elevator lighting states.
    /// </summary>
    public static class ElevatorLightingExtensions
    {
        #region Single Target Operations

        /// <summary>
        /// Turns off lights in the elevator cabin for the given duration.
        /// </summary>
        public static void TurnOffLights(this Elevator elevator, float duration)
        {
            // Placeholder: Awaiting integration with internal HDRP elevator lighting.
            Logger.LocalTrace("ElevatorLighting", "TurnOffLights invoked. Lighting suppression deferred.");
        }

        /// <summary>
        /// Turns on lights in the elevator cabin.
        /// </summary>
        public static void TurnOnLights(this Elevator elevator)
        {
            // Placeholder: Controlled by global facility power grid.
        }

        /// <summary>
        /// Returns true if the elevator cabin lights are currently disabled.
        /// </summary>
        public static bool AreLightsOff(this Elevator elevator)
        {
            return false;
        }

        /// <summary>
        /// Runs a flicker animation coroutine for elevator lighting.
        /// </summary>
        public static IEnumerator<float> FlickerElevatorLightsCoroutine(this Elevator elevator, float duration, float frequency)
        {
            yield break;
        }

        #endregion

        #region Batch Operations (IEnumerable + params)

        /// <summary>
        /// Turns off lights for multiple elevators.
        /// </summary>
        public static void TurnOffLights(this IEnumerable<Elevator> elevators, float duration)
        {
            if (elevators is null)
                return;

            if (elevators is List<Elevator> list)
            {
                int count = list.Count;
                for (int i = 0; i < count; i++)
                    list[i]?.TurnOffLights(duration);
                return;
            }

            foreach (var elevator in elevators)
                elevator?.TurnOffLights(duration);
        }

        /// <summary>
        /// Turns off lights for multiple elevators (params overload).
        /// </summary>
        public static void TurnOffLights(float duration, params Elevator[] elevators)
            => ((IEnumerable<Elevator>)elevators).TurnOffLights(duration);

        /// <summary>
        /// Turns on lights for multiple elevators.
        /// </summary>
        public static void TurnOnLights(this IEnumerable<Elevator> elevators)
        {
            if (elevators is null)
                return;

            if (elevators is List<Elevator> list)
            {
                int count = list.Count;
                for (int i = 0; i < count; i++)
                    list[i]?.TurnOnLights();
                return;
            }

            foreach (var elevator in elevators)
                elevator?.TurnOnLights();
        }

        /// <summary>
        /// Turns on lights for multiple elevators (params overload).
        /// </summary>
        public static void TurnOnLights(params Elevator[] elevators)
            => ((IEnumerable<Elevator>)elevators).TurnOnLights();

        #endregion
    }
}
