using Cassie;
using LabApi.Extensions.Misc;
using LabApi.Features.Wrappers;
using System;
using System.Collections.Generic;

namespace LabApi.Extensions
{
    /// <summary>
    /// Highly optimized helper extensions for working with the CASSIE announcement system.
    /// </summary>
    public static class CassieExtensions
    {
        /// <summary>
        /// Clears the CASSIE announcement queue.
        /// </summary>
        public static void CassieClear() => Announcer.Clear();

        /// <summary>
        /// Removes CR/LF and trims whitespace for safe CASSIE usage with zero redundant allocations.
        /// </summary>
        public static string SanitizeCassieString(this string rawMessage)
        {
            if (string.IsNullOrWhiteSpace(rawMessage))
                return string.Empty;

            // Check for carriage returns, newlines, and trim requirements first to avoid redundant heap allocations.
            bool hasCarriageReturn = rawMessage.IndexOf('\r') != -1;
            bool hasNewline = rawMessage.IndexOf('\n') != -1;
            bool needsTrim = char.IsWhiteSpace(rawMessage[0]) || char.IsWhiteSpace(rawMessage[rawMessage.Length - 1]);

            if (!hasCarriageReturn && !hasNewline && !needsTrim)
                return rawMessage;

            string sanitized = rawMessage;
            if (hasCarriageReturn)
                sanitized = sanitized.Replace("\r", string.Empty);

            if (hasNewline)
                sanitized = sanitized.Replace("\n", " ");

            return needsTrim ? sanitized.Trim() : sanitized;
        }

        #region Single Message Dispatchers

        /// <summary>
        /// Glitchifies and dispatches a message, returning the calculated playback duration.
        /// </summary>
        public static double DispatchGlitchyMessage(string message, float glitchChance, float jamChance)
        {
            string sanitized = message.SanitizeCassieString();
            if (string.IsNullOrEmpty(sanitized))
                return 0.0;

            try
            {
                string glitched = CassieGlitchifier.Glitchify(sanitized, glitchChance, jamChance);

                // Note: glitchScale is set to 0f as requested by glitch design.
                Announcer.Message(glitched, string.Empty, playBackground: false, glitchScale: 0f);
                return Announcer.CalculateDuration(glitched, default);
            }
            catch (Exception ex)
            {
                iLogger.Error("Cassie.GlitchyMessage", ex.Message);
                return 0.0;
            }
        }

        /// <summary>
        /// Dispatches a standard CASSIE message and returns playback duration.
        /// </summary>
        public static double DispatchMessage(string message)
        {
            string sanitized = message.SanitizeCassieString();
            if (string.IsNullOrEmpty(sanitized))
                return 0.0;

            try
            {
                Announcer.Message(sanitized, string.Empty, playBackground: false);
                return Announcer.CalculateDuration(sanitized, default);
            }
            catch (Exception ex)
            {
                iLogger.Error("Cassie.Message", ex.Message);
                return 0.0;
            }
        }

        /// <summary>
        /// Dispatches a formatted CASSIE message with optional subtitles and priority.
        /// </summary>
        public static void ProcessAndDispatchMessage(
            string message,
            string subtitles,
            bool clear,
            float priority,
            bool disableMessages = false)
        {
            string sanitized = message.SanitizeCassieString();
            if (string.IsNullOrEmpty(sanitized))
                return;

            if (clear)
                Announcer.Clear();

            string sanitizedSubs = subtitles.SanitizeCassieString();
            string finalSubs = (!string.IsNullOrEmpty(sanitizedSubs) && !disableMessages) ? sanitizedSubs : string.Empty;

            Announcer.Message(sanitized, customSubtitles: finalSubs, priority: priority, playBackground: false);
        }

        #endregion

        #region Batch Message Dispatchers

        /// <summary>
        /// Dispatches all messages in the collection with zero-allocation path optimization (no lambda closures).
        /// </summary>
        public static void DispatchMessage(this IEnumerable<string> messages)
        {
            if (messages == null)
                return;

            // Fast path for arrays - compile-time optimized, 0 allocations
            if (messages is string[] array)
            {
                int count = array.Length;
                for (int i = 0; i < count; i++)
                {
                    DispatchMessage(array[i]);
                }
                return;
            }

            // Fast path for Lists - compile-time optimized, 0 allocations
            if (messages is List<string> list)
            {
                int count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    DispatchMessage(list[i]);
                }
                return;
            }

