using Core.Consts;
using Core.Helpers;
using Igni.SDK;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace Core.Services.Runners
{
    public class PluginRunner
    {
        private readonly ConfigurationService _configurationService;
        private readonly PluginsManager _pluginsManager;

        private IEnumerable<IIgniPlugin> Plugins { get; set; }
        private IDictionary<string, bool> PluginsCongig { get; set; }

        public PluginRunner(ConfigurationService configurationService, PluginsManager pluginsManager)
        {
            _configurationService = configurationService;
            _pluginsManager = pluginsManager;

            Plugins = _pluginsManager.LoadPlugins();
            PluginsCongig = _configurationService.GetPluginSettins();

            foreach (var plugin in Plugins)
            {
                if(PluginsCongig.FirstOrDefault(x => x.Key == plugin.ToString()).Value)
                    Task.Run(() => SafeInitializePluginAsync(plugin));
            }
        }

        public async Task PerformPluginsAsync(string speech)
        {
            foreach (var plugin in Plugins)
            {
                if (PluginsCongig.FirstOrDefault(x => x.Key == plugin.ToString()).Value)
                    plugin.PerformPlugin();
            }
        }

        private async Task SafeInitializePluginAsync(IIgniPlugin plugin)
        {
            Task initializePlugin = Task.Run(() => plugin.InitializePlugin());

            if (await Task.WhenAny(initializePlugin, Task.Delay(2000)) == initializePlugin)
            {
                Console.WriteLine($"Initialized Plugin");
            }
            else
            {
                //TODO: add cancelation Token (CancellationToken cancellationToken) to SDK methods
                Console.WriteLine($"Plugin {plugin} initialization terminated");
            }
        }

        private async Task SafePerformPluginAsync(IIgniPlugin plugin)
        {
            Task initializePlugin = Task.Run(() => plugin.InitializePlugin());

            if (await Task.WhenAny(initializePlugin, Task.Delay(2000)) == initializePlugin)
            {
                Console.WriteLine($"Initialized Plugin");
            }
            else
            {
                //TODO: add cancelation Token (CancellationToken cancellationToken) to SDK methods
                Console.WriteLine($"Plugin {plugin} initialization terminated");
            }
        }

        private void RunPluggins()
        {
            
        }
    }
}
