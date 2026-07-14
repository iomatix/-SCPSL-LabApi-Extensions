using CommandSystem;
using LabApi.Features.Wrappers;
using NorthwoodLib.Pools;
using RemoteAdmin;
using System;
using System.Globalization;

namespace LabApi.Extensions
{
    /// <summary>
    /// Advanced and highly optimized helper extensions for command permission verification, 
    /// target resolution, and safe multi-format argument parsing.
    /// </summary>
    public static class CommandExtensions
    {
        /// <summary>
        /// Returns true if the sender has the required permission.
        /// Server console and non-player senders always bypass this check.
        /// </summary>
        public static bool ConfirmPermission(this ICommandSender sender, PlayerPermissions permission, out string errorResponse)
        {
            if (sender is null)
            {
                errorResponse = "Command sender is null.";
                return false;
            }

            if (sender is PlayerCommandSender playerSender)
            {
                if (!playerSender.CheckPermission(permission))
                {
                    errorResponse = $"You lack the required permission: {permission}.";
                    return false;
                }
            }

            errorResponse = null;
            return true;
        }

        #region Primitive Parsers

        /// <summary>
        /// Tries to read a float from the argument list. Uses invariant culture to prevent system-specific parsing bugs.
        /// </summary>
        public static bool TryGetFloat(this ArraySegment<string> arguments, int index, out float value, float minValue = float.MinValue)
        {
            value = 0f;

            string raw = arguments.GetArgument(index);
            if (string.IsNullOrEmpty(raw))
                return false;

            if (float.TryParse(raw, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsed) && parsed >= minValue)
            {
                value = parsed;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Tries to read an int from the argument list.
        /// </summary>
        public static bool TryGetInt(this ArraySegment<string> arguments, int index, out int value, int minValue = int.MinValue)
        {
            value = 0;

            string raw = arguments.GetArgument(index);
            if (string.IsNullOrEmpty(raw))
                return false;

            if (int.TryParse(raw, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed) && parsed >= minValue)
            {
                value = parsed;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Tries to read a boolean from the argument list. 
        /// Supports: true/false, 1/0, yes/no, on/off.
        /// </summary>
        public static bool TryGetBool(this ArraySegment<string> arguments, int index, out bool value)
        {
            value = false;

            string raw = arguments.GetArgument(index);
            if (string.IsNullOrEmpty(raw))
                return false;

            // Fast-path for single character matches
            if (raw.Length == 1)
            {
                if (raw[0] == '1') { value = true; return true; }
                if (raw[0] == '0') { value = false; return true; }
            }

            // Standard and colloquial matches
            if (raw.Equals("true", StringComparison.OrdinalIgnoreCase) ||
                raw.Equals("yes", StringComparison.OrdinalIgnoreCase) ||
                raw.Equals("on", StringComparison.OrdinalIgnoreCase))
            {
                value = true;
                return true;
            }

            if (raw.Equals("false", StringComparison.OrdinalIgnoreCase) ||
                raw.Equals("no", StringComparison.OrdinalIgnoreCase) ||
                raw.Equals("off", StringComparison.OrdinalIgnoreCase))
            {
                value = false;
                return true;
            }

            return false;
        }

        #endregion

        #region SCP:SL Specific Parsers

        /// <summary>
        /// Resolves a single player from the arguments at the specified index by ID or Name.
        /// </summary>
        public static bool TryGetPlayer(this ArraySegment<string> arguments, int index, out Player player)
        {
            player = null;

            string raw = arguments.GetArgument(index);
            if (string.IsNullOrEmpty(raw))
                return false;

            // 1. Try resolving by Player ID (fastest)
            if (int.TryParse(raw, out int id))
            {
                player = Player.Get(id);
                if (player != null)
                    return true;
            }

            // 2. Try resolving by Name/Nickname (fallback)
            player = Player.Get(raw);
            return player != null;
        }

        /// <summary>
        /// Tries to parse a generic Enum value from the arguments.
        /// </summary>
        public static bool TryGetEnum<TEnum>(this ArraySegment<string> arguments, int index, out TEnum value) where TEnum : struct, Enum
        {
            value = default;

            string raw = arguments.GetArgument(index);
            if (string.IsNullOrEmpty(raw))
                return false;

            return Enum.TryParse(raw, true, out value);
        }

        #endregion

        #region Text Aggregation

        /// <summary>
        /// Combines all remaining arguments starting from the specified index into a single spaced string.
        /// Uses the pooled StringBuilder to completely avoid generic GC allocations during generation.
        /// </summary>
        public static string GetRemainingText(this ArraySegment<string> arguments, int startIndex)
        {
            if (startIndex < 0 || startIndex >= arguments.Count)
                return string.Empty;

            var sb = StringBuilderPool.Shared.Rent();
            try
            {
                int end = arguments.Count;
                int actualOffset = arguments.Offset + startIndex;

                for (int i = startIndex; i < end; i++)
                {
                    string word = arguments.Array[arguments.Offset + i];
                    if (string.IsNullOrEmpty(word))
                        continue;

                    if (sb.Length > 0)
                        sb.Append(' ');

                    sb.Append(word);
                }

                return StringBuilderPool.Shared.ToStringReturn(sb);
            }
            catch
            {
                StringBuilderPool.Shared.Return(sb);
                throw;
            }
        }

        #endregion

        #region Helper

        /// <summary>
        /// Safe helper to retrieve an argument from an ArraySegment, resolving bounds checks and segment offsets.
        /// </summary>
        private static string GetArgument(this ArraySegment<string> arguments, int index)
        {
            if (index < 0 || index >= arguments.Count)
                return null;

            return arguments.Array?[arguments.Offset + index];
        }

        #endregion
    }
}