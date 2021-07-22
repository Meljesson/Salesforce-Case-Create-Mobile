using System;
using System.Collections.Generic;

using Xamarin.Forms;
using PalmCoastConnect.Models;
using CMS.Service.ConnectCases;
namespace PalmCoastConnect.Views
{
    public partial class CaseRequestSubType : ContentPage
    {
        private RequestType _requestType { get; set; }
        private Dictionary<string, RequestSubType> RequestSubMap { get; set; }

        public CaseRequestSubType(RequestType requestType)
        {
            InitializeComponent();
            _requestType = requestType;
            RequestSubMap = new Dictionary<string, RequestSubType>();
        
            foreach (var subtype in requestType.SubType)
            {
               
                RequestSubMap.Add(subtype.Title, subtype);

            }
           
            SubTypeList.ItemsSource = RequestSubMap.Keys;

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            SubTypeList.ItemTapped += async (s, e) =>
            {
                var emi = e.Item;
                if (Navigation.NavigationStack.Count == 0 ||
                     Navigation.NavigationStack[Navigation.NavigationStack.Count - 1].GetType() != typeof(CaseCreation))
                {

                    await Navigation.PushAsync(new CaseCreation(_requestType, RequestSubMap[emi.ToString()]));

                }



            };
        }
           
       

    }
}
