using System;
using System.Collections.Generic;
using System.Linq;
using Cassie;

namespace LabApi.Extensions
{
    /// <summary>
    /// Provides an enterprise-grade decoupled management matrix controlling speech synthesizer payloads, 
    /// real-time custom subtitle distribution pipelines, and multi-track duration timeline aggregation.
    /// </summary>
    internal static class CassieExtensions
    {
        /// <summary>
        /// Flushes the current real-time vocal audio broadcast queue entirely, instantly suspending any ongoing audio playback.
        /// </summary>
        public static void CassieClear() => Announcer.Clear();

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
            if (string.IsNullOrWhiteSpace(message)) return 0.0;

            try
            {
                CassiePlaybackModifiers playbackModifiers = default;
                playbackModifiers.Pitch = 0.95f;

                string finalPayload = $"pitch_0.95 {glitchifierOutput}";
                Announcer.Message(finalPayload, string.Empty, playBackground: false);

                return Announcer.CalculateDuration(glitchifierOutput, playbackModifiers);
            }
            catch (Exception ex)
            {
                iLogger.Error("Cassie.GlitchyMessage", $"Vocal grid runtime suspension tracking anomaly caught: {ex.Message}");
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
            if (string.IsNullOrWhiteSpace(message)) return 0.0;

            try
            {
                CassiePlaybackModifiers playbackModifiers = default;
                playbackModifiers.Pitch = 1.05f;

                string finalPayload = $"pitch_1.05 {message}";
                Announcer.Message(finalPayload, string.Empty, playBackground: false);

                return Announcer.CalculateDuration(message, playbackModifiers);
            }
            catch (Exception ex)
            {
                iLogger.Error("Cassie.Message", $"Vocal pipeline delivery grid critical failure handled: {ex.Message}");
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
            if (string.IsNullOrEmpty(message)) return;
            if (shouldClear) Announcer.Clear();

            string processedSubtitles = (!string.IsNullOrEmpty(customSubtitles) && !disableMessages) ? customSubtitles : string.Empty;
            string fullMessage = $"{pitchModifier} {message}";

            Announcer.Message(fullMessage, customSubtitles: processedSubtitles, priority: priority, playBackground: false);
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
            if (notifyTime < 5) return $".G3 {notifyTime} .G5";
            if (notifyTime >= 5 && notifyTime <= 20) return $".G3 {notifyTime} Seconds .G5";
            return $".G3 {notifyTime} {alertContext} .G5";
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
            CassiePlaybackModifiers modifiers = default;
            modifiers.Pitch = (float)speed;
            return Announcer.CalculateDuration(message, modifiers);
        }

        /// <summary>
        /// Iteratively aggregates and computes the total unified summation timeline bounds for a collection layout dictionary 
        /// mapping asynchronous text phrases to explicit local frequency speed bounds.
        /// </summary>
        /// <param name="messageSpeedDictionary">A concrete dictionary data matrix linking specific vocal scripts to execution tracking speeds.</param>
        /// <returns>The cumulative total tracking duration required to execute the entire matrix collection sequentially.</returns>
        public static double CalculateTotalMessagesDurations(IDictionary<string, float> messageSpeedDictionary)
        {
            return messageSpeedDictionary.Sum(kvp => CalculateCassieMessageDuration(kvp.Key, kvp.Value));
        }

        /// <summary>
        /// Aggregates linear string data collections and determines their absolute runtime playback footprints 
        /// utilizing a uniform fixed structural fallback speed index.
        /// </summary>
        /// <param name="messages">An enumerable collection tracking clean sequence phrases targeting the translation engine.</param>
        /// <param name="defaultSpeed">The standardized static speed parameter applied across all collection elements.</param>
        /// <returns>The global unified length duration evaluated across the aggregated sequence layout array bounds.</returns>
        public static double CalculateTotalMessagesDurations(IEnumerable<string> messages, float defaultSpeed = 1f)
        {
            return messages.Sum(message => CalculateCassieMessageDuration(message, defaultSpeed));
        }

        /// <summary>
        /// High-performance params collection inline overloading layer designed to process inline string listings 
        /// and capture cumulative timeline tracks without manual collection allocations.
        /// </summary>
        /// <param name="defaultSpeed">The unform speed coefficient factor applied to each argument node string text block.</param>
        /// <param name="messages">An expandable parameter array sequence capturing raw string blocks sequentially passed via method boundaries.</param>
        /// <returns>The aggregated total timeline calculation output track tracking all parameters.</returns>
        public static double CalculateTotalMessagesDurations(float defaultSpeed = 1f, params string[] messages)
        {
            return CalculateTotalMessagesDurations((IEnumerable<string>)messages, defaultSpeed);
        }
    }
}