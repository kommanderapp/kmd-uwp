using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using kmd.Core.Command.Configuration;
using kmd.Core.Hotkeys;
using kmd.Helpers;
using kmd.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel;
using Windows.System;
using Windows.UI.Xaml;

namespace kmd.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        public ObservableCollection<HotkeySettingAdapter> HotkeySettings { get; set; }

        private ElementTheme _elementTheme = ThemeSelectorService.Theme;

        public ElementTheme ElementTheme
        {
            get { return _elementTheme; }

            set { Set(ref _elementTheme, value); }
        }

        private string _versionDescription;

        public string VersionDescription
        {
            get { return _versionDescription; }

            set { Set(ref _versionDescription, value); }
        }

        private ICommand _switchThemeCommand;

        public ICommand SwitchThemeCommand
        {
            get
            {
                if (_switchThemeCommand == null)
                {
                    _switchThemeCommand = new RelayCommand<ElementTheme>(
                        async (param) =>
                        {
                            await ThemeSelectorService.SetThemeAsync(param);                            
                        });
                }

                return _switchThemeCommand;
            }
        }

        private ICommand _resetDefaultHotkeysCommand;

        public ICommand ResetDefaultHotkeysCommand
        {
            get
            {
                if (_resetDefaultHotkeysCommand == null)
                {
                    _resetDefaultHotkeysCommand = new RelayCommand(
                        async () =>
                        {
                            await HotkeyPersistenceService.ResetToDefaultsAsync();
                            _hasInstanceBeenInitialized = false;
                            EnsureInitialized();
                            RaisePropertyChanged(nameof(HotkeySettings));
                        });
                }

                return _resetDefaultHotkeysCommand;
            }
        }

        public SettingsViewModel()
        {            
        }

        private bool _hasInstanceBeenInitialized = false;

        public void EnsureInitialized()
        {
            if (!_hasInstanceBeenInitialized)
            {
                HotkeySettings = GetHotkeySettings();

                VersionDescription = GetVersionDescription();

                _hasInstanceBeenInitialized = true;
            }
        }

        private string GetVersionDescription()
        {
            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;

            return $"{package.DisplayName} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }

        private ObservableCollection<HotkeySettingAdapter> GetHotkeySettings()
        {
            var hotkeySettings = new List<HotkeySettingAdapter>();
            var commandDescriptors = CommandDescriptorProvider.GetCommandDescriptors().Where(x => x.HasHotkey);
            foreach (var commandDescriptor in commandDescriptors)
            {
                var hotkeySettingAdapter = HotkeySettingAdapter.From(commandDescriptor);
                hotkeySettings.Add(hotkeySettingAdapter);
                hotkeySettingAdapter.PropertyChanged += HotkeySettingAdapter_PropertyChanged;
            }

            return new ObservableCollection<HotkeySettingAdapter>(hotkeySettings);
        }

        private async void HotkeySettingAdapter_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            await SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            foreach (var hotkeySetting in HotkeySettings)
            {
                await HotkeyPersistenceService.ConfigPrefferedHotkeyAsync(hotkeySetting.Name, Hotkey.For(hotkeySetting.ModifierKey, hotkeySetting.Key));
            }
        }
    }
}
