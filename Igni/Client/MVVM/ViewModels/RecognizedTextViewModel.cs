using Core.Models.Notifications;
using Core.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Client.MVVM.ViewModels
{
    public class RecognizedTextViewModel : ViewModelBase
    {
        private string recognizedText;
        private string responseText;
        private Visibility responseVisibility;
        private CommunicationService _communicationService;

        public string RecognizedText
        {
            get { return recognizedText; }
            set
            {
                recognizedText = value;
                OnPropertyChanged(nameof(RecognizedText));
            }
        }

        public string ResponseText
        {
            get { return responseText; }
            set
            {
                responseText = value;
                OnPropertyChanged(nameof(ResponseText));
            }
        }

        public Visibility ResponseVisibility
        {
            get { return responseVisibility; }
            set
            {
                responseVisibility = value;
                OnPropertyChanged(nameof(ResponseVisibility));
            }
        }

        public ICommand CloseWindowCommand { get; }

        public RecognizedTextViewModel(CommunicationService communicationService)
        {
            _communicationService = communicationService;
            CloseWindowCommand = new ViewModelCommand(ExcecuteCloseWindowCommand);
        }

        private void ExcecuteCloseWindowCommand(object obj)
        {
            CancelSpeech();
        }

        private void CancelSpeech()
        {
            _communicationService.CancelSpeechAsync();
        }

    }
}
