using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using fourplaces.Models;
using Newtonsoft.Json;
using Plugin.Geolocator.Abstractions;
using Storm.Mvvm;
using Xamarin.Forms;

namespace fourplaces.ViewModels
{
    public class UserViewModel : ViewModelBase
    {
        private string _msg = "";
        private string _firstName;
        private string _lastName;
        private string _mail;
        private string _imageUrl;
        public ICommand GoProfilEditCommand { protected set; get; }
        public ICommand GoPassEditCommand { protected set; get; }
        public INavigation Navigation { get; set; }

        public string Msg
        {
            get => _msg;
            set => SetProperty(ref _msg, value);
        }

        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }

        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }
        
        public string Mail
        {
            get => _mail;
            set => SetProperty(ref _mail, value);
        }

        public string ImageUrl
        {
            get => _imageUrl;
            set => SetProperty(ref _imageUrl, value);
        }

        public UserViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            GoProfilEditCommand = new Command(async () => { await Navigation.PushAsync(new UserEditPage()); });
            GoPassEditCommand = new Command(async () => { await Navigation.PushAsync(new PassEditPage()); });
        }

        public override async Task OnResume()
        {
            await base.OnResume();
            Msg = "";
            FirstName = "Prénom: " + App.SESSION_PROFIL.FirstName;
            LastName = "Nom: " + App.SESSION_PROFIL.LastName;
            Mail = "EMail: " + App.SESSION_PROFIL.Email;
            ImageUrl = App.URI_BASE + App.URI_GET_IMAGE + App.SESSION_PROFIL.ImageId;
        }
    }
}