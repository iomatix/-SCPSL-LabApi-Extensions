using InventorySystem.Items.Firearms.Attachments;
using LabApi.Features.Wrappers;
using MEC;
using System;
using System.Collections.Generic;

namespace LabApi.Extensions
{
    /// <summary>
    /// Highly optimized modular extensions for player lighting states, darkness tracking, and weapon flashlight controls.
    /// Features compile-time zero-allocation hot paths, unique item serial verification, and network-safe coroutine cadences.
    /// </summary>
    public static class PlayerLightingExtensions
    {
        #region Constants & Operational Boundaries
        /// <summary>
        /// Base prefix applied to player-specific light flicker coroutine tracking tags.
        /// </summary>
        public const string PlayerFlickerTagPrefix = "LabApi.Extensions-playerFlickerLights";

        /// <summary>
        /// Absolute minimum safe delay in seconds enforced between successive light flicker pulses to prevent Mirror packet flooding.
        /// </summary>
        public const float MinimumFlickerDelay = 0.05f;
        #endregion

        #region Active Light Source State & Emission

        /// <summary>
        /// Determines whether the player's currently held item is an active, emitting light source (e.g., powered flashlight or enabled firearm attachment).
        /// </summary>
        /// <param name="player">The target player wrapper instance.</param>
        /// <returns><see langword="true"/> if the held item is emitting light; otherwise, <see langword="false"/>.</returns>
        public static bool HasActiveLightSource(this Player player)
        {
            if (player == null)
                return false;

            var item = player.CurrentItem;
            if (item == null)
                return false;

            if (item is LightItem lightItem)
                return lightItem.IsEmitting;

            if (item.Base is InventorySystem.Items.ToggleableLights.ToggleableLightItemBase baseLight)
                return baseLight.IsEmittingLight;

            if (item is FirearmItem firearm)
                return firearm.FlashlightEnabled && firearm.HasAttachment(AttachmentName.Flashlight);

            return false;
        }

        /// <summary>
        /// Retrieves the emission state of the player's currently equipped light source.
        /// </summary>
        /// <param name="player">The target player wrapper instance.</param>
        /// <returns><see langword="true"/> if the held light emitter is active; otherwise, <see langword="false"/>.</returns>
        public static bool GetHeldLightSourceState(this Player player)
        {
            if (player == null)
                return false;

            var item = player.CurrentItem;
            if (item == null)
                return false;

            if (item is LightItem lightItem)
                return lightItem.IsEmitting;

            if (item is FirearmItem firearm && firearm.HasAttachment(AttachmentName.Flashlight))
                return firearm.FlashlightEnabled;

            return false;
        }

        /// <summary>
        /// Sets the emission state of the player's currently equipped light source (standard flashlight or firearm attachment).
        /// </summary>
        /// <param name="player">The target player wrapper instance.</param>
        /// <param name="emit">The desired emission state.</param>
        public static void SetHeldLightSourceState(this Player player, bool emit)
        {
            if (player == null)
                return;

            var item = player.CurrentItem;
            if (item == null)
                return;

            if (item is LightItem lightItem)
            {
                lightItem.IsEmitting = emit;
                return;
            }

            if (item is FirearmItem firearm && firearm.HasAttachment(AttachmentName.Flashlight))
            {
                firearm.FlashlightEnabled = emit;
            }
        }

        /// <summary>
        /// Sets the emission state of held light sources across multiple players with zero closure heap allocations.
        /// </summary>
        /// <param name="players">The collection of target players.</param>
        /// <param name="emit">The desired emission state.</param>
        public static void SetHeldLightSourceState(this IEnumerable<Player> players, bool emit)
        {
            if (players == null)
                return;

            players.ForEach(emit, static (p, e) => p?.SetHeldLightSourceState(e));
        }

        /// <summary>
        /// Sets the emission state of held light sources across multiple players supplied via parameter array.
        /// </summary>
        /// <param name="emit">The desired emission state.</param>
        /// <param name="players">The parameter array of target players.</param>
        public static void SetHeldLightSourceState(bool emit, params Player[] players)
            => players.SetHeldLightSourceState(emit);

        #endregion

        #region Darkness & True Sensory Darkness Detection

        /// <summary>
        /// Determines whether the player is currently inside a room where electrical lighting is completely disabled.
        /// Fully optimized list loops bypassing struct-boxing.
        /// </summary>
        /// <param name="player">The target player wrapper instance.</param>
        /// <returns><see langword="true"/> if the current room has disabled light controllers; otherwise, <see langword="false"/>.</returns>
        public static bool IsInDarkRoom(this Player player)
        {
            var room = player?.Room;
            if (room == null || room.AllLightControllers == null)
                return false;

            if (room.AllLightControllers is List<LightsController> list)
            {
                int count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    var controller = list[i];
                    if (controller != null && controller.LightsEnabled)
                        return false;
                }
                return true;
            }

            if (room.AllLightControllers is LightsController[] array)
            {
                int count = array.Length;
                for (int i = 0; i < count; i++)
                {
                    var controller = array[i];
                    if (controller != null && controller.LightsEnabled)
                        return false;
                }
                return true;
            }

