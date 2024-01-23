using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Core.Services;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using MediatR;
using Microsoft.Extensions.Configuration;
using Core.Services.Runners;
using Igni.SDK;
using Core.Services.Plugins;
using Core.Services.Speech;
using Serilog;

var speechComfig = SpeechConfig.FromSubscription(
                        Environment.GetEnvironmentVariable("SpeechToTextKey"),
                        Environment.GetEnvironmentVariable("SpeechToTextRegion")
                     );
speechComfig.SpeechRecognitionLanguage = "en-GB";
speechComfig.SpeechSynthesisVoiceName = "en-AU-NeilNeural";

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
    {
        services.AddSingleton<SpeechService>();
        services.AddSingleton<KeywordService>();
        services.AddSingleton<RecognizeService>();
        services.AddSingleton<SpeechRecognizer>();
        services.AddSingleton<KeywordRecognizer>();
        services.AddSingleton<SpeechSynthesizer>();
        services.AddSingleton<SpeechConfig>(speechComfig);
        services.AddSingleton<ConfigurationService>();
        services.AddSingleton<AudioConfig>(AudioConfig.FromDefaultMicrophoneInput());
        services.AddSingleton<KeywordRecognitionModel>(KeywordRecognitionModel.FromFile(@"E:\HelloIgniTestModel.table"));
        services.AddSingleton<PowerShellHandler>();
        services.AddSingleton<CommandRunner>();
        services.AddSingleton<PluginRunner>();
        services.AddSingleton<PluginsManager>();
        services.AddSingleton<ComunicationService>();
        services.AddSingleton<IIgniContext,IgniContext>();
        services.AddMediatR(typeof(RecognizeService));
    })
    .Build();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs\\IgniLogs-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var app = host.Services.GetService<RecognizeService>();
var config = host.Services.GetService<ConfigurationService>();
await config?.GenerateDefaultOpenAppCommandsAsync();
app?.StartRecognising();
await host.RunAsync();