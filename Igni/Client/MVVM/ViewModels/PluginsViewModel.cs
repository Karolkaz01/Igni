using Client.MVVM.Models;
using Core.Models.Configuration;
using Core.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Client.MVVM.ViewModels
{
    public class PluginsViewModel : ViewModelBase
    {
        private ConfigurationService _configurationService;
        private ObservableCollection<PluginInfoMVVM> pluginsList;

        public ObservableCollection<PluginInfoMVVM> PluginsList
        {
            get
            {
                return pluginsList;
            }
            set
            {
                pluginsList = value;
                OnPropertyChanged(nameof(PluginsList));
            }
        }

        public ICommand DeletePluginCommand { get; }
        public ICommand SaveCommand {  get; }

        public ValidationRule NotEmptyStringValidationRule { get; }

        public PluginsViewModel(ConfigurationService configurationService)
        {
            _configurationService = configurationService;
            IDictionary<string, PluginConfig> pluginSettings = _configurationService.GetPluginSetting();
            var pluginsList = new List<PluginInfoMVVM>();
            foreach(var pluginSetting in pluginSettings)
            {
                pluginsList.Add(new PluginInfoMVVM
                {
                    PluginName = pluginSetting.Key,
                    FileName = pluginSetting.Value.FileName,
                    DirectoryName = pluginSetting.Value.DirectoryName,
                    IsEnabled = pluginSetting.Value.IsEnabled
                });
            }
            PluginsList = new ObservableCollection<PluginInfoMVVM>(pluginsList);

            DeletePluginCommand = new ViewModelCommand(ExecuteDeletePluginCommand);
            SaveCommand = new ViewModelCommand(ExecuteSaveCommand);
            NotEmptyStringValidationRule = new PluginValidationRule();
        }

        private void ExecuteSaveCommand(object obj)
        {
            var pluginConfig = GetPluginConfig();
            _configurationService.SavePluginConfig(pluginConfig);
        }

        private IDictionary<string, PluginConfig> GetPluginConfig()
        {
            var pluginConfig = new Dictionary<string, PluginConfig>();
            foreach(var plugin in PluginsList)
            {
                pluginConfig.Add(plugin.PluginName, new PluginConfig
                {
                    FileName = plugin.FileName,
                    DirectoryName = plugin.DirectoryName,
                    IsEnabled = plugin.IsEnabled
                });
            }
            return pluginConfig;
        }

        private void ExecuteDeletePluginCommand(object obj)
        {
            if(obj is PluginInfoMVVM pluginInfoMVVM)
            {
                PluginsList.Remove(pluginInfoMVVM);
            }
        }
    }
}
