using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Notifications
{
    public class SpeakNotification : INotification
    {
        public string Text { get; set; }
        public SpeakNotification(string text)
        {
            Text = text;
        }
    }
}
