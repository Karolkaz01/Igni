using Core.Services.Speech;
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

        public CommunicationService(SpeechService speechService, ConfigurationService configuration)
        {
            _speechService = speechService;
            _configurationService = configuration;
        }

        public void Speek(string text)
        {
            var setting = _configurationService.GetSetting("VoiceEnabled");
            if (bool.TryParse(setting, out bool isEnabled) && isEnabled)
                _speechService.Speak(text);
            Log.Information(text);
        }

        public void Unrecognized()
        {
            var setting = _configurationService.GetSetting("VoiceEnabled");
            if (bool.TryParse(setting, out bool isEnabled) && isEnabled)
                _speechService.Speak("I'm sorry, I didn't understand you");
            Log.Warning("Speech unrecognized");
        }
    }
}
