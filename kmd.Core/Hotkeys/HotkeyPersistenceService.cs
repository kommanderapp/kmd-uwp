using kmd.Core.Explorer.Commands.Configuration;
using kmd.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace kmd.Core.Hotkeys
{
    public class HotkeyPersistenceService
    {
        public static async Task<Hotkey> GetPrefferedHotkeyAsync(ExplorerCommandAttribute attribute)
        {
            return await Task.Run(async () =>
            {
                var defaultHotkey = attribute.DefaultHotkey;
                if (defaultHotkey == null) return null;

                var hotkeySettingKey = attribute.Name ?? throw new Exception($"Can't resolve the hotkey setting for {attribute.Name}, because the property {attribute.GetType().GetProperty(nameof(attribute.DefaultHotkey))} was not found.");
                var hotkeySetting = await ApplicationData.Current.LocalSettings.ReadAsync<Hotkey>(hotkeySettingKey);

                if (hotkeySetting != default(Hotkey))
                {
                    return hotkeySetting;
                }
                else
                {
                    await ApplicationData.Current.LocalSettings.SaveAsync(hotkeySettingKey, defaultHotkey);
                    return hotkeySetting;
                }
            });
        }

        public static async Task SetPrefferedHotkeyAsync(string name, Hotkey hotkey)
        {
            await ApplicationData.Current.LocalSettings.SaveAsync(name, hotkey);          
        }
    }
}
