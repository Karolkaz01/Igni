using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace InstallationActionRunnerDotNetFramework
{
    public class CustomActionRunner : Installer
    {
        public override void Install(System.Collections.IDictionary stateSaver)
        {
            base.Install(stateSaver);

            // Access the CustomActionData property
            string customActionData = Context.Parameters["CustomActionData"];

            // Split the string to handle multiple arguments
            string[] tokens = customActionData.Split(';');

            // Now you can access each argument by its position in the array
            string installDir = tokens[0]; // Assuming [INSTALLDIR] is the first argument
            string productName = tokens[1]; // Assuming [ProductName] is the second argument

            Console.WriteLine(installDir);
            Console.WriteLine(productName);
            // Perform your custom actions here using the installDir and productName
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = installDir + "InstallationAction.exe\\.exe",
                Arguments = installDir,
                UseShellExecute = false
            };

        }
    }
}
