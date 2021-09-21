using System;
using System.Collections.Generic;
using CMS.Service.ConnectCases;
using Xamarin.Forms.Maps;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using GoogleApi.Entities.Places.Details.Response;
using Library.SfFactory.Service.Models;
using Library.SfFactory.Service;

namespace PalmCoastConnect.Views
{
    public partial class CaseFormSubmit : ContentPage
    {
        private RequestType _RequestType { get; set; }
        private RequestSubType _SubRequest { get; set; }
        private Location _Location { get; set; }
        private PlacesDetailsResponse _CaseAddress { get; set; }
        private string PhotoPath { get; set; }

        public CaseFormSubmit(RequestType request, RequestSubType subrequest, Location location, PlacesDetailsResponse address)
        {
            InitializeComponent();
            _RequestType = request;
            _SubRequest = subrequest;
            _Location = location;
            _CaseAddress = address;
            requestLabel.Text = _RequestType.Title;
            subRequestLabel.Text = _SubRequest.Title;

            if (_SubRequest.Anonymous)
            {
                addressLabel.IsVisible = false;
                SubmitterAddress.IsVisible = false;
            }


            var addressZip = _CaseAddress.Result.AddressComponents;


        }

        private async void onSubmitCase(object sender, EventArgs args)
        {
            SfCase ticket = new SfCase();

            ticket.Status = "New";
            ticket.Subject = _RequestType.Title + " - " + _SubRequest.Title;
            ticket.Guest_Name__c = GuestName.Text;
            ticket.Guest_Phone__c = GuestPhone.Text;
            ticket.SuppliedEmail = SuppliedEmail.Text;
            ticket.Description = ticketDescription.Text;
            ticket.Type = _RequestType.TypeApiName;
            ticket.Sub_Type__c = _SubRequest.SubTypeApiName;
            ticket.Address_Description__c = _CaseAddress.Result.FormattedAddress;
            ticket.Location__c = _CaseAddress.Result.FormattedAddress;


            //Location
            ticket.Street_Address__c = _CaseAddress.Result.Name;
            ticket.City_Address__c = _CaseAddress.Result.Vicinity;

            var addressZip = _CaseAddress.Result.AddressComponents;
            var ZipsFilter = addressZip.Where(f => f.Types.FirstOrDefault().ToString().Contains("Postal_Code"));
            var resultZip = ZipsFilter.ToList().FirstOrDefault().LongName.ToString();
            ticket.Postal_Address__c = resultZip;

            var StateFilter = addressZip.Where(f => f.Types.FirstOrDefault().ToString().Contains("Administrative_Area_Level_1"));
            var resultState = StateFilter.ToList().FirstOrDefault().LongName.ToString();
            ticket.State_address__c = resultState;

            ticket.GeoLocation__latitude__s = _CaseAddress.Result.Geometry.Location.Latitude;
            ticket.GeoLocation__longitude__s = _CaseAddress.Result.Geometry.Location.Longitude;


            if (!_SubRequest.Anonymous)
            {
                ticket.Submitter_Address__c = SubmitterAddress.Text;
            }


            if (_SubRequest.Anonymous)
            {
                ticket.SuppliedEmail = null;
                ticket.Guest_Name__c = null;
                ticket.ContactEmail = null;
                ticket.ContactId = null;
               
            }

            ISfFactoryService _sfServiceClient = new SfFactoryClient();
            var SfUrl = Application.Current.Properties["AWSGatewayUrl"];
            var CreatedCase = await _sfServiceClient.CreateSfCase(SfUrl.ToString(), ticket);

            Console.WriteLine("Submitted!");
        }


        ///Can't Test Through virtual device
        /*
        private async void OnTakePhoto(object sender, EventArgs args)
        {
            Console.WriteLine("PHoto Called");

            await Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    // Code to run on the main thread
                    await TakePhotoAsync();
                });
            });
           
            
        }

        async Task TakePhotoAsync()
        {
           
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync();
                await LoadPhotoAsync(photo);
                Console.WriteLine($"CapturePhotoAsync COMPLETED: {PhotoPath}");
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature is not supported on the device
            }
            catch (PermissionException pEx)
            {
                // Permissions not granted
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CapturePhotoAsync THREW: {ex.Message}");
            }
        }

        async Task LoadPhotoAsync(FileResult photo)
        {
            // canceled
            if (photo == null)
            {
                PhotoPath = null;
                return;
            }
            // save the file into local storage
            var newFile = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
            using (var stream = await photo.OpenReadAsync())
            using (var newStream = File.OpenWrite(newFile))
                await stream.CopyToAsync(newStream);

            PhotoPath = newFile;
        }*/
    }
}
