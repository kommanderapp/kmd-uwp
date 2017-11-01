using kmd.Core.Helpers;
using kmd.Core.Services.Contracts;
using kmd.Storage.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;

namespace kmd.Core.Services.Impl
{
    public class LocationService : ILocationService
    {
        private const string _settingsKey = "LocationTokens";
        private IFolderPickerService _folderPickerService;

        public LocationService(IFolderPickerService folderPickerService)
        {
            _folderPickerService = folderPickerService ?? throw new ArgumentNullException(nameof(folderPickerService));
        }

        public async Task<IEnumerable<IStorageFolder>> GetLocationsAsync()
        {
            var locationTokens = await GetLocationTokensAsync();
            var locations = new List<IStorageFolder>();
            foreach (var lt in locationTokens)
            {
                var location = await GetFoldeFromFalAsync(lt.Token);
                if (location != null)
                {
                    locations.Add(location);
                }
            }

            // TODO fix this shit
            if (!locations.Any())
            {
                var music = await StorageLibrary.GetLibraryAsync(KnownLibraryId.Music);
                await AddLocationAsync(music.SaveFolder);
                var pictures = await StorageLibrary.GetLibraryAsync(KnownLibraryId.Pictures);
                await AddLocationAsync(pictures.SaveFolder);
                var videos = await StorageLibrary.GetLibraryAsync(KnownLibraryId.Videos);
                await AddLocationAsync(videos.SaveFolder);
            }

            return locations;
        }

        public async Task<IStorageFolder> PickLocationAsync()
        {
            var folder = await _folderPickerService.PickSingleAsync();
            await AddLocationAsync(folder);

            return folder;
        }

        public async Task AddLocationAsync(IStorageFolder folder)
        {
            if (folder != null)
            {
                var token = await AddFolderToFALAsync(folder);
                await AddLocationTokenToSettings(folder.Path, token);
            }
        }

        public async Task RemoveLocationAsync(IStorageFolder folder)
        {
            if (folder == null) throw new ArgumentNullException(nameof(folder));
            await RemoveLocationTokenFromSettings(folder.Path);
        }

        private async Task<IEnumerable<LocationToken>> GetLocationTokensAsync()
        {
            var driveTokens = await ApplicationData.Current.LocalSettings.ReadAsync<List<LocationToken>>(_settingsKey);
            if (driveTokens == null)
            {
                driveTokens = new List<LocationToken>();
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

        private async Task<string> AddFolderToFALAsync(IStorageFolder folder)
        {
            string token = null;
            if (folder != null)
            {
                var fal = StorageApplicationPermissions.FutureAccessList;
                token = fal.Add(folder);
            }
            return await Task.FromResult(token);
        }

        private async Task AddLocationTokenToSettings(string path, string token)
        {
            var oldLocationTokens = await GetLocationTokensAsync();
            var locationTokens = new List<LocationToken>(oldLocationTokens)
            {
                new LocationToken(path, token)
            };

            await ApplicationData.Current.LocalSettings.SaveAsync(_settingsKey, locationTokens);
        }

        private async Task RemoveLocationTokenFromSettings(string path)
        {
            var oldLocationTokens = await GetLocationTokensAsync();
            var locationTokens = new List<LocationToken>(oldLocationTokens);
            var tokenToRemove = locationTokens.Where(x => x.Path == path).FirstOrDefault();
            if (tokenToRemove != null)
            {
                locationTokens.Remove(tokenToRemove);
                await ApplicationData.Current.LocalSettings.SaveAsync(_settingsKey, locationTokens);
            }
        }
    }

    public class LocationToken
    {
        public LocationToken(string path, string token)
        {
            Path = path;
            Token = token;
        }

        public string Path { get; }
        public string Token { get; }
    }
}
