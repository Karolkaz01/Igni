using Core.Enums;
using Core.Models.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Runners
{
    public class CommandRunner
    {
        private readonly PowerShellHandler _powerShellHandler;
        private readonly ConfigurationService _configurationService;

        public CommandRunner(PowerShellHandler powerShellHandler, ConfigurationService configurationService)
        {
            _powerShellHandler = powerShellHandler;
            _configurationService = configurationService;

        }

        public async Task PerformCommandsAsync(string speech)
        {
            var allCommands = _configurationService.GetAllCommands();
            var text = speech.ToLower();
            foreach (var command in allCommands)
            {
                if (text.StartsWith(command.activationCommand.ToLower()))
                {
                    CommandRecognizedAsync(command);
                }
            }
        }

        private async void CommandRecognizedAsync(Command command)
        {
            switch (command.commandType)
            {
                case CommandType.runCommand:
                    RunCommandAsync(command);
                    break;
                case CommandType.feedbackCoomand:
                    RunFeedbackCommandAsync(command);
                    break;
                case CommandType.runScript:
                    RunScriptAsync(command);
                    break;
                case CommandType.feedbackScript:
                    RunFeedbackScriptAsync(command);
                    break;
                default:
                    ErrorCommandType();
                    break;
            }

            Console.WriteLine($"Command recognized: {command.activationCommand}");
        }

        private async void RunCommandAsync(Command command)
        {
            await _powerShellHandler.RunScript(command.value);
        }

        private async void RunFeedbackCommandAsync(Command command)
        {
            var feedback = await _powerShellHandler.RunScript(command.value);
            Console.WriteLine(feedback?.FirstOrDefault()?.ToString());
        }

        private async void RunScriptAsync(Command command)
        {
            await _powerShellHandler.RunScriptByFileName(command.value);
        }

        private async void RunFeedbackScriptAsync(Command command)
        {
            var feedback = await _powerShellHandler.RunScriptByFileName(command.value);
            Console.WriteLine(feedback?.FirstOrDefault()?.ToString());
        }

        private void ErrorCommandType()
        {
            Console.WriteLine("Error during performing command, command has unknown type check configuration.");
            //TODO: add error handling
        }
    }
}
