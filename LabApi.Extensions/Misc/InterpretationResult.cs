using System;
using System.Collections.Generic;
using System.Linq;

namespace LabApi.Extensions.Misc
{
    /// <summary>
    /// Outlines the structural resolution status of a fuzzy text interpretation sequence.
    /// </summary>
    public enum InterpretationStatus
    {
        DefinitiveMatch,
        Ambiguous,
        NoMatchFound
    }

    /// <summary>
    /// Immutable payload container holding the execution metrics of a smart string interpretation sequence.
    /// </summary>
    /// <typeparam name="T">The target structural enum restriction type.</typeparam>
    public sealed class InterpretationResult<T> where T : struct, Enum
    {
        /// <summary>
        /// Gets the final resolution status of the execution pipeline.
        /// </summary>
        public InterpretationStatus Status { get; }

        /// <summary>
        /// Gets the verified strongly-typed enum value. Evaluates to default if status is not DefinitiveMatch.
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// Gets the list of close structural candidates discovered if the command invocation was ambiguous.
        /// </summary>
        public string[] Candidates { get; }

        public InterpretationResult(InterpretationStatus status, T value, string[] candidates)
        {
            Status = status;
            Value = value;
            Candidates = candidates ?? Array.Empty<string>();
        }
    }

    /// <summary>
    /// Advanced heuristic parsing engine delivering fuzzy string resolution, acronym mapping, and typo-tolerance layers.
    /// </summary>
    public static class StringInterpretationExtensions
    {
        /// <summary>
        /// Fluently interprets a raw string input and resolves it against an enum layout using multi-stage heuristic cascades.
        /// </summary>
        /// <typeparam name="T">The target value mapping constraint conforming to standard system enums.</typeparam>
        /// <param name="input">The raw text block harvested from the command arguments list.</param>
        /// <returns>A comprehensive result mapping containing match values or closest operational suggestion listings.</returns>
        public static InterpretationResult<T> InterpretEnum<T>(this string input) where T : struct, Enum
        {
            if (string.IsNullOrWhiteSpace(input))
                return new InterpretationResult<T>(InterpretationStatus.NoMatchFound, default, null);

            string cleanInput = input.Trim().ToLowerInvariant();
            string[] enumNames = Enum.GetNames(typeof(T));
            T[] enumValues = (T[])Enum.GetValues(typeof(T));

            // STAGE 1: Exact / Case-Insensitive String Verification
            for (int i = 0; i < enumNames.Length; i++)
            {
                if (enumNames[i].Equals(cleanInput, StringComparison.OrdinalIgnoreCase))
                {
                    return new InterpretationResult<T>(InterpretationStatus.DefinitiveMatch, enumValues[i], null);
                }
            }

            var candidatesIndexes = new List<int>();

            // STAGE 2: Substring Inversion & Structural Acronym Discovery Loop
            for (int i = 0; i < enumNames.Length; i++)
            {
                string nameLower = enumNames[i].ToLowerInvariant();

                // Direct or inverted containment detection (e.g., "Light" matches "LightContainment")
                if (nameLower.Contains(cleanInput) || cleanInput.Contains(nameLower))
                {
                    candidatesIndexes.Add(i);
                    continue;
                }

                // Dynamic uppercase acronym signature matching (e.g., "LightContainment" -> "lc" or "lcz" contextual expansion)
                string acronym = new string(enumNames[i].Where(char.IsUpper).ToArray()).ToLowerInvariant();
                if (acronym == cleanInput || (acronym + "z") == cleanInput || acronym == (cleanInput + "z"))
                {
                    candidatesIndexes.Add(i);
                    continue;
                }

                // Hardcoded environmental shortcuts to bulletproof default SCP:SL mapping layouts
                if (cleanInput == "lcz" && nameLower.Contains("light")) { candidatesIndexes.Add(i); continue; }
                if (cleanInput == "hcz" && nameLower.Contains("heavy")) { candidatesIndexes.Add(i); continue; }
                if (cleanInput == "ez" && nameLower.Contains("entrance")) { candidatesIndexes.Add(i); continue; }
                if (cleanInput == "sz" && nameLower.Contains("surface")) { candidatesIndexes.Add(i); continue; }
            }

            // STAGE 3: Typo-Tolerance Matrix via Levenshtein Distance algorithms (Fallback layer)
            if (candidatesIndexes.Count == 0)
            {
                var distanceScores = new List<(int index, int distance)>();
                for (int i = 0; i < enumNames.Length; i++)
                {
                    int distance = StringExtensions.ComputeLevenshteinDistance(cleanInput, enumNames[i].ToLowerInvariant());
                    // Allow soft variations (maximum of 3 typographic mutations allowed based on lengths)
                    if (distance <= 3 && distance < enumNames[i].Length - 2)
                    {
                        distanceScores.Add((i, distance));
                    }
                }

                if (distanceScores.Count > 0)
                {
                    int minDistance = distanceScores.Min(s => s.distance);
                    foreach (var score in distanceScores.Where(s => s.distance == minDistance))
                    {
                        candidatesIndexes.Add(score.index);
                    }
                }
            }

            // Clean down duplicate registry hits cleanly
            var distinctMatches = candidatesIndexes.Distinct().ToList();

            if (distinctMatches.Count == 1)
            {
                return new InterpretationResult<T>(InterpretationStatus.DefinitiveMatch, enumValues[distinctMatches[0]], null);
            }

            if (distinctMatches.Count > 1)
            {
                string[] suggestionStrings = distinctMatches.Select(idx => enumNames[idx]).ToArray();
                return new InterpretationResult<T>(InterpretationStatus.Ambiguous, default, suggestionStrings);
            }

            return new InterpretationResult<T>(InterpretationStatus.NoMatchFound, default, null);
        }
    }
}