using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Notifications
{
    public class RecognizedNotification : INotification
    {
        public string Text { get; set; }

        public RecognizedNotification(string text)
        {
            Text = text;
        }
    }
}
