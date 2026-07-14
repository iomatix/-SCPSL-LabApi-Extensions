using LabApi.Extensions.Misc;
using LabApi.Features.Wrappers;
using System.Collections.Generic;
using UnityEngine;

namespace LabApi.Extensions
{
    /// <summary>
    /// Utility extensions for applying physics impulses to world pickups.
    /// </summary>
    public static class PickupExtensions
    {
        #region Single Pickup

        /// <summary>
        /// Applies a physics impulse to a pickup.
        /// </summary>
        public static void ApplyKineticBlast(this Pickup pickup, float linearVelocityMagnitude, float angularVelocityMagnitude)
        {
            if (pickup is null || pickup.IsDestroyed || !pickup.IsSpawned)
                return;

            var rb = pickup.Rigidbody;
            if (rb is null)
                return;

            rb.isKinematic = false;

            Vector3 force = VectorExtensions.GetRandomUpwardSphereVelocity(linearVelocityMagnitude);
            rb.linearVelocity = force;
            rb.angularVelocity = Random.insideUnitSphere * angularVelocityMagnitude;
        }

        #endregion

        #region Batch

        /// <summary>
        /// Applies a physics impulse to multiple pickups.
        /// </summary>
        public static void ApplyKineticBlast(this IEnumerable<Pickup> pickups, float linearVelocityMagnitude, float angularVelocityMagnitude)
            => pickups.ForEach(p => p?.ApplyKineticBlast(linearVelocityMagnitude, angularVelocityMagnitude));

        /// <summary>
        /// Applies a physics impulse to multiple pickups (params overload).
        /// </summary>
        public static void ApplyKineticBlast(float linearVelocityMagnitude, float angularVelocityMagnitude, params Pickup[] pickups)
            => ((IEnumerable<Pickup>)pickups).ApplyKineticBlast(linearVelocityMagnitude, angularVelocityMagnitude);

        #endregion
    }
}
