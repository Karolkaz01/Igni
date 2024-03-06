using Core.Models.Notifications;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Client.MVVM.ViewModels
{
    public class RecognizedTextViewModel : ViewModelBase
    {
        private string recognizedText;
        private string responseText;
        private Visibility responseVisibility;

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

        public RecognizedTextViewModel()
        {
        }
        
    }
}
