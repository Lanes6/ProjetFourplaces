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
    public class HomeViewModel : ViewModelBase
    {
        private string _msg="";
        private List<PlaceItemSummary> _lieux;
        public ICommand GoProfilCommand { protected set; get; }
        public ICommand AddPlaceCommand { protected set; get; }
        public ICommand ReloadCommand { protected set; get; }
        public INavigation Navigation { get; set; }

        public string Msg
        {
            get => _msg;
            set => SetProperty(ref _msg, value);
        }

        public List<PlaceItemSummary> Lieux
        {
            get => _lieux;
            set => SetProperty(ref _lieux, value);
        }

        public HomeViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            GoProfilCommand = new Command(async () => { await Navigation.PushAsync(new UserPage()); });
            AddPlaceCommand = new Command(async () => { await Navigation.PushAsync(new AddPlacePage()); });
            ReloadCommand = new Command(async () => { await OnResume(); });
        }

        public override async Task OnResume()
        {
            await base.OnResume();
            Msg = "";
            ListeLieux lieux = await App.SERVICE.GetPlaces();
            if (lieux!= null)
            {
                foreach (PlaceItemSummary element in lieux.Lieux)
                {
                    element.ImageUrl = App.URI_BASE + App.URI_GET_IMAGE + element.ImageId;
                }
                Position temp = await App.SERVICE.Localisation();
                Console.WriteLine(temp);
                if (temp != null)
                {
                    App.POSITION_DEVICE = temp;
                }
                else
                {
                    Msg = "Activer votre localisation!";
                }
                lieux.SortDistance();
                Lieux = lieux.Lieux;
            }
            else
            {
                Msg = "Erreur lors de la récupération des lieux";
            }
        }
    }
}