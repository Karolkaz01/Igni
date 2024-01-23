using Core.Models.Notifications;
using MediatR;
using Microsoft.CognitiveServices.Speech;
using Serilog;

namespace Core.Services.Speech
{
    public class KeywordService
    {
        private readonly KeywordRecognizer recognizer;
        private readonly KeywordRecognitionModel model;

        private readonly IMediator _mediator;

        public KeywordService(KeywordRecognizer recognizer, KeywordRecognitionModel model, IMediator mediator)
        {
            this.recognizer = recognizer;
            this.model = model;
            _mediator = mediator;
            this.recognizer.Recognized += OnKeywordRecognizedAsync;
        }

        public void StartRecognising()
        {
            recognizer.RecognizeOnceAsync(model);
        }

        public void StopRecognising()
        {
            recognizer.StopRecognitionAsync();
        }

        private async void OnKeywordRecognizedAsync(object? sender, KeywordRecognitionEventArgs e)
        {
            Log.Information("Keyword recognized");
            await _mediator.Publish(new KeywordRecognizedNotification());
        }
    }
}
