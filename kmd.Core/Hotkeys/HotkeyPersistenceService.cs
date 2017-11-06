using kmd.Core.Command.Configuration;
using kmd.Core.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace kmd.Core.Hotkeys
{
    public class HotkeyPersistenceService
    {
        public static void SetPrefferedHotkey(CommandDescriptor descriptor)
        {
            var defaultHotkey = descriptor.DefaultHotkey;
            if (defaultHotkey == null) return;

            var hotkeySettingKey = descriptor.UniqueName ?? throw new Exception($"Can't resolve the hotkey setting for {descriptor.UniqueName}.");
            var hotkeySetting = ApplicationData.Current.LocalSettings.Read<Hotkey>(hotkeySettingKey);

            if (hotkeySetting == default(Hotkey))
            {
                ApplicationData.Current.LocalSettings.Save(hotkeySettingKey, defaultHotkey);
            }
            descriptor.PreferredHotkey = hotkeySetting;
        }

        public static async Task ConfigPrefferedHotkeyAsync(string name, Hotkey hotkey)
        {
            await ApplicationData.Current.LocalSettings.SaveAsync(name, hotkey);
        }

        public static async Task ResetToDefaultsAsync()
        {
            var commandDescriptors = CommandDescriptorProvider.GetCommandDescriptors().Where(x => x.HasHotkey);
            foreach (var commandDescriptor in commandDescriptors)
            {
                await ConfigPrefferedHotkeyAsync(commandDescriptor.UniqueName, commandDescriptor.DefaultHotkey);
            }
        }
    }
}
