using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Windows.Storage;
using kmd.Core.Explorer.Commands.Configuration;
using kmd.Core.Helpers;
using kmd.Core.Hotkeys;

namespace kmd.Services
{
    public static class HotkeyPersistenceService
    {
        public static async Task InitializeAsync()
        {
            var explorerCommandDescriptors = ExplorerCommandBindingsProvider.GetExplorerCommandDescriptors();

            foreach (var explorerCommandDescriptor in explorerCommandDescriptors)
            {
                var defaultHotkey = explorerCommandDescriptor.DefaultHotkey;
                if (defaultHotkey == null) continue;

                var hotkeySettingKey = explorerCommandDescriptor.Attribute.Name ?? throw new Exception($"Can't resolve the hotkey setting for {explorerCommandDescriptor.GetType().FullName}, because the property {explorerCommandDescriptor.Attribute.GetType().GetProperty(nameof(explorerCommandDescriptor.Attribute.Name))} was not found.");

                var hotkeySetting = await ApplicationData.Current.LocalSettings.ReadAsync<Hotkey>(hotkeySettingKey);

                if (hotkeySetting != default(Hotkey))
                {
                    SetPrefferedHotkey(explorerCommandDescriptor, hotkeySetting);
                }
                else
                {
                    await ApplicationData.Current.LocalSettings.SaveAsync(hotkeySettingKey, defaultHotkey);
                    SetPrefferedHotkey(explorerCommandDescriptor, defaultHotkey);
                }
            }
        }

        private static void SetPrefferedHotkey(ExplorerCommandDescriptor explorerCommandDescriptor,
            Hotkey hotkeySetting)
        {
            explorerCommandDescriptor.PreferredHotkey = hotkeySetting;
        }
    }
}
