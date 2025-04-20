using Igni.SDK;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System;
using Microsoft.CognitiveServices.Speech.Transcription;
using Microsoft.CognitiveServices.Speech;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

namespace SearchBotPlugin
{
    public class SearchBotPlugin : IIgniPlugin
    {
        public IIgniContext Context { get; set; }
        private YouTubeService youTubeService;

        public SearchBotPlugin(IIgniContext context)
        {
            Context = context;

            youTubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyAuw2MimECk8N8pg0oHyQbUEvQ8QtlfVBQ",
                ApplicationName = this.GetType().ToString()
            });
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
                case var s when s.StartsWith("Search for"):
                    SearchCommand(speech);
                    break;

                case var s when s.StartsWith("Search something"):
                    await SearchCommand();
                    break;

                case var s when s.StartsWith("Play"):
                    await PlayCommand(speech);
                    break;
            }
        }

        private async Task PlayCommand(string speech)
        {
            Context.SignalCommandRun();
            var reg = new Regex(@"^Play\s+(.*)[.]$");
            var match = reg.Match(speech);
            var songName = match.Groups.Count == 2 ? match.Groups[1].Value : null;

            var searchListRequest = youTubeService.Search.List("snippet");
            searchListRequest.Q = songName;
            searchListRequest.MaxResults = 10;

            var searchListResponse = await searchListRequest.ExecuteAsync();

            foreach (var searchResult in searchListResponse.Items)
            {
                if (searchResult.Id.Kind == "youtube#video")
                {
                    var videoId = searchResult.Id.VideoId;
                    var url = "https://www.youtube.com/watch?v=" + videoId;
                    Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
                    break;
                }
            }
        }

        private async Task SearchCommand()
        {
            Context.SignalCommandRun();
            Context.StopListening();
            Context.Speak("What do you want to search?");
            int i = 0;
            do
            {
                var searchingValue = await Context.RecognizeOneSpeechAsync();
                if (searchingValue.Reason == ResultReason.RecognizedSpeech)
                {
                    Context.RecognizedSpeechAsync(searchingValue.Text);
                    var url = "http://www.google.com/search?q=" + searchingValue.Text.Replace(".", "");
                    Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
                    break;
                }
                i++;
            } while (i < 3);
            if (i >= 3)
                Context.Speak("Sorry, I didn't understand that.");

            Context.StartListening();
        }

        private void SearchCommand(string speech)
        {
            Context.SignalCommandRun();
            var reg = new Regex(@"^Search for\s+(.*)[.]$");
            var match = reg.Match(speech);
            var searchQuery = match.Groups.Count == 2 ? match.Groups[1].Value : null;

            var url = "http://www.google.com/search?q=" + searchQuery;
            Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
        }
    }
}
