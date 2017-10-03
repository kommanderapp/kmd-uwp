using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace kmd.Core.Config
{
    public class JsonConfigPersistanceService<T> : IConfigPersistanceService<T> where T : new()
    {
        public async Task<T> LoadAsync()
        {
            var settings = await _root.ReadAsync<T>(_settingsKey);

            if (settings == null)
            {
                settings = new T();
                await SaveAsync(settings);
            }

            return settings;
        }

        public async Task SaveAsync(T settings)
        {
            await _root.SaveAsync(_settingsKey, settings);
        }

        private ApplicationDataContainer _root = ApplicationData.Current.LocalSettings;
        private string _settingsKey = typeof(T).Name;
    }
}