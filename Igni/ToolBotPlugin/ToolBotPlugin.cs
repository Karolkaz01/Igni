using Igni.SDK;
using Microsoft.CognitiveServices.Speech;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using WindowsInput;

namespace ToolBotPlugin
{
    public class ToolBotPlugin : IIgniPlugin
    {
        public IIgniContext Context { get; set; }

        public ToolBotPlugin(IIgniContext context)
        {
            Context = context;
        }
        public void Initialize(CancellationToken? cancellationTokens)
        {
        }

        public async void ExecuteAsync(CancellationToken? cancellationToken, string speech)
        {
            if (string.IsNullOrEmpty(speech))
                return;

            switch (speech)
            {
                case var s when s.StartsWith("Close"):
                    CloseCommand(speech);
                    break;

                case var s when s.StartsWith("Start writing"):
                    await WriteTextWithVoiceAsyncCommand();
                    break;

                case var s when s.StartsWith("Take note"):
                    await TakingNoteCommandAsync();
                    break;
            }
        }

        private void CloseCommand(string speech)
        {
            Context.SignalCommandRun();

            var reg = new Regex(@"^Close\s+(.*)[.]$");
            var match = reg.Match(speech);
            var appName = match.Groups.Count == 2 ? match.Groups[1].Value : string.Empty;

            var mappingJson = Context.GetSetting("CloseAppNamesMapping");
            Dictionary<string, string> mapping = JsonConvert.DeserializeObject<Dictionary<string, string>>(mappingJson);
            if (mapping.ContainsKey(appName))
                appName = mapping[appName];

            if (appName.Length < 3)
            {
                Context.Speak("I can't close apps with name shorter then 4 letters.");
                return;
            }

            if (!string.IsNullOrEmpty(appName))
            {
                string script = $"Get-Process | Where-Object {{ $_.Description -Match '.*{appName}.*' }} | Select-Object -First 3 | Stop-Process";
                Context.RunScriptAsync(script);
                script = $"Get-Process | Where-Object {{ $_.MainWindowTitle -Match '.*{appName}.*' }} | Select-Object -First 3 | Stop-Process";
                Context.RunScriptAsync(script);
                script = $"Get-Process | Where-Object {{ $_.Name -eq '{appName}' }} | Select-Object -First 3 | Stop-Process";
                Context.RunScriptAsync(script);
            }
        }

        private async Task WriteTextWithVoiceAsyncCommand()
        {
            Context.SignalCommandRun();
            Context.StopListening();
            Context.Speak("You can speak.");

            InputSimulator inputSender = new InputSimulator();

            var cancellationTokenSource = new CancellationTokenSource();

            int i = 0;
            do
            {
                var recognizeResult = await Context.RecognizeOneSpeechAsync();
                if (recognizeResult.Reason == ResultReason.RecognizedSpeech)
                {
                    i = 0;
                    if (recognizeResult.Text == "Exit.")
                        break;
                    inputSender.Keyboard.TextEntry(recognizeResult.Text);
                }
                else
                {
                    i++;
                }
            } while (i < 5);
            Context.Speak("Writing has been stopped");
            Context.StartListening();
        }

        private async Task TakingNoteCommandAsync()
        {
            Context.SignalCommandRun();
            Context.StopListening();
            Context.Speak("Say your note?");
            int i = 0;
            do
            {
                var note = await Context.RecognizeOneSpeechAsync();
                if (note.Reason == ResultReason.RecognizedSpeech)
                {
                    //Context.RecognizedSpeechAsync(note.Text);
                    var noteDirectory = Context.GetSetting("NotesDirectory");
                    if(noteDirectory != null)
                    {
                        string noteName = $"Note_{DateTime.Now:yyyyMMdd}.txt";

                        // Combine the directory path and the filename to get the full path.
                        string filePath = Path.Combine(noteDirectory, noteName);
                        using (StreamWriter sw = new StreamWriter(filePath, true))
                        {
                            sw.WriteLine(note.Text);
                        }
                        Context.Speak("Note taken.");
                    }
                    break;
                }
                i++;
            } while (i < 3);
            if (i >= 3)
                Context.Speak("Sorry, I didn't understand that.");

            Context.StartListening();
        }
    }
}
