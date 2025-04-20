using Microsoft.CognitiveServices.Speech;

namespace Igni.SDK
{
    public interface IIgniContext
    {
        Task<SpeechRecognitionResult> RecognizeOneSpeechAsync();
        int GetCurrentCommandRunCount();
        void Speak(string text);
        Task RunScriptAsync(string script);
        Task RunScriptByFileNameAsync(string fileName);
        void SignalCommandRun();
        string? GetSetting(string key);
        void RecognizedSpeechAsync(string text);
        void StopListening();
        void StartListening();
    }
}
