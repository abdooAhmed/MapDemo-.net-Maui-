using System.Collections.ObjectModel;
using Microsoft.Maui.Controls.Maps;
using CommunityToolkit.Mvvm.Input;
using MapDemo.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Net;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace MapDemo.ViewModels
{
    public partial class MapViewModel : BaseViewModel
    {
        [ObservableProperty]
        bool isReady;

        [ObservableProperty]
        string url = "";

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
                location = new Location()
                {
                    Latitude = -37.96977418895127,
                    Longitude = 145.300701806977
                };
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
            var jsonDeserialized = new EntireData();
            using (WebClient wc = new WebClient())
            {
                var json = wc.DownloadString(Url);
                jsonDeserialized = JsonConvert.DeserializeObject<EntireData>(json);
            }
            try
            {
                if(jsonDeserialized != null)
                {
                    foreach(var item in jsonDeserialized.Items)
                    {
                        var data = item.Tag.Split(',');
                        Location loc = new Location()
                        {
                            Latitude = Convert.ToDouble(data[0]),
                            Longitude = Convert.ToDouble(data[1]),
                        };
                        Locations.Add(loc);
                    }
                    BindableLocation = new ObservableCollection<Location>(Locations);
                    IsReady = true;

                }
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
