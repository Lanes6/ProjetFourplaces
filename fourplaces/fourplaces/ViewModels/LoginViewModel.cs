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
    public class LoginViewModel : ViewModelBase
    {
        private string _msg = "";
        private string _mail = "lanes";
        private string _mdp = "lanes";
        public ICommand TryLoginCommand { protected set; get; }
        public ICommand TryRegisterCommand { protected set; get; }
        public INavigation Navigation { get; set; }

        public string Msg
        {
            get => _msg;
            set => SetProperty(ref _msg, value);
        }

        public string Mail
        {
            get => _mail;
            set => SetProperty(ref _mail, value);
        }

        public string Mdp
        {
            get => _mdp;
            set => SetProperty(ref _mdp, value);
        }

        public LoginViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            TryLoginCommand = new Command(async () => { await TryLoginAsync(); });
            TryRegisterCommand = new Command(async () => { await TryRegisterAsync(); });

        }
        public async Task TryLoginAsync()
        {
            bool res = await App.SERVICE.Login(Mail, Mdp);
            if (res)
            {
                await Navigation.PushAsync(new HomePage());
            }
            else
            {
                Mdp = "";
                Msg = "Erreur lors de la connection";
            }
        }

        public async Task TryRegisterAsync()
        {
            await Navigation.PushAsync(new RegisterPage());
        }
    }
}