using MediatR;

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
