using GalaSoft.MvvmLight;
using kmd.Core.Helpers;
using System.Threading.Tasks;
using Windows.Storage;

namespace kmd.Core.Explorer.States
{
    public class ExplorerViewStateManager : ObservableObject
    {
        private string _explorerTag;

        private bool? _showDisplayType;
        private bool? _showDateCreated;
        private bool? _showAttributes;

        public bool ShowDisplayType
        {
            get => _showDisplayType ?? true;
            set
            {
                Set(ref _showDisplayType, value);
                ApplicationData.Current.LocalSettings.SaveAsync($"{nameof(ShowDisplayType)}{_explorerTag}", value).FireAndForget();
            }
        }

        public bool ShowDateCreated
        {
            get => _showDateCreated ?? true;
            set
            {
                Set(ref _showDateCreated, value);
                ApplicationData.Current.LocalSettings.SaveAsync($"{nameof(ShowDateCreated)}{_explorerTag}", value).FireAndForget();
            }
        }

        public bool ShowAttributes
        {
            get => _showAttributes ?? true;
            set
            {
                Set(ref _showAttributes, value);             
                ApplicationData.Current.LocalSettings.SaveAsync($"{nameof(ShowAttributes)}{_explorerTag}", value).FireAndForget();
            }
        }

        public async Task InitializeAsync(string explorerTag)
        {
            _explorerTag = explorerTag;

            ShowDisplayType = await ApplicationData.Current.LocalSettings.ReadAsync<bool?>($"{nameof(ShowDisplayType)}{_explorerTag}") ?? true;
            ShowDateCreated = await ApplicationData.Current.LocalSettings.ReadAsync<bool?>($"{nameof(ShowDateCreated)}{_explorerTag}") ?? true;
            ShowAttributes = await ApplicationData.Current.LocalSettings.ReadAsync<bool?>($"{nameof(ShowAttributes)}{_explorerTag}") ?? true;
        }        
    }
}
