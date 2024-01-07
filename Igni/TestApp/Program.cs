using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Core.Services;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using MediatR;

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
        services.AddSingleton<AudioConfig>(AudioConfig.FromDefaultMicrophoneInput());
        services.AddSingleton<KeywordRecognitionModel>(KeywordRecognitionModel.FromFile(@"E:\HelloIgniTestModel.table"));
        services.AddMediatR(typeof(RecognizeService));
    })
    .Build();

var app = host.Services.GetService<RecognizeService>();
app?.StartRecognising();
await host.RunAsync();