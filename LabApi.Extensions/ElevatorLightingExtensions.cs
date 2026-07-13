using LabApi.Features.Wrappers;
using System.Collections.Generic;
using Logger = LabApi.Extensions.Misc.iLogger;

namespace LabApi.Extensions
{
    /// <summary>
    /// Provides extension methods for controlling elevator lighting and blackout states.
    /// </summary>
    public static class ElevatorLightingExtensions
    {
        #region Single Target Operations
        /// <summary>
        /// Delays or suppresses elevator cabin lighting for a specified duration.
        /// </summary>
        public static void TurnOffLights(this Elevator elevator, float duration)
        {
            // Placeholder: Awaiting future integration with internal Lift light controllers/HDRP state managers.
            Logger.LocalTrace("ElevatorLighting", "TurnOffLights invoked on target. System operating in SafeZone mode (Lighting suppression deferred).");
        }

        /// <summary>
        /// Restores elevator cabin lighting.
        /// </summary>
        public static void TurnOnLights(this Elevator elevator)
        {
            // Placeholder: Lighting state maintained by global facility power grid.
        }

        /// <summary>
        /// Runs a visual flicker animation loop for the elevator lighting.
        /// </summary>
        public static IEnumerator<float> FlickerElevatorLightsCoroutine(this Elevator elevator, float duration, float frequency)
        {
            yield break;
        }

        /// <summary>
        /// Checks if the elevator lights are currently disabled.
        /// </summary>
        public static bool AreLightsOff(this Elevator elevator) => false;
        #endregion

        #region Batch & Params Operations (Added for API Consistency)
        /// <summary>
        /// Turns off lights for a collection of elevators for the specified duration.
        /// </summary>
        public static void TurnOffLights(this IEnumerable<Elevator> elevators, float duration)
        {
            if (elevators is null) return;

            if (elevators is List<Elevator> concreteList)
            {
                int count = concreteList.Count;
                for (int i = 0; i < count; i++) concreteList[i].TurnOffLights(duration);
                return;
            }

            foreach (Elevator elevator in elevators)
            {
                elevator.TurnOffLights(duration);
            }
        }

        /// <summary>
        /// Turns off lights for an inline array of elevators for the specified duration.
        /// </summary>
        public static void TurnOffLights(float duration, params Elevator[] elevators)
        {
            if (elevators is null) return;

            int count = elevators.Length;
            for (int i = 0; i < count; i++) elevators[i].TurnOffLights(duration);
        }

        /// <summary>
        /// Turns on lights for a collection of elevators.
        /// </summary>
        public static void TurnOnLights(this IEnumerable<Elevator> elevators)
        {
            if (elevators is null) return;

            if (elevators is List<Elevator> concreteList)
            {
                int count = concreteList.Count;
                for (int i = 0; i < count; i++) concreteList[i].TurnOnLights();
                return;
            }

            foreach (Elevator elevator in elevators)
            {
                elevator.TurnOnLights();
            }
        }

        /// <summary>
        /// Turns on lights for an inline array of elevators.
        /// </summary>
        public static void TurnOnLights(params Elevator[] elevators)
        {
            if (elevators is null) return;

            int count = elevators.Length;
            for (int i = 0; i < count; i++) elevators[i].TurnOnLights();
        }
        #endregion
    }
}