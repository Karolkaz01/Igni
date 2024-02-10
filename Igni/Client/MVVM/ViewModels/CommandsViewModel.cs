using Client.MVVM.Models;
using Core.Enums;
using Core.Models.Configuration;
using Core.Services;
using Json.Schema;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace Client.MVVM.ViewModels
{
    public class CommandsViewModel : ViewModelBase
    {
        private readonly ConfigurationService _configurationService;
        private ObservableCollection<CommandSectionMVVM> commandSections;

        public ObservableCollection<CommandSectionMVVM> CommandSections
        {
            get
            {
                return commandSections;
            }
            set
            {
                commandSections = value;
                OnPropertyChanged(nameof(CommandSections));
            }
        }

        public ICommand DeleteCommandCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand DeleteSectionCommand { get; }

        public static Array GetEnumTypes => Enum.GetValues(typeof(CommandType));

        public CommandsViewModel(ConfigurationService configurationService)
        {
            _configurationService = configurationService;
            GetCommandSections();

            DeleteCommandCommand = new ViewModelCommand(ExecuteDeleteCommandCommand);
            SaveCommand = new ViewModelCommand(ExecuteSaveCommand);
            DeleteSectionCommand = new ViewModelCommand(ExecuteDeleteSectionCommand);
        }

        private void GetCommandSections()
        {
            IDictionary<string, CommandSection> sections = _configurationService.GetAllCommandSections();
            var commandSections = new ObservableCollection<CommandSectionMVVM>();
            foreach (var section in sections)
            {
                commandSections.Add(new CommandSectionMVVM
                {
                    Name = section.Key,
                    Description = section.Value.Description,
                    DisplayName = section.Value.Name,
                    Values = new ObservableCollection<Command>(section.Value.Values)
                });
            }
            CommandSections = commandSections;
        }

        private void ExecuteDeleteSectionCommand(object obj)
        {
            if (obj is CommandSectionMVVM sectionToDelete && CommandSections.Contains(sectionToDelete))
            {
                CommandSections.Remove(sectionToDelete);
            }
        }

        private void ExecuteSaveCommand(object obj)
        {
            var sections = GetCommandsSections();
            _configurationService.SaveCommandConfig(sections);
        }

        private void ExecuteDeleteCommandCommand(object obj)
        {
            if (obj is Command)
            {
                var command = (Command)obj;
                foreach (var section in CommandSections)
                {
                    if (section.Values.Contains(command))
                        section.Values.Remove(command);
                }
            }
        }

        private Dictionary<string,CommandSection> GetCommandsSections()
        {
            var commandSections = new Dictionary<string, CommandSection>();

            foreach (var section in CommandSections)
            {
                commandSections.Add(section.Name,new CommandSection
                {
                    Description = section.Description,
                    Name = section.DisplayName,
                    Values = new List<Command>(section.Values)
                });
            }

            return commandSections;
        }
    }
    
}
