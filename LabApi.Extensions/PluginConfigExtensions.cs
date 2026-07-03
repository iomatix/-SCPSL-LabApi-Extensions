using System;

namespace LabApi.Extensions
{
    /// <summary>
    /// Provides advanced high-performance utility extensions for LabAPI plugin lifecycle and modular sub-configuration deployment.
    /// </summary>
    internal static class PluginConfigExtensions
    {
        /// <summary>
        /// Atomically loads a decentralized sub-configuration file from the filesystem. 
        /// Automatically deploys a clean fallback instance, executes validation delegates, and flushes changes to disk if missing or corrupt.
        /// </summary>
        /// <typeparam name="TMainConfig">The core configuration type binding the primary plugin layout framework.</typeparam>
        /// <typeparam name="TSubConfig">The target modular sub-configuration class layer being initialized.</typeparam>
        /// <param name="plugin">The live executing framework plugin instance context.</param>
        /// <param name="fileName">The target file name literal tracking the resource on disk (e.g., "settings.yml").</param>
        /// <param name="validationAction">An optional custom validation delegate executed seamlessly to enforce runtime metric limits.</param>
        /// <returns>The fully initialized, validated strongly-typed <typeparamref name="TSubConfig"/> instance asset.</returns>
        public static TSubConfig LoadOrCreateSubConfig<TMainConfig, TSubConfig>(
            this LabApi.Loader.Features.Plugins.Plugin<TMainConfig> plugin,
            string fileName,
            Action<TSubConfig> validationAction = null)
            where TMainConfig : LabApi.Loader.Features.Configs.IConfig
            where TSubConfig : class, new()
        {
            if (plugin is null || string.IsNullOrEmpty(fileName))
            {
                return new TSubConfig();
            }

            TSubConfig finalConfig;

            if (plugin.TryLoadConfig(fileName, out TSubConfig loadedConfig))
            {
                finalConfig = loadedConfig ?? new TSubConfig();
            }
            else
            {
                finalConfig = new TSubConfig();
            }

            // Fire custom validation constraints if attached by the developer
            validationAction?.Invoke(finalConfig);

            // Instantly sync filesystem state mirror to preserve formatting integrity
            plugin.TrySaveConfig(finalConfig, fileName);

            return finalConfig;
        }
    }
}