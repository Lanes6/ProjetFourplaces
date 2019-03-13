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
    public class PassEditViewModel : ViewModelBase
    {
        private string _msg = "";
        private string _oldMdp;
        private string _mdp;
        private string _mdp2;
        public ICommand EditCommand { protected set; get; }
        public INavigation Navigation { get; set; }

        public string Msg
        {
            get => _msg;
            set => SetProperty(ref _msg, value);
        }

        public string OldMdp
        {
            get => _oldMdp;
            set => SetProperty(ref _oldMdp, value);
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

        public PassEditViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            OldMdp = "";
            Mdp = "";
            Mdp2 = "";
            EditCommand = new Command(async () => { await Edit(); });
            }

        public async Task Edit()
        {
            if (Mdp==Mdp2 && Mdp != "")
            {
                bool res = await App.SERVICE.EditMdp(OldMdp, Mdp);
                if (res)
                {
                    await Navigation.PopAsync();
                }
                else
                {
                    Msg = "Echec de la mise à jour du mot de passe";
                }
            }
            else
            {
                Msg = "Champs mal remplis";
            }
        }
    }
}