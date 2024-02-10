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
    public class MainWindowViewModel : ViewModelBase, INotificationHandler<RecognizedNotification>
    {
        private ViewModelBase _currentChildView;
        private string _title;
        private IconChar _icon;

        private ConfigurationService _configurationService;
        private RecognizeService _recognizeService;
        private CommunicationViewModel _communicationViewModel;
        private CommunicationWindow _communicationWindow;

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

        public MainWindowViewModel(ConfigurationService configurationService, RecognizeService recognizeService, CommunicationViewModel communicationViewModel, CommunicationWindow communicationWindow)
        {
            _configurationService = configurationService;
            _recognizeService = recognizeService;
            _communicationWindow = communicationWindow;
            _communicationViewModel = communicationViewModel;

            //Initialize commands
            ShowCommandsViewCommand = new ViewModelCommand(ExecuteShowCommandsViewCommand);
            ShowPluginsViewCommand = new ViewModelCommand(ExecuteShowPluginsViewCommand);
            ShowSettingsViewCommand = new ViewModelCommand(ExecuteShowSettingsViewCommand);
            ToggleActivationIgniCommand = new ViewModelCommand(ExecuteToggleActivationIgniCommand);

            ExecuteShowCommandsViewCommand(null);
        }

        private void ExecuteToggleActivationIgniCommand(object obj)
        {
            if(obj is bool status)
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

        public async Task Handle(RecognizedNotification notification, CancellationToken cancellationToken)
        {
            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                var window = new CommunicationWindow();
                window.RecognizedT.Content = notification.Text;
                window.Show();
                Thread.Sleep(5000);
                window.Hide();
                //_communicationWindow.DataContext = _communicationViewModel;
                //_communicationWindow.Show();
                //_communicationViewModel.RecognizedText = notification.Text;
                //Thread.Sleep(5000);
                //_communicationWindow.Hide();
            });
        }

    }
}
