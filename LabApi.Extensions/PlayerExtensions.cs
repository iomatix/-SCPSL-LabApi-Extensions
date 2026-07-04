using LabApi.Extensions.Components;
using LabApi.Features.Wrappers;
using MapGeneration;
using System.Collections.Generic;
using UnityEngine;

namespace LabApi.Extensions
{
    /// <summary>
    /// Provides enterprise-grade global broadcast grid interfaces, real-time spatial proximity evaluation tracking, 
    /// and telemetry notification distribution adapters for server-side <see cref="Player"/> collections.
    /// </summary>
    internal static class PlayerExtensions
    {
        #region Tracking
        /// <summary>
        /// Seamlessly attaches any external tracking GameObject source to a live Player target utilizing native sub-frame anchor locking components.
        /// </summary>
        public static void AttachTrackingObject(this Player player, GameObject followerObject, Vector3 offset = default)
        {
            if (player?.GameObject is null || followerObject is null) return;

            var tracker = followerObject.GetComponent<RuntimeTransformTracker>() ?? followerObject.AddComponent<RuntimeTransformTracker>();
            tracker.Setup(player.GameObject.transform, offset, () => player.IsReady && player.IsAlive);
        }
        #endregion

        #region Mass Notifications
        /// <summary>
        /// Dispatches a localized streaming Hint display message layer across all currently verified, 
        /// ready human and active simulation subjects simultaneously.
        /// </summary>
        /// <param name="hintContent">The raw textual payload character sequence or UI element layout string targeted for projection on client screens.</param>
        /// <param name="duration">The operational tracking lifespan timeframe measured in seconds, specifying how long the overlay layer remains visible on user displays.</param>
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
        /// Dispatches a visual streaming Hint display message across a specific filtered collection of player instances.
        /// Defensively filters out null entities or subjects that are not fully initialized and round-ready.
        /// </summary>
        /// <param name="players">The targeted enumerable collection of <see cref="Player"/> instances to receive the interface overlay.</param>
        /// <param name="hintContent">The localized notification text sequence to display on the user interface grid.</param>
        /// <param name="duration">The visual display lifetime duration in seconds allocated for the hint layout overlay.</param>
        public static void BroadcastHint(this System.Collections.Generic.IEnumerable<Player> players, string hintContent, float duration = 5f)
        {
            if (players == null || string.IsNullOrEmpty(hintContent))
            {
                return;
            }

            foreach (Player player in players)
            {
                if (player == null || !player.IsReady)
                {
                    continue;
                }

                player.SendHint(hintContent, duration);
            }
        }
        #endregion

        #region Status Tracking
        /// <summary>
        /// Safe, high-performance extraction wrapper designed to query the underlying structural statistical tracking systems 
        /// and yield the current active value coefficient of a subject's Hume Shield asset pool.
        /// </summary>
        /// <param name="player">The target <see cref="Player"/> instance whose modular stats matrix is being safely evaluated.</param>
        /// <returns>The real-time scalar volume tracking value of the active hume shield; defaults to 0.0f if the module is non-existent on the subject role.</returns>
        public static float GetHumeShieldValue(this Player player)
        {
            if (player == null || player.ReferenceHub == null)
            {
                return 0f;
            }

            if (player.ReferenceHub.playerStats.TryGetModule<PlayerStatsSystem.HumeShieldStat>(out var shieldStat))
            {
                return shieldStat.CurValue;
            }

            return 0f;
        }

        /// <summary>
        /// Evaluates with high-performance metrics whether the targeted player instance represents 
        /// a fully initialized, active human subject who is currently alive in the current round lifecycle.
        /// </summary>
        /// <param name="player">The source <see cref="Player"/> entity execution boundary context to evaluate.</param>
        /// <returns><c>true</c> if the subject is structurally ready, alive, and belongs to a human faction; otherwise, <c>false</c>.</returns>
        public static bool IsLivingHuman(this Player player)
        {
            return player is not null && player.IsReady && player.IsAlive && player.IsHuman;
        }
        #endregion

        #region Spatial Proximity Tracking
        /// <summary>
        /// Computes the precise linear Euclidean distance between the target player's current spatial coordinates and a specified 3D position vector.
        /// </summary>
        /// <param name="player">The source <see cref="Player"/> instance executing the spatial distance calculation.</param>
        /// <param name="position">The target <see cref="Vector3"/> spatial coordinates vector checked inside the active simulation workspace.</param>
        /// <returns>A single-precision floating-point scalar value representing the calculated physical distance in meters; otherwise, <c>0f</c> if structural dependencies evaluate to null.</returns>
        public static float GetDistanceTo(this Player player, Vector3 position)
        {
            if (player?.GameObject == null) return 0f;
            return Vector3.Distance(player.Position, position);
        }

