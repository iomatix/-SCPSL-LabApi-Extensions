using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabApi.Extensions.Components
{
    /// <summary>
    /// An ultra-performance native Unity MonoBehaviour component that locks its transform position 
    /// smoothly onto a live target source, neutralizing ticking garbage collection and coordinate stutter.
    /// </summary>
    internal sealed class RuntimeTransformTracker : MonoBehaviour
    {
        private Transform _targetTransform;
        private Vector3 _positionOffset;
        private System.Func<bool> _validityPredicate;

        /// <summary>
        /// Configures the tracking metrics and binds the operational data targets natively.
        /// </summary>
        public void Setup(Transform target, Vector3 offset = default, System.Func<bool> validityPredicate = null)
        {
            _targetTransform = target;
            _positionOffset = offset;
            _validityPredicate = validityPredicate;
        }

        /// <summary>
        /// Execution loop hooked directly by the Unity Engine core. Fires precisely after standard physics updates 
        /// to guarantee perfectly synchronized sub-frame position locking with zero tracking lag.
        /// </summary>
        private void LateUpdate()
        {
            // Failsafe execution check: if the target object is destroyed or custom predicate yields false, self-destruct immediately
            if (_targetTransform == null || (_validityPredicate != null && !_validityPredicate.Invoke()))
            {
                Destroy(gameObject);
                return;
            }

            // Lock position coordinates natively on the engine layer
            transform.position = _targetTransform.position + _positionOffset;
        }
    }
}
