using Microsoft.CognitiveServices.Speech;

namespace Igni.SDK
{
    public interface IIgniContext
    {
        Task<SpeechRecognitionResult> RecognizeOneSpeechAsync();
        int GetCurrentCommandRunCount();
        void Speak(string text);
    }
}
