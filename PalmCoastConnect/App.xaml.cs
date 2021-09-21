using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PalmCoastConnect.Views;

namespace PalmCoastConnect
{
    public partial class App : Application
    {
        public static string ApiKey { get; set; }
        public static string StrapiUrl { get; set; }
        public static string AwsGateWay { get; set; }

        public App()
        {
            InitializeComponent();

            // MainPage = new NavigationPage(new Splash());
            MainPage = new NavigationPage(new MainPage());
            //Holds my apikeys sets them
            LocalApiKeys local = new LocalApiKeys();
        }

        protected override void OnStart()
        {
            Application.Current.Properties["StrapiUrl"] = App.StrapiUrl;
            Application.Current.Properties["AWSGatewayUrl"] = App.AwsGateWay;
            Application.Current.Properties["GoogleMapApi"] = App.ApiKey;
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