            // Fallback for custom collections (avoids lambda display class / closure allocations)
            foreach (string message in messages)
            {
                DispatchMessage(message);
            }
        }

        /// <summary>
        /// Dispatches all provided messages.
        /// </summary>
        public static void DispatchMessage(params string[] messages)
            => messages.DispatchMessage();

        #endregion

        #region Utilities & Countdown Mechanics

        /// <summary>
        /// Converts an integer into a formatted CASSIE countdown string.
        /// </summary>
        public static string ToCassieCountdown(this int notifyTime, string context = "seconds until event detonation")
        {
            switch (notifyTime)
            {
                case < 5:
                    return $".G3 {notifyTime} .G5";
                case >= 5 and <= 20:
                    return $".G3 {notifyTime} seconds .G5";
                default:
                    // OPTIMIZATION: Only pay the sanitization performance penalty when actually using the context.
                    string sanitized = context.SanitizeCassieString();
                    if (string.IsNullOrEmpty(sanitized))
                        sanitized = "seconds";

                    return $".G3 {notifyTime} {sanitized} .G5";
            }
        }

        /// <summary>
        /// Calculates playback duration of a single message.
        /// </summary>
        public static double CalculateCassieMessageDuration(string message, CassiePlaybackModifiers modifiers = default)
        {
            string sanitized = message.SanitizeCassieString();
            if (string.IsNullOrEmpty(sanitized))
                return 0.0;

            return Announcer.CalculateDuration(sanitized, modifiers);
        }

        #endregion

        #region Duration Aggregation

        /// <summary>
        /// Calculates total duration of messages with per-message pitch modifiers, optimized to avoid GC heap allocations.
        /// </summary>
        public static double CalculateTotalMessagesDurations(IDictionary<string, float> messageSpeedDictionary)
        {
            if (messageSpeedDictionary == null || messageSpeedDictionary.Count == 0)
                return 0.0;

            double total = 0.0;

            // OPTIMIZATION: Pattern match to concrete Dictionary to leverage fast, non-boxing struct enumerator.
            if (messageSpeedDictionary is Dictionary<string, float> concreteDict)
            {
                foreach (var kvp in concreteDict)
                {
                    CassiePlaybackModifiers mod = default;
                    mod.Pitch = kvp.Value;
                    total += CalculateCassieMessageDuration(kvp.Key, mod);
                }
                return total;
            }

            // Fallback for other IDictionary implementations
            foreach (var kvp in messageSpeedDictionary)
            {
                CassiePlaybackModifiers mod = default;
                mod.Pitch = kvp.Value;
                total += CalculateCassieMessageDuration(kvp.Key, mod);
            }

            return total;
        }

        /// <summary>
        /// Calculates total duration of all messages in the collection, optimized to avoid GC heap allocations.
        /// </summary>
        public static double CalculateTotalMessagesDurations(this IEnumerable<string> messages, CassiePlaybackModifiers modifiers = default)
        {
            if (messages == null)
                return 0.0;

            double total = 0.0;

            // OPTIMIZATION: Fast path to avoid generic enumerator boxing on common collections
            if (messages is string[] array)
            {
                int count = array.Length;
                for (int i = 0; i < count; i++)
                {
                    total += CalculateCassieMessageDuration(array[i], modifiers);
                }
                return total;
            }

            if (messages is List<string> list)
            {
                int count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    total += CalculateCassieMessageDuration(list[i], modifiers);
                }
                return total;
            }

            foreach (string m in messages)
            {
                total += CalculateCalculateDurationFallback(m, modifiers);
            }

            return total;
        }

        // Helper method to keep loop body extremely clean and free from redundant sanitizations.
        private static double CalculateCalculateDurationFallback(string message, CassiePlaybackModifiers modifiers)
        {
            return CalculateCassieMessageDuration(message, modifiers);
        }

        /// <summary>
        /// Calculates total duration of provided messages (default modifiers).
        /// </summary>
        public static double CalculateTotalMessagesDurations(params string[] messages)
            => messages.CalculateTotalMessagesDurations(default);

        /// <summary>
        /// Calculates total duration of provided messages with custom modifiers.
        /// </summary>
        public static double CalculateTotalMessagesDurations(CassiePlaybackModifiers modifiers, params string[] messages)
            => messages.CalculateTotalMessagesDurations(modifiers);

        #endregion
    }
}