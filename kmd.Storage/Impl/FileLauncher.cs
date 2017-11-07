using kmd.Storage.Contracts;
using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.System;

namespace kmd.Storage.Impl
{
    public class FileLauncher : IFileLauncher
    {
        public async Task LaunchAsync(IStorageFile file, bool displayApplicationPicker = false)
        {
            var result = await Launcher.LaunchFileAsync(file, new LauncherOptions
            {
                TreatAsUntrusted = false,
                DisplayApplicationPicker = displayApplicationPicker
            });
        }
    }
}
