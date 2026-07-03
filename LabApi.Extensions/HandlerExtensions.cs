using LabApi.Events.CustomHandlers;
using LabApi.Events.CustomHandlers.Management;

namespace LabApi.Extensions
{
    /// <summary>
    /// Provides high-performance utility abstraction overloads for batch manipulating LabAPI event subscription pipelines.
    /// </summary>
    internal static class HandlerExtensions
    {
        /// <summary>
        /// Systematically registers an aggregated inline array sequence of event handlers onto the central framework routing engine.
        /// </summary>
        /// <param name="handlers">The array layout tracking the target custom events handlers deployed into server memory spaces.</param>
        public static void RegisterAll(params CustomEventsHandler[] handlers)
        {
            if (handlers is null) return;

            int count = handlers.Length;
            for (int i = 0; i < count; i++)
            {
                CustomEventsHandler handler = handlers[i];
                if (handler is not null)
                {
                    CustomHandlersManager.RegisterEventsHandler(handler);
                }
            }
        }

        /// <summary>
        /// Systematically evicts and unregisters an aggregated inline array sequence of event handlers from the central framework routing engine.
        /// </summary>
        /// <param name="handlers">The array layout tracking the target custom events handlers cleared out from memory spaces.</param>
        public static void UnregisterAll(params CustomEventsHandler[] handlers)
        {
            if (handlers is null) return;

            int count = handlers.Length;
            for (int i = 0; i < count; i++)
            {
                CustomEventsHandler handler = handlers[i];
                if (handler is not null)
                {
                    CustomHandlersManager.UnregisterEventsHandler(handler);
                }
            }
        }
    }
}