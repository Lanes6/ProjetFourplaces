using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using fourplaces.Models;
using MonkeyCache.SQLite;
using Newtonsoft.Json;
using Plugin.Geolocator.Abstractions;
using Storm.Mvvm;
using Xamarin.Forms;

namespace fourplaces.ViewModels
{
    public class UserEditViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string _msg = "";
        private string _firstName;
        private string _lastName;
        private List<ImageItem2> _images;
        private ImageItem2 _selectedImage;
        private int? _imageId;
        private string _imageUrl;
        public ICommand EditCommand { protected set; get; }
        public ICommand LoadPictureCommand { protected set; get; }
        public ICommand TakePictureCommand { protected set; get; }
        public INavigation Navigation { get; set; }

        public ImageItem2 SelectedImage
        {
            get
            {
                return _selectedImage;
            }

            set
            {
                _selectedImage = value;
                OnPropertyChanged("SelectedImage");
                ImageId = value.Id.ToString();
                OnPropertyChanged("ImageId");
            }
        }

        public List<ImageItem2> Images
        {
            get => _images;
            set => SetProperty(ref _images, value);
        }

        public event PropertyChangedEventHandler PropertyChanged;

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

        public string ImageUrl
        {
            get => _imageUrl;
            set{
                SetProperty(ref _imageUrl, value);
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
                    _imageUrl = App.URI_BASE + App.URI_GET_IMAGE +ImageId;
                }
                catch
                {
                    SetProperty(ref _imageId, 1);
                    _imageUrl = App.URI_BASE + App.URI_GET_IMAGE + "1";
                }
                OnPropertyChanged("ImageUrl");
            }
        }

        public UserEditViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            _firstName = Barrel.Current.Get<UserItem>(key: "Client").FirstName;
            _lastName = Barrel.Current.Get<UserItem>(key: "Client").LastName;
            _imageId = Barrel.Current.Get<UserItem>(key: "Client").ImageId;
            _imageUrl = App.URI_BASE+App.URI_GET_IMAGE+ Barrel.Current.Get<UserItem>(key: "Client").ImageId;
            _images = new List<ImageItem2>();
            EditCommand = new Command(async () => { await Edit(); });
            LoadPictureCommand = new Command(async () => { await TryLoadPicture(); });
            TakePictureCommand = new Command(async () => { await TryTakePicture(); });
        }

    public override async Task OnResume()
        {
            await base.OnResume();
            await LoadPictures();
        }

    public async Task Edit()
        {
            bool res = await App.SERVICE.EditUser(FirstName, LastName, int.Parse(ImageId));
            if (res)
            {
                await Navigation.PopAsync();
            }
            else
            {
                Msg = "Echec de la mise à jour du profil";
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
            }
        }

        public async Task LoadPictures()
        {
            Images = new List<ImageItem2>();
            int id = 1;
            int idMax =await App.SERVICE.FindEndImage();
            while (id!=idMax+1){
                Images.Add(new ImageItem2(id, "https://td-api.julienmialon.com/images/" + id));
                id++;
            }
            OnPropertyChanged("Images");
        }

        public virtual void OnPropertyChanged(string s)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(s));
        }
    }
}
