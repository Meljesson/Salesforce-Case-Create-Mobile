using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PalmCoastConnect.Views;

namespace PalmCoastConnect
{
    public partial class App : Application
    {
        public static string ApiKey;


        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
            ApiKey = "AIzaSyCEeX_u4ZP9LM47M5J3JFODMLXtrjcBt1g";
        }

        protected override void OnStart()
        {
            Application.Current.Properties["StrapiUrl"] = "https://manage.palmcoast.gov/graphql";


        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
