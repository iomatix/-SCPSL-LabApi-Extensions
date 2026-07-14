using InventorySystem.Items.Firearms.Attachments;
using LabApi.Extensions;
using LabApi.Extensions.Components;
using LabApi.Features.Wrappers;
using MapGeneration;
using MEC;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerExtensions
{
    #region Tracking (Transform Followers)

    /// <summary>
    /// Attaches a follower object to the player.
    /// The follower tracks the player's transform with an optional offset.
    /// </summary>
    public static void AttachTrackingObject(this Player player, GameObject followerObject, Vector3 offset = default)
    {
        if (player?.GameObject is null || followerObject is null)
            return;

        var tracker = followerObject.GetComponent<RuntimeTransformTracker>()
                     ?? followerObject.AddComponent<RuntimeTransformTracker>();

        tracker.Setup(player.GameObject.transform, offset, () => player.IsReady && player.IsAlive);
    }

    /// <summary>
    /// Attaches a follower object to multiple players.
    /// </summary>
    public static void AttachTrackingObject(this IEnumerable<Player> players, GameObject followerObject, Vector3 offset = default)
    => players?.ForEach(p => p?.AttachTrackingObject(followerObject, offset));

    /// <summary>
    /// Attaches a follower object to multiple players (params overload).
    /// </summary>
    public static void AttachTrackingObject(GameObject followerObject, Vector3 offset, params Player[] players)
        => ((IEnumerable<Player>)players).AttachTrackingObject(followerObject, offset);

    #endregion

    #region Notifications (Hints)

    /// <summary>
    /// Sends a hint to all ready players.
    /// </summary>
    public static void BroadcastHintToAll(string hintContent, float duration = 5f)
    {
        if (string.IsNullOrEmpty(hintContent))
            return;

        foreach (var player in Player.ReadyList)
        {
            if (player != null && player.IsReady)
                player.SendHint(hintContent, duration);
        }
    }

    /// <summary>
    /// Sends a hint to multiple players.
    /// Only ready players receive the hint.
    /// </summary>
    public static void BroadcastHint(this IEnumerable<Player> players, string hintContent, float duration = 5f)
    => players?.ForEach(p =>
    {
        if (p?.IsReady == true)
            p.SendHint(hintContent, duration);
    });

    /// <summary>
    /// Sends a hint to multiple players (params overload).
    /// </summary>
    public static void BroadcastHint(string hintContent, float duration, params Player[] players)
        => ((IEnumerable<Player>)players).BroadcastHint(hintContent, duration);

    #endregion


    #region Status & Identity

    /// <summary>
    /// Returns the player's current Hume Shield value.
    /// Returns 0 if the stat module is missing.
    /// </summary>
    public static float GetHumeShieldValue(this Player player)
    {
        if (player?.ReferenceHub == null)
            return 0f;

        if (player.ReferenceHub.playerStats.TryGetModule<PlayerStatsSystem.HumeShieldStat>(out var shield))
            return shield.CurValue;

        return 0f;
    }

    /// <summary>
    /// Returns true if the player is ready, alive and belongs to a human faction.
    /// </summary>
    public static bool IsLivingHuman(this Player player)
    {
        return player != null
               && player.IsReady
               && player.IsAlive
               && player.IsHuman;
    }

    /// <summary>
    /// Returns true if the player's held item emits light (flashlight or light item).
    /// </summary>
    public static bool HasActiveLightSource(this Player player)
    {
        if (player is null)
            return false;

        var item = player.CurrentItem;
        if (item is null)
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
        if (player?.CurrentItem is LightItem lightItem)
            return lightItem.IsEmitting;

        if (player?.CurrentItem is FirearmItem firearm && firearm.HasAttachment(AttachmentName.Flashlight))
            return firearm.FlashlightEnabled;

        return false;
    }

    /// <summary>
    /// Sets the emission state of the player's held light source.
    /// </summary>
    public static void SetHeldLightSourceState(this Player player, bool emit)
    {
        if (player?.CurrentItem is LightItem lightItem)
        {
            lightItem.IsEmitting = emit;
            return;
        }

        if (player?.CurrentItem is FirearmItem firearm && firearm.HasAttachment(AttachmentName.Flashlight))
        {
            firearm.FlashlightEnabled = emit;
        }
    }

    /// <summary>
    /// Sets the emission state of held light sources for multiple players.
    /// </summary>
    public static void SetHeldLightSourceState(this IEnumerable<Player> players, bool emit)
    => players?.ForEach(p => p?.SetHeldLightSourceState(emit));

    /// <summary>
    /// Sets the emission state of held light sources (params overload).
    /// </summary>
    public static void SetHeldLightSourceState(bool emit, params Player[] players)
        => ((IEnumerable<Player>)players).SetHeldLightSourceState(emit);

    #endregion


    #region Spatial (Distance, Radius, Rooms, Darkness)

    /// <summary>
    /// Returns the distance between the player and a world position.
    /// </summary>
    public static float GetDistanceTo(this Player player, Vector3 position)
    {
        if (player?.GameObject is null)
            return 0f;

        return Vector3.Distance(player.Position, position);
    }

    /// <summary>
    /// Returns true if the player is within the given distance from a world position.
    /// Uses squared magnitude for performance.
    /// </summary>
    public static bool IsWithinDistance(this Player player, Vector3 position, float maxDistance)
    {
        if (player?.GameObject is null)
            return false;

        float sqr = (player.Position - position).sqrMagnitude;
        return sqr <= maxDistance * maxDistance;
    }

    /// <summary>
    /// Returns true if the player is within the given radius from a world position.
    /// Uses squared magnitude for performance.
    /// </summary>
    public static bool IsWithinRadius(this Player player, Vector3 targetPosition, float radiusSize)
    {
        if (player?.GameObject is null)
            return false;

        float sqr = (player.Position - targetPosition).sqrMagnitude;
        return sqr <= radiusSize * radiusSize;
    }

    /// <summary>
    /// Returns true if the player is within the given radius from the room center.
    /// </summary>
    public static bool IsWithinRadius(this Player player, Room room, float radiusSize)
    {
        if (player?.GameObject is null || room?.Base is null)
            return false;

        return player.IsWithinRadius(room.Position, radiusSize);
    }

    /// <summary>
    /// Returns true if the player is within the given radius of any provided positions.
    /// Uses squared magnitude for performance.
    /// </summary>
    public static bool IsWithinAnyRadius(this Player player, IEnumerable<Vector3> positions, float radiusSize)
    {
        if (player?.GameObject is null || positions is null)
            return false;

        float sqrRadius = radiusSize * radiusSize;

        if (positions is List<Vector3> list)
        {
            int count = list.Count;
            for (int i = 0; i < count; i++)
            {
                if ((player.Position - list[i]).sqrMagnitude <= sqrRadius)
                    return true;
            }
            return false;
        }

        foreach (var pos in positions)
        {
            if ((player.Position - pos).sqrMagnitude <= sqrRadius)
                return true;
        }

        return false;
    }

    /// <summary>
    /// Returns true if the player is currently inside any of the given rooms.
    /// </summary>
    public static bool IsInRoom(this Player player, params RoomName[] roomNames)
    {
        var room = player?.Room;
        if (room is null)
            return false;

        RoomName current = room.Name;

        if (roomNames is null || roomNames.Length == 0)
            return current == RoomName.EzEvacShelter;

        int count = roomNames.Length;
        for (int i = 0; i < count; i++)
        {
            if (roomNames[i] == current)
                return true;
        }

        return false;
    }

    /// <summary>
    /// Returns true if the player is in a room with lights turned off.
    /// </summary>
    public static bool IsInDarkRoom(this Player player)
    {
        var room = player?.Room;
        if (room?.AllLightControllers is null)
            return false;

        foreach (var controller in room.AllLightControllers)
        {
            if (controller != null && controller.LightsEnabled)
                return false;
        }

        return true;
    }

    /// <summary>
    /// Returns true if the player is in total darkness (dark room or dark elevator cabin).
    /// </summary>
    public static bool IsInTrueDarkness(this Player player)
    {
        if (!player.TryGetNearbyElevatorCabin(out var cabin))
            return player.IsInDarkRoom();

        return cabin.AreLightsOff();
    }

    /// <summary>
    /// Returns the room the player is currently standing in.
    /// </summary>
    public static Room GetRoom(this Player player)
    {
        if (player?.GameObject is null)
            return null;

        return player.Position.GetRoom();
    }

    /// <summary>
    /// Returns the distance between the player and the room center.
    /// </summary>
    public static float GetDistanceTo(this Player player, Room room)
    {
        if (player?.GameObject is null || room?.Base is null)
            return 0f;

        return player.GetDistanceTo(room.Position);
    }

    #endregion


    #region Effects (Sensory Shock & Light Flicker)

    /// <summary>
    /// Applies visual and auditory impairments to the player.
    /// </summary>
    public static void ApplySensoryShock(
        this Player player,
        float flashDuration = 0f,
        float blindDuration = 0f,
        float blurDuration = 0f,
        float deafenDuration = 0f)
    {
        if (player?.GameObject is null)
            return;

        if (flashDuration > 0f)
            player.EnableEffect(FacilityEffectType.Flashed, 1, flashDuration);

        if (blindDuration > 0f)
            player.EnableEffect(FacilityEffectType.Blindness, 1, blindDuration);

        if (blurDuration > 0f)
            player.EnableEffect(FacilityEffectType.Blurred, 1, blurDuration);

        if (deafenDuration > 0f)
            player.EnableEffect(FacilityEffectType.Deafened, 1, deafenDuration);
    }

    /// <summary>
    /// Applies sensory shock to multiple players.
    /// </summary>
    public static void ApplySensoryShock(
        this IEnumerable<Player> players,
        float flashDuration = 0f,
        float blindDuration = 0f,
        float blurDuration = 0f,
        float deafenDuration = 0f)
    => players?.ForEach(p => p?.ApplySensoryShock(flashDuration, blindDuration, blurDuration, deafenDuration));

    /// <summary>
    /// Applies sensory shock to multiple players (params overload).
    /// </summary>
    public static void ApplySensoryShock(
        float flashDuration,
        float blindDuration,
        float blurDuration,
        float deafenDuration,
        params Player[] players)
        => ((IEnumerable<Player>)players).ApplySensoryShock(flashDuration, blindDuration, blurDuration, deafenDuration);


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
        if (player?.GameObject is null)
            yield break;

        var initialType = player.CurrentItem?.GetType();
        if (initialType is null)
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
    /// Starts the flicker coroutine for multiple players.
    /// </summary>
    public static void FlickerHeldLightSource(
        this IEnumerable<Player> players,
        int flickerCount,
        float delayPerFlicker,
        bool forceOff = false,
        Action<Player, bool> onTickFeedback = null)
    => players?.ForEach(p =>
    {
        if (p != null) Timing.RunCoroutine(p.FlickerHeldLightSourceCoroutine(flickerCount, delayPerFlicker, forceOff, onTickFeedback));
    });


    /// <summary>
    /// Starts the flicker coroutine for multiple players (params overload).
    /// </summary>
    public static void FlickerHeldLightSource(
        int flickerCount,
        float delayPerFlicker,
        bool forceOff = false,
        Action<Player, bool> onTickFeedback = null,
        params Player[] players)
        => ((IEnumerable<Player>)players).FlickerHeldLightSource(flickerCount, delayPerFlicker, forceOff, onTickFeedback);

    #endregion

    #region Queries (Alive / Human / NotInPocket)

    /// <summary>
    /// Returns only players who are ready and alive.
    /// </summary>
    public static IEnumerable<Player> WhereAlive(this IEnumerable<Player> players)
    {
        if (players is null)
            yield break;

        if (players is List<Player> list)
        {
            int count = list.Count;
            for (int i = 0; i < count; i++)
            {
                var player = list[i];
                if (player != null && player.IsReady && player.IsAlive)
                    yield return player;
            }
            yield break;
        }

        foreach (var player in players)
        {
            if (player != null && player.IsReady && player.IsAlive)
                yield return player;
        }
    }

    /// <summary>
    /// Returns only players who belong to a human faction.
    /// </summary>
    public static IEnumerable<Player> WhereHuman(this IEnumerable<Player> players)
    {
        if (players is null)
            yield break;

        if (players is List<Player> list)
        {
            int count = list.Count;
            for (int i = 0; i < count; i++)
            {
                var player = list[i];
                if (player != null && player.IsHuman)
                    yield return player;
            }
            yield break;
        }

        foreach (var player in players)
        {
            if (player != null && player.IsHuman)
                yield return player;
        }
    }

    /// <summary>
    /// Returns only players who are not inside the Pocket Dimension.
    /// </summary>
    public static IEnumerable<Player> WhereNotInPocket(this IEnumerable<Player> players)
    {
        if (players is null)
            yield break;

        if (players is List<Player> list)
        {
            int count = list.Count;
            for (int i = 0; i < count; i++)
            {
                var player = list[i];
                if (player != null && player.Room?.Name != RoomName.Pocket)
                    yield return player;
            }
            yield break;
        }

        foreach (var player in players)
        {
            if (player != null && player.Room?.Name != RoomName.Pocket)
                yield return player;
        }
    }

    #endregion


    #region Conditions 

    /// <summary>
    /// Returns true if the player meets basic conditions to escape.
    /// </summary>
    public static bool IsEligibleForEscape(this Player player, IEnumerable<Vector3> escapeZones, float escapeZoneSize)
    {
        if (player == null || player.IsSCP || !player.IsAlive)
            return false;

        return player.IsWithinAnyRadius(escapeZones, escapeZoneSize);
    }

    /// <summary>
    /// Returns true if the player is inside a shelter room or within any shelter radius.
    /// </summary>
    public static bool IsInShelter(this Player player, float shelterZoneSize, IEnumerable<Vector3> shelterLocations, params RoomName[] additionalRooms)
    {
        if (player == null)
            return false;

        if (player.IsInRoom(additionalRooms))
            return true;

        return player.IsWithinAnyRadius(shelterLocations, shelterZoneSize);
    }

    /// <summary>
    /// Returns true if the player is inside an elevator cabin.
    /// </summary>
    public static bool IsInExecutiveElevator(this Player player)
    {
        return player.TryGetNearbyElevatorCabin(out _);
    }

    #region Private Region of Elevator Check
    private const float ElevatorCabinRadius = 4.5f;
    private const float ElevatorCabinRadiusSqr = ElevatorCabinRadius * ElevatorCabinRadius;


    /// <summary>
    /// Returns true if there is an elevator cabin near the player and outputs it.
    /// </summary>
    private static bool TryGetNearbyElevatorCabin(this Player player, out Elevator cabin)
    {
        cabin = null;

        if (player?.GameObject is null)
            return false;

        // Zero-allocation fast-path
        if (Elevator.List is List<Elevator> list)
        {
            int count = list.Count;
            for (int i = 0; i < count; i++)
            {
                var elevator = list[i];
                if (elevator?.Base?.transform is null)
                    continue;

                if ((player.Position - elevator.Base.transform.position).sqrMagnitude <= ElevatorCabinRadiusSqr)
                {
                    cabin = elevator;
                    return true;
                }
            }

            return false;
        }

        // Fallback: IEnumerable scan
        foreach (var elevator in Elevator.List)
        {
            if (elevator?.Base?.transform is null)
                continue;

            if ((player.Position - elevator.Base.transform.position).sqrMagnitude <= ElevatorCabinRadiusSqr)
            {
                cabin = elevator;
                return true;
            }
        }

        return false;
    }
    #endregion
    #endregion

    #region Identity Resolution (Fuzzy)

    /// <summary>
    /// Attempts to resolve a player from a fuzzy identifier.
    /// Supports exact ID, UserId, exact nickname, substring match, and Levenshtein fallback.
    /// </summary>
    public static bool TryResolveFuzzy(this IEnumerable<Player> players, string identifier, out Player target, out string error)
    {
        target = null;
        error = null;

        string search = identifier?.Trim().ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(search))
        {
            error = "Target Assignment Failed: Identifier cannot be empty.";
            return false;
        }

        // Zero-allocation fast-path: list enumerator
        if (players is List<Player> list)
            return TryResolveFuzzyCore(list, search, out target, out error);

        // Fallback: IEnumerable enumerator (still zero-alloc)
        return TryResolveFuzzyCore(players, search, out target, out error);
    }

    /// <summary>
    /// Core fuzzy resolver. Single source of truth.
    /// </summary>
    private static bool TryResolveFuzzyCore(IEnumerable<Player> players, string search, out Player target, out string error)
    {
        target = null;
        error = null;

        Player best = null;
        int bestDist = int.MaxValue;
        bool substringFound = false;

        // 1. Exact match pass
        foreach (var p in players)
        {
            if (p == null) continue;

            if (p.PlayerId.ToString() == search ||
                p.UserId.NormalizeUserId() == search ||
                p.Nickname.Equals(search, StringComparison.OrdinalIgnoreCase))
            {
                target = p;
                return true;
            }
        }

        // 2. Substring + Levenshtein pass
        foreach (var p in players)
        {
            if (p == null) continue;

            string nick = p.Nickname.ToLowerInvariant();

            // Substring match
            if (nick.Contains(search) || search.Contains(nick))
            {
                if (!substringFound)
                {
                    best = p;
                    substringFound = true;
                }
                else
                {
                    error = $"Ambiguous Player Target. Multiple candidates matched '{search}'.";
                    return false;
                }
                continue;
            }

            // Levenshtein fallback
            int dist = search.ComputeLevenshteinDistance(nick);
            if (dist <= 3)
            {
                if (dist < bestDist)
                {
                    bestDist = dist;
                    best = p;
                }
                else if (dist == bestDist)
                {
                    error = $"Ambiguous Player Target. Multiple candidates matched '{search}'.";
                    return false;
                }
            }
        }

        if (best == null)
        {
            error = $"Target Assignment Failed: No player matched '{search}'.";
            return false;
        }

        target = best;
        return true;
    }

    #endregion

}
