using Core.Consts;
using Core.Models.Configuration;
using Igni.SDK;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Core.Services.Plugins
{
    public class PluginsManager : AssemblyLoadContext
    {
        private readonly ConfigurationService _configurationService;
        private readonly IIgniContext _igniContext;


        public PluginsManager(ConfigurationService configurationService, IIgniContext igniContext)
        {
            _configurationService = configurationService;
            _igniContext = igniContext;
        }

        public IDictionary<PluginConfig, IIgniPlugin> LoadAllPlugins()
        {
            var pluginsLists = new Dictionary<PluginConfig, IIgniPlugin>();

            var direcories = Directory.GetDirectories(Paths.PLUGINS).Select(Path.GetFileName).ToArray();
            var pluginsConfigs = _configurationService.GetPluginSettins().Where(p => direcories.Contains(p.Value.DirectoryName))
                .ToDictionary();

            foreach (var pluginConfig in pluginsConfigs)
            {
                var pluginAssambly = LoadAssemblyPlugin(pluginConfig.Value);
                var pluginInstance = CreatePlugin(pluginAssambly);

                if (pluginInstance != null)
                {
                    pluginsLists.Add(pluginConfig.Value, pluginInstance);
                }
            }

            return pluginsLists;
        }

        private void LoadAndCreatePlugin(List<IIgniPlugin> pluginsLists, string file)
        {


            //var assembly = Assembly.LoadFile(Path.Combine(Directory.GetCurrentDirectory(), file));
            //var types = assembly.GetTypes().Where(t => typeof(IIgniPlugin).IsAssignableFrom(t) && !t.IsInterface).ToArray();

            //foreach (var type in types)
            //{
            //    if (type.GetInterface(typeof(IIgniPlugin).ToString()) != null)
            //    {
            //        var plugin = Activator.CreateInstance(type, assembly) as IIgniPlugin;

            //        if (plugin != null)
            //            pluginsLists.Add(plugin);
            //        else
            //            throw new Exception("Plugin is null");
            //    }
            //}
            //LoadCongiguration(pluginsLists);
        }

        private Assembly LoadAssemblyPlugin(PluginConfig pluginConfig)
        {
            var pluginPath = Path.Combine(Directory.GetCurrentDirectory(), Paths.PLUGINS, pluginConfig.DirectoryName, pluginConfig.FileName);

            PluginLoadContext loadContext = new PluginLoadContext(pluginPath);
            return loadContext.LoadFromAssemblyName(AssemblyName.GetAssemblyName(pluginPath));
        }

        private IIgniPlugin? CreatePlugin(Assembly pluginAssambly)
        {
            var types = pluginAssambly.GetTypes();
                //.Where(t => typeof(IIgniPlugin).IsAssignableFrom(t) && !t.IsInterface).ToArray();

            foreach (var type in types)
            {
                if (typeof(IIgniPlugin).IsAssignableFrom(type))
                {
                    var plugin = Activator.CreateInstance(type,_igniContext) as IIgniPlugin;

                    if (plugin != null)
                        return plugin;
                }
            }

            return null;
        }

        private void LoadCongiguration(List<IIgniPlugin> pluginsLists)
        {
            var pluginsConfig = _configurationService.GetPluginSettins();
            foreach (var plugin in pluginsLists)
            {
                if (!pluginsConfig.ContainsKey(plugin.ToString()))
                {
                    //_configurationService.AddPluginSetting(plugin.ToString(), true);
                }
            }
        }
    }
}
