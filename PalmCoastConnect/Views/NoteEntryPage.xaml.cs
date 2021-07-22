using System;
using System.Collections.Generic;
using System.IO;
using PalmCoastConnect.Models;
using Xamarin.Forms;
using CMS.Service.ConnectCases;
namespace PalmCoastConnect.Views
{
   
    public partial class NoteEntryPage : ContentPage
    {
        private Catergory _CaseCategory { get; set;}
        private Dictionary<string, RequestType> RequestMap { get; set; }

        public NoteEntryPage(Catergory catergory)
        {
            InitializeComponent();
            //Console.WriteLine(catergory.Title + " Loaded");
            _CaseCategory = catergory;
        }
        protected override void OnAppearing()
        {
            var requestTypes = _CaseCategory.RequestType;
            RequestMap = new Dictionary<string, RequestType>();
           
            foreach(var rtype in requestTypes)
            {
               
                RequestMap.Add(rtype.Title, rtype);

            }
            RequestList.ItemsSource = RequestMap.Keys;
            RequestList.ItemTapped += async (s, e) =>
            {
                var emi = e.Item;
                if (Navigation.NavigationStack.Count == 0 ||
                     Navigation.NavigationStack[Navigation.NavigationStack.Count - 1].GetType() != typeof(CaseRequestSubType))
                { 

                    
                   await Navigation.PushAsync(new CaseRequestSubType(RequestMap[emi.ToString()]));
                }
                
               
               
            };

        }



    }
}
