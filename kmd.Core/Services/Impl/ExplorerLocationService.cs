using kmd.Core.Config;
using kmd.Core.Helpers;
using kmd.Storage.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;

namespace kmd.Core.Services.Impl
{
    public class DriveToken
    {
        public DriveToken(string drive, string token)
        {
            Drive = drive;
            Token = token;
        }

        public string Drive { get; }
        public string Token { get; }
    }

    // TODO refactor
    public class LocationAccessService
    {
        public LocationAccessService(IFolderPickerService folderPickerService)
        {
            _folderPickerService = folderPickerService ?? throw new ArgumentNullException(nameof(folderPickerService));
        }

        public async Task<IStorageFolder> GetDefaultDriveAsync()
        {
            var tokens = await GetDriveTokensSettingAsync();
            var driveToken = tokens.FirstOrDefault();
            if (driveToken == null) return null;
            var folder = await GetFoldeFromFalAsync(driveToken.Token);
            return folder;
        }

        public async Task<IStorageFolder> GetDriveAsync(string drive)
        {
            var tokens = await GetDriveTokensSettingAsync();
            var driveToken = tokens.FirstOrDefault(x => x.Drive == drive);
            var folder = await GetFoldeFromFalAsync(driveToken.Token);
            return folder;
        }

        public async Task<IEnumerable<IStorageFolder>> GetDrivesAsync()
        {
            var tokens = await GetDriveTokensSettingAsync();
            var folders = new List<IStorageFolder>();
            foreach (var token in tokens)
            {
                var folder = await GetFoldeFromFalAsync(token.Token);
                if (folder != null) folders.Add(folder);
            }

            return folders;
        }

        public async Task<IStorageFolder> PickDriveAsync()
        {
            var driveFolder = await _folderPickerService.PickSingleAsync();
            var parent = await (driveFolder as IStorageItem2).GetParentAsync();
            if (parent != null)
            {
                // Not root drive picked
                return null;
            }
            if (driveFolder != null)
            {
                var token = await AddFolderToFalAsync(driveFolder);
                await AddDriveTokenSettingAsync(driveFolder.Name, token);
            }

            return driveFolder;
        }

        private const string _settingsKey = "LocationTokens";
        private IFolderPickerService _folderPickerService;

        private async Task AddDriveTokenSettingAsync(string drive, string token)
        {
            var driveTokens = await GetDriveTokensSettingAsync();
            var driveTokensList = new List<DriveToken>(driveTokens)
            {
                new DriveToken(drive, token)
            };

            await ApplicationData.Current.LocalSettings.SaveAsync(_settingsKey, driveTokensList);
        }

        private async Task<string> AddFolderToFalAsync(IStorageFolder folder)
        {
            string token = null;
            if (folder != null)
            {
                var fal = StorageApplicationPermissions.FutureAccessList;
                token = fal.Add(folder);
            }
            return await Task.FromResult(token);
        }

        private async Task<IEnumerable<DriveToken>> GetDriveTokensSettingAsync()
        {
            var driveTokens = await ApplicationData.Current.LocalSettings.ReadAsync<List<DriveToken>>(_settingsKey);
            if (driveTokens == null)
            {
                driveTokens = new List<DriveToken>();
            }
            return driveTokens;
        }

        private async Task<IStorageFolder> GetFoldeFromFalAsync(string token)
        {
            IStorageFolder storageItem = null;
            var fal = StorageApplicationPermissions.FutureAccessList;
            if (token != null)
            {
                storageItem = await fal.GetFolderAsync(token);
            }
            return storageItem;
        }
    }
}