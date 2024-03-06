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
        private readonly CommunicationService _communicationService;
        private readonly CommandRunner _commandRunner;
        private readonly PluginRunner _pluginRunner;

        private readonly IMediator _mediator;

        public RecognizeService(SpeechService STTService, KeywordService KWService, IMediator mediator,
            CommunicationService communicationService, CommandRunner commandRunner, PluginRunner pluginRunner)
        {
            _STTService = STTService;
            _KWService = KWService;
            _mediator = mediator;
            _communicationService = communicationService;
            _commandRunner = commandRunner;
            _pluginRunner = pluginRunner;
        }

        public void StartListening()
        {
            Console.WriteLine("I'm listening...");
            _KWService.StartRecognising();
        }

        public void StopListening()
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
                await _pluginRunner.PerformPluginsAsync(response.Text);
            }
            else
            {
                _communicationService.Unrecognized();
            }
            _KWService.StartRecognising();
        }

    }
}
