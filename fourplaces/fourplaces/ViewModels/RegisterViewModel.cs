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
    public class RegisterViewModel : ViewModelBase
    {
        private string _msg = "";
        private string _mail="";
        private string _firstName = "";
        private string _lastName = "";
        private string _mdp = "";
        private string _mdp2 = "";
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

        public string Mdp
        {
            get => _mdp;
            set => SetProperty(ref _mdp, value);
        }

        public string Mdp2
        {
            get => _mdp2;
            set => SetProperty(ref _mdp2, value);
        }

        public RegisterViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            TryRegisterCommand = new Command(async () => { await TryRegisterAsync(); });
        }

        public async Task TryRegisterAsync()
        {
            if(Mdp == Mdp2 && Mail != "" && Mdp != "" && FirstName != "" && LastName != "")
            {
                bool res = await App.SERVICE.Register(Mail, FirstName, LastName, Mdp);
                if (res)
                {
                    await Navigation.PushAsync(new HomePage());
                }
                else
                {
                    Msg = "Erreur lors de l'enregistrement";
                }
            }
            else
            {
                Msg = "Un des champs est incorrect";
            }
        }
    }
}