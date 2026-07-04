using LabApi.Features.Wrappers;
using System;
using UnityEngine;

namespace LabApi.Extensions.Escape
{
    /// <summary>
    /// Represents a generalized, highly configurable architectural template for spatial evacuation, 
    /// teleportation sequences, and timed server-side gameplay progression scenarios.
    /// </summary>
    internal class EscapeScenario
    {
        #region Core Identification
        /// <summary>
        /// Gets or sets the unique structural identity or display label assigned to this execution scenario.
        /// </summary>
        public string Name { get; set; } = "Default Evacuation";
        #endregion

        #region Spatial & Physics Boundaries
        /// <summary>
        /// Gets or sets the global world-space coordinate vector defining the center anchor of the escape verification perimeter.
        /// </summary>
        public Vector3 EscapeZone { get; set; } = Vector3.zero;

        /// <summary>
        /// Gets or sets the maximum linear radial boundary threshold constraint in meters allowed to pass proximity verification.
        /// </summary>
        public float EscapeRadius { get; set; } = 5f;

        /// <summary>
        /// Gets or sets the target destination coordinate vector where eligible entities are transferred post-validation.
        /// </summary>
        public Vector3 TeleportPosition { get; set; } = Vector3.zero;
        #endregion

        #region Automated UI/UX Telemetry
        /// <summary>
        /// Gets or sets the localized notification Hint text sequence broadcast to the entire server pool when the timeline initiates.
        /// </summary>
        public string InitialHint { get; set; }

        /// <summary>
        /// Gets or sets the visual display lifetime duration in seconds allocated for the initial broadcast interface layer.
        /// </summary>
        public float InitialHintDuration { get; set; } = 6f;

        /// <summary>
        /// Gets or sets the direct text notification string transmitted exclusively to an individual entity upon successful scenario verification.
        /// </summary>
        public string SuccessHint { get; set; }
        #endregion

        #region Timeline Chronology Delays
        /// <summary>
        /// Gets or sets the temporal operational standby delay in seconds executed immediately after sequence activation.
        /// </summary>
        public float InitialDelay { get; set; } = 0f;

        /// <summary>
        /// Gets or sets the secondary simulation processing delay in seconds spent during structural gameplay transition steps.
        /// </summary>
        public float ProcessingDelay { get; set; } = 0f;

        /// <summary>
        /// Gets or sets the brief terminal resolution delay in seconds applied directly before role transformation commits.
        /// </summary>
        public float FinalDelay { get; set; } = 0.5f;
        #endregion

        #region Sensory Amortization Configuration
        /// <summary>
        /// Gets or sets the duration coefficient in seconds for the post-escape screen flash trauma overlay.
        /// </summary>
        public float FlashDuration { get; set; } = 1.75f;

        /// <summary>
        /// Gets or sets the duration coefficient in seconds for the post-escape structural blindness status block.
        /// </summary>
        public float BlindDuration { get; set; } = 0f;

        /// <summary>
        /// Gets or sets the duration coefficient in seconds for the post-escape visual lens blur distortion layer.
        /// </summary>
        public float BlurDuration { get; set; } = 0f;

        /// <summary>
        /// Gets or sets the duration coefficient in seconds for the post-escape environment auditory dampening filter.
        /// </summary>
        public float DeafenDuration { get; set; } = 30f;
        #endregion

        #region Lifecycle Event Interceptors
        /// <summary>
        /// Occurs instantly when the master execution pipeline initializes the scenario tracking tree.
        /// </summary>
        public Action OnSequenceStarted { get; set; }

        /// <summary>
        /// Occurs immediately after the initial standby delay threshold expires, separating arrival steps from processing sweeps.
        /// </summary>
        public Action OnSequenceProcessing { get; set; }

        /// <summary>
        /// Occurs dynamically whenever an individual player entity successfully passes position, alignment, and structural life-state audits.
        /// Ideal for custom external data caching, point allocations, or inventory tracking mutations.
        /// </summary>
        public Action<Player> OnPlayerValidated { get; set; }

        /// <summary>
        /// Occurs upon complete finalization of the sequence loop iteration sweep.
        /// </summary>
        public Action OnSequenceFinalized { get; set; }
        #endregion
    }
}