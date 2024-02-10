using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.MVVM.Models
{
    public class PluginInfoMVVM
    {
        public string PluginName { get; set; }
        public string FileName { get; set; }
        public string DirectoryName { get; set; }
        public bool IsEnabled { get; set; }
    }
}
