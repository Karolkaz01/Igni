using Core.Services;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;

namespace InstallerActions
{
    public class InstallerAction
    {
        static async void Main(string[] args)
        {
            if (args.Length == 1)
            {
                Console.WriteLine("Usage : installutil.exe Installer.exe ");
                Console.Beep();
                Console.Beep();
                var configurationService = new ConfigurationService(new PowerShellHandler());
                await configurationService.GenerateDefaultOpenAppCommandsAsync();
                Console.Beep();
                Console.Beep();
                Console.Beep();
            }
        }
    }
}
