using Core.Enums;
using Core.Models.Configuration;
using Serilog;
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
        private readonly CommunicationService _communicationService;

        private int currentCommandsRunCount;
        public int CurrentCommandsRunCount { get { return currentCommandsRunCount; } }

        public CommandRunner(PowerShellHandler powerShellHandler, ConfigurationService configurationService, CommunicationService comunicationService)
        {
            _powerShellHandler = powerShellHandler;
            _configurationService = configurationService;
            _communicationService = comunicationService;
            currentCommandsRunCount = 0;
        }

        public void IncreaseCurrentCommandsRunCount()
        {
            currentCommandsRunCount++;
        }

        public async Task PerformCommandsAsync(string speech)
        {
            var allCommands = _configurationService.GetAllCommands();
            var text = speech.ToLower();
            var runCommandsCount = 0;
            foreach (var command in allCommands)
            {
                if (text.StartsWith(command.ActivationCommand.ToLower()))
                {
                    CommandRecognizedAsync(command);
                    runCommandsCount++;
                }
            }
            currentCommandsRunCount = runCommandsCount;
        }

        private async void CommandRecognizedAsync(Command command)
        {
            Log.Information($"Command recognized: {command.ActivationCommand}");

            switch (command.CommandType)
            {
                case CommandType.runCommand:
                    RunCommandAsync(command);
                    break;
                case CommandType.feedbackCommand:
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
        }

        private async void RunCommandAsync(Command command)
        {
            await _powerShellHandler.RunScript(command.Value);
        }

        private async void RunFeedbackCommandAsync(Command command)
        {
            var feedback = await _powerShellHandler.RunScript(command.Value);
            //Console.WriteLine(feedback?.FirstOrDefault()?.ToString());
            if(feedback != null)
                _communicationService.Speak(feedback?.FirstOrDefault()?.ToString());
        }

        private async void RunScriptAsync(Command command)
        {
            await _powerShellHandler.RunScriptByFileName(command.Value);
        }

        private async void RunFeedbackScriptAsync(Command command)
        {
            var feedback = await _powerShellHandler.RunScriptByFileName(command.Value);
            //Console.WriteLine(feedback?.FirstOrDefault()?.ToString());
            if (feedback != null)
                _communicationService.Speak(feedback?.FirstOrDefault()?.ToString());
        }

        private void ErrorCommandType()
        {
            Console.WriteLine("Error during performing command, command has unknown type check configuration.");
            //TODO: add error handling
        }
    }
}
