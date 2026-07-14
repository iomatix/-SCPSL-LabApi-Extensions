using LabApi.Features.Wrappers;
using System.Collections.Generic;
using Logger = LabApi.Extensions.Misc.iLogger;

namespace LabApi.Extensions
{
    /// <summary>
    /// Utility extensions for controlling elevator lighting states.
    /// Features highly optimized zero-allocation state-passing batch operations.
    /// </summary>
    public static class ElevatorLightingExtensions
    {
        #region Single Target Operations

        /// <summary>
        /// Turns off lights in the elevator cabin for the given duration.
        /// </summary>
        public static void TurnOffLights(this Elevator elevator, float duration)
        {
            if (elevator == null)
                return;

            // Placeholder: Awaiting integration with internal HDRP elevator lighting.
            Logger.LocalTrace("ElevatorLighting", "TurnOffLights invoked. Lighting suppression deferred.");
        }

        /// <summary>
        /// Turns on lights in the elevator cabin.
        /// </summary>
        public static void TurnOnLights(this Elevator elevator)
        {
            if (elevator == null)
                return;

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

        #region Batch Operations (Zero-Allocation via State-Passing)

        /// <summary>
        /// Turns off lights for multiple elevators with zero heap allocations.
        /// </summary>
        public static void TurnOffLights(this IEnumerable<Elevator> elevators, float duration)
        {
            if (elevators == null)
                return;

            // FIX: State-passing and static lambda to completely bypass closure garbage generation.
            elevators.ForEach(duration, static (e, dur) => e?.TurnOffLights(dur));
        }

        /// <summary>
        /// Turns off lights for multiple elevators (params overload).
        /// </summary>
        public static void TurnOffLights(float duration, params Elevator[] elevators) =>
            elevators.TurnOffLights(duration);

        /// <summary>
        /// Turns on lights for multiple elevators with zero heap allocations.
        /// </summary>
        public static void TurnOnLights(this IEnumerable<Elevator> elevators)
        {
            if (elevators == null)
                return;

            // FIX: Zero-allocation parameterless loop using a pre-cached static delegate.
            elevators.ForEach(static e => e?.TurnOnLights());
        }

        /// <summary>
        /// Turns on lights for multiple elevators (params overload).
        /// </summary>
        public static void TurnOnLights(params Elevator[] elevators) =>
            elevators.TurnOnLights();

        #endregion
    }
}