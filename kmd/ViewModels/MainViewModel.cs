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

        public async Task InitializeAsync()
        {
            LoadSecondExplorerTabs = await ApplicationData.Current.LocalSettings.ReadAsync<bool>(nameof(LoadSecondExplorerTabs));
        }

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
