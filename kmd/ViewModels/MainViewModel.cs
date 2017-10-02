using GalaSoft.MvvmLight;
using kmd.Core.Services.Impl;
using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace kmd.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel(LocationAccessService driveAccessService)
        {
            _driveAccessService = driveAccessService ?? throw new ArgumentNullException(nameof(driveAccessService));
        }

        public StorageFolder RootFolder
        {
            get
            {
                return _rootFolder;
            }
            set
            {
                Set(ref _rootFolder, value);
            }
        }

        public async Task InitializeAsync()
        {
            IStorageFolder drive = null;
            drive = await _driveAccessService.GetDefaultDriveAsync();
            if (drive == null)
            {
                drive = await _driveAccessService.PickDriveAsync();
            }
            RootFolder = drive as StorageFolder;
        }

        private LocationAccessService _driveAccessService;
        private StorageFolder _rootFolder;
    }
}
