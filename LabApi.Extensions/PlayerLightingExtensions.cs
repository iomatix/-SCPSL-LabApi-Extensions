using InventorySystem.Items.Firearms.Attachments;
using LabApi.Features.Wrappers;
using MEC;
using System;
using System.Collections.Generic;

namespace LabApi.Extensions
{
    /// <summary>
    /// Highly optimized modular extensions for player lighting states, darkness tracking and weapon flashlight controls.
    /// Features compile-time zero-allocation paths.
    /// </summary>
    public static class PlayerLightingExtensions
    {
        #region Active Light Source State & Emission

        /// <summary>
        /// Returns true if the player's held item emits light (flashlight or light item).
        /// </summary>
        public static bool HasActiveLightSource(this Player player)
        {
            if (player == null)
                return false;

            var item = player.CurrentItem;
            if (item == null)
                return false;

            // Toggleable light items
            if (item.Base is InventorySystem.Items.ToggleableLights.ToggleableLightItemBase lightItem)
                return lightItem.IsEmittingLight;

            // Firearm flashlight
            if (item is FirearmItem firearm)
                return firearm.FlashlightEnabled && firearm.HasAttachment(AttachmentName.Flashlight);

            return false;
        }

        /// <summary>
        /// Returns true if the player's held light source is currently emitting.
        /// </summary>
        public static bool GetHeldLightSourceState(this Player player)
        {
            if (player == null)
                return false;

            if (player.CurrentItem is LightItem lightItem)
                return lightItem.IsEmitting;

            if (player.CurrentItem is FirearmItem firearm && firearm.HasAttachment(AttachmentName.Flashlight))
                return firearm.FlashlightEnabled;

            return false;
        }

        /// <summary>
        /// Sets the emission state of the player's held light source.
        /// </summary>
        public static void SetHeldLightSourceState(this Player player, bool emit)
        {
            if (player == null)
                return;

            if (player.CurrentItem is LightItem lightItem)
            {
                lightItem.IsEmitting = emit;
                return;
            }

            if (player.CurrentItem is FirearmItem firearm && firearm.HasAttachment(AttachmentName.Flashlight))
            {
                firearm.FlashlightEnabled = emit;
            }
        }

        /// <summary>
        /// Sets the emission state of held light sources for multiple players with zero heap allocations.
        /// </summary>
        public static void SetHeldLightSourceState(this IEnumerable<Player> players, bool emit)
        {
            if (players == null)
                return;

            // FIX: Zero-allocation parameter-passing with static lambda.
            players.ForEach(emit, static (p, e) => p?.SetHeldLightSourceState(e));
        }

        /// <summary>
        /// Sets the emission state of held light sources (params overload).
        /// </summary>
        public static void SetHeldLightSourceState(bool emit, params Player[] players)
            => players.SetHeldLightSourceState(emit);

        #endregion

        #region Darkness & True Sensory Darkness Detection

        /// <summary>
        /// Returns true if the player is in a room with lights turned off.
        /// Fully optimized list loops bypassing struct-boxing.
        /// </summary>
        public static bool IsInDarkRoom(this Player player)
        {
            var room = player?.Room;
            if (room == null || room.AllLightControllers == null)
                return false;

            // FIX: Direct index loops on Lights Controllers lists to bypass IEnumerable boxing allocations.
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

            foreach (var controller in room.AllLightControllers)
            {
                if (controller != null && controller.LightsEnabled)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Returns true if the player is in total darkness (dark room or dark elevator cabin).
        /// Reuses modular Player spatial extensions to minimize code duplication.
        /// </summary>
        public static bool IsInTrueDarkness(this Player player)
        {
            if (player == null)
                return false;

            // FIX: Secure DRY call reusing TryGetNearbyElevatorCabin from PlayerExtensions.
            if (!player.TryGetNearbyElevatorCabin(out var cabin))
                return player.IsInDarkRoom();

            return cabin.AreLightsOff();
        }

        #endregion

        #region Flicker Animations & Coroutines

        /// <summary>
        /// Coroutine that flickers the player's held light source.
        /// </summary>
        public static IEnumerator<float> FlickerHeldLightSourceCoroutine(
            this Player player,
            int flickerCount,
            float delayPerFlicker,
            bool forceOff = false,
            Action<Player, bool> onTickFeedback = null)
        {
            if (player == null || player.GameObject == null)
                yield break;

            var initialType = player.CurrentItem?.GetType();
            if (initialType == null)
                yield break;

            for (int i = 0; i < flickerCount; i++)
            {
                if (!player.IsReady || !player.IsAlive || player.CurrentItem?.GetType() != initialType)
                    break;

                bool isLast = (i == flickerCount - 1);
                onTickFeedback?.Invoke(player, isLast && forceOff);

                player.SetHeldLightSourceState(!player.GetHeldLightSourceState());
                yield return Timing.WaitForSeconds(delayPerFlicker);
            }

            if (forceOff &&
                player.IsReady &&
                player.IsAlive &&
                player.CurrentItem?.GetType() == initialType)
            {
                player.SetHeldLightSourceState(false);
                onTickFeedback?.Invoke(player, true);
            }
        }

        /// <summary>
        /// Starts the flicker coroutine for multiple players with zero heap allocations.
        /// Uses ValueTuple state-passing to completely prevent closure garbage generation.
        /// </summary>
        public static void FlickerHeldLightSource(
            this IEnumerable<Player> players,
            int flickerCount,
            float delayPerFlicker,
            bool forceOff = false,
            Action<Player, bool> onTickFeedback = null,
            string coroutineTag = "LabApi.Extensions-playerFlickerLights")
        {
            if (players == null)
                return;

            players.ForEach((flickerCount, delayPerFlicker, forceOff, onTickFeedback, coroutineTag), static (p, s) =>
            {
                if (p != null)
                {
                    Timing.RunCoroutine(
                        p.FlickerHeldLightSourceCoroutine(s.flickerCount, s.delayPerFlicker, s.forceOff, s.onTickFeedback),
                        s.coroutineTag
                    );
                }
            });
        }

        /// <summary>
        /// Starts the flicker coroutine for multiple players (params overload).
        /// </summary>
        public static void FlickerHeldLightSource(
            int flickerCount,
            float delayPerFlicker,
            bool forceOff = false,
            Action<Player, bool> onTickFeedback = null,
            string coroutineTag = "LabApi.Extensions-playerFlickerLights",
            params Player[] players)
            => players.FlickerHeldLightSource(flickerCount, delayPerFlicker, forceOff, onTickFeedback, coroutineTag);

        #endregion
    }
}