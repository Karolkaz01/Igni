using Core.Consts;
using Core.Models.Configuration;
using Core.Services.Plugins;
using Igni.SDK;
using Serilog;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace Core.Services.Runners
{
    public class PluginRunner
    {
        private readonly ConfigurationService _configurationService;
        private readonly PluginsManager _pluginsManager;

        private IDictionary<PluginConfig, IIgniPlugin> Plugins { get; set; }
        private IDictionary<string, PluginConfig> PluginsCongig { get; set; }

        public PluginRunner(ConfigurationService configurationService, PluginsManager pluginsManager)
        {
            _configurationService = configurationService;
            _pluginsManager = pluginsManager;

            Plugins = _pluginsManager.LoadAllPlugins();
            ////PluginsCongig = _configurationService.GetPluginSettins();

            foreach (var plugin in Plugins)
            {
                if (plugin.Key.IsEnabled)
                    Task.Run(() => SafeInitializePluginAsync(plugin.Value));
            }
        }

        public async Task PerformPluginsAsync(string speech)
        {
            foreach (var plugin in Plugins)
            {
                if (plugin.Key.IsEnabled)
                    await SafeExcecutePluginAsync(plugin.Value, speech);
            }
        }

        private async Task SafeInitializePluginAsync(IIgniPlugin plugin)
        {
            try
            {
                CancellationTokenSource source = new CancellationTokenSource();
                Task initializePlugin = Task.Run(() => plugin.Initialize(source.Token));

                if (await Task.WhenAny(initializePlugin, Task.Delay(2000)) == initializePlugin)
                {
                    Console.WriteLine($"Initialized Plugin");
                }
                else
                {
                    source.Cancel();
                    Console.WriteLine($"Plugin {plugin} initialization cancelled");
                }
            }
            catch (Exception ex)
            {
                Log.Warning($"Plugin {plugin.ToString()} crashed while initializing");
            }
        }

        private async Task SafeExcecutePluginAsync(IIgniPlugin plugin, string speech)
        {
            try
            {
                CancellationTokenSource source = new CancellationTokenSource();
                Task executePlugin = Task.Run(() => plugin.Execute(source.Token, speech));

                if (await Task.WhenAny(executePlugin, Task.Delay(2000)) == executePlugin)
                {
                    Console.WriteLine($"Initialized Plugin");
                }
                else
                {
                    source.Cancel();
                    Console.WriteLine($"Plugin {plugin} execution cancelled");
                }
            }
            catch (Exception ex)
            {
                Log.Warning($"Plugin {plugin.ToString()} crashed while executing");
            }
        }
    }
}
