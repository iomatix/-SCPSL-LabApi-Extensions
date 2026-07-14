using LabApi.Loader;
using LabApi.Loader.Features.Configuration;
using LabApi.Loader.Features.Plugins;
using System;

namespace LabApi.Extensions
{
    /// <summary>
    /// Helpers for loading and validating plugin sub‑config files.
    /// </summary>
    public static class PluginConfigExtensions
    {
        /// <summary>
        /// Loads a sub‑config file or creates a new one if missing or invalid.
        /// Runs optional validation and saves the result back to disk.
        /// </summary>
        /// <typeparam name="TMainConfig">Main plugin config type.</typeparam>
        /// <typeparam name="TSubConfig">Sub‑config type to load.</typeparam>
        /// <param name="plugin">Plugin instance.</param>
        /// <param name="fileName">File name on disk (e.g. "settings.yml").</param>
        /// <param name="validationAction">Optional validation callback.</param>
        /// <returns>Loaded or newly created sub‑config instance.</returns>
        public static TSubConfig LoadOrCreateSubConfig<TMainConfig, TSubConfig>(
            this Plugin<TMainConfig> plugin,
            string fileName,
            Action<TSubConfig> validationAction = null)
            where TMainConfig : LabApiConfig, new()
            where TSubConfig : class, new()
        {
            if (plugin is null || string.IsNullOrEmpty(fileName))
                return new TSubConfig();

            TSubConfig finalConfig;

            if (plugin.TryLoadConfig(fileName, out TSubConfig loaded))
                finalConfig = loaded ?? new TSubConfig();
            else
                finalConfig = new TSubConfig();

            validationAction?.Invoke(finalConfig);

            plugin.TrySaveConfig(finalConfig, fileName);

            return finalConfig;
        }
    }
}
