using LabApi.Features.Wrappers;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LabApi.Extensions
{
    /// <summary>
    /// Represents an immutable, zero-allocation snapshot capturing a room's physical power status and active color override.
    /// Used by automated orchestration systems to preserve ambient lighting states across transient facility disruptions.
    /// </summary>
    public readonly struct RoomLightSnapshot : IEquatable<RoomLightSnapshot>
    {
        /// <summary>
        /// Gets the active ambient override light color at the time the snapshot was taken.
        /// Evaluates to <see cref="Color.clear"/> if default room profile lighting was active.
        /// </summary>
        public Color OverrideColor { get; }

        /// <summary>
        /// Gets a value indicating whether the electrical grid feeding the room light controller was active and powered.
        /// Evaluates to <see langword="false"/> if lights were disabled by a blackout, EMP, or containment surge.
        /// </summary>
        public bool IsPowered { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoomLightSnapshot"/> struct.
        /// </summary>
        /// <param name="overrideColor">The captured override color spectrum.</param>
        /// <param name="isPowered">Whether the room light infrastructure was actively powered.</param>
        public RoomLightSnapshot(Color overrideColor, bool isPowered)
        {
            OverrideColor = overrideColor;
            IsPowered = isPowered;
        }

        /// <inheritdoc />
        public bool Equals(RoomLightSnapshot other) =>
            OverrideColor.Equals(other.OverrideColor) && IsPowered == other.IsPowered;

        /// <inheritdoc />
        public override bool Equals(object obj) =>
            obj is RoomLightSnapshot other && Equals(other);

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return (OverrideColor.GetHashCode() * 397) ^ IsPowered.GetHashCode();
            }
        }

        /// <summary>
        /// Checks two <see cref="RoomLightSnapshot"/> structures for equality.
        /// </summary>
        public static bool operator ==(RoomLightSnapshot left, RoomLightSnapshot right) => left.Equals(right);

        /// <summary>
        /// Checks two <see cref="RoomLightSnapshot"/> structures for inequality.
        /// </summary>
        public static bool operator !=(RoomLightSnapshot left, RoomLightSnapshot right) => !left.Equals(right);
    }

    /// <summary>
    /// Highly optimized extensions for controlling room lighting states, executing server-friendly flicker animations,
    /// and inspecting electrical grid telemetry without allocations.
    /// </summary>
    public static class RoomLightingExtensions
    {
        #region Constants & Safety Limits
        /// <summary>
        /// Minimum allowable frequency in Hertz to avoid zero-division and infinite delay states.
        /// </summary>
        public const float MinimumFrequency = 0.1f;

        /// <summary>
        /// Maximum safe frequency in Hertz. Constrains network SyncVar / RPC dispatch rates to prevent Mirror buffer saturation.
        /// </summary>
        public const float MaximumSafeFrequency = 1.2f;

        /// <summary>
        /// Default coroutine tag assigned to room light flicker operations.
        /// </summary>
        public const string DefaultFlickerTag = "LabApi.Extensions-flickerLights";
        #endregion

        #region Telemetry & State Inspection

        /// <summary>
        /// Verifies whether the lighting infrastructure in the specified room currently has active electrical power.
        /// </summary>
        /// <param name="room">The target room wrapper instance.</param>
        /// <returns>
        /// <see langword="true"/> if at least one light controller in the room reports active power feed (<see cref="LightsController.LightsEnabled"/> is <see langword="true"/>);
        /// otherwise, <see langword="false"/> (indicating an active blackout from anomalies such as SCP-575, SCP-079, or warhead surges).
        /// </returns>
        public static bool IsLightingPowered(this Room room)
        {
            if (room?.AllLightControllers == null)
                return false;

            foreach (var controller in room.AllLightControllers)
            {
                if (controller != null && controller.LightsEnabled)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Retrieves the current ambient override light color assigned to the specified room.
        /// </summary>
        /// <param name="room">The target room wrapper instance.</param>
        /// <returns>
        /// The currently active <see cref="Color"/> override, or <see cref="Color.clear"/> if no custom spectrum is applied or no controllers exist.
        /// </returns>
        public static Color GetOverrideLightsColor(this Room room)
        {
            if (room == null || room.LightController == null)
                return Color.clear;

            return room.LightController.OverrideLightsColor;
        }

        /// <summary>
        /// Captures an atomic, zero-heap-allocation snapshot of the room's current electrical and color override state.
        /// </summary>
        /// <param name="room">The target room instance to sample.</param>
        /// <returns>A stack-allocated <see cref="RoomLightSnapshot"/> structure representing current lighting parameters.</returns>
        public static RoomLightSnapshot CaptureLightSnapshot(this Room room)
        {
            if (room == null || room.LightController == null)
                return new RoomLightSnapshot(Color.clear, isPowered: false);

            return new RoomLightSnapshot(room.LightController.OverrideLightsColor, room.IsLightingPowered());
        }

        /// <summary>
        /// Restores a room's ambient light override color to match a previously sampled <see cref="RoomLightSnapshot"/>.
        /// Note: This method purposely preserves existing blackout conditions and does NOT force lights on if the grid remains unpowered.
        /// </summary>
        /// <param name="room">The target room instance to restore.</param>
        /// <param name="snapshot">The source snapshot containing the target override color profile.</param>
        public static void RestoreLightSnapshot(this Room room, RoomLightSnapshot snapshot)
        {
            if (room == null || room.LightController == null)
                return;

            room.LightController.OverrideLightsColor = snapshot.OverrideColor;
        }

        /// <summary>
        /// Restores ambient light override colors across a collection of paired rooms and snapshots with zero delegate allocations.
        /// </summary>
        /// <param name="snapshots">A collection of key-value pairs linking rooms to their respective snapshots.</param>
        public static void RestoreLightSnapshots(this IEnumerable<KeyValuePair<Room, RoomLightSnapshot>> snapshots)
        {
            if (snapshots == null)
                return;

            foreach (var kvp in snapshots)
            {
                kvp.Key?.RestoreLightSnapshot(kvp.Value);
            }
        }

        #endregion

        #region Single Room Operations

        /// <summary>
        /// Turns off lights in the specified room for a set duration using the native engine flicker pipeline.
        /// </summary>
        /// <param name="room">The target room wrapper instance.</param>
        /// <param name="duration">The total duration in seconds for which the lights remain off.</param>
        public static void TurnOffLights(this Room room, float duration)
        {
            if (room == null || room.AllLightControllers == null)
                return;

            room.AllLightControllers.ForEach(duration, static (lc, dur) => lc.FlickerLights(dur));
        }

        /// <summary>
        /// Turns on lights in the specified room and optionally initiates an introductory native flicker effect.
        /// </summary>
        /// <param name="room">The target room wrapper instance.</param>
        /// <param name="flickerDuration">The duration in seconds of the introductory native flicker effect. Defaults to 0.</param>
        public static void TurnOnLights(this Room room, float flickerDuration = 0f)
        {
            if (room == null || room.AllLightControllers == null)
                return;

            room.AllLightControllers.ForEach(flickerDuration, static (lc, dur) => lc.FlickerLights(dur));
        }

        /// <summary>
        /// Turns off lights in the specified room and all directly adjacent neighboring rooms.
        /// </summary>
        /// <param name="room">The origin room instance.</param>
        /// <param name="duration">The duration in seconds during which lighting remains disabled.</param>
        public static void TurnOffRoomAndNeighborLights(this Room room, float duration) =>
            room.ExecuteActionOnRoomAndNeighbors(duration, static (r, dur) => r.TurnOffLights(dur));

        /// <summary>
        /// Turns on lights in the specified room and all directly adjacent neighboring rooms.
        /// </summary>
        /// <param name="room">The origin room instance.</param>
        /// <param name="duration">The duration in seconds for the optional startup flicker sequence.</param>
        public static void TurnOnRoomAndNeighborLights(this Room room, float duration = 0f) =>
            room.ExecuteActionOnRoomAndNeighbors(duration, static (r, dur) => r.TurnOnLights(dur));

        /// <summary>
        /// Sets the ambient override light color for the specified room.
        /// </summary>
        /// <param name="room">The target room instance.</param>
        /// <param name="color">The target color spectrum applied to the room lighting controllers.</param>
        public static void SetLightsColor(this Room room, Color color)
        {
            if (room == null || room.LightController == null)
                return;

            room.LightController.OverrideLightsColor = color;
        }

        #endregion

        #region Batch Operations (Zero-Allocation via State-Passing)

        /// <summary>
        /// Applies an override light color across a collection of rooms with zero closure allocations.
        /// </summary>
        /// <param name="rooms">The collection of target rooms.</param>
        /// <param name="color">The color spectrum to apply.</param>
        public static void SetLightsColor(this IEnumerable<Room> rooms, Color color)
        {
            if (rooms == null)
                return;

            rooms.ForEach(color, static (r, col) => r?.SetLightsColor(col));
        }

        /// <summary>
        /// Applies an override light color across multiple rooms supplied via parameter array.
        /// </summary>
        /// <param name="color">The color spectrum to apply.</param>
        /// <param name="rooms">The array of rooms to modify.</param>
        public static void SetLightsColor(Color color, params Room[] rooms) =>
            rooms.SetLightsColor(color);

        /// <summary>
        /// Turns off lights across a collection of rooms for a specified duration with zero heap allocations.
        /// </summary>
        /// <param name="rooms">The collection of target rooms.</param>
        /// <param name="duration">The duration in seconds for which lights remain disabled.</param>
        public static void TurnOffLights(this IEnumerable<Room> rooms, float duration)
        {
            if (rooms == null)
                return;

            rooms.ForEach(duration, static (r, dur) => r?.TurnOffLights(dur));
        }

        /// <summary>
        /// Turns off lights across multiple rooms supplied via parameter array.
        /// </summary>
        /// <param name="duration">The duration in seconds for which lights remain disabled.</param>
        /// <param name="rooms">The array of rooms to modify.</param>
        public static void TurnOffLights(float duration, params Room[] rooms) =>
            rooms.TurnOffLights(duration);

        /// <summary>
        /// Turns on lights across a collection of rooms with zero heap allocations.
        /// </summary>
        /// <param name="rooms">The collection of target rooms.</param>
        /// <param name="flickerDuration">Optional startup flicker duration in seconds.</param>
        public static void TurnOnLights(this IEnumerable<Room> rooms, float flickerDuration = 0f)
        {
            if (rooms == null)
                return;

            rooms.ForEach(flickerDuration, static (r, dur) => r?.TurnOnLights(dur));
        }

        /// <summary>
        /// Turns on lights across multiple rooms supplied via parameter array.
        /// </summary>
        /// <param name="flickerDuration">Optional startup flicker duration in seconds.</param>
        /// <param name="rooms">The array of rooms to modify.</param>
        public static void TurnOnLights(float flickerDuration, params Room[] rooms) =>
            rooms.TurnOnLights(flickerDuration);

        #endregion

        #region Flicker Animations & Coroutines

        /// <summary>
        /// Executes a synchronized native pulse flicker animation across an individual room.
        /// Utilizes the native engine light controller pipelines and clamps dispatch frequency to preserve server TPS.
        /// </summary>
        /// <param name="room">The target room to animate.</param>
        /// <param name="color">The ambient override color applied during the flicker sequence.</param>
        /// <param name="duration">The total lifetime of the flicker sequence in seconds.</param>
        /// <param name="frequency">The desired cadence frequency in Hertz. Automatically clamped between <see cref="MinimumFrequency"/> and <see cref="MaximumSafeFrequency"/>.</param>
        /// <returns>A coroutine enumerator managing the temporal pulse progression.</returns>
        public static IEnumerator<float> FlickerLightsCoroutine(this Room room, Color color, float duration, float frequency)
        {
            if (room == null)
                yield break;

            yield return Timing.WaitUntilDone(Timing.RunCoroutine(new[] { room }.FlickerLightsBatchCoroutine(color, duration, frequency)));
        }

        /// <summary>
        /// Executes a unified, single-coroutine batch pulse flicker animation across multiple rooms.
        /// Enforces native engine light triggers at the specified cadence while shielding the Mirror networking pipeline from packet saturation.
        /// </summary>
        /// <param name="rooms">The collection of rooms to animate.</param>
        /// <param name="color">The ambient override color active throughout the sequence.</param>
        /// <param name="duration">Total animation lifetime in seconds.</param>
        /// <param name="frequency">The rhythm frequency in Hertz. Clamped internally to protect network buffers.</param>
        /// <returns>A unified coroutine enumerator controlling all target room lights simultaneously.</returns>
        public static IEnumerator<float> FlickerLightsBatchCoroutine(this IEnumerable<Room> rooms, Color color, float duration, float frequency)
        {
            if (rooms == null || duration <= 0f)
                yield break;

            List<Room> targetRooms = rooms as List<Room> ?? new List<Room>(rooms);
            int roomCount = targetRooms.Count;
            if (roomCount == 0)
                yield break;

            float clampedFrequency = Mathf.Clamp(frequency, MinimumFrequency, MaximumSafeFrequency);
            float interval = 1f / clampedFrequency;
            float pulseDuration = Mathf.Min(interval * 0.45f, 0.6f);

            for (int r = 0; r < roomCount; r++)
            {
                targetRooms[r]?.SetLightsColor(color);
            }

            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float remainingTime = duration - elapsedTime;
                float currentPulse = Mathf.Min(pulseDuration, remainingTime);

                for (int r = 0; r < roomCount; r++)
                {
                    Room room = targetRooms[r];
                    if (room?.AllLightControllers == null)
                        continue;

                    room.AllLightControllers.ForEach(currentPulse, static (lc, pDur) => lc.FlickerLights(pDur));
                }

                float sleepDuration = Mathf.Min(interval, remainingTime);
                elapsedTime += sleepDuration;

                yield return Timing.WaitForSeconds(sleepDuration);
            }

            for (int r = 0; r < roomCount; r++)
            {
                targetRooms[r]?.SetLightsColor(Color.clear);
            }
        }

        /// <summary>
        /// Initiates a synchronized, low-overhead native flicker animation across multiple rooms using a single centralized coroutine.
        /// </summary>
        /// <param name="rooms">The collection of target rooms undergoing visual containment disruption.</param>
        /// <param name="color">The override color displayed during active flicker intervals.</param>
        /// <param name="duration">The operational window of the flicker effect in seconds.</param>
        /// <param name="frequency">The pulse cadence in Hertz. Clamped to safe thresholds to guarantee packet stability.</param>
        /// <param name="coroutineTag">MEC tracking tag used to manage or cancel active coroutine pipelines.</param>
        public static void FlickerLights(
            this IEnumerable<Room> rooms,
            Color color,
            float duration,
            float frequency,
            string coroutineTag = DefaultFlickerTag)
        {
            if (rooms == null)
                return;

            Timing.RunCoroutine(rooms.FlickerLightsBatchCoroutine(color, duration, frequency), coroutineTag);
        }

        /// <summary>
        /// Initiates a synchronized native flicker animation across multiple rooms supplied via parameter array.
        /// </summary>
        /// <param name="color">The override color displayed during active flicker intervals.</param>
        /// <param name="duration">The operational window of the flicker effect in seconds.</param>
        /// <param name="frequency">The pulse cadence in Hertz.</param>
        /// <param name="coroutineTag">MEC tracking tag used to manage or cancel active coroutine pipelines.</param>
        /// <param name="rooms">The parameter array of target rooms.</param>
        public static void FlickerLights(
            Color color,
            float duration,
            float frequency,
            string coroutineTag = DefaultFlickerTag,
            params Room[] rooms) =>
            rooms.FlickerLights(color, duration, frequency, coroutineTag);

        #endregion
    }
}