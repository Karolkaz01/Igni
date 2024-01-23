using Microsoft.CognitiveServices.Speech;

namespace Igni.SDK
{
    public interface IIgniContext
    {
        Task<SpeechRecognitionResult> RecognizeOneSpeechAsync();

        void Speak(string text);
    }
}
