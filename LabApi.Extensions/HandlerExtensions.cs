using LabApi.Events.CustomHandlers;
using System.Collections.Generic;

namespace LabApi.Extensions
{
    /// <summary>
    /// Utility extensions for registering and unregistering LabAPI event handlers.
    /// </summary>
    public static class HandlerExtensions
    {
        #region Register

        /// <summary>
        /// Registers multiple event handlers.
        /// </summary>
        public static void RegisterAll(this IEnumerable<CustomEventsHandler> handlers)
            => handlers.ForEach(h => CustomHandlersManager.RegisterEventsHandler(h));

        /// <summary>
        /// Registers multiple event handlers (params overload).
        /// </summary>
        public static void RegisterAll(params CustomEventsHandler[] handlers)
            => ((IEnumerable<CustomEventsHandler>)handlers).RegisterAll();

        #endregion

        #region Unregister

        /// <summary>
        /// Unregisters multiple event handlers.
        /// </summary>
        public static void UnregisterAll(this IEnumerable<CustomEventsHandler> handlers)
            => handlers.ForEach(h => CustomHandlersManager.UnregisterEventsHandler(h));

        /// <summary>
        /// Unregisters multiple event handlers (params overload).
        /// </summary>
        public static void UnregisterAll(params CustomEventsHandler[] handlers)
            => ((IEnumerable<CustomEventsHandler>)handlers).UnregisterAll();

        #endregion
    }
}
