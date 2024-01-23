using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Configuration
{
    public class Configuration
    {
        public IDictionary<string, CommandSection> commandSections { get; set; }
        public IDictionary<string,string> settings { get; set; }
        public IDictionary<string, PluginConfig> pluginsInfo { get; set; }
    }
}
