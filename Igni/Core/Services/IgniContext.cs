using Core.Services.Runners;
using Core.Services.Speech;
using Igni.SDK;
using Microsoft.CognitiveServices.Speech;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class IgniContext : IIgniContext
    {
        private readonly SpeechService _speechService;
        private readonly CommunicationService _comunicationService;
        private readonly ConfigurationService _configurationService;
        private readonly CommandRunner _commandRunner;

        public IgniContext(SpeechService speechService, CommunicationService comunicationService, ConfigurationService configurationService, CommandRunner commandRunner)
        {
            _speechService = speechService;
            _comunicationService = comunicationService;
            _configurationService = configurationService;
            _commandRunner = commandRunner;
        }

        public async Task<SpeechRecognitionResult> RecognizeOneSpeechAsync()
        {
            var response = await _speechService.RecognizeOneSpeechAsync();
            return response;
        }

        public int GetCurrentCommandRunCount()
        {
            return _commandRunner.CurrentCommandsRunCount;
        }

        public void Speak(string text)
        {
            _comunicationService.Speak(text);
        }
    }
}
