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
using System.Threading;
using GoogleApi.Entities.Places.Details.Request;
using GoogleApi.Entities.Places.Details.Response;
using GoogleApi.Entities.Places.AutoComplete.Response;
using System.Threading.Tasks;

namespace PalmCoastConnect.Views
{
    public partial class CaseCreation : ContentPage
    {
        private RequestType _RequestType { get; set; }
        private RequestSubType _SubRequest { get; set; }
        private PlacesAutoCompleteResponse autoComplete {get; set;}
        private PlacesDetailsResponse placesDetail { get; set; }


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

            OnAlertCaseCreate();


            var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
        }

        private async Task GetPlaceDetails(string address)
        {
            var GMapApiKey = Application.Current.Properties["GoogleMapApi"];
            var request = new PlacesAutoCompleteRequest
            {
                Key = App.ApiKey,
                Input = address,
                Types = new List<RestrictPlaceType> { RestrictPlaceType.Address },

            };

            autoComplete = await GooglePlaces.AutoComplete.QueryAsync(request);

            
            var request2 = new PlacesDetailsRequest
            {
                Key = GMapApiKey.ToString(),
                PlaceId = autoComplete.Predictions.Select(x => x.PlaceId).FirstOrDefault(),

            };

            placesDetail = await GooglePlaces.Details.QueryAsync(request2);
        }


        async void OnAlertCaseCreate()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if(status != PermissionStatus.Granted)
            {
                return;
            }
            bool answer = await DisplayAlert("Choose Case Location", "Would you like to use current location?", "Yes", "No");
            if(answer)
            {
                var location = await Geolocation.GetLastKnownLocationAsync();
               

                //get address name by coordinates
                Geocoder geoCoder = new Geocoder();
             
                Position position = new Position(location.Latitude, location.Longitude);
                IEnumerable<string> possibleAddresses = await geoCoder.GetAddressesForPositionAsync(position);
               
                string address = possibleAddresses.FirstOrDefault();

                //Google Api to get Place Details
                await GetPlaceDetails(address);

              

                Pin pin = new Pin
                {
                    Label = address,
                    Type = PinType.Place,
                    Position = new Position(location.Latitude, location.Longitude)
                };

                map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(location.Latitude, location.Longitude), Distance.FromMiles(1)));
                map.Pins.Add(pin);
                
                AddressSearch.Text = address;
               
                CaseFormsPage(this._RequestType, this._SubRequest, location, placesDetail);


            }
        }


        async void CaseFormsPage(RequestType request, RequestSubType subrequest, Location location, PlacesDetailsResponse caseAddress)
        {
            
            
            if (Navigation.NavigationStack.Count == 0 ||
                     Navigation.NavigationStack[Navigation.NavigationStack.Count - 1].GetType() != typeof(CaseFormSubmit))
            {


                await Navigation.PushAsync(new CaseFormSubmit(request, subrequest, location, caseAddress));
            }
        }

       async void OnMapClicked(object sender, MapClickedEventArgs e)
        {
           
            Pin pin = new Pin
            {
                Label = "Connect Case",
                Type = PinType.Place,
                Position = new Position(e.Position.Latitude, e.Position.Longitude)
            };
            map.Pins.Add(pin);



            Location location = new Location { Latitude = e.Position.Latitude, Longitude = e.Position.Longitude };
            Geocoder geoCoder = new Geocoder();

            Position position = new Position(location.Latitude, location.Longitude);
            IEnumerable<string> possibleAddresses = await geoCoder.GetAddressesForPositionAsync(position);

            string address = possibleAddresses.FirstOrDefault();
            await GetPlaceDetails(address);
        

            CaseFormsPage(this._RequestType, this._SubRequest, location, placesDetail);
        }

        private async void OnTextChanged(object sender, EventArgs eventArgs)
        {
            var textEntry = (SearchBar)sender;
           
            if (textEntry.Text.Length >= 3)
            {
                addressList.IsVisible = true;

                await GetPlaceDetails(textEntry.Text);
                ObservableCollection<Prediction> predictions = new ObservableCollection<Prediction>(this.autoComplete.Predictions);
               
                addressList.ItemsSource = predictions;
                
               
            }
            else
            {
               

                addressList.ItemsSource = new ObservableCollection<Prediction>();
                addressList.IsVisible = false;
            }
            
        }
        private async void OnAddressSelect (object sender, EventArgs args)
        {
            var selectedItem = (TextCell)sender;
            AddressSearch.Text = selectedItem.Text;

            Geocoder geoCoder = new Geocoder();
            IEnumerable<Position> approximateLocations = await geoCoder.GetPositionsForAddressAsync(selectedItem.Text);
            Position position = approximateLocations.FirstOrDefault();
            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromMiles(1)));


            addressList.ItemsSource = new ObservableCollection<Prediction>();
            addressList.IsVisible = false;

        }

        //Maybe unecessary
        private async void VerifyAddress(object sender, EventArgs args)
        { 

            Geocoder geoCoder = new Geocoder();
            IEnumerable<Position> approximateLocations = await geoCoder.GetPositionsForAddressAsync(AddressSearch.Text);
            Position position = approximateLocations.FirstOrDefault();
          
            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromMiles(1)));
            Pin pin = new Pin
            {
                Label = AddressSearch.Text,
                Type = PinType.Place,
                Position = new Position(position.Latitude, position.Longitude)
            };
            Location location = new Location { Latitude = position.Latitude, Longitude = position.Longitude };
            map.Pins.Add(pin);

            //Google Api to get Place Details
            await GetPlaceDetails(AddressSearch.Text);



            CaseFormsPage(this._RequestType, this._SubRequest, location, placesDetail);
        }
    }
}
