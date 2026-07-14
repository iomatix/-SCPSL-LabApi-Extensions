using Cassie;
using LabApi.Extensions.Misc;
using LabApi.Features.Wrappers;
using System;
using System.Collections.Generic;

namespace LabApi.Extensions
{
    public static class CassieExtensions
    {
        public static void CassieClear() => Announcer.Clear();

        /// <summary>
        /// Removes CR/LF and trims whitespace for safe CASSIE usage.
        /// </summary>
        public static string SanitizeCassieString(this string rawMessage)
            => string.IsNullOrWhiteSpace(rawMessage)
                ? string.Empty
                : rawMessage.Replace("\r", "").Replace("\n", " ").Trim();

        #region Single Message Dispatchers

        /// <summary>
        /// Glitchifies and dispatches a message, returning playback duration.
        /// </summary>
        public static double DispatchGlitchyMessage(string message, float glitchChance, float jamChance)
        {
            string sanitized = message.SanitizeCassieString();
            if (string.IsNullOrEmpty(sanitized)) return 0.0;

            try
            {
                string glitched = CassieGlitchifier.Glitchify(sanitized, glitchChance, jamChance);
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
        public static double DispatchMessage(string message, CassiePlaybackModifiers modifiers = default)
        {
            string sanitized = message.SanitizeCassieString();
            if (string.IsNullOrEmpty(sanitized)) return 0.0;

            try
            {
                Announcer.Message(sanitized, string.Empty, playBackground: false);
                return Announcer.CalculateDuration(sanitized, modifiers);
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
        public static void ProcessAndDispatchMessage(string message, string subtitles, bool clear, float priority, bool disableMessages = false, CassiePlaybackModifiers modifiers = default)
        {
            string sanitized = message.SanitizeCassieString();
            if (string.IsNullOrEmpty(sanitized)) return;

            if (clear) Announcer.Clear();

            string sanitizedSubs = subtitles.SanitizeCassieString();
            string finalSubs = (!string.IsNullOrEmpty(sanitizedSubs) && !disableMessages) ? sanitizedSubs : string.Empty;

            Announcer.Message(sanitized, customSubtitles: finalSubs, priority: priority, playBackground: false);
        }

        #endregion

        #region Batch Message Dispatchers (DRY, KISS, Zero-Allocation)

        /// <summary>
        /// Dispatches all messages in the collection.
        /// </summary>
        public static void DispatchMessage(this IEnumerable<string> messages, CassiePlaybackModifiers modifiers = default)
            => messages.ForEach(m => DispatchMessage(m, modifiers));

        /// <summary>
        /// Dispatches all provided messages (default modifiers).
        /// </summary>
        public static void DispatchMessage(params string[] messages)
            => ((IEnumerable<string>)messages).DispatchMessage(default);

        /// <summary>
        /// Dispatches all provided messages with custom modifiers.
        /// </summary>
        public static void DispatchMessage(CassiePlaybackModifiers modifiers, params string[] messages)
            => ((IEnumerable<string>)messages).DispatchMessage(modifiers);

        #endregion

        #region Utilities & Countdown Mechanics

        /// <summary>
        /// Converts an integer into a formatted CASSIE countdown string.
        /// </summary>
        public static string ToCassieCountdown(this int notifyTime, string context = "seconds until event detonation")
        {
            string sanitized = context.SanitizeCassieString();
            if (string.IsNullOrEmpty(sanitized)) sanitized = "seconds";

            return notifyTime switch
            {
                < 5 => $".G3 {notifyTime} .G5",
                >= 5 and <= 20 => $".G3 {notifyTime} seconds .G5",
                _ => $".G3 {notifyTime} {sanitized} .G5"
            };
        }

        /// <summary>
        /// Calculates playback duration of a single message.
        /// </summary>
        public static double CalculateCassieMessageDuration(string message, CassiePlaybackModifiers modifiers = default)
        {
            string sanitized = message.SanitizeCassieString();
            if (string.IsNullOrEmpty(sanitized)) return 0.0;

            return Announcer.CalculateDuration(sanitized, modifiers);
        }

        #endregion

        #region Duration Aggregation (DRY, Zero-Allocation)

        /// <summary>
        /// Calculates total duration of messages with per-message pitch modifiers.
        /// </summary>
        public static double CalculateTotalMessagesDurations(IDictionary<string, float> messageSpeedDictionary)
        {
            if (messageSpeedDictionary == null || messageSpeedDictionary.Count == 0)
                return 0.0;

            double total = 0.0;

            foreach (var kvp in messageSpeedDictionary)
            {
                CassiePlaybackModifiers mod = default;
                mod.Pitch = kvp.Value;
                total += CalculateCassieMessageDuration(kvp.Key, mod);
            }

            return total;
        }

        /// <summary>
        /// Calculates total duration of all messages in the collection.
        /// </summary>
        public static double CalculateTotalMessagesDurations(this IEnumerable<string> messages, CassiePlaybackModifiers modifiers = default)
        {
            if (messages == null) return 0.0;

            double total = 0.0;
            messages.ForEach(m => total += CalculateCassieMessageDuration(m, modifiers));
            return total;
        }

        /// <summary>
        /// Calculates total duration of provided messages (default modifiers).
        /// </summary>
        public static double CalculateTotalMessagesDurations(params string[] messages)
            => ((IEnumerable<string>)messages).CalculateTotalMessagesDurations(default);

        /// <summary>
        /// Calculates total duration of provided messages with custom modifiers.
        /// </summary>
        public static double CalculateTotalMessagesDurations(CassiePlaybackModifiers modifiers, params string[] messages)
            => ((IEnumerable<string>)messages).CalculateTotalMessagesDurations(modifiers);

        #endregion
    }
}
