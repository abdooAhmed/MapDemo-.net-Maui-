using System.Collections.ObjectModel;
using Microsoft.Maui.Controls.Maps;
using CommunityToolkit.Mvvm.Input;
using MapDemo.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MapDemo.ViewModels
{
    public partial class MapViewModel : BaseViewModel
    {
        [ObservableProperty]
        bool isReady;

        [ObservableProperty]
        double longitude = 0;
        [ObservableProperty]
        double latitude = 0;

        [ObservableProperty]
        ObservableCollection<Place> bindablePlaces;

        [ObservableProperty]
        ObservableCollection<Location> bindableLocation;

        public ObservableCollection<Place> Places { get; } = new();

        public ObservableCollection<Location> Locations { get; } = new();

        private CancellationTokenSource cts;
        private IGeolocation geolocation;
        private IGeocoding geocoding;

        public MapViewModel(IGeolocation geolocation, IGeocoding geocoding)
        {
            this.geolocation = geolocation;
            this.geocoding = geocoding;
        }

        [RelayCommand]
        private async Task GetCurrentLocationAsync()
        {
            try
            {
                cts = new CancellationTokenSource();

                var request = new GeolocationRequest(
                    GeolocationAccuracy.Medium,
                    TimeSpan.FromSeconds(10));

                var location = await Geolocation.GetLocationAsync(request, cts.Token);
                var placemarks = await Geocoding.GetPlacemarksAsync(location);
                var address = placemarks?.FirstOrDefault()?.AdminArea;

                Places.Clear();
                var place = new Place()
                {
                    Location = location,
                    Address = address,
                    Description = "Current Location"
                };

                Places.Add(place);
                var placeList = new List<Place>() { place };
                BindablePlaces = new ObservableCollection<Place>(placeList);
                IsReady = true;
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
        }

        [RelayCommand]
        private async Task AddLocation()
        {
            try
            {
                Location loc = new Location()
                {
                    Latitude = this.Latitude,
                    Longitude = this.Longitude,
                };
                Locations.Add(loc);
                BindableLocation = new ObservableCollection<Location>(Locations);
                IsReady = true;
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
        }

        [RelayCommand]
        private void DisposeCancellationToken()
        {
            if (cts != null && !cts.IsCancellationRequested)
                cts.Cancel();
        }
    }
}
