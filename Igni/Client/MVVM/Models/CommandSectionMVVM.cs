using Client.MVVM.ViewModels;
using Core.Models.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.MVVM.Models
{
    public class CommandSectionMVVM
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public ObservableCollection<Command> Values { get; set; }
    }
}
