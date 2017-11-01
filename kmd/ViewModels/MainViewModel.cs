using GalaSoft.MvvmLight;
using kmd.Core.Helpers;
using kmd.Core.Services.Impl;
using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace kmd.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
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

            LoadSecondExplorerTabs = await ApplicationData.Current.LocalSettings.ReadAsync<bool>(nameof(LoadSecondExplorerTabs));
        }

        private LocationAccessService _driveAccessService;
        private StorageFolder _rootFolder;

        private bool _loadSecondExplorerTabs;
        public bool LoadSecondExplorerTabs
        {
            get => _loadSecondExplorerTabs;
            set
            {
                Set(ref _loadSecondExplorerTabs, value);
                ApplicationData.Current.LocalSettings.SaveAsync<bool>(nameof(LoadSecondExplorerTabs), value).FireAndForget();
            }
        }
    }
}
