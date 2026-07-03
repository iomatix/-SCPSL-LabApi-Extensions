using System.Collections.Generic;
using LabApi.Features.Wrappers;
using UnityEngine;

namespace LabApi.Extensions
{
    /// <summary>
    /// Provides high-performance utility abstraction layers for managing dynamic items and active world-space pickup assets.
    /// </summary>
    internal static class PickupExtensions
    {
        /// <summary>
        /// Forcibly applies kinetic physical propulsion forces onto a single world-space pickup asset.
        /// Single Point of Truth for individual pickup physics manipulation.
        /// </summary>
        /// <param name="pickup">The target pickup entity undergoing physical acceleration.</param>
        /// <param name="linearVelocityMagnitude">The directional scale baseline applied to drive linear movement trajectories.</param>
        /// <param name="angularVelocityMagnitude">The rotational torque scale baseline applied to trigger spinning effects.</param>
        public static void ApplyKineticBlast(this Pickup pickup, float linearVelocityMagnitude, float angularVelocityMagnitude)
        {
            if (pickup is null || pickup.IsDestroyed || !pickup.IsSpawned) return;

            Rigidbody rb = pickup.Rigidbody;
            if (rb is null) return;

            try
            {
                rb.isKinematic = false;

                // Leverage our math extensions directly to compute floor deflection patterns
                Vector3 forceDirection = VectorExtensions.GetRandomUpwardSphereVelocity(linearVelocityMagnitude);

                rb.linearVelocity = forceDirection;
                rb.angularVelocity = Random.insideUnitSphere * angularVelocityMagnitude;
            }
            catch (System.Exception ex)
            {
                iLogger.Error("PickupExtensions.KineticBlast", $"Failed to apply physical velocity to pickup item: {ex.Message}");
            }
        }

        /// <summary>
        /// Iterates over an aggregated collection layout of spawned pickups and forcibly applies batch kinetic physical propulsion forces.
        /// Seamlessly delegates item execution to maintain absolute single responsibility and zero duplication.
        /// </summary>
        /// <param name="pickups">The target enumerable stream tracking world item entities undergoing physical acceleration.</param>
        /// <param name="linearVelocityMagnitude">The directional scale baseline applied to drive linear movement trajectories.</param>
        /// <param name="angularVelocityMagnitude">The rotational torque scale baseline applied to trigger spinning effects.</param>
        public static void ApplyKineticBlast(this IEnumerable<Pickup> pickups, float linearVelocityMagnitude, float angularVelocityMagnitude)
        {
            if (pickups is null) return;

            foreach (Pickup pickup in pickups)
            {
                // Absolute structural reuse – zero duplicated physics or try-catch logs.
                pickup.ApplyKineticBlast(linearVelocityMagnitude, angularVelocityMagnitude);
            }
        }
    }
}