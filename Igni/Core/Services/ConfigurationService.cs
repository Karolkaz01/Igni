using Core.Consts;
using Core.Enums;
using Core.Models.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Tracing;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class ConfigurationService
    {
        private Configuration configuration;
        private readonly PowerShellHandler _powerShellHandler;

        private readonly string _filePath;

        public ConfigurationService(PowerShellHandler powerShellHandler)
        {
            _filePath = Directory.GetCurrentDirectory() + Paths.CONFIGURATION;
            _powerShellHandler = powerShellHandler;
            FethConfiguration();
        }

        public CommandSection? GetSection(string sectionName)
        {
            return configuration?.commandSections?.FirstOrDefault(s => s.Key == sectionName).Value;
        }

        public List<Command> GetAllCommands()
        {
            try
            {
                return configuration?.commandSections?.SelectMany(s => s.Value.values?.Select(v => v)).ToList() ?? new List<Command>();
            }
            catch
            {
                //TODO: Add error handling
                return new List<Command>();
            }
        }

        public IDictionary<string, string> GetSettings()
        {
            return configuration?.settings ?? new Dictionary<string, string>();
        }

        public IDictionary<string, bool> GetPluginSettins()
        {
            return configuration?.plugins ?? new Dictionary<string, bool>();
        }

        public void AddPluginSetting(string pluginName, bool isEnabled)
        {
            if (configuration?.plugins?.FirstOrDefault(p => p.Key == pluginName).Key != null)
            {
                configuration.plugins[pluginName] = isEnabled;
            }
            else
            {
                configuration?.plugins?.Add(pluginName, isEnabled);
            }

            SaveConfiguration();
            FethConfiguration();
        }

        public async Task GenerateDefaultOpenAppCommandsAsync()
        {
            List<PSObject> appDictionaryPSObjects = (await _powerShellHandler.RunScript("Get-StartApps")).ToList();
            var apps = new Dictionary<string, string>();
            List<Command> commands = new List<Command>();

            foreach (var a in appDictionaryPSObjects)
            {
                var name = a?.Properties["Name"]?.Value?.ToString()?.ToLower();
                var appID = a.Properties["AppID"].Value?.ToString();
                if (name != null && appID != null && !apps.ContainsKey(name))
                    commands.Add(new Command
                    {
                        activationCommand = $"Open {name}",
                        commandType = CommandType.runCommand,
                        value = $"start shell:appsFolder\\'{appID}'"
                    });
            }

            if (configuration?.commandSections?.FirstOrDefault(s => s.Key == "OpenCommands").Key != null)
            {
                configuration.commandSections["OpenCommands"].values = commands;
            }
            else
            {
                configuration?.commandSections?.Add("OpenCommands", new CommandSection
                {
                    name = "Open Commands",
                    description = "All commands for opening apps",
                    values = commands
                });
            }

            SaveConfiguration();
            FethConfiguration();
        }

        private void SaveConfiguration()
        {
            JsonSerializerSettings _options = new() { NullValueHandling = NullValueHandling.Ignore };
            string json = JsonConvert.SerializeObject(configuration, _options);
            File.WriteAllText(_filePath, json);
        }

        private void FethConfiguration()
        {
            using StreamReader reader = new(_filePath);
            var json = reader.ReadToEnd();
            configuration = JsonConvert.DeserializeObject<Configuration>(json);
        }
    }
}
