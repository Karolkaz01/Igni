using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Configuration
{
    public class PluginConfig
    {
        public string FileName { get; set; }
        public string DirectoryName { get; set; }
        public bool IsEnabled { get; set; }
    }
}
