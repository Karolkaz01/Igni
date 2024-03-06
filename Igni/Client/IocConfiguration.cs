using Client.MVVM.ViewModels;
using Core.Consts;
using Core.Models.Notifications;
using Core.Services;
using Core.Services.Plugins;
using Core.Services.Runners;
using Core.Services.Speech;
using Igni.SDK;
using MediatR;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Client
{
    public static class IocConfiguration
    {
        private static IHost host;

        public static void LoadDependenciesAsync()
        {
            var configuration = new ConfigurationService(new PowerShellHandler());

            var speechConfig = SpeechConfig.FromSubscription(
                        Environment.GetEnvironmentVariable("SpeechToTextKey"),
                        Environment.GetEnvironmentVariable("SpeechToTextRegion")
                     );
            speechConfig.SpeechRecognitionLanguage = "en-GB";
            var voiceSetting = configuration.GetSetting("Voice") ?? "None";
            speechConfig.SpeechSynthesisVoiceName = voiceSetting;
            var keyWordSetting = configuration.GetSetting("ActivationKeyword") ?? configuration.GetKeyWords().FirstOrDefault().Value;
            var keyWordPath = Directory.GetCurrentDirectory() + Paths.KEYWORDS + keyWordSetting;
            keyWordPath = @"E:\HeyCompyuter.table";
            var keyWordModel = KeywordRecognitionModel.FromFile(keyWordPath);

            RecognizedTextViewModel communicationViewModel = new RecognizedTextViewModel();

            host = Host.CreateDefaultBuilder()
                .ConfigureServices((_, services) =>
                {
                    services.AddSingleton<SpeechService>();
                    services.AddSingleton<KeywordService>();
                    services.AddSingleton<RecognizeService>();
                    services.AddSingleton<SpeechRecognizer>();
                    services.AddSingleton<KeywordRecognizer>();
                    services.AddSingleton<SpeechSynthesizer>();
                    services.AddSingleton<SpeechConfig>(speechConfig);
                    services.AddSingleton<ConfigurationService>(configuration);
                    services.AddSingleton<AudioConfig>(AudioConfig.FromDefaultMicrophoneInput());
                    services.AddSingleton<KeywordRecognitionModel>(keyWordModel);
                    services.AddSingleton<PowerShellHandler>();
                    services.AddSingleton<CommandRunner>();
                    services.AddSingleton<PluginRunner>();
                    services.AddSingleton<PluginsManager>();
                    services.AddSingleton<CommunicationService>();
                    services.AddSingleton<IIgniContext, IgniContext>();
                    services.AddMediatR(typeof(RecognizeService));
                    services.AddMediatR(typeof(SingletonNotification));
                    services.AddSingleton<SingletonNotification>();
                    services.AddSingleton<CommandsViewModel>();
                    services.AddSingleton<PluginsViewModel>();
                    services.AddSingleton<SettingsViewModel>();
                    services.AddSingleton<MainWindow>();
                    services.AddSingleton<MainWindowViewModel>();
                    services.AddSingleton<RecognizedTextViewModel>();
                    services.AddSingleton<RecognizedTextWindow>();
                })
                .Build();

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("logs\\IgniLogs-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            configuration.GenerateDefaultOpenAppCommandsAsync();
        }

        public static T? Get<T>()
        {
            return host.Services.GetService<T>();
        }
    }
}
