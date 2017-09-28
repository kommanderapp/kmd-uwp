using System;
using GalaSoft.MvvmLight;
using Windows.Storage;
using System.Threading.Tasks;
using kdm.Core.Services.Impl;

namespace kmd.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private StorageFolder _rootFolder;

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

        private LocationAccessService _driveAccessService;

        public MainViewModel(LocationAccessService driveAccessService)
        {
            _driveAccessService = driveAccessService ?? throw new ArgumentNullException(nameof(driveAccessService));
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
    }
}
