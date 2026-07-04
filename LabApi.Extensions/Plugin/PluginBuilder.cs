using LabApi.Loader.Features.Configuration;
using LabApi.Loader.Features.Plugins;
using System;

namespace LabApi.Extensions.Plugin
{
    /// <summary>
    /// A high-performance fluent orchestration builder designed to streamline sub-configuration deployment and module initialization sequences.
    /// </summary>
    /// <typeparam name="TConfig">The primary framework configuration type conforming to <see cref="LabApiConfig"/>.</typeparam>
    public sealed class PluginBuilder<TConfig> where TConfig : LabApiConfig, new()
    {
        private readonly Plugin<TConfig> _plugin;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginBuilder{TConfig}"/> class.
        /// </summary>
        /// <param name="plugin">The live framework plugin context instance.</param>
        /// <exception cref="ArgumentNullException">Thrown if the plugin instance is null.</exception>
        public PluginBuilder(Plugin<TConfig> plugin)
        {
            _plugin = plugin ?? throw new ArgumentNullException(nameof(plugin));
        }

        /// <summary>
        /// Dynamically loads, validates, and binds a decentralized sub-configuration file to the plugin ecosystem.
        /// </summary>
        /// <typeparam name="TSubConfig">The target modular sub-configuration class type being loaded.</typeparam>
        /// <param name="fileName">The specific file name literal layout tracked on disk (e.g., "settings.yml").</param>
        /// <param name="bindAction">The assignment delegate mapping the loaded instance to a plugin property field.</param>
        /// <param name="validationAction">An optional validation delegate executed to enforce runtime boundary metrics.</param>
        /// <returns>The current <see cref="PluginBuilder{TConfig}"/> instance context to support method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the fileName or bindAction is null.</exception>
        public PluginBuilder<TConfig> BindSubConfig<TSubConfig>(
            string fileName,
            Action<TSubConfig> bindAction,
            Action<TSubConfig> validationAction = null)
            where TSubConfig : class, new()
        {
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentNullException(nameof(fileName));
            if (bindAction is null) throw new ArgumentNullException(nameof(bindAction));

            // Leverage the generic configuration loader extension engine under the hood
            TSubConfig config = _plugin.LoadOrCreateSubConfig(fileName, validationAction);
            bindAction.Invoke(config);

            return this; // Return the builder context to allow continuous method piping
        }

        /// <summary>
        /// Executes an initialization routine or boots up a subsystem component as part of the fluent pipeline.
        /// </summary>
        /// <param name="initAction">The delegate housing the initialization payload sequence.</param>
        /// <returns>The current <see cref="PluginBuilder{TConfig}"/> instance context to support method chaining.</returns>
        public PluginBuilder<TConfig> InitializeModule(Action initAction)
        {
            initAction?.Invoke();
            return this;
        }
    }
}