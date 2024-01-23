using Core.Enums;
using Core.Models.Configuration;
using Core.Models.Notifications;
using Core.Services.Runners;
using Core.Services.Speech;
using MediatR;
using Microsoft.CognitiveServices.Speech;
using Serilog;
using System.Speech.Recognition;

namespace Core.Services
{
    public class RecognizeService : INotificationHandler<KeywordRecognizedNotification>
    {
        private readonly SpeechService _STTService;
        private readonly KeywordService _KWService;
        private readonly ComunicationService _comunicationService;
        private readonly CommandRunner _commandRunner;
        private readonly PluginRunner _plugginRunner;

        private readonly IMediator _mediator;

        public RecognizeService(SpeechService STTService, KeywordService KWService, IMediator mediator,
            ComunicationService comunicationService, CommandRunner commandRunner, PluginRunner pluginRunner)
        {
            _STTService = STTService;
            _KWService = KWService;
            _mediator = mediator;
            _comunicationService = comunicationService;
            _commandRunner = commandRunner;
            _plugginRunner = pluginRunner;
        }

        public void StartRecognising()
        {
            Console.WriteLine("I'm listening...");
            _KWService.StartRecognising();
        }

        public void StopRecognising()
        {
            _KWService.StopRecognising();
        }

        public async Task Handle(KeywordRecognizedNotification notification, CancellationToken cancellationToken)
        {
            //TODO: add beep sound
            Console.Beep();
            var response = await _STTService.RecognizeOneSpeechAsync();
            if (response.Reason == ResultReason.RecognizedSpeech)
            {
                Log.Information($"Recognized speech: {response.Text}");
                await _mediator.Publish(new RecognizedNotification(response.Text));
                await _commandRunner.PerformCommandsAsync(response.Text);
                await _plugginRunner.PerformPluginsAsync(response.Text);
            }
            else
            {
                await _mediator.Publish(new UnrecognizedNotification());
                _comunicationService.Unrecognized();
            }
            

            _KWService.StartRecognising();
        }

    }
}
