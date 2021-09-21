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
using System.Collections.ObjectModel;

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


            var productIndex = 0;
            int rowsCategories = (orderedCategories.Count % 3 == 0) ? (orderedCategories.Count / 3) : Convert.ToInt32(Math.Floor((decimal)orderedCategories.Count / 3)) + 1;
            for (int rowIndex = 0; rowIndex < rowsCategories; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < 3; columnIndex++)
                {
                    if (productIndex >= orderedCategories.Count)
                    {
                        break;
                    }
                    var catergory = orderedCategories[productIndex];
                    
                    productIndex += 1;
                    var label = new Label
                    {
                        Text = catergory.Title,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center
                    };
                    var image = new Image
                    {
                        Source = "https://cdn.palmcoastgov.com/images/paw-solid.png",
                        HeightRequest = 25
                    };
                    var stacklayout = new StackLayout
                    {
                        Children = { image, label },

                    };
                    //On tap event handler
                    var tapGestureRecognizer = new TapGestureRecognizer();
                    tapGestureRecognizer.Tapped += (s, e) => {
                        //Console.WriteLine("Tapped " + catergory.Title);
                        Navigation.PushAsync(new NoteEntryPage(catergory));
                    };

                    stacklayout.GestureRecognizers.Add(tapGestureRecognizer);
                    var frame = new Frame
                    {
                       
                        HeightRequest = 35,
                        WidthRequest = 45,
                        CornerRadius = 20,
                        BorderColor = Color.Black
                        
                    };

                    
                    gridLayout.Children.Add(frame, columnIndex, rowIndex);
                    
                }
            }


        }

       

       
    }

}
