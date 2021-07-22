using System;
using System.Collections.Generic;
using CMS.Service.ConnectCases;
using Xamarin.Forms.Maps;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Linq;
using GoogleApi.Entities.Places.AutoComplete.Request;
using GoogleApi;
using GoogleApi.Entities.Places.AutoComplete.Request.Enums;
using System.Collections.ObjectModel;
using GoogleApi.Entities.Places.Common;

namespace PalmCoastConnect.Views
{
    public partial class CaseCreation : ContentPage
    {
        private RequestType _RequestType { get; set; }
        private RequestSubType _SubRequest {get; set;}
        public CaseCreation(RequestType request, RequestSubType subrequest)
        {
            InitializeComponent();
            _RequestType = request;
            _SubRequest = subrequest;
            addressList.IsVisible = false;


        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var location = await Geolocation.GetLastKnownLocationAsync();
            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(location.Latitude, location.Longitude), Distance.FromMiles(1)));

            


        }
        void OnMapClicked(object sender, MapClickedEventArgs e)
        {
            Console.WriteLine($"MapClick: {e.Position.Latitude}, {e.Position.Longitude}");
            Pin pin = new Pin
            {
                Label = "Location 1",
                Type = PinType.Place,
                Position = new Position(e.Position.Latitude, e.Position.Longitude)
            };
           // map.Pins.Add(pin);
        }
        private async void OnTextChanged(object sender, EventArgs eventArgs)
        {
            var textEntry = (SearchBar)sender;
           
            if (textEntry.Text.Length >= 3)
            {
                addressList.IsVisible = true;
                var request = new PlacesAutoCompleteRequest
                {
                    Key = App.ApiKey,
                    Input = textEntry.Text,
                    Types = new List<RestrictPlaceType> { RestrictPlaceType.Address }
                };

                var response = await GooglePlaces.AutoComplete.QueryAsync(request);
                ObservableCollection<Prediction> predictions = new ObservableCollection<Prediction>(response.Predictions);
                addressList.ItemsSource = predictions;
               
            }
            else
            {
               

                addressList.ItemsSource = new ObservableCollection<Prediction>();
                addressList.IsVisible = false;
            }
            
        }
        private void OnAddressSelect (object sender, EventArgs args)
        {
            var selectedItem = (TextCell)sender;
            AddressSearch.Text = selectedItem.Text;

            addressList.ItemsSource = new ObservableCollection<Prediction>();
            addressList.IsVisible = false;
        }
        private async void VerifyAddress(object sender, EventArgs args)
        { 

            Geocoder geoCoder = new Geocoder();
            IEnumerable<Position> approximateLocations = await geoCoder.GetPositionsForAddressAsync(AddressSearch.Text);
            Position position = approximateLocations.FirstOrDefault();
          
            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromMiles(0.4)));
            Pin pin = new Pin
            {
                Label = AddressSearch.Text,
                Type = PinType.SearchResult,
                Position = new Position(position.Latitude, position.Longitude)
            };
            map.Pins.Add(pin);
        }
    }
}
