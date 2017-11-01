using GalaSoft.MvvmLight;
using kmd.Core.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
