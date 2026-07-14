using System;
using UnityEngine;

namespace LabApi.Extensions.Components
{
    /// <summary>
    /// A lightweight Unity MonoBehaviour that smoothly locks its position (and optionally rotation) 
    /// to a target Transform with a specified offset.
    /// </summary>
    internal sealed class RuntimeTransformTracker : MonoBehaviour
    {
        private Transform _myTransform;
        private Transform _targetTransform;
        private Vector3 _positionOffset;
        private Quaternion _rotationOffset;
        private bool _trackRotation;
        private Func<bool> _validityPredicate;

        /// <summary>
        /// Cached self transform reference initialized on startup to completely bypass native property-get overhead.
        /// </summary>
        private void Awake()
        {
            _myTransform = transform;
        }

        /// <summary>
        /// Configures the tracking targets, offset parameters, and custom validity checks safely.
        /// </summary>
        /// <param name="target">The target transform to follow.</param>
        /// <param name="positionOffset">The position offset relative to the target.</param>
        /// <param name="validityPredicate">Optional callback check. If it returns false, the tracker auto-destructs.</param>
        /// <param name="trackRotation">If set to true, the tracker will also match the target's rotation.</param>
        /// <param name="rotationOffset">The rotation offset relative to the target (defaults to identity).</param>
        public void Setup(
            Transform target,
            Vector3 positionOffset = default,
            Func<bool> validityPredicate = null, // FIX: Restored to 3rd position to guarantee 100% legacy call compatibility!
            bool trackRotation = false,
            Quaternion? rotationOffset = null) // FIX: Used nullable type to avoid invalid default(Quaternion) (0,0,0,0) bug.
        {
            _targetTransform = target;
            _positionOffset = positionOffset;
            _validityPredicate = validityPredicate;
            _trackRotation = trackRotation;
            _rotationOffset = rotationOffset ?? Quaternion.identity;
        }

        /// <summary>
        /// Updates the position precisely after physics to prevent tracking stutter or visual lag.
        /// </summary>
        private void LateUpdate()
        {
            // FIX: Safe Unity reference check. If the target is destroyed, or the predicate returns false, destroy ourselves.
            if (_targetTransform == null || (_validityPredicate != null && !_validityPredicate.Invoke()))
            {
                Destroy(gameObject);
                return;
            }

            // FIX: Used cached transform reference to avoid the native property-get overhead of 'transform'.
            _myTransform.position = _targetTransform.position + _positionOffset;

            // Optional rotation tracking (only executes if explicitly requested during Setup)
            if (_trackRotation)
            {
                _myTransform.rotation = _targetTransform.rotation * _rotationOffset;
            }
        }
    }
}