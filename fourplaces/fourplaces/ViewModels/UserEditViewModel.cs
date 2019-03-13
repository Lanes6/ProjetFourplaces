using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using fourplaces.Models;
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
        private int? _imageId;
        private string _imageUrl;
        public ICommand EditCommand { protected set; get; }
        public ICommand NewPictureCommand { protected set; get; }
        public INavigation Navigation { get; set; }

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
            _firstName = App.SESSION_PROFIL.FirstName;
            _lastName = App.SESSION_PROFIL.LastName;
            _imageId = App.SESSION_PROFIL.ImageId;
            _imageUrl = App.URI_BASE+App.URI_GET_IMAGE+App.SESSION_PROFIL.ImageId;
            EditCommand = new Command(async () => { await Edit(); });
            NewPictureCommand = new Command(async () => { await TryNewPicture(); });
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

        public async Task TryNewPicture()
        {
            int? res = await App.SERVICE.UploadPicture();
            if (res != null)
            {
                ImageId = res.ToString();
                OnPropertyChanged("ImageId");
            }
            else
            {
                Msg = "Echec, la photo n'a pas été enregistrée";
            }
        }

        public virtual void OnPropertyChanged(string s)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(s));
        }
    }
}