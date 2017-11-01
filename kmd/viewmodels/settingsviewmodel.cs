using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using kmd.Core.Explorer.Commands.Configuration;
using kmd.Core.Hotkeys;
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
        public ObservableCollection<HotkeySettingDto> HotkeySettings { get; set; }

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

        private ObservableCollection<HotkeySettingDto> GetHotkeySettings()
        {
            var hotkeySettings = new List<HotkeySettingDto>();
            var commandDescriptors = ExplorerCommandBindingsProvider.ExplorerCommandDescriptors.Where(x=> x.PreferredHotkey != null);
            foreach (var commandDescriptor in commandDescriptors)
            {
                hotkeySettings.Add(HotkeySettingDto.From(commandDescriptor));
            }

            return new ObservableCollection<HotkeySettingDto>(hotkeySettings);
        }

        public async Task SaveChangesAsync()
        {
            foreach (var hotkeySetting in HotkeySettings)
            {
                await HotkeyPersistenceService.SetPrefferedHotkeyAsync(hotkeySetting.Name, Hotkey.For(hotkeySetting.ModifierKey, hotkeySetting.Key));
            }

            await ExplorerCommandBindingsProvider.RefreshCommandBindingsAsync();
        }
    }

    public class HotkeySettingDto : ObservableObject
    {
        private string _name;
        public string Name { get => _name; set => Set(ref _name, value); }
        private string _description;
        public string Description { get => _description; set => Set(ref _description, value); }

        private VirtualKey _key;
        public VirtualKey Key
        {
            get => _key;
            set
            {
                if (_key == value) return;
                var matchingCommand = ExplorerCommandBindingsProvider.ExplorerCommandDescriptors.Where(x => x.Attribute.Name != Name && x.PreferredHotkey != null).FirstOrDefault(y => y.PreferredHotkey == Hotkey.For(ModifierKey, value));

                if (matchingCommand !=null)
                {
                    var dialogService = new DialogService();
                    dialogService.ShowError($"This combination is reserved for {matchingCommand.Attribute.ShortcutText}", "Warning", "OK", () => { return; });
                }

                Set(ref _key, value);
            }
        }

        private ModifierKeys _modifierKey;
        public ModifierKeys ModifierKey
        {
            get => _modifierKey;
            set
            {
                if (_modifierKey == value) return;
                var matchingCommand = ExplorerCommandBindingsProvider.ExplorerCommandDescriptors.Where(x => x.Attribute.Name != Name && x.PreferredHotkey != null).FirstOrDefault(y => y.PreferredHotkey == Hotkey.For(value, Key));
                if (matchingCommand != null)
                {
                    var dialogService = new DialogService();
                    dialogService.ShowError($"This combination is reserved for {matchingCommand.Attribute.ShortcutText}", "Warning", "OK", ()=> { return; });                    
                }

                Set(ref _modifierKey, value);
            }
        }

        public static HotkeySettingDto From(ExplorerCommandDescriptor commandDescriptor)
        {
            return new HotkeySettingDto
            {
                 Description = commandDescriptor.Attribute.ShortcutText,
                 Name = commandDescriptor.Attribute.Name,
                 Key = commandDescriptor.PreferredHotkey.Key,
                 ModifierKey = commandDescriptor.PreferredHotkey.ModifierKey
            };
        }       

        private HotkeySettingDto()
        {

        }
    }
}
