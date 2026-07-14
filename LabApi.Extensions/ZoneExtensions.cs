using Interactables.Interobjects.DoorUtils;
using LabApi.Features.Wrappers;
using MapGeneration;
using MEC;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LabApi.Extensions
{
    /// <summary>
    /// Utility extensions for working with <see cref="FacilityZone"/>:
    /// doors, lights, elevators and zone-wide animations.
    /// </summary>
    public static class ZoneExtensions
    {
        #region Zone Registry

        /// <summary>
        /// Cached array of all facility zones (zero enum allocation).
        /// </summary>
        public static readonly FacilityZone[] All = (FacilityZone[])Enum.GetValues(typeof(FacilityZone));

        #endregion

        #region Door Operations

        /// <summary>
        /// Returns all doors located inside the zone.
        /// </summary>
        public static IEnumerable<Door> GetDoors(this FacilityZone zone)
        {
            foreach (var room in Room.List)
            {
                if (room is null || room.Zone != zone || room.Doors is null)
                    continue;

                foreach (var door in room.Doors)
                    yield return door;
            }
        }

        public static void OpenDoors(this FacilityZone zone, bool bypassLocks = false)
            => zone.GetDoors().Open(bypassLocks);

        public static void CloseDoors(this FacilityZone zone, bool bypassLocks = false)
            => zone.GetDoors().Close(bypassLocks);

        /// <summary>
        /// Sets the lock state of all doors in the zone.  
        /// Use <paramref name="locked"/> = false to unlock them.
        /// </summary>
        public static void SetDoorsLockState(this FacilityZone zone, DoorLockReason reason, bool locked = true)
            => zone.GetDoors().SetLockState(reason, locked);

        public static void LockElevators(this FacilityZone zone)
        {
            zone.GetElevators().ForEach(e => e.LockAllDoors());
        }

        public static void LockElevatorsInZone(FacilityZone zone)
            => zone.LockElevators();

        public static void UnlockElevators(this FacilityZone zone)
        {
            zone.GetElevators().ForEach(e => e.UnlockAllDoors());
        }

        public static void UnlockElevatorsInZone(FacilityZone zone)
            => zone.UnlockElevators();

        #endregion

        #region Elevator Operations

        /// <summary>
        /// Returns all elevators whose destination rooms belong to the given zone.
        /// </summary>
        public static IEnumerable<Elevator> GetElevators(this FacilityZone zone)
        {
            foreach (var elevator in Elevator.List)
            {
                var rooms = elevator?.CurrentDestination?.Rooms;
                if (rooms is null)
                    continue;

                foreach (var r in rooms)
                {
                    if (Room.Get(r.Base)?.Zone == zone)
                    {
                        yield return elevator;
                        break;
                    }
                }
            }
        }

        public static IEnumerable<Elevator> GetElevatorsInZone(FacilityZone zone)
            => zone.GetElevators();

        #endregion

        #region Single-Zone Lights

        /// <summary>
        /// Turns off lights in the zone and its elevators.
        /// </summary>
        public static void TurnOffLights(this FacilityZone zone, float duration)
        {
            Map.TurnOffLights(duration, zone);
            zone.GetElevators().ForEach(e => e.TurnOffLights(duration));
        }

        /// <summary>
        /// Turns on lights in the zone and its elevators.
        /// </summary>
        public static void TurnOnLights(this FacilityZone zone)
        {
            Map.TurnOnLights(zone);
            zone.GetElevators().ForEach(e => e.TurnOnLights());
        }

        #endregion

        #region Multi-Zone Lights (IEnumerable + params)

        /// <summary>
        /// Turns off lights across multiple zones.
        /// </summary>
        public static void TurnOffLights(this IEnumerable<FacilityZone> zones, float duration)
        => zones.ForEach(z => z.TurnOffLights(duration));
      
        /// <summary>
        /// Turns off lights across multiple zones (params overload).
        /// </summary>
        public static void TurnOffLights(float duration, params FacilityZone[] zones)
            => ((IEnumerable<FacilityZone>)zones).TurnOffLights(duration);

        /// <summary>
        /// Turns on lights across multiple zones.
        /// </summary>
        public static void TurnOnLights(this IEnumerable<FacilityZone> zones)
        => zones.ForEach(z => z.TurnOnLights());
        

        /// <summary>
        /// Turns on lights across multiple zones (params overload).
        /// </summary>
        public static void TurnOnLights(params FacilityZone[] zones)
            => ((IEnumerable<FacilityZone>)zones).TurnOnLights();

        #endregion

        #region Zone Animations (Unified Flicker)

        /// <summary>
        /// Performs a flicker animation on zone lights.
        /// </summary>
        public static IEnumerator<float> FlickerLightsCoroutine(this FacilityZone zone, Color color, float duration, float frequency)
        {
            float interval = 1f / frequency.LimitMin(0.1f);
            float half = interval * 0.5f;
            int flickers = (int)(duration / interval);

            Map.SetColorOfLights(color, zone);

            for (int i = 0; i < flickers; i++)
            {
                Map.TurnOffLights(half, zone);
                yield return Timing.WaitForSeconds(half);

                Map.TurnOnLights(zone);
                yield return Timing.WaitForSeconds(half);
            }

            Map.ResetColorOfLights(zone);
        }

        #endregion
    }
}
