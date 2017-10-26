using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using kmd.Core.Explorer.Commands.Configuration;
using kmd.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel;
using Windows.UI.Xaml;

namespace kmd.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private ObservableCollection<ExplorerCommandDescriptor> _explorerCommandDescriptors;
        public ObservableCollection<ExplorerCommandDescriptor> ExplorerCommandDescriptors
        {
            get => _explorerCommandDescriptors;
            set
            {
                if(value != _explorerCommandDescriptors)
                {

                }
            }
        }

        private ElementTheme _elementTheme = ThemeSelectorService.Theme;

        public ElementTheme ElementTheme
        {
            get { return _elementTheme; }

            set { Set(ref _elementTheme, value); }
        }

        private string _versionDescription;

        public string VersionDescription
        {
            get { return _versionDescription; }

            set { Set(ref _versionDescription, value); }
        }

        private ICommand _switchThemeCommand;

        public ICommand SwitchThemeCommand
        {
            get
            {
                if (_switchThemeCommand == null)
                {
                    _switchThemeCommand = new RelayCommand<ElementTheme>(
                        async (param) =>
                        {
                            await ThemeSelectorService.SetThemeAsync(param);
                        });
                }

                return _switchThemeCommand;
            }
        }

        public SettingsViewModel()
        {
        }

        public void Initialize()
        {
            VersionDescription = GetVersionDescription();
        }

        private string GetVersionDescription()
        {
            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;

            return $"{package.DisplayName} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }

        private bool _hasInstanceBeenInitialized = false;

        public async Task EnsureInstanceInitializedAsync()
        {
            if (!_hasInstanceBeenInitialized)
            {
              

                Initialize();

                _hasInstanceBeenInitialized = true;
            }
        }
    }
}
