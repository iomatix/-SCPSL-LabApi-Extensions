using LabApi.Features.Wrappers;
using MEC;
using PlayerRoles;
using System.Collections.Generic;

namespace LabApi.Extensions.Escape
{
    /// <summary>
    /// Provides a highly optimized, decoupled asynchronous execution matrix designed to orchestrate 
    /// multi-stage <see cref="EscapeScenario"/> lifecycles using standardized MEC coroutine tokens.
    /// </summary>
    internal static class EscapeEngine
    {
        /// <summary>
        /// Runs a comprehensive, non-blocking asynchronous evacuation routine driven entirely by the provided scenario parameters matrix.
        /// </summary>
        /// <param name="scenario">The concrete structural configuration rules dataset blueprint driving the execution pipeline.</param>
        /// <returns>An enumerator tracking the active execution timeline states across processing boundaries.</returns>
        public static IEnumerator<float> RunScenarioRoutine(EscapeScenario scenario)
        {
            if (scenario == null) yield break;

            // Trigger systemic structural event hooks immediately upon initialization
            scenario.OnSequenceStarted?.Invoke();

            // Broadcast the initial hint utilizing the zunified global extension layer seamlessly
            if (!string.IsNullOrEmpty(scenario.InitialHint))
            {
                PlayerExtensions.BroadcastHintToAll(scenario.InitialHint, scenario.InitialHintDuration);
            }

            // Phase 1 Standby: Wait for arrival vehicle transit timelines to resolve
            yield return Timing.WaitForSeconds(scenario.InitialDelay);

            // Trigger intermediary step execution bindings
            scenario.OnSequenceProcessing?.Invoke();

            // Phase 2 Processing: Wait for boarding corridors or doors to cycle open
            yield return Timing.WaitForSeconds(scenario.ProcessingDelay);

            // Execute single-pass spatial filtration across the global active entity map matrix
            foreach (Player player in Player.ReadyList)
            {
                if (player == null || !player.IsReady || !player.IsAlive || player.IsSCP)
                {
                    continue;
                }

                // Leverage our zoptimized squared displacement math extension directly
                if (player.IsWithinRadius(scenario.EscapeZone, scenario.EscapeRadius))
                {
                    // Fire decoupled custom plugin logic (e.g., Cache tracking, team scoring metrics)
                    scenario.OnPlayerValidated?.Invoke(player);

                    // Execute integrated batch status effect injection matrix
                    player.IsGodModeEnabled = true;
                    if (!string.IsNullOrEmpty(scenario.SuccessHint))
                    {
                        player.SendHint(scenario.SuccessHint);
                    }
                    player.Position = scenario.TeleportPosition;
                    player.ClearInventory();
                    player.ApplySensoryShock(scenario.FlashDuration, scenario.BlindDuration, scenario.BlurDuration, scenario.DeafenDuration);

                    // Defer final infrastructure entity role commit by localized timeline parameters safely
                    Timing.CallDelayed(scenario.FinalDelay, () =>
                    {
                        if (player != null && player.IsReady)
                        {
                            player.SetRole(RoleTypeId.Spectator, RoleChangeReason.Escaped);
                        }
                    });
                }
            }

            // Terminate sequence processing lifecycle nodes cleanly
            scenario.OnSequenceFinalized?.Invoke();
        }
    }
}