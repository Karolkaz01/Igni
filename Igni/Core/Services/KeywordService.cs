using Core.Models;
using Core.Models.Notifications;
using MediatR;
using Microsoft.CognitiveServices.Speech;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
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
            await _mediator.Publish(new KeywordRecognizedNotification());
        }
    }
}
