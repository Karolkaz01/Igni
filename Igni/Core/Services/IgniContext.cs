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
        private readonly ComunicationService _comunicationService;

        public IgniContext(SpeechService speechService, ComunicationService comunicationService)
        {
            _speechService = speechService;
            _comunicationService = comunicationService;
        }
        public async Task<SpeechRecognitionResult> RecognizeOneSpeechAsync()
        {
            var response = await _speechService.RecognizeOneSpeechAsync();
            return response;
        }

        public void Speak(string text)
        {
            _comunicationService.Speek(text);
        }
    }
}
