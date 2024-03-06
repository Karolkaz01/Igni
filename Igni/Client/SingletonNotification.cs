using Client.MVVM.ViewModels;
using Core.Models.Notifications;
using Core.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Client
{
    public class SingletonNotification : INotificationHandler<RecognizedNotification>, INotificationHandler<SpeakNotification>
    {
        private RecognizedTextViewModel _communicationViewModel;
        private RecognizedTextWindow _communicationWindow;
        private ConfigurationService _configurationService;

        public SingletonNotification(RecognizedTextViewModel recognizedTextViewModel, RecognizedTextWindow recognizedTextWindow, ConfigurationService configurationService)
        {
            _communicationViewModel = recognizedTextViewModel;
            _communicationWindow = recognizedTextWindow;
            _configurationService = configurationService;
        }

        public async Task Handle(RecognizedNotification notification, CancellationToken cancellationToken)
        {
            var popUpWindowEnabledSetting = _configurationService.GetSetting("PopUpWindowEnabled");

            if (bool.TryParse(popUpWindowEnabledSetting, out bool isPopUpEnabled) && !isPopUpEnabled)
                return;

            Application.Current.Dispatcher.Invoke(() =>
            {
                _communicationViewModel.RecognizedText = notification.Text;
                _communicationViewModel.ResponseVisibility = Visibility.Collapsed;
                _communicationWindow.Height = 200;
                _communicationWindow.SetWindowPosition();    
                _communicationWindow.Show();
                _communicationWindow.Focus();
            });
            Hide();
        }

        public async Task Handle(SpeakNotification notification, CancellationToken cancellationToken)
        {
            var popUpWindowEnabledSetting = _configurationService.GetSetting("PopUpWindowEnabled");
            var responseTextEnabledSetting = _configurationService.GetSetting("ResponseTextEnabled");
            if (bool.TryParse(popUpWindowEnabledSetting, out bool isPopUpEnabled) && !isPopUpEnabled ||
                bool.TryParse(responseTextEnabledSetting, out bool isResponseTextEnabled) && !isResponseTextEnabled)
                return;

            Application.Current.Dispatcher.Invoke(() =>
            {
                _communicationViewModel.ResponseText = notification.Text;
                _communicationViewModel.ResponseVisibility = Visibility.Visible;
                _communicationWindow.Height = 500;
                _communicationWindow.Show();
                _communicationWindow.SetWindowPosition();
            });
            Hide();
            return;
        }

        private async void Hide()
        {
            var timeDisappearSetting = _configurationService.GetSetting("PopUpDisappearTimer");
            var delayInMs = int.TryParse(timeDisappearSetting,out int delay) ? delay*1000 : 3000;
            await Task.Delay(delayInMs);
            if (_communicationViewModel.ResponseVisibility != Visibility.Visible)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    _communicationWindow.Hide();
                });
            }
        }
    }
}
