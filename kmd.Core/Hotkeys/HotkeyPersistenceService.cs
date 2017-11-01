using kmd.Core.Explorer.Commands.Configuration;
using kmd.Core.Helpers;
using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace kmd.Core.Hotkeys
{
    public class HotkeyPersistenceService
    {
        public static Hotkey GetPrefferedHotkeyAsync(ExplorerCommandAttribute attribute)
        {
            var defaultHotkey = attribute.DefaultHotkey;
            if (defaultHotkey == null) return null;

            var hotkeySettingKey = attribute.Name ?? throw new Exception($"Can't resolve the hotkey setting for {attribute.Name}, because the property {attribute.GetType().GetProperty(nameof(attribute.DefaultHotkey))} was not found.");
            var hotkeySetting = ApplicationData.Current.LocalSettings.Read<Hotkey>(hotkeySettingKey);

            if (hotkeySetting != default(Hotkey))
            {
                return hotkeySetting;
            }
            else
            {
                ApplicationData.Current.LocalSettings.Save(hotkeySettingKey, defaultHotkey);
                return hotkeySetting;
            }
        }

        public static async Task SetPrefferedHotkeyAsync(string name, Hotkey hotkey)
        {
            await ApplicationData.Current.LocalSettings.SaveAsync(name, hotkey);          
        }
    }
}
