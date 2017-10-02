using kmd.Core.Services.Contracts;
using kmd.Core.Explorer.Contracts;
using kmd.Core.Explorer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Storage;

namespace kmd.Core.Services.Impl
{
    public class ExplorerItemMapper : IExplorerItemMapper
    {
        public async Task<ObservableCollection<IExplorerItem>> MapAsync(IEnumerable<IStorageItem2> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            var result = new ObservableCollection<IExplorerItem>();
            foreach (var item in items)
            {
                var explorerItem = await ExplorerItem.CreateAsync(item);
                result.Add(explorerItem);
            }
            return result;
        }
    }
}