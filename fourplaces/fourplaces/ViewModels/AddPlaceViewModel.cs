using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using fourplaces.Models;
using MonkeyCache.SQLite;
using Storm.Mvvm;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace fourplaces.ViewModels
{
    public class AddPlaceViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private List<ImageItem2> _images;
        private ImageItem2 _selectedImage;
        private string _msg = "";
        private string _title = "";
        private string _description = "";
        private int? _imageId;
        private string _imageUrl;
        private double _latitude;
        private double _longitude;
        private Map _map;
        public ICommand LoadPictureCommand { protected set; get; }
        public ICommand TakePictureCommand { protected set; get; }
        public ICommand AddPlaceCommand { protected set; get; }
        public INavigation Navigation { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public ImageItem2 SelectedImage
        {
            get
            {
                return _selectedImage;
            }

            set
            {
                _selectedImage = value;
                if (value.Id != 0)
                {
                    OnPropertyChanged("SelectedImage");
                    ImageId = value.Id.ToString();
                    OnPropertyChanged("ImageId");
                }
            }
        }

        public List<ImageItem2> Images
        {
            get => _images;
            set => SetProperty(ref _images, value);
        }

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
            LoadPictureCommand = new Command(async () => { await TryLoadPicture(); });
            TakePictureCommand = new Command(async () => { await TryTakePicture(); });
            AddPlaceCommand = new Command(async () => { await TryAddPlace(); });
            _map = new Map();
            _images = new List<ImageItem2>();
            _images.Add(new ImageItem2(0, "loading.png"));
            _images.Add(new ImageItem2(0, "loading.png"));
            _images.Add(new ImageItem2(0, "loading.png"));
            _imageId = 1;
            _imageUrl = App.URI_BASE + App.URI_GET_IMAGE +1;
            _latitude = Barrel.Current.Get<Plugin.Geolocator.Abstractions.Position>(key: "Localisation").Latitude;
            _longitude = Barrel.Current.Get<Plugin.Geolocator.Abstractions.Position>(key: "Localisation").Longitude;
        }

        public override async Task OnResume()
        {
            await base.OnResume();
            await LoadPictures();
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
                OnPropertyChanged("Msg");
            }
        }

        public async Task TryLoadPicture()
        {
            int? res = await App.SERVICE.LoadPicture(true);
            if (res != null)
            {
                ImageId = res.ToString();
                OnPropertyChanged("ImageId");
                await LoadPictures();
            }
            else
            {
                Msg = "Echec, la photo n'a pas été enregistrée";
                OnPropertyChanged("Msg");
            }
        }

        public async Task TryTakePicture()
        {
            int? res = await App.SERVICE.LoadPicture(false);
            if (res != null)
            {
                ImageId = res.ToString();
                OnPropertyChanged("ImageId");
                await LoadPictures();
            }
            else
            {
                Msg = "Echec, la photo n'a pas été enregistrée";
                OnPropertyChanged("Msg");
            }
        }

        public async Task LoadPictures()
        {
            Images = new List<ImageItem2>();
            int id = 1;
            while (await App.SERVICE.TestImage(id))
            {
                Images.Add(new ImageItem2(id, "https://td-api.julienmialon.com/images/" + id));
                id++;
            }
            OnPropertyChanged("Images");
        }

        private void UpdateMap()
        {
            Xamarin.Forms.Maps.Position position_pin = new Xamarin.Forms.Maps.Position(Latitude, Longitude);
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