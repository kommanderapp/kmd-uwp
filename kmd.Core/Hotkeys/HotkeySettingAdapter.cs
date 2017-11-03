using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using kmd.Core.Explorer.Commands.Configuration;
using System.Linq;
using Windows.System;

namespace kmd.Core.Hotkeys
{
    public class HotkeySettingAdapter : ObservableObject
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

                if (value == VirtualKey.None)
                {
                    value = _key;
                    RaisePropertyChanged();
                }

                var matchingCommand = ExplorerCommandBindingsProvider.ExplorerCommandDescriptors.Where(x => x.Attribute.Name != Name && x.PreferredHotkey != null).FirstOrDefault(y => y.PreferredHotkey == Hotkey.For(ModifierKey, value));
                if (matchingCommand != null)
                {
                    ShowWarningDialog(Description, matchingCommand.Attribute.ShortcutText);
                    RaisePropertyChanged();
                    return;
                }

                Set(ref _key, value);
            }
        }

        public string KeyString
        {
            get
            {
                return Key.ToStringRepresentation();
            }
        }

        public bool HasModifierKey
        {
            get => ModifierKey != ModifierKeys.None;
        }

        private static void ShowWarningDialog(string currentCommandText, string matchingCommandText)
        {
            var dialogService = new DialogService();
            dialogService.ShowError($"The combination you are trying to use for {currentCommandText} is in use for {matchingCommandText}", "Warning", "OK", () => { return; });
        }

        private ModifierKeys _modifierKey;
        public ModifierKeys ModifierKey
        {
            get => _modifierKey;
            set
            {
                if (_modifierKey == value || (value == ModifierKeys.None && Key == VirtualKey.None)) return;

                var matchingCommand = ExplorerCommandBindingsProvider.ExplorerCommandDescriptors.Where(x => x.Attribute.Name != Name && x.PreferredHotkey != null).FirstOrDefault(y => y.PreferredHotkey == Hotkey.For(value, Key));
                if (matchingCommand != null)
                {
                    ShowWarningDialog(Description, matchingCommand.Attribute.ShortcutText);
                    RaisePropertyChanged();
                    return;
                }

                Set(ref _modifierKey, value);
            }
        }

        public static HotkeySettingAdapter From(ExplorerCommandDescriptor commandDescriptor)
        {

            return new HotkeySettingAdapter
            {
                Description = commandDescriptor.Attribute.ShortcutText,
                Name = commandDescriptor.Attribute.Name,
                _key = commandDescriptor.PreferredHotkey.Key,
                _modifierKey = commandDescriptor.PreferredHotkey.ModifierKey
            };
        }

        private HotkeySettingAdapter()
        {
            this.PropertyChanged += HotkeySettingAdapter_PropertyChanged;
        }

        private void HotkeySettingAdapter_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Key)) RaisePropertyChanged(nameof(KeyString));
            else if (e.PropertyName == nameof(ModifierKey)) RaisePropertyChanged(nameof(HasModifierKey));
        }
    }
}
