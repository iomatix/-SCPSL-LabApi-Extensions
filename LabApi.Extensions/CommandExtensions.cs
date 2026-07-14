using CommandSystem;
using RemoteAdmin;
using System;
using System.Globalization;

namespace LabApi.Extensions
{
    /// <summary>
    /// Highly optimized helper extensions for command permission verification and safe argument parsing.
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

        /// <summary>
        /// Tries to read a float from the argument list. Uses invariant culture to prevent system-specific parsing bugs.
        /// </summary>
        public static bool TryGetFloat(this ArraySegment<string> arguments, int index, out float value, float minValue = float.MinValue)
        {
            value = 0f;

            string raw = arguments.GetArgument(index);
            if (string.IsNullOrEmpty(raw))
                return false;

            // FIX: Explicitly enforce InvariantCulture so decimals parse correctly (using dot '.') regardless of the host OS language.
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

            // FIX: Enforce InvariantCulture for consistency across all parsing extensions.
            if (int.TryParse(raw, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed) && parsed >= minValue)
            {
                value = parsed;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Safe helper to retrieve an argument from an ArraySegment, resolving bounds checks and segment offsets.
        /// </summary>
        private static string GetArgument(this ArraySegment<string> arguments, int index)
        {
            // FIX: Added explicit negative index validation to prevent critical index out of bounds vulnerabilities.
            if (index < 0 || index >= arguments.Count)
                return null;

            return arguments.Array?[arguments.Offset + index];
        }
    }
}