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
        /// Sanitizes a raw CASSIE string by removing carriage returns, replacing newlines with spaces, and trimming whitespace.
        /// </summary>
        public static string SanitizeCassieString(this string rawMessage)
            => string.IsNullOrWhiteSpace(rawMessage)
                ? string.Empty
                : rawMessage.Replace("\r", "").Replace("\n", " ").Trim();

        #region Single Message Dispatchers
        /// <summary>
        /// Glitchifies the specified message internally and dispatches the announcement, returning its playback duration.
        /// </summary>
        public static double DispatchGlitchyMessage(string message, float glitchChance, float jamChance)
        {
            string sanitizedInput = message.SanitizeCassieString();
            if (string.IsNullOrEmpty(sanitizedInput)) return 0.0;

            try
            {
                // Encapuslate the glitchification internally to eliminate duplicate API arguments
                string glitchedMessage = CassieGlitchifier.Glitchify(sanitizedInput, glitchChance, jamChance);

                // We set glMessageitchScale to 0f because the text already contains explicit glitch/jam tokens
                Announcer.Message(glitchedMessage, string.Empty, playBackground: false, glitchScale: 0f);

                return Announcer.CalculateDuration(glitchedMessage, default);
            }
            catch (Exception ex)
            {
                iLogger.Error("Cassie.GlitchyMessage", $"Vocal grid runtime suspension anomaly: {ex.Message}");
                return 0.0;
            }
        }

        /// <summary>
        /// Dispatches a standard CASSHE announcement and returns its estimated playback duration using the provided modifiers.
        /// </summary>
        public static double DispatchMessage(string message, CassiePlaybackModifiers modifiers = default)
        {
            string sanitizedMessage = message.SanitizeCassieString();
            if (string.IsNullOrEmpty(sanitizedMessage)) return 0.0;

            try
            {
                Announcer.Message($"{sanitizedMessage}", string.Empty, playBackground: false);
                return Announcer.CalculateDuration(sanitizedMessage, modifiers);
            }
            catch (Exception ex)
            {
                iLogger.Error("Cassie.Message", $"Vocal pipeline critical failure: {ex.Message}");
                return 0.0;
            }
        }

        /// <summary>
        /// Sanitizes, formats, and dispatches a CASSIE announcement with custom subtitles and queue priority.
        /// </summary>
        public static void ProcessAndDispatchMessage(string message, string customSubtitles, bool shouldClear, float priority, bool disableMessages = false, CassiePlaybackModifiers modifiers = default)
        {
            string sanitizedMessage = message.SanitizeCassieString();
            if (string.IsNullOrEmpty(sanitizedMessage)) return;

            if (shouldClear) Announcer.Clear();

            string sanitizedSubtitles = customSubtitles.SanitizeCassieString();
            string processedSubtitles = (!string.IsNullOrEmpty(sanitizedSubtitles) && !disableMessages) ? sanitizedSubtitles : string.Empty;

            // Note: If your native Announcer.Message overload supports CassiePlaybackModifiers, pass it as a parameter here.
            Announcer.Message($"{sanitizedMessage}", customSubtitles: processedSubtitles, priority: priority, playBackground: false);
        }
        #endregion

        #region Batch Message Dispatchers (Zero-Allocation Internal Implementations)
        /// <summary>
        /// Dispatches a collection of messages sequentially using the specified playback modifiers.
        /// </summary>
        public static void DispatchMessage(IEnumerable<string> messages, CassiePlaybackModifiers modifiers = default)
        {
            if (messages is null) return;

            if (messages is List<string> concreteList)
            {
                int count = concreteList.Count;
                for (int i = 0; i < count; i++)
                {
                    DispatchMessage(concreteList[i], modifiers);
                }
                return;
            }

            foreach (string message in messages)
            {
                DispatchMessage(message, modifiers);
            }
        }

        /// <summary>
        /// Dispatches an inline array of messages sequentially using default playback modifiers.
        /// </summary>
        public static void DispatchMessage(params string[] messages)
            => DispatchMessage(messages, default);

        /// <summary>
        /// Dispatches an inline array of messages sequentially using the specified playback modifiers.
        /// </summary>
        public static void DispatchMessage(CassiePlaybackModifiers modifiers, params string[] messages)
        {
            if (messages is null) return;

            int count = messages.Length;
            for (int i = 0; i < count; i++)
            {
                DispatchMessage(messages[i], modifiers);
            }
        }
        #endregion

        #region Utilities & Countdown Mechanics
        /// <summary>
        /// Converts a remaining time integer into a formatted CASSIE countdown announcement string.
        /// </summary>
        public static string ToCassieCountdown(this int notifyTime, string alertContext = "seconds until event detonation")
        {
            string sanitizedContext = alertContext.SanitizeCassieString();
            if (string.IsNullOrEmpty(sanitizedContext)) sanitizedContext = "seconds";

            return notifyTime switch
            {
                < 5 => $".G3 {notifyTime} .G5",
                >= 5 and <= 20 => $".G3 {notifyTime} seconds .G5",
                _ => $".G3 {notifyTime} {sanitizedContext} .G5"
            };
        }

        /// <summary>
        /// Calculates the playback duration of a single CASSIE message using the provided modifiers.
        /// </summary>
        public static double CalculateCassieMessageDuration(string message, CassiePlaybackModifiers modifiers = default)
        {
            string sanitizedMessage = message.SanitizeCassieString();
            if (string.IsNullOrEmpty(sanitizedMessage)) return 0.0;

            return Announcer.CalculateDuration(sanitizedMessage, modifiers);
        }
        #endregion

        #region Aggregation Overhead Loops (Zero-Allocation Internal Implementations)
        /// <summary>
        /// Calculates the total duration of a dictionary of messages, mapping each float value to a modifier pitch.
        /// </summary>
        public static double CalculateTotalMessagesDurations(IDictionary<string, float> messageSpeedDictionary)
        {
            if (messageSpeedDictionary is null || messageSpeedDictionary.Count == 0) return 0.0;

            double cumulativeDuration = 0.0;
            if (messageSpeedDictionary is Dictionary<string, float> concreteDict)
            {
                foreach (var kvp in concreteDict)
                {
                    CassiePlaybackModifiers modifiers = default;
                    modifiers.Pitch = kvp.Value;
                    cumulativeDuration += CalculateCassieMessageDuration(kvp.Key, modifiers);
                }
                return cumulativeDuration;
            }

            foreach (var kvp in messageSpeedDictionary)
            {
                CassiePlaybackModifiers modifiers = default;
                modifiers.Pitch = kvp.Value;
                cumulativeDuration += CalculateCassieMessageDuration(kvp.Key, modifiers);
            }
            return cumulativeDuration;
        }

        /// <summary>
        /// Calculates the cumulative playback duration for a collection of messages using the specified modifiers.
        /// </summary>
        public static double CalculateTotalMessagesDurations(IEnumerable<string> messages, CassiePlaybackModifiers modifiers = default)
        {
            if (messages is null) return 0.0;

            double cumulativeDuration = 0.0;
            if (messages is List<string> concreteList)
            {
                int count = concreteList.Count;
                for (int i = 0; i < count; i++)
                {
                    cumulativeDuration += CalculateCassieMessageDuration(concreteList[i], modifiers);
                }
                return cumulativeDuration;
            }

            foreach (string message in messages)
            {
                cumulativeDuration += CalculateCassieMessageDuration(message, modifiers);
            }
            return cumulativeDuration;
        }

        /// <summary>
        /// Calculates the cumulative playback duration for an inline array of messages using default modifiers.
        /// </summary>
        public static double CalculateTotalMessagesDurations(params string[] messages)
            => CalculateTotalMessagesDurations(messages, default);

        /// <summary>
        /// Calculates the cumulative playback duration for an inline array of messages using the specified modifiers.
        /// </summary>
        public static double CalculateTotalMessagesDurations(CassiePlaybackModifiers modifiers, params string[] messages)
        {
            if (messages is null || messages.Length == 0) return 0.0;

            double cumulativeDuration = 0.0;
            int count = messages.Length;
            for (int i = 0; i < count; i++)
            {
                cumulativeDuration += CalculateCassieMessageDuration(messages[i], modifiers);
            }
            return cumulativeDuration;
        }
        #endregion
    }
}