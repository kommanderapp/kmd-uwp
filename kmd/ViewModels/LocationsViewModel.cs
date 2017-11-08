using GalaSoft.MvvmLight;
using kmd.Core.Services.Contracts;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;

namespace kmd.ViewModels
{
    public class LocationsViewModel : ViewModelBase
    {
        private ILocationService _locationService;
      
        public LocationsViewModel(ILocationService locationService)
        {
            _locationService = locationService ?? throw new ArgumentNullException(nameof(locationService));
        }

        public ObservableCollection<IStorageFolder> Locations
        {
            get
            {
                return _locations;
            }
            set
            {
                Set(ref _locations, value);
            }
        }

        private ObservableCollection<IStorageFolder> _locations;

        public async Task InitializeAsync()
        {
            var locations = await _locationService.GetLocationsAsync();
            Locations = new ObservableCollection<IStorageFolder>(locations);
        }

        public async Task PickLocationAsync()
        {
            var location = await _locationService.PickLocationAsync();
            if (location != null)
            {
                Locations.Add(location);
            }
        }

        public async Task RemoveLocationAsync(IStorageFolder location)
        {
            await _locationService.RemoveLocationAsync(location);
            Locations.Remove(location);
        }
    }
}
