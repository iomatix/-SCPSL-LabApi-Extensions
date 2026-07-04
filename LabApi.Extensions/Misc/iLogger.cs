using LabApi.Features.Console;
using System.Diagnostics;

namespace LabApi.Extensions.Misc
{
    /// <summary>
    /// Provides an enterprise-grade centralized logging matrix for the iomatix ecosystem,
    /// natively bridging runtime configuration metadata checks with the underlying LabAPI console engine.
    /// </summary>
    internal static class iLogger
    {
        #region Informational Broadcasters
        /// <summary>
        /// Dispatches a standardized informational telemetry entry directly onto the global console stream interface.
        /// </summary>
        /// <param name="message">The structural runtime notification string text payload targeted for transmission.</param>
        public static void Info(string message) => Logger.Info(message);

        /// <summary>
        /// Dispatches an informational trace message bound into a discrete, bracketed subsystem contextual source tag.
        /// </summary>
        /// <param name="source">The operational identifier tracking the originating system module domain location.</param>
        /// <param name="message">The structural runtime notification string text payload targeted for transmission.</param>
        public static void Info(string source, string message) => Logger.Info($"[{source}] {message}");
        #endregion

        #region Warning Alert Core
        /// <summary>
        /// Captures a non-fatal anomaly alert execution sequence or automated threshold correction trace to indicate soft system variances.
        /// </summary>
        /// <param name="message">The detailed descriptive warning text sequence tracking the non-fatal anomaly event bounds.</param>
        public static void Warn(string message) => Logger.Warn(message);

        /// <summary>
        /// Captures a non-fatal anomaly alert execution sequence bound within a distinct tracking domain header tag.
        /// </summary>
        /// <param name="source">The operational identifier tracking the originating layout sector producing the warning event.</param>
        /// <param name="message">The detailed descriptive warning text sequence tracking the non-fatal anomaly event bounds.</param>
        public static void Warn(string source, string message) => Logger.Warn($"[{source}] {message}");
        #endregion

        #region Exception and Error Handlers
        /// <summary>
        /// Records a critical runtime failure path or exceptional pipeline interruption that severely compromises system integrity.
        /// </summary>
        /// <param name="message">The raw exceptional error trace description text layout targeting the logging grid.</param>
        public static void Error(string message) => Logger.Error(message);

        /// <summary>
        /// Records a critical runtime failure trace wrapped inside a contextual tracking system exception identifier tag.
        /// </summary>
        /// <param name="source">The concrete architectural infrastructure module or component instance registering the underlying crash tracking telemetry.</param>
        /// <param name="message">The raw exceptional error trace description text layout targeting the logging grid.</param>
        public static void Error(string source, string message) => Logger.Error($"[{source}] {message}");
        #endregion

        #region Production-Grade Debug Diagnostic Vector
        /// <summary>
        /// Routes a low-level diagnostic trace message onto the console output pipe conditionally based on a production runtime state evaluation block.
        /// Safely insulates live server allocations against verbose data pollution during standard execution deployments.
        /// </summary>
        /// <param name="message">The granular evaluation string sequence mapping real-time asset properties or system parameters.</param>
        /// <param name="isDebugEnabled">The runtime configuration Boolean state toggle controlling active message stream output generation.</param>
        public static void Debug(string message, bool isDebugEnabled)
        {
            if (isDebugEnabled)
            {
                Logger.Debug($"[DEBUG] {message}");
            }
        }

        /// <summary>
        /// Routes a contextual low-level trace sequence wrapped inside an explicit subsystem domain tag if live diagnostics evaluate to true.
        /// </summary>
        /// <param name="source">The sub-system infrastructure label originating the granular diagnostic evaluation telemetry stream.</param>
        /// <param name="message">The granular evaluation string sequence mapping real-time asset properties or system parameters.</param>
        /// <param name="isDebugEnabled">The runtime configuration Boolean state toggle controlling active message stream output generation.</param>
        public static void Debug(string source, string message, bool isDebugEnabled)
        {
            if (isDebugEnabled)
            {
                Logger.Debug($"[{source}] [DEBUG] {message}");
            }
        }

        /// <summary>
        /// Executes an aggressive, highly detailed debugging pipeline trace. Active exclusively during local local IDE sandbox compilation phases.
        /// Completely omitted and stripped out by the compiler in release builds to guarantee absolute zero deployment allocation footprints.
        /// </summary>
        /// <param name="source">The internal structural signature pinpointing the code execution node path tracking.</param>
        /// <param name="message">The heavy raw dump literal tracking data payload evaluated during runtime code validation.</param>
        [Conditional("DEBUG")]
        public static void LocalTrace(string source, string message)
        {
            Logger.Debug($"[{source}] [LOCAL-TRACE] {message}");
        }
        #endregion
    }
}