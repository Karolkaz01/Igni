using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Configuration
{
    public class ConfigurationModel
    {
        public IDictionary<string, CommandSection> CommandSections { get; set; }
        public IDictionary<string,string> Settings { get; set; }
        public IDictionary<string, PluginConfig> PluginsInfo { get; set; }
        public IDictionary<string,string> KeyWords { get; set; }
        public IDictionary<string,string> Voices { get; set; }
    }
}
