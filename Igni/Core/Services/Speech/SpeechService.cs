using Microsoft.CognitiveServices.Speech;

namespace Core.Services.Speech
{
    public class SpeechService
    {
        private readonly SpeechRecognizer _recognizer;
        private readonly SpeechSynthesizer _synthesizer;

        public SpeechService(SpeechRecognizer recognizer, SpeechSynthesizer synthesizer)
        {
            _recognizer = recognizer;
            _synthesizer = synthesizer;
        }

        public Task<SpeechRecognitionResult> RecognizeOneSpeechAsync()
        {
            return _recognizer.RecognizeOnceAsync();
        }

        public Task<SpeechSynthesisResult> Speak(string text)
        {
            return _synthesizer.SpeakTextAsync(text);
        }

        public async void CancelSpeechAsync()
        {
            _synthesizer.StopSpeakingAsync();
        }
    }
}