            foreach (var controller in room.AllLightControllers)
            {
                if (controller != null && controller.LightsEnabled)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Determines whether the player is enveloped in total environmental darkness (either inside an unlit room or an unlit elevator cabin).
        /// </summary>
        /// <param name="player">The target player wrapper instance.</param>
        /// <returns><see langword="true"/> if the environmental enclosure is pitch dark; otherwise, <see langword="false"/>.</returns>
        public static bool IsInTrueDarkness(this Player player)
        {
            if (player == null)
                return false;

            if (!player.TryGetNearbyElevatorCabin(out var cabin))
                return player.IsInDarkRoom();

            return cabin.AreLightsOff();
        }

        #endregion

        #region Flicker Animations & Coroutines

        /// <summary>
        /// Executes an isolated flicker coroutine on the player's held light source.
        /// Tracks the exact physical item instance via its unique serial number to prevent state contamination on weapon swapping.
        /// </summary>
        /// <param name="player">The target player instance.</param>
        /// <param name="flickerCount">The total number of state toggle intervals.</param>
        /// <param name="delayPerFlicker">The delay between state toggles in seconds. Automatically clamped to <see cref="MinimumFlickerDelay"/>.</param>
        /// <param name="forceOff">If set to <see langword="true"/>, the light will remain forcibly turned off upon coroutine completion.</param>
        /// <param name="onTickFeedback">Optional callback dispatched on each tick, passing the player and whether the final force-off state has been reached.</param>
        /// <returns>A coroutine enumerator controlling the flicker progression.</returns>
        public static IEnumerator<float> FlickerHeldLightSourceCoroutine(
            this Player player,
            int flickerCount,
            float delayPerFlicker,
            bool forceOff = false,
            Action<Player, bool> onTickFeedback = null)
        {
            if (player == null || player.GameObject == null)
                yield break;

            var currentItem = player.CurrentItem;
            if (currentItem == null)
                yield break;

            ushort initialSerial = currentItem.Serial;
            float clampedDelay = Math.Max(delayPerFlicker, MinimumFlickerDelay);

            for (int i = 0; i < flickerCount; i++)
            {
                if (!player.IsReady || !player.IsAlive || player.CurrentItem?.Serial != initialSerial)
                    yield break;

                bool isLast = (i == flickerCount - 1);
                onTickFeedback?.Invoke(player, isLast && forceOff);

                player.SetHeldLightSourceState(!player.GetHeldLightSourceState());
                yield return Timing.WaitForSeconds(clampedDelay);
            }

            if (forceOff && player.IsReady && player.IsAlive && player.CurrentItem?.Serial == initialSerial)
            {
                player.SetHeldLightSourceState(false);
                onTickFeedback?.Invoke(player, true);
            }
        }

        /// <summary>
        /// Initiates a synchronized flicker sequence on held light sources for multiple players with zero heap allocations.
        /// Generates deterministic per-player tracking tags to eliminate coroutine race conditions.
        /// </summary>
        /// <param name="players">The collection of target players.</param>
        /// <param name="flickerCount">The number of flicker state changes.</param>
        /// <param name="delayPerFlicker">Delay in seconds between state toggles.</param>
        /// <param name="forceOff">If <see langword="true"/>, the light source is left disabled after completion.</param>
        /// <param name="onTickFeedback">Optional per-tick callback invoked with the player and terminal status.</param>
        public static void FlickerHeldLightSource(
            this IEnumerable<Player> players,
            int flickerCount,
            float delayPerFlicker,
            bool forceOff = false,
            Action<Player, bool> onTickFeedback = null)
        {
            if (players == null)
                return;

            players.ForEach((flickerCount, delayPerFlicker, forceOff, onTickFeedback), static (p, s) =>
            {
                if (p != null && p.IsReady && p.IsAlive)
                {
                    string uniqueTag = $"{PlayerFlickerTagPrefix}-{p.PlayerId}";
                    Timing.KillCoroutines(uniqueTag);
                    Timing.RunCoroutine(
                        p.FlickerHeldLightSourceCoroutine(s.flickerCount, s.delayPerFlicker, s.forceOff, s.onTickFeedback),
                        uniqueTag
                    );
                }
            });
        }

        /// <summary>
        /// Initiates a flicker sequence on held light sources for multiple players supplied via parameter array.
        /// </summary>
        /// <param name="flickerCount">The number of flicker state changes.</param>
        /// <param name="delayPerFlicker">Delay in seconds between state toggles.</param>
        /// <param name="forceOff">If <see langword="true"/>, the light source is left disabled after completion.</param>
        /// <param name="onTickFeedback">Optional per-tick callback invoked with the player and terminal status.</param>
        /// <param name="players">The parameter array of target players.</param>
        public static void FlickerHeldLightSource(
            int flickerCount,
            float delayPerFlicker,
            bool forceOff = false,
            Action<Player, bool> onTickFeedback = null,
            params Player[] players)
            => players.FlickerHeldLightSource(flickerCount, delayPerFlicker, forceOff, onTickFeedback);

        #endregion
    }
}