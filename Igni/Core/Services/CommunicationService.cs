using Core.Models.Notifications;
using Core.Services.Speech;
using MediatR;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class CommunicationService
    {
        private readonly SpeechService _speechService;
        private readonly ConfigurationService _configurationService;
        private readonly IMediator _mediator;

        public CommunicationService(SpeechService speechService, ConfigurationService configuration, IMediator mediator)
        {
            _speechService = speechService;
            _configurationService = configuration;
            _mediator = mediator;
        }

        public async void RecognizedSpeechAsync(string text)
        {
            await _mediator.Publish(new RecognizedNotification(text));
        }

        public void Speak(string text)
        {
            var voiceSetting = _configurationService.GetSetting("Voice");
            _mediator.Publish(new SpeakNotification(text));
            if (!string.IsNullOrEmpty(voiceSetting) && !voiceSetting.Equals("None"))
                _speechService.Speak(text);
            Log.Information(text);
        }

        public void Unrecognized()
        {
            var voiceSetting = _configurationService.GetSetting("VoiceEnabled");
            _mediator.Publish(new UnrecognizedNotification());
            if (!string.IsNullOrEmpty(voiceSetting) && !voiceSetting.Equals("None"))
                _speechService.Speak("I'm sorry, I didn't understand you");
            Log.Warning("Speech unrecognized");
        }

        public async void CancelSpeechAsync()
        {
            _speechService.CancelSpeechAsync();
        }
    }
}
