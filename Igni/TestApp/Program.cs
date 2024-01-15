using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Core.Services;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using MediatR;
using Microsoft.Extensions.Configuration;
using Core.Services.Runners;
using Core.Helpers;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
    {
        services.AddSingleton<SpeechService>();
        services.AddSingleton<KeywordService>();
        services.AddSingleton<RecognizeService>();
        services.AddSingleton<SpeechRecognizer>();
        services.AddSingleton<KeywordRecognizer>();
        services.AddSingleton<SpeechConfig>(SpeechConfig.FromSubscription(
                        Environment.GetEnvironmentVariable("SpeechToTextKey"),
                        Environment.GetEnvironmentVariable("SpeechToTextRegion")
                     ));
        services.AddSingleton<ConfigurationService>();
        services.AddSingleton<AudioConfig>(AudioConfig.FromDefaultMicrophoneInput());
        services.AddSingleton<KeywordRecognitionModel>(KeywordRecognitionModel.FromFile(@"E:\HelloIgniTestModel.table"));
        services.AddSingleton<PowerShellHandler>();
        services.AddSingleton<CommandRunner>();
        services.AddSingleton<PluginRunner>();
        services.AddSingleton<PluginsManager>();
        services.AddMediatR(typeof(RecognizeService));
    })
    .Build();

var app = host.Services.GetService<RecognizeService>();
var config = host.Services.GetService<ConfigurationService>();
await config?.GenerateDefaultOpenAppCommandsAsync();
app?.StartRecognising();
await host.RunAsync();