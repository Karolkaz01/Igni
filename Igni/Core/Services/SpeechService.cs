using Microsoft.CognitiveServices.Speech;

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
