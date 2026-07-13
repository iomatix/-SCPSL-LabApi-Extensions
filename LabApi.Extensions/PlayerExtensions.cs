using InventorySystem.Items.Firearms.Attachments;
using LabApi.Extensions.Components;
using LabApi.Features.Wrappers;
using MapGeneration;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LabApi.Extensions
{
    /// <summary>
    /// Provides extension methods for player tracking, notifications, spatial queries, and batch state updates.
    /// </summary>
    public static class PlayerExtensions
    {
        #region Tracking
        /// <summary>
        /// Attaches a tracking GameObject to a single player using a transform tracker component.
        /// </summary>
        public static void AttachTrackingObject(this Player player, GameObject followerObject, Vector3 offset = default)
        {
            if (player?.GameObject is null || followerObject is null) return;

            var tracker = followerObject.GetComponent<RuntimeTransformTracker>() ?? followerObject.AddComponent<RuntimeTransformTracker>();
            tracker.Setup(player.GameObject.transform, offset, () => player.IsReady && player.IsAlive);
        }

        /// <summary>
        /// Attaches a tracking GameObject to a collection of players.
        /// </summary>
        public static void AttachTrackingObject(this IEnumerable<Player> players, GameObject followerObject, Vector3 offset = default)
        {
            if (players is null || followerObject is null) return;

            if (players is List<Player> concreteList)
            {
                int count = concreteList.Count;
                for (int i = 0; i < count; i++) concreteList[i].AttachTrackingObject(followerObject, offset);
                return;
            }

            foreach (Player player in players)
            {
                player.AttachTrackingObject(followerObject, offset);
            }
        }

        /// <summary>
        /// Attaches a tracking GameObject to an inline array of players.
        /// </summary>
        public static void AttachTrackingObject(GameObject followerObject, Vector3 offset, params Player[] players)
        {
            if (players is null || followerObject is null) return;

            int count = players.Length;
            for (int i = 0; i < count; i++) players[i].AttachTrackingObject(followerObject, offset);
        }
        #endregion

        #region Mass Notifications
        /// <summary>
        /// Sends a hint message to all fully initialized and ready players.
        /// </summary>
        public static void BroadcastHintToAll(string hintContent, float duration = 5f)
        {
            if (string.IsNullOrEmpty(hintContent)) return;

            foreach (Player player in Player.ReadyList)
            {
                if (player != null && player.IsReady)
                {
                    player.SendHint(hintContent, duration);
                }
            }
        }

        /// <summary>
        /// Sends a hint message to a collection of players, filtering out null or unready entities.
        /// </summary>
        public static void BroadcastHint(this IEnumerable<Player> players, string hintContent, float duration = 5f)
        {
            if (players == null || string.IsNullOrEmpty(hintContent)) return;

            if (players is List<Player> concreteList)
            {
                int count = concreteList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (concreteList[i] != null && concreteList[i].IsReady)
                    {
                        concreteList[i].SendHint(hintContent, duration);
                    }
                }
                return;
            }

            foreach (Player player in players)
            {
                if (player != null && player.IsReady)
                {
                    player.SendHint(hintContent, duration);
                }
            }
        }

        /// <summary>
        /// Sends a hint message to an inline array of players, filtering out null or unready entities.
        /// </summary>
        public static void BroadcastHint(string hintContent, float duration, params Player[] players)
            => BroadcastHint(players, hintContent, duration);
        #endregion

        #region Status Tracking
        /// <summary>
        /// Gets the current Hume Shield value of a player, returning 0 if the stat module is missing.
        /// </summary>
        public static float GetHumeShieldValue(this Player player)
        {
            if (player == null || player.ReferenceHub == null) return 0f;

            if (player.ReferenceHub.playerStats.TryGetModule<PlayerStatsSystem.HumeShieldStat>(out var shieldStat))
            {
                return shieldStat.CurValue;
            }

            return 0f;
        }

        /// <summary>
        /// Determines if the player is an active, ready, and living human faction member.
        /// </summary>
        public static bool IsLivingHuman(this Player player)
        {
            return player is not null && player.IsReady && player.IsAlive && player.IsHuman;
        }
        #endregion

        #region Spatial Proximity Tracking
        /// <summary>
        /// Calculates the Euclidean distance between a player and a specified position vector.
        /// </summary>
        public static float GetDistanceTo(this Player player, Vector3 position)
        {
            if (player?.GameObject == null) return 0f;
            return Vector3.Distance(player.Position, position);
        }

        /// <summary>
        /// Checks if a player is within a maximum distance threshold from a position vector.
        /// </summary>
        public static bool IsWithinDistance(this Player player, Vector3 position, float maxDistance)
        {
            if (player?.GameObject == null) return false;
            return (player.Position - position).sqrMagnitude <= (maxDistance * maxDistance);
        }
        #endregion

        #region Light Emission
        /// <summary>
        /// Checks if the player's currently held light source item or weapon flashlight attachment is active.
        /// </summary>
        public static bool GetHeldLightSourceState(this Player player)
        {
            if (player?.CurrentItem is LightItem lightItem) return lightItem.IsEmitting;
            if (player?.CurrentItem is FirearmItem firearm && firearm.HasAttachment(AttachmentName.Flashlight)) return firearm.FlashlightEnabled;
            return false;
        }

        /// <summary>
        /// Toggles the activation state of the player's held light source item or weapon flashlight attachment.
        /// </summary>
        public static void SetHeldLightSourceState(this Player player, bool emit)
        {
            if (player?.CurrentItem is LightItem lightItem) lightItem.IsEmitting = emit;
            else if (player?.CurrentItem is FirearmItem firearm && firearm.HasAttachment(AttachmentName.Flashlight)) firearm.FlashlightEnabled = emit;
        }

        /// <summary>
        /// Toggles the activation state of a collection of players' held light sources.
        /// </summary>
        public static void SetHeldLightSourceState(this IEnumerable<Player> players, bool emit)
        {
            if (players is null) return;

            if (players is List<Player> concreteList)
            {
                int count = concreteList.Count;
                for (int i = 0; i < count; i++) concreteList[i].SetHeldLightSourceState(emit);
                return;
            }

            foreach (Player player in players)
            {
                player.SetHeldLightSourceState(emit);
            }
        }

        /// <summary>
        /// Toggles the activation state of an inline array of players' held light sources.
        /// </summary>
        public static void SetHeldLightSourceState(bool emit, params Player[] players)
        {
            if (players is null) return;

            int count = players.Length;
            for (int i = 0; i < count; i++) players[i].SetHeldLightSourceState(emit);
        }

        /// <summary>
        /// Runs a coroutine loop that flickers the player's held light source.
        /// </summary>
        public static IEnumerator<float> FlickerHeldLightSourceCoroutine(this Player player, int flickerCount, float delayPerFlicker, bool forceOff = false, Action<Player, bool> onTickFeedback = null)
        {
            if (player?.GameObject is null) yield break;

            Type initialItemType = player.CurrentItem?.GetType();
            if (initialItemType is null) yield break;

            for (int i = 0; i < flickerCount; i++)
            {
                if (!player.IsReady || !player.IsAlive || player.CurrentItem?.GetType() != initialItemType) break;

                bool isLastIteration = (i == flickerCount - 1);
                onTickFeedback?.Invoke(player, isLastIteration && forceOff);

                player.SetHeldLightSourceState(!player.GetHeldLightSourceState());
                yield return Timing.WaitForSeconds(delayPerFlicker);
            }

            if (forceOff && player.IsReady && player.IsAlive && player.CurrentItem?.GetType() == initialItemType)
            {
                player.SetHeldLightSourceState(false);
                onTickFeedback?.Invoke(player, true);
            }
        }

        /// <summary>
        /// Triggers the flicker coroutine loop for a collection of players.
        /// </summary>
        public static void FlickerHeldLightSource(this IEnumerable<Player> players, int flickerCount, float delayPerFlicker, bool forceOff = false, Action<Player, bool> onTickFeedback = null)
        {
            if (players is null) return;

            foreach (Player player in players)
            {
                if (player is not null)
                {
                    Timing.RunCoroutine(player.FlickerHeldLightSourceCoroutine(flickerCount, delayPerFlicker, forceOff, onTickFeedback));
                }
            }
        }

        /// <summary>
        /// Triggers the flicker coroutine loop for an inline array of players.
        /// </summary>
        public static void FlickerHeldLightSource(int flickerCount, float delayPerFlicker, bool forceOff = false, Action<Player, bool> onTickFeedback = null, params Player[] players)
            => FlickerHeldLightSource(players, flickerCount, delayPerFlicker, forceOff, onTickFeedback);

        /// <summary>
        /// Verifies if the player is actively emitting light via flashlights or firearm attachments.
        /// </summary>
        public static bool HasActiveLightSource(this Player player)
        {
            if (player is null) return false;

            var currentItem = player.CurrentItem;
            if (currentItem is null) return false;

            if (currentItem.Base is InventorySystem.Items.ToggleableLights.ToggleableLightItemBase lightItem)
            {
                return lightItem.IsEmittingLight;
            }

            if (currentItem is FirearmItem firearm)
            {
                return firearm.FlashlightEnabled && firearm.HasAttachment(AttachmentName.Flashlight);
            }

            return false;
        }
        #endregion

        #region Advanced Environmental & Position Filtering
        /// <summary>
        /// Checks if the player is in any of the specified rooms using a zero-allocation evaluation loop.
        /// </summary>
        public static bool IsInRoom(this Player player, params RoomName[] roomNames)
        {
            if (player?.Room == null) return false;

            RoomName currentRoomName = player.Room.Name;

            if (roomNames == null || roomNames.Length == 0)
            {
                return currentRoomName == RoomName.EzEvacShelter;
            }

            for (int i = 0; i < roomNames.Length; i++)
            {
                if (roomNames[i] == currentRoomName)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Verifies if the player is in total darkness, accounting for both standard facility rooms and elevator cabins.
        /// </summary>
        public static bool IsInTrueDarkness(this Player player)
        {
            if (player?.GameObject is null) return false;

            Elevator currentCabin = null;
            const float maxCabinRadiusSqr = 4.5f * 4.5f;

            foreach (Elevator elevator in Elevator.List)
            {
                if (elevator?.Base?.transform is null) continue;

                if ((player.Position - elevator.Base.transform.position).sqrMagnitude <= maxCabinRadiusSqr)
                {
                    currentCabin = elevator;
                    break;
                }
            }

            if (currentCabin is not null)
            {
                return currentCabin.AreLightsOff();
            }

            return player.IsInDarkRoom();
        }

        /// <summary>
        /// Checks if the standard room lighting controllers are disabled in the player's current location.
        /// </summary>
        public static bool IsInDarkRoom(this Player player)
        {
            Room room = player?.Room;
            if (room?.AllLightControllers is null) return false;

            foreach (var controller in room.AllLightControllers)
            {
                if (controller is not null && controller.LightsEnabled)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Validates circular proximity to a target vector using square magnitude to avoid performance overhead.
        /// </summary>
        public static bool IsWithinRadius(this Player player, Vector3 targetPosition, float radiusSize)
        {
            if (player?.GameObject == null) return false;

            float sqrDistance = (player.Position - targetPosition).sqrMagnitude;
            return sqrDistance <= (radiusSize * radiusSize);
        }

        /// <summary>
        /// Checks if the player falls within a radius threshold of any provided target positions.
        /// </summary>
        public static bool IsWithinAnyRadius(this Player player, IEnumerable<Vector3> positions, float radiusSize)
        {
            if (player?.GameObject == null || positions == null) return false;

            float sqrRadiusSize = radiusSize * radiusSize;

            foreach (Vector3 checkpointPos in positions)
            {
                if ((player.Position - checkpointPos).sqrMagnitude <= sqrRadiusSize)
                {
                    return true;
                }
            }

            return false;
        }
        #endregion

        #region Collection Query Extensions
        /// <summary>
        /// Filters a player collection to return only ready and living instances.
        /// </summary>
        public static IEnumerable<Player> WhereAlive(this IEnumerable<Player> players)
        {
            if (players == null) yield break;
            foreach (Player player in players)
            {
                if (player != null && player.IsReady && player.IsAlive) yield return player;
            }
        }

        /// <summary>
        /// Filters a player collection to return only human faction members.
        /// </summary>
        public static IEnumerable<Player> WhereHuman(this IEnumerable<Player> players)
        {
            if (players == null) yield break;
            foreach (Player player in players)
            {
                if (player != null && player.IsHuman) yield return player;
            }
        }

        /// <summary>
        /// Filters a player collection to return players who are not in the Pocket Dimension.
        /// </summary>
        public static IEnumerable<Player> WhereNotInPocket(this IEnumerable<Player> players)
        {
            if (players == null) yield break;
            foreach (Player player in players)
            {
                if (player != null && player.Room?.Name != RoomName.Pocket) yield return player;
            }
        }
        #endregion

        #region Status Effects Injection
        /// <summary>
        /// Applies a combination of visual and auditory status impairments to a single player.
        /// </summary>
        public static void ApplySensoryShock(this Player player, float flashDuration = 0.0f, float blindDuration = 0.0f, float blurDuration = 0.0f, float deafenDuration = 0.0f)
        {
            if (player?.GameObject is null) return;

            if (flashDuration > 0f) player.EnableEffect(FacilityEffectType.Flashed, 1, flashDuration);
            if (blindDuration > 0f) player.EnableEffect(FacilityEffectType.Blindness, 1, blindDuration);
            if (blurDuration > 0f) player.EnableEffect(FacilityEffectType.Blurred, 1, blurDuration);
            if (deafenDuration > 0f) player.EnableEffect(FacilityEffectType.Deafened, 1, deafenDuration);
        }

        /// <summary>
        /// Applies a combination of visual and auditory status impairments to a collection of players.
        /// </summary>
        public static void ApplySensoryShock(this IEnumerable<Player> players, float flashDuration = 0.0f, float blindDuration = 0.0f, float blurDuration = 0.0f, float deafenDuration = 0.0f)
        {
            if (players is null) return;

            if (players is List<Player> concreteList)
            {
                int count = concreteList.Count;
                for (int i = 0; i < count; i++)
                {
                    concreteList[i].ApplySensoryShock(flashDuration, blindDuration, blurDuration, deafenDuration);
                }
                return;
            }

            foreach (Player player in players)
            {
                player.ApplySensoryShock(flashDuration, blindDuration, blurDuration, deafenDuration);
            }
        }

        /// <summary>
        /// Applies a combination of visual and auditory status impairments to an inline array of players.
        /// </summary>
        public static void ApplySensoryShock(float flashDuration, float blindDuration, float blurDuration, float deafenDuration, params Player[] players)
        {
            if (players is null) return;

            int count = players.Length;
            for (int i = 0; i < count; i++)
            {
                players[i].ApplySensoryShock(flashDuration, blindDuration, blurDuration, deafenDuration);
            }
        }
        #endregion

        #region Cross-Entity Spatial Forwarding Overloads
        /// <summary>
        /// Resolves the facility room matching the player's current spatial coordinates.
        /// </summary>
        public static Room GetRoom(this Player player)
        {
            if (player?.GameObject == null) return null;
            return player.Position.GetRoom();
        }

        /// <summary>
        /// Calculates the distance between a player and a room center anchor.
        /// </summary>
        public static float GetDistanceTo(this Player player, Room room)
        {
            if (player?.GameObject == null || room?.Base == null) return 0f;
            return player.GetDistanceTo(room.Position);
        }

        /// <summary>
        /// Checks if the player is within a radius of the targeted room center.
        /// </summary>
        public static bool IsWithinRadius(this Player player, Room room, float radiusSize)
        {
            if (player?.GameObject == null || room?.Base == null) return false;
            return player.IsWithinRadius(room.Position, radiusSize);
        }
        #endregion

        #region Decoupled Gameplay Scenarios Validation
        /// <summary>
        /// Determines if a player fulfills basic criteria to trigger an escape sequence.
        /// </summary>
        public static bool IsEligibleForEscape(this Player player, Vector3 escapeZone, float escapeZoneSize)
        {
            if (player == null || player.IsSCP || !player.IsAlive) return false;
            return player.IsWithinRadius(escapeZone, escapeZoneSize);
        }

        /// <summary>
        /// Checks if a player is safely inside a designated shelter structure or location coordinate.
        /// </summary>
        public static bool IsInShelter(this Player player, float shelterZoneSize, IEnumerable<Vector3> cachedShelterLocations, params RoomName[] additionalRooms)
        {
            if (player == null) return false;

            if (player.IsInRoom(additionalRooms)) return true;

            return player.IsWithinAnyRadius(cachedShelterLocations, shelterZoneSize);
        }
        #endregion

        #region Identity
        /// <summary>
        /// Resolves a single player from a fuzzy string identifier using exact matching, substring containment, and Levenshtein distance checks.
        /// </summary>
        public static bool TryResolveFuzzy(this IEnumerable<Player> players, string identifier, out Player target, out string errorResponse)
        {
            target = null;
            string cleanSearch = identifier.Trim().ToLowerInvariant();

            if (string.IsNullOrWhiteSpace(cleanSearch))
            {
                errorResponse = "Target Assignment Failed: Provided player identifier descriptor cannot be empty.";
                return false;
            }

            var allPlayers = players.ToList();

            Player exactMatch = allPlayers.FirstOrDefault(p =>
                p.PlayerId.ToString() == cleanSearch ||
                p.UserId.NormalizeUserId() == cleanSearch ||
                p.Nickname.Equals(cleanSearch, StringComparison.OrdinalIgnoreCase));

            if (exactMatch is not null)
            {
                target = exactMatch;
                errorResponse = null;
                return true;
            }

            var candidates = allPlayers.Where(p =>
            {
                string nickLower = p.Nickname.ToLowerInvariant();
                return nickLower.Contains(cleanSearch) || cleanSearch.Contains(nickLower);
            }).ToList();

            if (candidates.Count == 0)
            {
                var distanceScores = new List<(Player player, int distance)>();
                foreach (Player p in allPlayers)
                {
                    string nickLower = p.Nickname.ToLowerInvariant();
                    int distance = cleanSearch.ComputeLevenshteinDistance(nickLower);

                    if (distance <= 3 && distance < nickLower.Length - 2)
                    {
                        distanceScores.Add((p, distance));
                    }
                }

                if (distanceScores.Count > 0)
                {
                    int minDistance = distanceScores.Min(s => s.distance);
                    candidates = distanceScores.Where(s => s.distance == minDistance).Select(s => s.player).ToList();
                }
            }

            candidates = candidates.Distinct().ToList();

            if (candidates.Count == 0)
            {
                errorResponse = $"Target Assignment Failed: No active player matching identifier literal '{identifier}' could be located.";
                return false;
            }

            if (candidates.Count > 1)
            {
                errorResponse = $"Ambiguous Player Target. Multiple candidates matched '{identifier}' simultaneously:\n" +
                                string.Join(", ", candidates.Select(p => $"{p.Nickname} (ID: {p.PlayerId})"));
                return false;
            }

            target = candidates[0];
            errorResponse = null;
            return true;
        }
        #endregion
    }
}