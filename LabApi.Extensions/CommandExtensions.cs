using CommandSystem;
using RemoteAdmin;
using System;

namespace LabApi.Extensions
{
    /// <summary>
    /// Simple helpers for command permission checks and argument parsing.
    /// </summary>
    public static class CommandExtensions
    {
        /// <summary>
        /// Returns true if the sender has the required permission.
        /// Server console always passes.
        /// </summary>
        public static bool ConfirmPermission(this ICommandSender sender, PlayerPermissions permission, out string errorResponse)
        {
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
        /// Tries to read a float from the argument list.
        /// </summary>
        public static bool TryGetFloat(this ArraySegment<string> arguments, int index, out float value, float minValue = float.MinValue)
        {
            value = 0f;
            if (arguments.Count <= index)
                return false;

            var raw = arguments.Array?[arguments.Offset + index];
            if (string.IsNullOrEmpty(raw))
                return false;

            if (float.TryParse(raw, out var parsed) && parsed >= minValue)
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
            if (arguments.Count <= index)
                return false;

            var raw = arguments.Array?[arguments.Offset + index];
            if (string.IsNullOrEmpty(raw))
                return false;

            if (int.TryParse(raw, out var parsed) && parsed >= minValue)
            {
                value = parsed;
                return true;
            }

            return false;
        }
    }
}
