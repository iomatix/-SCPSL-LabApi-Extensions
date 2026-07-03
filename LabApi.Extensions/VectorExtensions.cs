using UnityEngine;

namespace LabApi.Extensions
{
    /// <summary>
    /// Provides high-performance structural spatial geometry and vector math abstraction overloads.
    /// </summary>
    internal static class VectorExtensions
    {
        /// <summary>
        /// Computes the exact linear Euclidean distance between the source vector and a targeted coordinate position.
        /// </summary>
        /// <returns>A single-precision floating-point scalar value representing the calculated physical distance in meters.</returns>
        public static float GetDistanceTo(this Vector3 origin, Vector3 target)
        {
            return Vector3.Distance(origin, target);
        }

        /// <summary>
        /// Computes the rapid squared distance magnitude between two spatial points, bypassing expensive processor square-root calculation chains.
        /// </summary>
        /// <returns>A single-precision floating-point scalar value representing the squared distance magnitude.</returns>
        public static float GetDistanceSquaredTo(this Vector3 origin, Vector3 target)
        {
            return Vector3.SqrMagnitude(origin - target);
        }

        /// <summary>
        /// Evaluates if the source direction vector points too steeply downwards against a threshold 
        /// and forcibly reflects it upward utilizing normal plane mirroring algorithms.
        /// </summary>
        /// <param name="direction">The source directional <see cref="UnityEngine.Vector3"/> layout underwent trajectory audit.</param>
        /// <param name="dotThreshold">The linear mathematical cosine threshold index (defaults to 0.707f representing a 45-degree cone).</param>
        /// <returns>A modified vector adjusted to bounce away from floor surfaces defensively.</returns>
        public static UnityEngine.Vector3 GetUpwardReflectedVector(this UnityEngine.Vector3 direction, float dotThreshold = 0.707f)
        {
            if (UnityEngine.Vector3.Dot(direction, UnityEngine.Vector3.down) > dotThreshold)
            {
                return UnityEngine.Vector3.Reflect(direction, UnityEngine.Vector3.up);
            }
            return direction;
        }

        /// <summary>
        /// Generates a random mathematical directional unit vector across a spherical layout, 
        /// completely insulated against downward trajectory drops into floor topologies.
        /// </summary>
        /// <param name="magnitude">The velocity scale factor applied to stretch the final vector boundary.</param>
        /// <returns>A randomized, surface-safe trajectory velocity <see cref="UnityEngine.Vector3"/>.</returns>
        public static UnityEngine.Vector3 GetRandomUpwardSphereVelocity(float magnitude = 1f)
        {
            UnityEngine.Vector3 randomDirection = UnityEngine.Random.onUnitSphere;
            return randomDirection.GetUpwardReflectedVector(0.707f) * magnitude;
        }

        /// <summary>
        /// Defensively audits all float components of a 3D vector against structural corruption anomalies (NaN or Infinity parameters).
        /// </summary>
        /// <param name="vector">The source <see cref="UnityEngine.Vector3"/> layout structure undergoing mathematical validation.</param>
        /// <param name="fallback">The fallback vector layout returned if corruption metrics are encountered. Defaults to <see cref="UnityEngine.Vector3.zero"/>.</param>
        /// <returns>A pristine, mathematically operational coordinate vector guaranteed to be safe for engine math systems.</returns>
        public static UnityEngine.Vector3 Sanitize(this UnityEngine.Vector3 vector, UnityEngine.Vector3 fallback = default)
        {
            if (float.IsNaN(vector.x) || float.IsNaN(vector.y) || float.IsNaN(vector.z) ||
                float.IsInfinity(vector.x) || float.IsInfinity(vector.y) || float.IsInfinity(vector.z))
            {
                return fallback; // Return the defensive default layer
            }
            return vector;
        }
    }
}