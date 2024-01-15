using Core.Consts;
using Core.Services;
using Igni.SDK;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helpers
{
    public class PluginsManager
    {
        private readonly ConfigurationService _configurationService;


        public PluginsManager(ConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        public List<IIgniPlugin> LoadPlugins()
        {
            var pluginsLists = new List<IIgniPlugin>();
            var files = Directory.GetFiles(Paths.PLUGINS, "*.dll");

            foreach (var file in files)
            {
                LoadFile(pluginsLists, file);
            }

            return pluginsLists;
        }

        private void LoadFile(List<IIgniPlugin> pluginsLists, string file)
        {
            var assembly = Assembly.LoadFile(Path.Combine(Directory.GetCurrentDirectory(), file));
            var types = assembly.GetTypes().Where(t => typeof(IIgniPlugin).IsAssignableFrom(t) && !t.IsInterface).ToArray();

            foreach (var type in types)
            {
                if (type.GetInterface(typeof(IIgniPlugin).ToString()) != null)
                {
                    var plugin = Activator.CreateInstance(type) as IIgniPlugin;

                    if (plugin != null)
                        pluginsLists.Add(plugin);
                    else
                        throw new Exception("Plugin is null");
                }
            }
            LoadCongiguration(pluginsLists);
        }

        private void LoadCongiguration(List<IIgniPlugin> pluginsLists)
        {
            var pluginsConfig = _configurationService.GetPluginSettins();
            foreach(var plugin in pluginsLists)
            {
                if (!pluginsConfig.ContainsKey(plugin.ToString()))
                {
                    _configurationService.AddPluginSetting(plugin.ToString(), true);
                }
            }
        }
    }
}
