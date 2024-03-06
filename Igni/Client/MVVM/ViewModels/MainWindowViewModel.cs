using Core.Models.Notifications;
using Core.Services;
using FontAwesome.Sharp;
using MediatR;
using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Management.Automation.Remoting;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Client.MVVM.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase _currentChildView;
        private string _title;
        private IconChar _icon;

        private ConfigurationService _configurationService;
        private RecognizeService _recognizeService;
        private RecognizedTextViewModel _communicationViewModel;
        private RecognizedTextWindow _communicationWindow;

        public ViewModelBase CurrentChildView
        {
            get => _currentChildView;
            set
            {
                _currentChildView = value;
                OnPropertyChanged(nameof(CurrentChildView));
            }
        }
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }
        public IconChar Icon
        {
            get => _icon;
            set
            {
                _icon = value;
                OnPropertyChanged(nameof(Icon));
            }
        }

        public ICommand ShowCommandsViewCommand { get; }
        public ICommand ShowPluginsViewCommand { get; }
        public ICommand ShowSettingsViewCommand { get; }
        public ICommand ToggleActivationIgniCommand { get; }

        public MainWindowViewModel()
        {
            OnPropertyChanged(nameof(Icon));
        }

        public MainWindowViewModel(ConfigurationService configurationService, RecognizeService recognizeService, RecognizedTextViewModel communicationViewModel, RecognizedTextWindow communicationWindow)
        {
            _configurationService = configurationService;
            _recognizeService = recognizeService;
            _communicationWindow = communicationWindow;
            _communicationViewModel = communicationViewModel;
            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                _communicationWindow = IocConfiguration.Get<RecognizedTextWindow>();
                _communicationViewModel = IocConfiguration.Get<RecognizedTextViewModel>();
                _communicationWindow.DataContext = _communicationViewModel;
            });

            //Initialize commands
            ShowCommandsViewCommand = new ViewModelCommand(ExecuteShowCommandsViewCommand);
            ShowPluginsViewCommand = new ViewModelCommand(ExecuteShowPluginsViewCommand);
            ShowSettingsViewCommand = new ViewModelCommand(ExecuteShowSettingsViewCommand);
            ToggleActivationIgniCommand = new ViewModelCommand(ExecuteToggleActivationIgniCommand);

            ExecuteShowCommandsViewCommand(null);
        }

        private void ExecuteToggleActivationIgniCommand(object obj)
        {
            if (obj is bool status)
            {
                if (status)
                {
                    _recognizeService.StartListening();
                }
                else
                {
                    _recognizeService.StopListening();
                }
            }
        }

        private void ExecuteShowCommandsViewCommand(object? obj)
        {
            CurrentChildView = IocConfiguration.Get<CommandsViewModel>();
            Title = "Commands board";
            Icon = IconChar.Terminal;
        }

        private void ExecuteShowPluginsViewCommand(object? obj)
        {
            CurrentChildView = IocConfiguration.Get<PluginsViewModel>();
            Title = "Plugins board";
            Icon = IconChar.PuzzlePiece;
        }

        private void ExecuteShowSettingsViewCommand(object? obj)
        {
            CurrentChildView = IocConfiguration.Get<SettingsViewModel>();
            Title = "Settings board";
            Icon = IconChar.Gear;
        }

    }
}
