using Microsoft.PowerShell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class PowerShellHandler
    {
        public async Task<ICollection<PSObject>> RunScript(string scriptText)
        {
            InitialSessionState initialSessionState = InitialSessionState.CreateDefault();
            initialSessionState.ExecutionPolicy = ExecutionPolicy.Unrestricted;

            Runspace runspace = RunspaceFactory.CreateRunspace(initialSessionState);

            runspace.Open();

            Pipeline pipeline = runspace.CreatePipeline();
            pipeline.Commands.AddScript(scriptText);

            var results = pipeline.Invoke();

            runspace.Close();

            return results;
        }

        public async Task<ICollection<PSObject>> RunScriptByFileName(string fileName)
        {
            InitialSessionState initialSessionState = InitialSessionState.CreateDefault();
            initialSessionState.ExecutionPolicy = ExecutionPolicy.Unrestricted;

            Runspace runspace = RunspaceFactory.CreateRunspace(initialSessionState);

            runspace.Open();

            Pipeline pipeline = runspace.CreatePipeline();
            pipeline.Commands.AddScript("Powershell.exe -File '" + Directory.GetCurrentDirectory() + @"\Scripts\" + fileName + "'");

            var results = pipeline.Invoke();

            runspace.Close();

            return results;
        }
    }
}
