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
        /// Systematically scrubs raw text fields, stripping hidden carriage returns and formatting errors 
        /// while safely preserving empty strings for intentional text muting configurations.
        /// </summary>
        /// <param name="rawMessage">The raw, un-sanitized source configuration string text layout.</param>
        /// <returns>A fully sanitized, single-line phonetic string ready for speech synthesis engines.</returns>
        public static string SanitizeCassieString(this string rawMessage)
        {
            if (string.IsNullOrWhiteSpace(rawMessage)) return string.Empty;
            return rawMessage.Replace("\r", "").Replace("\n", " ").Trim();
        }

        /// <summary>
        /// Dispatches an intentionally modulated, corrupted vocal sequence across the global audio matrix, 
        /// computing its final playback duration tracking envelope.
        /// </summary>
        /// <param name="message">The raw source text layout requested for transmission processing.</param>
        /// <param name="glitchChance">The evaluated anomaly coefficient percentage for structural corruption tracing.</param>
        /// <param name="jamChance">The structural distortion probability ceiling assigned for audio degradation scaling.</param>
        /// <param name="glitchifierOutput">The pre-processed, corrupted phonetic string text payload ready for announcer consumption.</param>
        /// <returns>The precise runtime track width duration metric measured in fractional seconds; otherwise, 0.0 on execution failure.</returns>
        public static double Cassie_GlitchyMessage(string message, float glitchChance, float jamChance, string glitchifierOutput)
        {
            string sanitizedOutput = glitchifierOutput.SanitizeCassieString();
            if (string.IsNullOrEmpty(sanitizedOutput)) return 0.0;

            try
            {
                CassiePlaybackModifiers playbackModifiers = default;
                playbackModifiers.Pitch = 0.95f;
                Announcer.Message($"pitch_0.95 {sanitizedOutput}", string.Empty, playBackground: false);
                return Announcer.CalculateDuration(sanitizedOutput, playbackModifiers);
            }
            catch (Exception ex)
            {
                iLogger.Error("Cassie.GlitchyMessage", $"Vocal grid runtime suspension anomaly: {ex.Message}");
                return 0.0;
            }
        }

        /// <summary>
        /// Dispatches a clean, high-clarity vocal notification sequence using optimized high-pass pitch metrics across global facility channels.
        /// </summary>
        /// <param name="message">The targeting structural notification string payload requested for public broadcast.</param>
        /// <returns>The computed execution track timeline width evaluated in seconds; otherwise, 0.0 if an anomaly is intercepted.</returns>
        public static double Cassie_Message(string message)
        {
            string sanitizedMessage = message.SanitizeCassieString();
            if (string.IsNullOrEmpty(sanitizedMessage)) return 0.0;

            try
            {
                CassiePlaybackModifiers playbackModifiers = default;
                playbackModifiers.Pitch = 1.05f;
                Announcer.Message($"pitch_1.05 {sanitizedMessage}", string.Empty, playBackground: false);
                return Announcer.CalculateDuration(sanitizedMessage, playbackModifiers);
            }
            catch (Exception ex)
            {
                iLogger.Error("Cassie.Message", $"Vocal pipeline critical failure: {ex.Message}");
                return 0.0;
            }
        }

        /// <summary>
        /// Executes defensive sanitization, structural sequence formatting, and real-time streaming transmission of CASSIE phrases 
        /// under concrete queue priority matrices, optionally overriding active subtitle layers.
        /// </summary>
        /// <param name="message">The base notification message payload targeting the vocal execution array.</param>
        /// <param name="customSubtitles">The alternative textual closed-captioning layout mapped to active user displays.</param>
        /// <param name="shouldClear">Indicates whether the active global announcer sequence queue must be aggressively flushed prior to initialization.</param>
        /// <param name="pitchModifier">The concrete pitch adjustment syntax string (e.g., 'pitch_1.05').</param>
        /// <param name="priority">The relative scheduling weight index tracking the message's positional allocation inside the global pipeline.</param>
        /// <param name="disableMessages">Defensive toggle bypass to suppress contextual subtitle generation arrays if evaluation bounds are met.</param>
        public static void ProcessAndDispatchMessage(string message, string customSubtitles, bool shouldClear, string pitchModifier, float priority, bool disableMessages = false)
        {
            string sanitizedMessage = message.SanitizeCassieString();
            if (string.IsNullOrEmpty(sanitizedMessage)) return;

            if (shouldClear) Announcer.Clear();

            string sanitizedSubtitles = customSubtitles.SanitizeCassieString();
            string processedSubtitles = (!string.IsNullOrEmpty(sanitizedSubtitles) && !disableMessages) ? sanitizedSubtitles : string.Empty;
            string cleanPitch = string.IsNullOrWhiteSpace(pitchModifier) ? "pitch_1.0" : pitchModifier.Trim();

            Announcer.Message($"{cleanPitch} {sanitizedMessage}", customSubtitles: processedSubtitles, priority: priority, playBackground: false);
        }

        /// <summary>
        /// Transforms a primitive integer value token into a structured CASSIE vocal countdown phonetic sequence format 
        /// matching the specific operational context tracking threshold boundaries.
        /// </summary>
        /// <param name="notifyTime">The remaining historical timeframe integer token measured in structural seconds.</param>
        /// <param name="alertContext">The terminal contextual description phrase appended to values tracking above safety limits.</param>
        /// <returns>A fully synchronized, ready-to-broadcast phonetic CASSIE phrase format configuration string.</returns>
        public static string ToCassieCountdown(this int notifyTime, string alertContext = "Seconds until Event Detonation")
        {
            string sanitizedContext = alertContext.SanitizeCassieString();
            if (string.IsNullOrEmpty(sanitizedContext)) sanitizedContext = "Seconds";

            if (notifyTime < 5) return $".G3 {notifyTime} .G5";
            if (notifyTime >= 5 && notifyTime <= 20) return $".G3 {notifyTime} Seconds .G5";
            return $".G3 {notifyTime} {sanitizedContext} .G5";
        }

        /// <summary>
        /// Simulates the exact runtime execution footprint duration matching a specific message phrase sequence 
        /// under a localized speed modification configuration envelope.
        /// </summary>
        /// <param name="message">The underlying string sequence targeted for structural timeline parsing measurements.</param>
        /// <param name="speed">The playback velocity translation index factor mapped directly to vocal frequency pitches.</param>
        /// <returns>A precise double floating-point scalar evaluation reflecting the calculated operational track length in seconds.</returns>
        public static double CalculateCassieMessageDuration(string message, double speed = 0.95)
        {
            string sanitizedMessage = message.SanitizeCassieString();
            if (string.IsNullOrEmpty(sanitizedMessage)) return 0.0;

            CassiePlaybackModifiers modifiers = default;
            modifiers.Pitch = (float)speed;
            return Announcer.CalculateDuration(sanitizedMessage, modifiers);
        }

        #region Aggregation Overhead Loops (Zero-Allocation Internal Implementations)
        /// <summary>
        /// Aggregates and computes total durations avoiding allocation-heavy LINQ structure parsing.
        /// </summary>
        public static double CalculateTotalMessagesDurations(IDictionary<string, float> messageSpeedDictionary)
        {
            if (messageSpeedDictionary is null || messageSpeedDictionary.Count == 0) return 0.0;

            double cumulativeDuration = 0.0;
            if (messageSpeedDictionary is Dictionary<string, float> concreteDict)
            {
                foreach (var kvp in concreteDict)
                {
                    cumulativeDuration += CalculateCassieMessageDuration(kvp.Key, kvp.Value);
                }
                return cumulativeDuration;
            }

            foreach (var kvp in messageSpeedDictionary)
            {
                cumulativeDuration += CalculateCassieMessageDuration(kvp.Key, kvp.Value);
            }
            return cumulativeDuration;
        }

        /// <summary>
        /// Aggregates linear collection playback footprints over a zero-allocation sequential iteration map.
        /// </summary>
        public static double CalculateTotalMessagesDurations(IEnumerable<string> messages, float defaultSpeed = 1f)
        {
            if (messages is null) return 0.0;

            double cumulativeDuration = 0.0;
            if (messages is List<string> concreteList)
            {
                int count = concreteList.Count;
                for (int i = 0; i < count; i++)
                {
                    cumulativeDuration += CalculateCassieMessageDuration(concreteList[i], defaultSpeed);
                }
                return cumulativeDuration;
            }

            foreach (string message in messages)
            {
                cumulativeDuration += CalculateCassieMessageDuration(message, defaultSpeed);
            }
            return cumulativeDuration;
        }

        /// <summary>
        /// High-performance overload routing parameters sequentially directly into structural loop wrappers.
        /// </summary>
        public static double CalculateTotalMessagesDurations(float defaultSpeed, params string[] messages)
        {
            if (messages is null || messages.Length == 0) return 0.0;

            double cumulativeDuration = 0.0;
            int count = messages.Length;
            for (int i = 0; i < count; i++)
            {
                cumulativeDuration += CalculateCassieMessageDuration(messages[i], defaultSpeed);
            }
            return cumulativeDuration;
        }
        #endregion
    }
}