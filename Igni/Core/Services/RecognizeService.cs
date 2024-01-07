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
    public class RecognizeService : INotificationHandler<KeywordRecognizedNotification>
    {
        private readonly SpeechService STTService;
        private readonly KeywordService KWService;

        private readonly IMediator _mediator;

        public RecognizeService(SpeechService STTService, KeywordService KWService, IMediator mediator)
        {
            this.STTService = STTService;
            this.KWService = KWService;
            _mediator = mediator;
        }

        public void StartRecognising()
        {
            Console.WriteLine("I'm listening...");
            KWService.StartRecognising();
        }

        public void StopRecognising()
        {
            KWService.StopRecognising();
        }

        public async Task Handle(KeywordRecognizedNotification notification, CancellationToken cancellationToken)
        {
            //TODO: add beep sound
            Console.Beep();
            var response = await STTService.RecognizeOneSpeechAsync();
            if(response.Reason == ResultReason.RecognizedSpeech)
            {
                await _mediator.Publish(new RecognizedNotification(response.Text));
                Console.WriteLine(response.Text);
            }
            else
            {
                await _mediator.Publish(new UnrecognizedNotification());
                Console.WriteLine(">Unrecognized !!!<");
            }
            //TODO: log data
            KWService.StartRecognising();
        }
    }
}
