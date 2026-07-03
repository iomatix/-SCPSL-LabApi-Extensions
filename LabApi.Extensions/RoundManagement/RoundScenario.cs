using System;

namespace LabApi.Extensions.RoundManagement
{
    /// <summary>
    /// Provides an abstract architectural template for defining custom server-side round ending sequences, 
    /// gamemode transitions, or cataclysmic map lifecycle events.
    /// </summary>
    internal abstract class RoundScenario
    {
        protected readonly RoundController Controller;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoundScenario"/> class bound to a central lifecycle controller.
        /// </summary>
        protected RoundScenario(RoundController roundController)
        {
            Controller = roundController;
        }

        /// <summary>
        /// Gets or sets an optional custom lifecycle reclamation hook executed automatically when the scenario completes.
        /// </summary>
        public Action OnCleanupRequested { get; set; }

        /// <summary>
        /// Executes the concrete internal scenario logic matrix. Must be overridden by implementation nodes.
        /// </summary>
        public abstract void Execute();

        /// <summary>
        /// Finalizes the scenario lifecycle step, automatically invoking registered external subsystem cleanup actions.
        /// </summary>
        protected virtual void OnComplete()
        {
            OnCleanupRequested?.Invoke();
        }
    }
}