        /// <summary>
        /// Evaluates defensively whether the specified player is positioned within a concrete linear maximum distance radius from a target spatial coordinate vector.
        /// </summary>
        /// <param name="player">The source <see cref="Player"/> instance tracked for spatial proximity validation.</param>
        /// <param name="position">The target destination <see cref="Vector3"/> coordinates vector evaluated for proximity boundaries.</param>
        /// <param name="maxDistance">The maximum physical radius limit coefficient allowed for positive structural validation.</param>
        /// <returns><c>true</c> if the player entity is verified within the designated distance radius; otherwise, <c>false</c>.</returns>
        public static bool IsWithinDistance(this Player player, Vector3 position, float maxDistance)
        {
            if (player?.GameObject == null) return false;
            return Vector3.Distance(player.Position, position) <= maxDistance;
        }
        #endregion

        #region Light Emission Tracking
        /// <summary>
        /// Evaluates whether the targeted player instance is currently emitting mobile photon fields 
        /// via active standalone flashlights or tactical firearm attachments.
        /// </summary>
        /// <param name="player">The source <see cref="Player"/> entity execution boundary context to evaluate.</param>
        /// <returns><c>true</c> if the subject possesses a validated and actively emitting light emission vector; otherwise, <c>false</c>.</returns>
        public static bool HasActiveLightSource(this Player player)
        {
            if (player is null) return false;

            var currentItem = player.CurrentItem;
            if (currentItem is null) return false;

            // 1. Evaluate dedicated standalone light sources (Flashlights, lanterns)
            if (currentItem.Base is InventorySystem.Items.ToggleableLights.ToggleableLightItemBase lightItem)
            {
                return lightItem.IsEmittingLight;
            }

            // 2. Evaluate weapon-mounted tactical attachments using our new FirearmExtensions tool!
            if (currentItem is FirearmItem firearm)
            {
                return firearm.FlashlightEnabled && firearm.HasAttachment(InventorySystem.Items.Firearms.Attachments.AttachmentName.Flashlight);
            }

            return false;
        }
        #endregion

