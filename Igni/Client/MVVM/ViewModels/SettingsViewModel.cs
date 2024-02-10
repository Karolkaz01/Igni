using Client.MVVM.Models;
using Core.Models.Configuration;
using Core.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Client.MVVM.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private ConfigurationService _configurationService;

        private IDictionary<string, string> activationDictionary = new Dictionary<string, string>();

        private ObservableCollection<string> activationKeys = new ObservableCollection<string>();

        public ObservableCollection<string> ActivationKeys
        {
            get
            {
                return activationKeys;
            }
            set
            {
                activationKeys = value;
                OnPropertyChanged(nameof(ActivationKeys));
            }
        }

        private int keyWordIndex = 0;
        public int KeyWordIndex
        {
            get
            {
                return keyWordIndex;
            }
            set
            {
                keyWordIndex = value;
                _configurationService.SetSetting("ActivationKeyword", activationDictionary.FirstOrDefault(k => k.Key == activationKeys[value]).Value);
                OnPropertyChanged(nameof(KeyWordIndex));
                UpdateSettings();
                OnPropertyChanged(nameof(Settings));
            }
        }

        private IDictionary<string,string> voicesDictionary = new Dictionary<string,string>();

        private ObservableCollection<string> voices = new ObservableCollection<string>();

        public ObservableCollection<string> Voices
        {
            get
            {
                return voices;
            }
            set
            {
                voices = value;
                OnPropertyChanged(nameof(Voices));
            }
        }

        private int voiceIndex = 0;

        public int VoiceIndex
        {
            get
            {
                return voiceIndex;
            }
            set
            {
                voiceIndex = value;
                _configurationService.SetSetting("Voice", voicesDictionary.FirstOrDefault(k => k.Key == voices[value]).Value);
                OnPropertyChanged(nameof(Voices));
                UpdateSettings();
                OnPropertyChanged(nameof(Settings));
            }
        }

        private ObservableCollection<SettingMVVM> settings = new ObservableCollection<SettingMVVM>();

        public ObservableCollection<SettingMVVM> Settings
        {
            get
            {
                return settings;
            }
            set
            {
                settings = value;
                OnPropertyChanged(nameof(Settings));
            }
        }

        

        public ICommand DeleteSettingCommand { get; }
        public ICommand SaveCommand { get; }

        public SettingsViewModel(ConfigurationService configurationService)
        {
            _configurationService = configurationService;
            UpdateSettings();
            SetUpKeyWord();
            SetUpVoices();
            DeleteSettingCommand = new ViewModelCommand(ExecuteDeleteSettingCommand); 
            SaveCommand = new ViewModelCommand(ExecuteSaveCommand);
        }

        private void ExecuteSaveCommand(object obj)
        {
            var settingsNamesList = Settings.Select(s => s.Name).ToList();
            var settingsNamesHashSet = new HashSet<string>(settingsNamesList);
            if (settingsNamesList.Count == settingsNamesHashSet.Count)
            {
                var settingsConfig = Settings.ToDictionary(s => s.Name, s => s.Value);
                _configurationService.SaveSettingsConfig(settingsConfig);
                //Save Display
            }
            else
            {
                //Error Display
            }
        }
        private void ExecuteDeleteSettingCommand(object obj)
        {
            if(obj is SettingMVVM setting)
            {
                Settings.Remove(setting);
                _configurationService.DeleteSetting(setting.Name);
            }
        }

        private void SetUpKeyWord()
        {
            activationDictionary = _configurationService.GetKeyWords();
            var currentKeyWordSetting = _configurationService.GetSetting("ActivationKeyword");
            KeyValuePair<string, string> currentKeyWord;
            if (!string.IsNullOrEmpty(currentKeyWordSetting))
                currentKeyWord = activationDictionary.FirstOrDefault(k => k.Value == currentKeyWordSetting);
            else
            {
                currentKeyWord = activationDictionary.FirstOrDefault();
                _configurationService.SetSetting("ActivationKeyword", currentKeyWord.Value);
            }
            ActivationKeys = new ObservableCollection<string>(activationDictionary.Keys.ToList());
            keyWordIndex = ActivationKeys.IndexOf(currentKeyWord.Key);
        }

        private void SetUpVoices()
        {
            voicesDictionary = _configurationService.GetVoices();
            var currentVoiceSetting = _configurationService.GetSetting("Voice");
            KeyValuePair<string, string> currentVoice;
            if (!string.IsNullOrEmpty(currentVoiceSetting))
            {
                currentVoice = voicesDictionary.FirstOrDefault(v => v.Value == currentVoiceSetting);
            }
            else
            {
                currentVoice = voicesDictionary.FirstOrDefault();
                _configurationService.SetSetting("Voice", currentVoice.Value);
            }
            Voices = new ObservableCollection<string>(voicesDictionary.Keys.ToList());

        }

        private void UpdateSettings()
        {
            Settings = new ObservableCollection<SettingMVVM>( _configurationService.GetAllSettings().Select(s => new SettingMVVM { Name = s.Key, Value = s.Value }).ToList());
        }
    }
}
