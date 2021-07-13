using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xamarin.Forms;
using PalmCoastConnect.Models;
using PalmCoastConnect.Views;
using Library.SfFactory.Service;
using CMS.Service.Client;
using CMS.Service.ConnectCases;

namespace PalmCoastConnect
{
    public partial class MainPage : ContentPage
    {
        public CaseOptions ConnectCaseOptions { get; set; }
      

        public MainPage()
        {
            InitializeComponent();
           
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();


            var StrapiUrl = Application.Current.Properties["StrapiUrl"];
            CMSPalmCoastConnectCasesService _pccclient = new CMSPalmCoastConnectCasesService(StrapiUrl.ToString());
            var tempCategories = await _pccclient.GetCaseCategoriesAsync();
            List<Catergory> orderedCategories = tempCategories.OrderBy(f => f.Title).ToList();

            // Triggers on ready



            var tempQuickLinks = await _pccclient.GetQuickCaseChoicesAsync();
            List<QuickLink> orderedQuickLinks = tempQuickLinks.OrderBy(f => f.Title).ToList();
            //ConnectCaseOptions = new CaseOptions
            //{
            //    CaseCategories = orderedCategories,
            //    QuickLinks = orderedQuickLinks
            //};



        }

        async void OnAddClicked(object sender, EventArgs e)
        {
            // Navigate to the NoteEntryPage, without passing any data.
            await Shell.Current.GoToAsync(nameof(NoteEntryPage));
        }

        async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null)
            {
                // Navigate to the NoteEntryPage, passing the filename as a query parameter.
                Note note = (Note)e.CurrentSelection.FirstOrDefault();
                await Shell.Current.GoToAsync($"{nameof(NoteEntryPage)}?{nameof(NoteEntryPage.ItemId)}={note.Filename}");
            }
        }

       
    }
}
