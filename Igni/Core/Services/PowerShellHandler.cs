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

            // create Powershell runspace
            Runspace runspace = RunspaceFactory.CreateRunspace(initialSessionState);

            // open it
            runspace.Open();

            // create a pipeline and feed it the script text
            Pipeline pipeline = runspace.CreatePipeline();
            pipeline.Commands.AddScript(scriptText);

            // add an extra command to transform the script output objects into nicely formatted strings
            // remove this line to get the actual objects that the script returns. For example, the script
            // "Get-Process" returns a collection of System.Diagnostics.Process instances.
            // pipeline.Commands.Add("Out-String");

            // execute the script
            var results = pipeline.Invoke();

            // close the runspace
            runspace.Close();

            // convert the script result into a single string
            // StringBuilder stringBuilder = new StringBuilder();
            // foreach (PSObject obj in results)
            // {
            //     stringBuilder.AppendLine(obj.ToString());
            // }

            // return the results of the script that has
            // now been converted to text
            return results;
        }

        public async Task<ICollection<PSObject>> RunScriptByFileName(string fileName)
        {
            InitialSessionState initialSessionState = InitialSessionState.CreateDefault();
            initialSessionState.ExecutionPolicy = ExecutionPolicy.Unrestricted;

            // create Powershell runspace
            Runspace runspace = RunspaceFactory.CreateRunspace(initialSessionState);

            // open it
            runspace.Open();

            // create a pipeline and feed it the script text
            Pipeline pipeline = runspace.CreatePipeline();
            pipeline.Commands.AddScript("Powershell.exe -File '" + Directory.GetCurrentDirectory() + @"\Scripts\" + fileName + "'");

            // add an extra command to transform the script output objects into nicely formatted strings
            // remove this line to get the actual objects that the script returns. For example, the script
            // "Get-Process" returns a collection of System.Diagnostics.Process instances.
            // pipeline.Commands.Add("Out-String");

            // execute the script
            var results = pipeline.Invoke();

            // close the runspace
            runspace.Close();

            // convert the script result into a single string
            // StringBuilder stringBuilder = new StringBuilder();
            // foreach (PSObject obj in results)
            // {
            //     stringBuilder.AppendLine(obj.ToString());
            // }

            // return the results of the script that has
            // now been converted to text
            return results;
        }
    }
}
