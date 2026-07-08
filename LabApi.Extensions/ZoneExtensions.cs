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
    /// Provides extension methods for <see cref="FacilityZone"/> structures,
    /// enabling zone-wide illumination overrides, strobe animations, and bulk door operations.
    /// </summary>
    public static class ZoneExtensions
    {
        #region Cached Zone Registry
        /// <summary>
        /// Cached array containing all valid facility zone tokens to eliminate runtime enum allocation overhead.
        /// </summary>
        public static readonly FacilityZone[] All = (FacilityZone[])Enum.GetValues(typeof(FacilityZone));
        #endregion

        #region Zone Door Operations
        /// <summary>
        /// Resolves and aggregates all <see cref="Door"/> instances deployed within the specified facility zone boundary.
        /// </summary>
        /// <param name="zone">The target facility zone context to query.</param>
        /// <returns>A sequence of doors located inside the targeted zone.</returns>
        public static IEnumerable<Door> GetDoors(this FacilityZone zone)
        {
            foreach (Room room in Room.List)
            {
                if (room is null || room.Zone != zone || room.Doors is null) continue;

                foreach (Door door in room.Doors)
                {
                    yield return door;
                }
            }
        }

        /// <summary>
        /// Forcibly unseals all doors located within the targeted facility zone boundary.
        /// </summary>
        /// <param name="zone">The target facility zone undergoing bulk door opening.</param>
        /// <param name="bypassLocks">If set to <c>true</c>, overrides active server-side door locks.</param>
        public static void OpenDoors(this FacilityZone zone, bool bypassLocks = false)
        {
            zone.GetDoors().Open(bypassLocks);
        }

        /// <summary>
        /// Forcibly seals all doors located within the targeted facility zone boundary.
        /// </summary>
        /// <param name="zone">The target facility zone undergoing bulk door closure.</param>
        /// <param name="bypassLocks">If set to <c>true</c>, overrides active server-side door locks.</param>
        public static void CloseDoors(this FacilityZone zone, bool bypassLocks = false)
        {
            zone.GetDoors().Close(bypassLocks);
        }

        /// <summary>
        /// Updates the administrative lock state for all doors across an entire facility zone under a specific lock reason.
        /// </summary>
        /// <param name="zone">The target facility zone undergoing lock state mutation.</param>
        /// <param name="reason">The specific structural lock reason applied or released.</param>
        /// <param name="locked">If set to <c>true</c>, engages the lock; if <c>false</c>, releases the lock constraint.</param>
        public static void SetDoorsLockState(this FacilityZone zone, DoorLockReason reason, bool locked = true)
        {
            zone.GetDoors().SetLockState(reason, locked);
        }
        #endregion

        #region Single Zone Illumination Overrides
        /// <summary>
        /// Forcibly suppresses all layout light controllers and connected elevator cabins across an entire facility zone for a designated duration.
        /// </summary>
        public static void TurnOffLights(this FacilityZone zone, float duration)
        {
            Map.TurnOffLights(duration, zone);

            foreach (Elevator elevator in ElevatorExtensions.GetElevatorsInZone(zone))
            {
                elevator.TurnOffLights(duration);
            }
        }

        /// <summary>
        /// Instantly restores electrical power to all light controllers and connected elevator cabins across a specific facility zone.
        /// </summary>
        public static void TurnOnLights(this FacilityZone zone)
        {
            Map.TurnOnLights(zone);

            foreach (Elevator elevator in ElevatorExtensions.GetElevatorsInZone(zone))
            {
                elevator.TurnOnLights();
            }
        }
        #endregion

        #region Bulk Collection Illumination Overrides
        /// <summary>
        /// Forcibly suppresses illumination across a collection sequence of facility zones simultaneously.
        /// </summary>
        public static void TurnOffLights(this IEnumerable<FacilityZone> zones, float duration)
        {
            if (zones is null) return;

            foreach (FacilityZone zone in zones)
            {
                zone.TurnOffLights(duration);
            }
        }

        /// <summary>
        /// Instantly restores operational grid power parameters across an entire collection sequence of facility zones.
        /// </summary>
        public static void TurnOnLights(this IEnumerable<FacilityZone> zones)
        {
            if (zones is null) return;

            foreach (FacilityZone zone in zones)
            {
                zone.TurnOnLights();
            }
        }
        #endregion

        #region Zone Animation Pipelines
        /// <summary>
        /// Fluently executes a batch synchronized visual lighting flicker animation sequence across a concrete <see cref="FacilityZone"/>.
        /// </summary>
        public static IEnumerator<float> FlickerLightsCoroutine(this FacilityZone targetZone, Color color, float duration, float frequency)
        {
            float interval = 1f / frequency.LimitMin(0.1f);
            int flickers = (int)Math.Round(duration / interval);

            Map.SetColorOfLights(color, targetZone);
            for (int i = 0; i < flickers; i++)
            {
                Map.TurnOffLights(interval * 0.5f, targetZone);
                yield return Timing.WaitForSeconds(interval * 0.5f);
                Map.TurnOnLights(targetZone);
                yield return Timing.WaitForSeconds(interval * 0.5f);
            }
            Map.ResetColorOfLights(targetZone);
        }
        #endregion
    }
}