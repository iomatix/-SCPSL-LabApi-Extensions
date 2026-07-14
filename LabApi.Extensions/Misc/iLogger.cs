using LabApi.Features.Console;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;

namespace LabApi.Extensions.Misc
{
    /// <summary>
    /// Centralized logging utility providing fast, structured console outputs with automatic full call-path resolution.
    /// </summary>
    public static class iLogger
    {
        // FIX: High-performance cache mapped by MethodBase to instantly resolve full dotted call paths with 0 heap overhead.
        private static readonly ConcurrentDictionary<MethodBase, string> _callPathCache = new();

        /// <summary>
        /// Automatically resolves and caches the full dotted call path (Namespace.Class.Method) of the caller.
        /// </summary>
        private static string GetDefaultSource()
        {
            // Skip 2 frames: Frame 0 is GetDefaultSource, Frame 1 is the iLogger method (Info/Warn), Frame 2 is the actual plugin method!
            MethodBase callingMethod = new StackFrame(2, false).GetMethod();

            if (callingMethod == null)
                return "Unknown";

            if (!_callPathCache.TryGetValue(callingMethod, out string callPath))
            {
                string typeName = callingMethod.DeclaringType?.FullName ?? "UnknownContext";
                string methodName = callingMethod.Name;

                // Generates e.g. "LabApi.Extensions.Escape.EscapeEngine.ProcessPlayerEscape"
                callPath = string.Concat(typeName, ".", methodName);
                _callPathCache[callingMethod] = callPath;
            }

            return callPath;
        }

        #region Informational Broadcasters

        /// <summary>
        /// Logs an informational message, automatically resolving the full dotted call path.
        /// </summary>
        public static void Info(string message)
            => Logger.Info(string.Concat("[", GetDefaultSource(), "] ", message));

        /// <summary>
        /// Logs an informational message with a specified custom tag.
        /// </summary>
        public static void Info(string source, string message)
            => Logger.Info(string.Concat("[", source, "] ", message));

        #endregion

        #region Warning Alert Core

        /// <summary>
        /// Logs a warning message, automatically resolving the full dotted call path.
        /// </summary>
        public static void Warn(string message)
            => Logger.Warn(string.Concat("[", GetDefaultSource(), "] ", message));

        /// <summary>
        /// Logs a warning message with a specified custom tag.
        /// </summary>
        public static void Warn(string source, string message)
            => Logger.Warn(string.Concat("[", source, "] ", message));

        #endregion

        #region Exception and Error Handlers

        /// <summary>
        /// Logs an error message, automatically resolving the full dotted call path.
        /// </summary>
        public static void Error(string message)
            => Logger.Error(string.Concat("[", GetDefaultSource(), "] ", message));

        /// <summary>
        /// Logs an error message with a specified custom tag.
        /// </summary>
        public static void Error(string source, string message)
            => Logger.Error(string.Concat("[", source, "] ", message));

        #endregion

        #region Debug Diagnostic Overloads

        /// <summary>
        /// Logs a debug message if debugging is enabled, automatically resolving the full dotted call path.
        /// </summary>
        public static void Debug(string message, bool isDebugEnabled)
        {
            if (isDebugEnabled)
            {
                Logger.Debug(string.Concat("[", GetDefaultSource(), "] [DEBUG] ", message));
            }
        }

        /// <summary>
        /// Logs a debug message with a custom tag if debugging is enabled.
        /// </summary>
        public static void Debug(string source, string message, bool isDebugEnabled)
        {
            if (isDebugEnabled)
            {
                Logger.Debug(string.Concat("[", source, "] [DEBUG] ", message));
            }
        }

        /// <summary>
        /// Logs a debug message using deferred evaluation, automatically resolving the full dotted call path.
        /// </summary>
        public static void Debug(Func<string> messageFactory, bool isDebugEnabled)
        {
            if (isDebugEnabled && messageFactory != null)
            {
                Logger.Debug(string.Concat("[", GetDefaultSource(), "] [DEBUG] ", messageFactory()));
            }
        }

        /// <summary>
        /// Logs a debug message with a custom tag using deferred evaluation.
        /// </summary>
        public static void Debug(string source, Func<string> messageFactory, bool isDebugEnabled)
        {
            if (isDebugEnabled && messageFactory != null)
            {
                Logger.Debug(string.Concat("[", source, "] [DEBUG] ", messageFactory()));
            }
        }

        #endregion

        #region Local Trace (Stripped in Release Builds)

        /// <summary>
        /// Logs a detailed local trace message. This call is completely stripped out by the compiler in Release builds.
        /// </summary>
        [Conditional("DEBUG")]
        public static void LocalTrace(string source, string message)
        {
            Logger.Debug(string.Concat("[", source, "] [LOCAL-TRACE] ", message));
        }

        #endregion
    }
}