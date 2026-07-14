using LabApi.Features.Wrappers;
using MEC;
using System.Collections.Generic;
using UnityEngine;

namespace LabApi.Extensions
{
    /// <summary>
    /// Highly optimized extensions for controlling room lighting states and executing flicker animations.
    /// Reuses core Room spatial extensions to minimize allocation paths.
    /// </summary>
    public static class RoomLightingExtensions
    {
        #region Single Room Operations

        /// <summary>
        /// Turns off lights in the room for a given duration.
        /// </summary>
        public static void TurnOffLights(this Room room, float duration)
        {
            if (room == null || room.AllLightControllers == null)
                return;

            room.AllLightControllers.ForEach(duration, static (lc, dur) => lc.FlickerLights(dur));
        }

        /// <summary>
        /// Turns on lights in the room and optionally flickers them.
        /// </summary>
        public static void TurnOnLights(this Room room, float flickerDuration = 0f)
        {
            if (room == null || room.AllLightControllers == null)
                return;

            room.AllLightControllers.ForEach(flickerDuration, static (lc, dur) => lc.FlickerLights(dur));
        }

        /// <summary>
        /// Turns off lights in the room and its neighbors. Reuses bezalokacyjne propagation helper.
        /// </summary>
        public static void TurnOffRoomAndNeighborLights(this Room room, float duration) =>
            room.ExecuteActionOnRoomAndNeighbors(duration, static (r, dur) => r.TurnOffLights(dur));

        /// <summary>
        /// Turns on lights in the room and its neighbors. Reuses bezalokacyjne propagation helper.
        /// </summary>
        public static void TurnOnRoomAndNeighborLights(this Room room, float duration = 0f) =>
            room.ExecuteActionOnRoomAndNeighbors(duration, static (r, dur) => r.TurnOnLights(dur));

        /// <summary>
        /// Sets the light color for the room.
        /// </summary>
        public static void SetLightsColor(this Room room, Color color)
        {
            if (room == null || room.LightController == null)
                return;

            room.LightController.OverrideLightsColor = color;
        }

        #endregion

        #region Batch Operations (Zero-Allocation via State-Passing)

        /// <summary>
        /// Sets light color for multiple rooms with 0 allocations.
        /// </summary>
        public static void SetLightsColor(this IEnumerable<Room> rooms, Color color)
        {
            if (rooms == null)
                return;

            rooms.ForEach(color, static (r, col) => r?.SetLightsColor(col));
        }

        /// <summary>
        /// Sets light color for multiple rooms (params overload).
        /// </summary>
        public static void SetLightsColor(Color color, params Room[] rooms) =>
            rooms.SetLightsColor(color);

        /// <summary>
        /// Turns off lights for multiple rooms with 0 allocations.
        /// </summary>
        public static void TurnOffLights(this IEnumerable<Room> rooms, float duration)
        {
            if (rooms == null)
                return;

            rooms.ForEach(duration, static (r, dur) => r?.TurnOffLights(dur));
        }

        /// <summary>
        /// Turns off lights for multiple rooms (params overload).
        /// </summary>
        public static void TurnOffLights(float duration, params Room[] rooms) =>
            rooms.TurnOffLights(duration);

        /// <summary>
        /// Turns on lights for multiple rooms with 0 allocations.
        /// </summary>
        public static void TurnOnLights(this IEnumerable<Room> rooms, float flickerDuration = 0f)
        {
            if (rooms == null)
                return;

            rooms.ForEach(flickerDuration, static (r, dur) => r?.TurnOnLights(dur));
        }

        /// <summary>
        /// Turns on lights for multiple rooms (params overload).
        /// </summary>
        public static void TurnOnLights(float flickerDuration, params Room[] rooms) =>
            rooms.TurnOnLights(flickerDuration);

        #endregion

        #region Flicker Animations & Coroutines

        /// <summary>
        /// Executes a flicker animation on the room lights with 0 array allocation in standard lists/arrays.
        /// </summary>
        public static IEnumerator<float> FlickerLightsCoroutine(this Room room, Color color, float duration, float frequency)
        {
            if (room == null || room.AllLightControllers == null)
                yield break;

            float interval = 1f / frequency.LimitMin(0.1f);
            float half = interval * 0.5f;
            int flickers = (int)(duration / interval);

            room.SetLightsColor(color);

            // FIX: Prevent array allocation and copying inside the coroutine loop for common collections.
            if (room.AllLightControllers is List<LightsController> list)
            {
                int count = list.Count;
                for (int i = 0; i < flickers; i++)
                {
                    for (int c = 0; c < count; c++)
                        list[c].LightsEnabled = false;

                    yield return Timing.WaitForSeconds(half);

                    for (int c = 0; c < count; c++)
                        list[c].LightsEnabled = true;

                    yield return Timing.WaitForSeconds(half);
                }
            }
            else if (room.AllLightControllers is LightsController[] array)
            {
                int count = array.Length;
                for (int i = 0; i < flickers; i++)
                {
                    for (int c = 0; c < count; c++)
                        array[c].LightsEnabled = false;

                    yield return Timing.WaitForSeconds(half);

                    for (int c = 0; c < count; c++)
                        array[c].LightsEnabled = true;

                    yield return Timing.WaitForSeconds(half);
                }
            }
            else
            {
                // Fallback for general IEnumerable (extremely rare)
                var fallbackList = new List<LightsController>(room.AllLightControllers);
                int count = fallbackList.Count;
                for (int i = 0; i < flickers; i++)
                {
                    for (int c = 0; c < count; c++)
                        fallbackList[c].LightsEnabled = false;

                    yield return Timing.WaitForSeconds(half);

                    for (int c = 0; c < count; c++)
                        fallbackList[c].LightsEnabled = true;

                    yield return Timing.WaitForSeconds(half);
                }
            }

            room.SetLightsColor(Color.clear);
        }

        /// <summary>
        /// Starts a flicker animation on multiple rooms with 0 allocations.
        /// </summary>
        public static void FlickerLights(
            this IEnumerable<Room> rooms,
            Color color,
            float duration,
            float frequency,
            string coroutineTag = "LabApi.Extensions-flickerLights")
        {
            if (rooms == null)
                return;

            rooms.ForEach((color, duration, frequency, coroutineTag), static (r, s) =>
            {
                if (r != null)
                {
                    Timing.RunCoroutine(r.FlickerLightsCoroutine(s.color, s.duration, s.frequency), s.coroutineTag);
                }
            });
        }

        /// <summary>
        /// Starts a flicker animation on multiple rooms (params overload).
        /// </summary>
        public static void FlickerLights(
            Color color,
            float duration,
            float frequency,
            string coroutineTag = "LabApi.Extensions-flickerLights",
            params Room[] rooms) =>
            rooms.FlickerLights(color, duration, frequency, coroutineTag);

        #endregion
    }
}