        #region Advanced Environmental & Position Filtering
        /// <summary>
        /// Evaluates with absolute zero heap allocation whether the player's active room context 
        /// matches any of the specified structural room layout signatures.
        /// </summary>
        /// <param name="player">The source <see cref="Player"/> instance tracked for environmental location boundaries.</param>
        /// <param name="roomNames">An expandable parameters array sequence containing target <see cref="RoomName"/> layout tokens to compare against.</param>
        /// <returns><c>true</c> if the player is verified inside any of the targeted room signatures; otherwise, <c>false</c>.</returns>
        public static bool IsInRoom(this Player player, params RoomName[] roomNames)
        {
            if (player?.Room == null) return false;

            RoomName currentRoomName = player.Room.Name;

            // Fast-path evaluation execution grid: fallback to default evacuation shelter if no parameters are passed
            if (roomNames == null || roomNames.Length == 0)
            {
                return currentRoomName == RoomName.EzEvacShelter;
            }

            // Slow-path evaluation execution grid: optimized zero-allocation linear for-loop avoiding enumerator generation
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
        /// Evaluates whether the localized room lighting grid envelope encompassing a specific <see cref="Player"/> instance has had its active illumination disabled.
        /// </summary>
        /// <param name="player">The target <see cref="Player"/> subject instance checked for dark room environmental parameters.</param>
        /// <returns><c>true</c> if the target player is verified within a room sector containing fully unpowered or disabled lighting grids; otherwise, <c>false</c>.</returns>
        public static bool IsInDarkRoom(this Player player)
        {
            var room = player?.Room;
            if (room?.AllLightControllers == null) return false;

            foreach (var lc in room.AllLightControllers)
            {
                if (lc.LightsEnabled) return false;
            }
            return true;
        }

        /// <summary>
        /// Performs a high-performance distance query utilizing underlying Unity vector squaring math (<c>sqrMagnitude</c>).
        /// Completely circumvents expensive processor square root calculations (<c>Math.Sqrt</c>) to validate circular perimeter limits.
        /// </summary>
        /// <param name="player">The source <see cref="Player"/> instance targeted for spatial boundary verification.</param>
        /// <param name="targetPosition">The target destination <see cref="Vector3"/> coordinate vector representing the radius center tracking point.</param>
        /// <param name="radiusSize">The maximum radial threshold limitation value constraint in meters.</param>
        /// <returns><c>true</c> if the player entity position falls inside the computed radial boundary space; otherwise, <c>false</c>.</returns>
        public static bool IsWithinRadius(this Player player, Vector3 targetPosition, float radiusSize)
        {
            if (player?.GameObject == null) return false;

            // Utilizing squared displacement differences to eliminate high overhead computational roots
            float sqrDistance = (player.Position - targetPosition).sqrMagnitude;
            return sqrDistance <= (radiusSize * radiusSize);
        }

        /// <summary>
        /// Evaluates defensively whether the player's spatial coordinates fall within a concrete radial proximity constraint 
        /// intersecting any item position stored inside an aggregated sequence matrix tracking layout coordinates.
        /// </summary>
        /// <param name="player">The source <see cref="Player"/> instance tracked for structural proximity validation loops.</param>
        /// <param name="positions">An enumerable sequence tracking absolute 3D positional coordinates vectors cached by sub-system layers.</param>
        /// <param name="radiusSize">The maximum range allocation limit tracking circle threshold bounds.</param>
        /// <returns><c>true</c> if the player entity is verified near at least one vector location tracked inside the sequence query; otherwise, <c>false</c>.</returns>
        public static bool IsWithinAnyRadius(this Player player, IEnumerable<Vector3> positions, float radiusSize)
        {
            if (player?.GameObject == null || positions == null) return false;

            float sqrRadiusSize = radiusSize * radiusSize;

            // Iterating decoupled telemetry points safely to track spatial intersections
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
        /// Filters an enumerable collection layout stream of players utilizing high-performance lazy execution 
        /// to yield exclusively active, ready human subjects who are currently alive in the current round lifecycle.
        /// </summary>
        /// <param name="players">The source enumerable collection tracking global or localized player entity matrix blocks.</param>
        /// <returns>An evaluated historical sequence iteration tracking only verified active and alive <see cref="Player"/> instances.</returns>
        public static IEnumerable<Player> WhereAlive(this IEnumerable<Player> players)
        {
            if (players == null) yield break;

            foreach (Player player in players)
            {
                if (player != null && player.IsReady && player.IsAlive)
                {
                    yield return player;
                }
            }
        }

        /// <summary>
        /// Filters an enumerable collection layout stream of players utilizing high-performance lazy execution 
        /// to yield exclusively player subjects belonging to human factions.
        /// </summary>
        /// <param name="players">The source collection of player entities to undergo faction filtration.</param>
        /// <returns>A filtered enumerable sequence layout containing only human player entities.</returns>
        public static System.Collections.Generic.IEnumerable<Player> WhereHuman(this System.Collections.Generic.IEnumerable<Player> players)
        {
            if (players == null)
            {
                return System.Linq.Enumerable.Empty<Player>();
            }

            System.Collections.Generic.List<Player> filtered = new System.Collections.Generic.List<Player>();
            foreach (Player player in players)
            {
                if (player != null && player.IsHuman)
                {
                    filtered.Add(player);
                }
            }
            return filtered;
        }

        /// <summary>
        /// Filters an enumerable collection layout stream of players to insulate the pipeline against subjects 
        /// currently trapped inside the low-level execution bounds of SCP-106's Pocket Dimension.
        /// </summary>
        /// <param name="players">The source collection of player entities checked for spatial dimension parameters.</param>
        /// <returns>A filtered enumerable sequence layout containing players outside the pocket dimension zone.</returns>
        public static System.Collections.Generic.IEnumerable<Player> WhereNotInPocket(this System.Collections.Generic.IEnumerable<Player> players)
        {
            if (players == null)
            {
                return System.Linq.Enumerable.Empty<Player>();
            }

            System.Collections.Generic.List<Player> filtered = new System.Collections.Generic.List<Player>();
            foreach (Player player in players)
            {
                if (player != null && player.Room != null && player.Room.Name != RoomName.Pocket)
                {
                    filtered.Add(player);
                }
            }
            return filtered;
        }
        #endregion

        #region Batch Status Effects Injection
        /// <summary>
        /// Deploys an aggregated tactical batch of adverse visual and auditory status effect matrices simultaneously onto a single target entity.
        /// Safely skips internal duration initialization routines for any specific effect parameter mapped to zero or negative temporal scales.
        /// </summary>
        public static void ApplySensoryShock(this Player player, float flashDuration = 0.0f, float blindDuration = 0.0f, float blurDuration = 0.0f, float deafenDuration = 0.0f)
        {
            if (player?.GameObject is null) return;

            if (flashDuration > 0f) player.EnableEffect(FacilityEffectType.Flashed, 1, flashDuration);
            if (blindDuration > 0f) player.EnableEffect(FacilityEffectType.Blindness, 1, blindDuration);
            if (blurDuration > 0f) player.EnableEffect(FacilityEffectType.Blurred, 1, blurDuration);
            if (deafenDuration > 0f) player.EnableEffect(FacilityEffectType.Deafened, 1, deafenDuration);
        }
        #endregion

        #region Cross-Entity Spatial Forwarding Overloads
        /// <summary>
        /// Explicitly resolves the live structural <see cref="Room"/> sector currently encompassing the player's 
        /// real-time position vector by forwarding the query directly onto the underlying map topology grid.
        /// </summary>
        /// <param name="player">The source <see cref="Player"/> instance whose active spatial coordinates are evaluated.</param>
        /// <returns>The concrete <see cref="Room"/> instance containing the player entity; otherwise, <see langword="null"/> if coordinates map to void zones.</returns>
        public static Room GetRoom(this Player player)
        {
            if (player?.GameObject == null)
            {
                return null;
            }

            // Forwarding the structural translation lookup seamlessly using the player's spatial coordinate vector
            return player.Position.GetRoom();
        }

        /// <summary>
        /// Computes the precise linear Euclidean distance between the player's current spatial coordinates 
        /// and the central transform anchor position of the targeted room entity.
        /// </summary>
        /// <param name="player">The source <see cref="Player"/> instance serving as the origin coordinate spatial anchor.</param>
        /// <param name="room">The destination target <see cref="Room"/> instance checked for spatial displacement tracking.</param>
        /// <returns>A single-precision floating-point scalar value indicating the physical displacement distance in meters.</returns>
        public static float GetDistanceTo(this Player player, Room room)
        {
            if (player?.GameObject == null || room?.Base == null)
            {
                return 0f;
            }

            // Re-using the core vector-based linear calculation module under the hood
            return player.GetDistanceTo(room.Position);
        }

        /// <summary>
        /// Performs a high-performance cross-entity proximity validation sweep between the player and a target room center
        /// utilizing underlying Unity vector squaring math (<c>sqrMagnitude</c>) to avoid high overhead calculation paths.
        /// </summary>
        /// <param name="player">The source <see cref="Player"/> instance tracked for perimeter boundary alignment.</param>
        /// <param name="room">The target destination <see cref="Room"/> architecture context serving as the circle tracking point center.</param>
        /// <param name="radiusSize">The maximum range limitation value constraint in meters allowed for positive validation.</param>
        /// <returns><c>true</c> if the player entity position falls inside the computed room radial envelope boundary; otherwise, <c>false</c>.</returns>
        public static bool IsWithinRadius(this Player player, Room room, float radiusSize)
        {
            if (player?.GameObject == null || room?.Base == null)
            {
                return false;
            }

            // Forwarding directly onto the highly optimized squared displacement calculation pipeline
            return player.IsWithinRadius(room.Position, radiusSize);
        }
        #endregion

        #region Decoupled Gameplay Scenarios Validation
        /// <summary>
        /// Validates defensively whether a player entity satisfies generic escape criteria based on life state, 
        /// anomalous faction restrictions, and optimized radial proximity to an extraction zone vector.
        /// </summary>
        /// <param name="player">The source <see cref="Player"/> instance tracking escape capability states.</param>
        /// <param name="escapeZone">The target <see cref="Vector3"/> spatial coordinates representing the center of the extraction zone.</param>
        /// <param name="escapeZoneSize">The maximum radial fallback size limit tracking extraction verification.</param>
        /// <returns><c>true</c> if the player is alive, not an SCP subject, and positioned inside the radial extraction vector bounds; otherwise, <c>false</c>.</returns>
        public static bool IsEligibleForEscape(this Player player, Vector3 escapeZone, float escapeZoneSize)
        {
            if (player == null || player.IsSCP || !player.IsAlive)
            {
                return false;
            }

            return player.IsWithinRadius(escapeZone, escapeZoneSize);
        }

        /// <summary>
        /// Evaluates with high-performance metrics whether a player is safely positioned inside a valid protection shelter infrastructure.
        /// Dynamically verifies structural room identifiers first before cascading into cached multi-point radial proximity checks.
        /// </summary>
        /// <param name="player">The source <see cref="Player"/> instance queried for tactical shelter containment.</param>
        /// <param name="shelterZoneSize">The maximum boundary radius threshold assigned for spatial proximity calculations.</param>
        /// <param name="cachedShelterLocations">An enumerable sequence tracking absolute 3D coordinate vectors of registered shelters generated by the host plugin.</param>
        /// <param name="additionalRooms">An expandable parameters array sequence tracking custom <see cref="RoomName"/> layout tokens validated as shelter zones.</param>
        /// <returns><c>true</c> if the player entity is confirmed inside a designated shelter structure or radius; otherwise, <c>false</c>.</returns>
        public static bool IsInShelter(this Player player, float shelterZoneSize, IEnumerable<Vector3> cachedShelterLocations, params RoomName[] additionalRooms)
        {
            if (player == null)
            {
                return false;
            }

            // Phase 1: Fast-path zero-allocation room layout filtering
            if (player.IsInRoom(additionalRooms))
            {
                return true;
            }

            // Phase 2: High-performance squared displacement calculation sweep across cached infrastructure positions
            return player.IsWithinAnyRadius(cachedShelterLocations, shelterZoneSize);
        }
        #endregion


    }
}