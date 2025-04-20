using Core.Consts;
using Core.Enums;
using Core.Models.Configuration;
using Newtonsoft.Json;
using Serilog;
using System.Management.Automation;

namespace Core.Services
{
    public class ConfigurationService
    {
        private ConfigurationModel configuration;
        private readonly PowerShellHandler _powerShellHandler;

        private readonly string _filePath;

        public ConfigurationService(PowerShellHandler powerShellHandler)
        {
            _filePath = Directory.GetCurrentDirectory() + Paths.CONFIGURATION;
            _powerShellHandler = powerShellHandler;
            FetchConfiguration();
        }

        public CommandSection? GetCommandSection(string sectionName)
        {
            return configuration?.CommandSections?.FirstOrDefault(s => s.Key == sectionName).Value;
        }

        public IDictionary<string, CommandSection> GetAllCommandSections()
        {
            return configuration?.CommandSections;
        }

        public List<Command> GetAllCommands()
        {
            try
            {
                return configuration?.CommandSections?.SelectMany(s => s.Value.Values?.Select(v => v)).ToList() ?? new List<Command>();
            }
            catch
            {
                //TODO: Add error handling
                return new List<Command>();
            }
        }

        public void SetSetting(string key, string value)
        {
            var settingValue = GetSetting(key);
            if(settingValue == null)
            {
                configuration?.Settings?.Add(key, value);
            }
            else
            {
                configuration.Settings[key] = value;
            }
            Synchronize();
        }

        public void DeleteSetting(string key)
        {
            var settingValue = GetSetting(key);
            if(settingValue != null)
            {
                configuration?.Settings?.Remove(key);
                Synchronize();
            }
        }

        public IDictionary<string, string> GetKeyWords()
        {
            return configuration?.KeyWords ?? new Dictionary<string, string>();
        }

        public IDictionary<string, string> GetVoices()
        {
            return configuration?.Voices ?? new Dictionary<string, string>();
        }

        public string? GetSetting(string settingName)
        {
            return configuration?.Settings?.FirstOrDefault(s => s.Key == settingName).Value;
        }

        public IDictionary<string, string> GetAllSettings()
        {
            return configuration?.Settings ?? new Dictionary<string, string>();
        }

        public IDictionary<string, PluginConfig> GetPluginSetting()
        {
            return configuration?.PluginsInfo ?? new Dictionary<string, PluginConfig>();
        }

        public void AddPluginSetting(string directoryName, string fileName)
        {
            if (configuration?.PluginsInfo?.FirstOrDefault(p => p.Key == fileName) != null)
            {
                Log.Warning("Can't add pluggin with the same name");
            }
            else
            {
                configuration?.PluginsInfo?.Add(
                    fileName,
                    new PluginConfig
                    {
                        DirectoryName = directoryName,
                        FileName = fileName,
                        IsEnabled = true
                    });
            }

            Synchronize();
        }

        public async Task GenerateDefaultOpenAppCommandsAsync()
        {
            Log.Information($"Generating default open commands");
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
                        ActivationCommand = $"Open {name}",
                        CommandType = CommandType.runCommand,
                        Value = $"start shell:appsFolder\\'{appID}'"
                    });
            }

            if (configuration?.CommandSections?.FirstOrDefault(s => s.Key == "OpenCommands").Key != null)
            {
                configuration.CommandSections["OpenCommands"].Values = commands;
            }
            else
            {
                configuration?.CommandSections?.Add("OpenCommands", new CommandSection
                {
                    Name = "Open Commands",
                    Description = "All commands for opening apps",
                    Values = commands
                });
            }

            //TEST
            Console.WriteLine("TEST Install ACTION");
            Synchronize();
        }

        public void SaveCommandConfig(IDictionary<string,CommandSection> commandConfig)
        {
            if (commandConfig != null)
            {
                configuration.CommandSections = commandConfig;
                Synchronize();
            }
        }

        public void SavePluginConfig(IDictionary<string, PluginConfig> pluginConfig)
        {
            if(pluginConfig != null)
            {
                configuration.PluginsInfo = pluginConfig;
                Synchronize();
            }
        }

        public void SaveSettingsConfig(IDictionary<string, string> settingsConfig)
        {
            if (settingsConfig != null)
            {
                configuration.Settings = settingsConfig;
                Synchronize();
            }
        }

        private void SaveConfiguration()
        {
            JsonSerializerSettings _options = new() { NullValueHandling = NullValueHandling.Ignore };
            string json = JsonConvert.SerializeObject(configuration, _options);
            File.WriteAllText(_filePath, json);
        }

        private void FetchConfiguration()
        {
            using StreamReader reader = new(_filePath);
            var json = reader.ReadToEnd();
            configuration = JsonConvert.DeserializeObject<ConfigurationModel>(json);
        }

        private void Synchronize()
        {
            SaveConfiguration();
            FetchConfiguration();
        }
    }
}
