using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using fourplaces.Models;
using Newtonsoft.Json;
using Storm.Mvvm;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace fourplaces.ViewModels
{
    public class AddPlaceViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string _msg = "";
        private string _title = "";
        private string _description = "";
        private int? _imageId;
        private string _imageUrl;
        private double _latitude;
        private double _longitude;
        private Map _map;
        public ICommand NewPictureCommand { protected set; get; }
        public ICommand AddPlaceCommand { protected set; get; }
        public INavigation Navigation { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public string Msg
        {
            get => _msg;
            set => SetProperty(ref _msg, value);
        }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public string ImageUrl
        {
            get => _imageUrl;
            set => SetProperty(ref _imageUrl, value);
        }

        public double Latitude
        {
            get => _latitude;
            set
            {
                SetProperty(ref _latitude, value);
                UpdateMap();
            }
        }
        public double Longitude
        {
            get => _longitude;
            set
            {
                SetProperty(ref _longitude, value);
                UpdateMap();
            }
        }

        public string ImageId
        {
            get
            {
                if (_imageId == null)
                    return "";
                else
                    return _imageId.ToString();
            }

            set
            {
                try
                {
                    int? temp = int.Parse(value);
                    SetProperty(ref _imageId, temp);
                    _imageUrl = App.URI_BASE + App.URI_GET_IMAGE + ImageId;
                }
                catch
                {
                    SetProperty(ref _imageId, 1);
                    _imageUrl = App.URI_BASE + App.URI_GET_IMAGE + "1";
                }
                OnPropertyChanged("ImageUrl");

            }
        }

        public Map Map
        {
            get => _map;
            set => SetProperty(ref _map, value);
        }

        public AddPlaceViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            NewPictureCommand = new Command(async () => { await TryNewPicture(); });
            AddPlaceCommand = new Command(async () => { await TryAddPlace(); });
            _map = new Map();
            _imageId = 1;
            _imageUrl = App.URI_BASE + App.URI_GET_IMAGE +1;
            _latitude =App.POSITION_DEVICE.Latitude;
            _longitude=App.POSITION_DEVICE.Longitude;
        }

        public override async Task OnResume()
        {
            await base.OnResume();
            UpdateMap();
        }


        private async Task TryAddPlace()
        {
            bool res = await App.SERVICE.AddPlace(Title,Description, int.Parse(ImageId), Latitude,Longitude);
            if (res)
            {
                await Navigation.PopAsync();
            }
            else
            {
                Msg = "Echec de la création";
            }
        }

        private async Task TryNewPicture()
        {
            int? res = await App.SERVICE.UploadPicture();
            if (res!=null)
            {
                ImageId = res.ToString();
                OnPropertyChanged("ImageId");
            }
            else
            {
                Msg = "Echec, la photo n'a pas été enregistrée";
            }
        }

        private void UpdateMap()
        {
            Position position_pin = new Position(Latitude, Longitude);
            Map.MoveToRegion(MapSpan.FromCenterAndRadius(position_pin, Distance.FromKilometers(150)));
            var pin = new Pin
            {
                Position = position_pin,
                Label = Title
            };
            Map.Pins.Clear();
            Map.Pins.Add(pin);
        }

        public virtual void OnPropertyChanged(string s)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(s));
        }

    }
}