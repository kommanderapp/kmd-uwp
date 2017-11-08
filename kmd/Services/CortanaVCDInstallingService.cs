using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;

namespace kmd.Services
{
    public static class CortanaVCDInstallingService
    {
        public static async Task InstallAsync()
        {
            try
            {
                StorageFile vcdStorageFile = await Package.Current.InstalledLocation.GetFileAsync(@"KommanderVoiceCommands.xml");
                await Windows.ApplicationModel.VoiceCommands.VoiceCommandDefinitionManager.InstallCommandDefinitionsFromStorageFileAsync(vcdStorageFile);
            }
            catch (Exception ex)

            {
                Debug.WriteLine("Installing Voice Commands Failed: " + ex.ToString());
            }
        }
    }
}
