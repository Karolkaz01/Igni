using Core.Enums;
using Core.Models.Configuration;
using Core.Models.Notifications;
using Core.Services.Runners;
using MediatR;
using Microsoft.CognitiveServices.Speech;
using System.Speech.Recognition;

namespace Core.Services
{
    public class RecognizeService : INotificationHandler<KeywordRecognizedNotification>
    {
        private readonly SpeechService _STTService;
        private readonly KeywordService _KWService;
        private readonly ConfigurationService _configurationService;
        private readonly CommandRunner _commandRunner;
        private readonly PluginRunner _plugginRunner;

        private readonly IMediator _mediator;

        public RecognizeService(SpeechService STTService, KeywordService KWService, IMediator mediator,
            ConfigurationService configurationService, CommandRunner commandRunner, PluginRunner pluginRunner)
        {
            _STTService = STTService;
            _KWService = KWService;
            _mediator = mediator;
            _configurationService = configurationService;
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
                await _mediator.Publish(new RecognizedNotification(response.Text));
                _commandRunner.PerformCommandsAsync(response.Text);
                await _plugginRunner.PerformPluginsAsync(response.Text);
                Console.WriteLine(response.Text);
            }
            else
            {
                await _mediator.Publish(new UnrecognizedNotification());
                Console.WriteLine(">Unrecognized !!!<");
            }
            

            _KWService.StartRecognising();
        }

    }
}
