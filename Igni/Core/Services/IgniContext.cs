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
        private readonly CommunicationService _communicationService;
        private readonly ConfigurationService _configurationService;
        private readonly CommandRunner _commandRunner;
        private readonly PowerShellHandler _powerShellHandler;
        private readonly KeywordService _keywordService;

        public IgniContext(SpeechService speechService, CommunicationService communicationService, ConfigurationService configurationService, CommandRunner commandRunner,
            PowerShellHandler powerShellHandler, KeywordService keywordService)
        {
            _speechService = speechService;
            _communicationService = communicationService;
            _configurationService = configurationService;
            _commandRunner = commandRunner;
            _powerShellHandler = powerShellHandler;
            _keywordService = keywordService;
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
            _communicationService.Speak(text);
        }

        public async Task RunScriptAsync(string script)
        {
            _powerShellHandler.RunScript(script);
        }

        public async Task RunScriptByFileNameAsync(string fileName)
        {
            _powerShellHandler.RunScriptByFileName(fileName);
        }

        public void SignalCommandRun()
        {
            _commandRunner.IncreaseCurrentCommandsRunCount();
        }

        public string? GetSetting(string key)
        {
            return _configurationService.GetSetting(key);
        }

        public void RecognizedSpeechAsync(string text)
        {
            _communicationService.RecognizedSpeechAsync(text);
        }

        public void StopListening()
        {
            _keywordService.StopRecognising();
        }

        public void StartListening()
        {
            _keywordService.StartRecognising();
        }
    }
}
