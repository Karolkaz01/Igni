using Core.Services;

namespace InitializeConfigurationApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Start initializing configuration for apliation");
            var configurationService = new ConfigurationService(new PowerShellHandler());
            await configurationService.GenerateDefaultOpenAppCommandsAsync();
            Console.WriteLine("Initialization completed!!!");
            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }
    }
}
