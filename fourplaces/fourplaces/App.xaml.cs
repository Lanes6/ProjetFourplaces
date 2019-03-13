using System;
using fourplaces;
using fourplaces.Models;
using Plugin.Geolocator.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace fourplaces
{
    public partial class App : Application
    {
        public static Position POSITION_DEVICE { get; set; }
        public static string URI_BASE { get; set; }
        public static string URI_GET_PLACES { get; set; }
        public static string URI_GET_IMAGE { get; set; }
        public static string URI_POST_IMAGE { get; set; }
        public static string URI_LOGIN { get; set; }
        public static string URI_REGISTER { get; set; }
        public static string URI_USER { get; set; }
        public static string URI_COMMENT { get; set; }
        public static string URI_PASS { get; set; }
        public static string URI_REFRESH { get; set; }

        public static UserItem SESSION_PROFIL { get; set; }
        public static LoginResult SESSION_LOGIN { get; set; }
        public static Service SERVICE { get; set; }


        public App()
        {
            POSITION_DEVICE = new Position(0.0, 0.0);
            URI_BASE = "https://td-api.julienmialon.com";
            URI_GET_PLACES = "/places";
            URI_COMMENT = "/comments";
            URI_GET_IMAGE = "/images/";
            URI_POST_IMAGE = "/images";
            URI_LOGIN ="/auth/login";
            URI_REGISTER = "/auth/register";
            URI_USER = "/me";
            URI_PASS = "/password";
            URI_REFRESH = "/auth/refresh";
            SERVICE = new Service();
            InitializeComponent();
            MainPage = new NavigationPage(new LoginPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
