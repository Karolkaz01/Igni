using Igni.SDK;
using Microsoft.CognitiveServices.Speech;
using OpenAI_API;

namespace ChatBotPlugin
{
    public class ChatBotPlugin : IIgniPlugin
    {
        public IIgniContext Context { get; set; }

        //private readonly string APIKey = "";
        private readonly string endpoint = "https://api.openai.com/v1/chat/completions";

        private readonly OpenAIAPI OpenAi;

        public ChatBotPlugin(IIgniContext context)
        {
            Context = context;
            OpenAi = new OpenAIAPI(new APIAuthentication(APIKey));
        }

        public void Initialize(CancellationToken? cancellationTokens)
        {
        }

        public async void ExcecuteAsync(CancellationToken? cancellationToken, string speech)
        {
            if (!speech.Equals("Listen."))
                return;

            var conversation = OpenAi.Chat.CreateConversation();

            while (true)
            {
                SpeechRecognitionResult speechResult = await Context.RecognizeOneSpeechAsync(); ;

                if (speechResult.Reason == ResultReason.RecognizedSpeech)
                {
                    if(speechResult.Text.Equals("Stop listening."))
                    {
                        Context.Speak("Ok, I'll stop listening.");
                        break;
                    }

                    conversation.AppendUserInput(speechResult.Text);
                    var response = await conversation.GetResponseFromChatbotAsync();
                    Context.Speak(response);
                }
                else if(speechResult.Reason == ResultReason.NoMatch)
                {
                    Context.Speak("Sorry, I didn't understand that.");
                }
            };

        }
    }
}
