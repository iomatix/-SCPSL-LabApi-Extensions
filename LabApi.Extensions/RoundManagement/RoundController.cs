using LabApi.Features.Wrappers;
using MEC;
using System.Collections.Generic;
using System.Linq;

namespace LabApi.Extensions.RoundManagement
{
    /// <summary>
    /// Provides a highly optimized, stateful controller matrix designed to register, isolate, 
    /// and safely execute custom <see cref="RoundScenario"/> logic branches while overriding native round ending metrics.
    /// </summary>
    internal class RoundController
    {
        private readonly HashSet<RoundScenario> _registeredScenarios = new HashSet<RoundScenario>();
        private bool _autoRoundEndLocked;

        /// <summary>
        /// Gets a value indicating whether the native server automatic round evaluation loop is currently locked out.
        /// </summary>
        public bool IsRoundEndLocked => _autoRoundEndLocked;

        /// <summary>
        /// Gets an enumerable matrix stream tracking all currently registered architectural scenario nodes.
        /// </summary>
        public IEnumerable<RoundScenario> EndingScenarios => _registeredScenarios;

        /// <summary>
        /// Commits a concrete scenario node structure into the internal tracking state registry.
        /// </summary>
        public void RegisterScenario(RoundScenario scenario)
        {
            if (scenario != null)
            {
                _registeredScenarios.Add(scenario);
            }
        }

        /// <summary>
        /// Locks or unlocks native automatic round ending routines utilizing underlying LabAPI framework properties.
        /// </summary>
        public void SetAutoRoundEndLock(bool locked)
        {
            _autoRoundEndLocked = locked;
            Round.IsLocked = locked;
        }

        /// <summary>
        /// Enforces an administrative lockout on standard round termination and executes the targeted scenario logic stream immediately.
        /// </summary>
        public void ExecuteScenario(RoundScenario scenario)
        {
            if (scenario == null) return;

            SetAutoRoundEndLock(true);
            scenario.Execute();
        }

        /// <summary>
        /// Gracefully terminates the round lifecycle sequence after a deferred temporal delay window, forcing execution metrics.
        /// </summary>
        public void EndRoundGracefully(float delay = 0f, string coroutineTag = "CustomRoundEnd")
        {
            var endCoroutine = Timing.CallDelayed(delay, () =>
            {
                SetAutoRoundEndLock(false);
                Round.End(force: true);
            });
            endCoroutine.Tag = coroutineTag;
        }

        /// <summary>
        /// Queries the registered scenario registry matrix to resolve and yield a specific strongly-typed scenario implementation.
        /// </summary>
        public T GetScenario<T>() where T : RoundScenario
        {
            return _registeredScenarios.OfType<T>().FirstOrDefault();
        }
    }
}