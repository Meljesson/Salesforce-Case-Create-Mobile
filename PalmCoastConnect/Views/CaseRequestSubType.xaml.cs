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

        public CaseRequestSubType(RequestType requestType)
        {
            InitializeComponent();
            _requestType = requestType;
            List<string> subTypeTitle = new List<string>();
            foreach (var subtype in requestType.SubType)
            {
                subTypeTitle.Add(subtype.Title);
                

            }
            SubTypeList.ItemsSource = subTypeTitle;
            
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
           
       

    }
}
