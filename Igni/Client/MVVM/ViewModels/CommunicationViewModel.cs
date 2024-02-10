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
    public class CommunicationViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string recognizedText;

        public CommunicationViewModel()
        {
        }

        public string RecognizedText
        {
            get { return recognizedText; }
            set
            {
                recognizedText = value;
                OnPropertyChanged(nameof(RecognizedText));
            }
        }
    }
}
