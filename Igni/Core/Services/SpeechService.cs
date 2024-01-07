using Core.Models;
using Microsoft.CognitiveServices.Speech;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class SpeechService
    {
        private readonly SpeechRecognizer recognizer;

        public SpeechService(SpeechRecognizer recognizer)
        {
            this.recognizer = recognizer;
        }

        public Task<SpeechRecognitionResult> RecognizeOneSpeechAsync()
        {
            return recognizer.RecognizeOnceAsync();
        }
    }
}
