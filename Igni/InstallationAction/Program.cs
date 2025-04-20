using Core.Services;

namespace InstallationAction
{
    public class Program
    {
        static async void Main(string[] args)
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
