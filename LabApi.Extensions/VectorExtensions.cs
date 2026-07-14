using UnityEngine;

namespace LabApi.Extensions
{
    /// <summary>
    /// Simple and fast helpers for working with Unity vectors.
    /// </summary>
    public static class VectorExtensions
    {
        /// <summary>
        /// Returns the distance between two points.
        /// </summary>
        public static float GetDistanceTo(this Vector3 origin, Vector3 target)
            => Vector3.Distance(origin, target);

        /// <summary>
        /// Returns the squared distance between two points (no square root).
        /// </summary>
        public static float GetDistanceSquaredTo(this Vector3 origin, Vector3 target)
            => Vector3.SqrMagnitude(origin - target);

        /// <summary>
        /// Reflects the vector upward if it points too far downward.
        /// </summary>
        public static Vector3 GetUpwardReflectedVector(this Vector3 direction, float dotThreshold = 0.707f)
        {
            return Vector3.Dot(direction, Vector3.down) > dotThreshold
                ? Vector3.Reflect(direction, Vector3.up)
                : direction;
        }

        /// <summary>
        /// Returns a random upward-facing direction scaled by the given magnitude.
        /// </summary>
        public static Vector3 GetRandomUpwardSphereVelocity(float magnitude = 1f)
        {
            Vector3 randomDirection = Random.onUnitSphere;
            return randomDirection.GetUpwardReflectedVector(0.707f) * magnitude;
        }

        /// <summary>
        /// Returns the vector unless it contains NaN or Infinity, in which case the fallback is returned.
        /// </summary>
        public static Vector3 Sanitize(this Vector3 vector, Vector3 fallback = default)
        {
            return vector.x.IsNanOrInfinity() ||
                   vector.y.IsNanOrInfinity() ||
                   vector.z.IsNanOrInfinity()
                ? fallback
                : vector;
        }
    }
}
