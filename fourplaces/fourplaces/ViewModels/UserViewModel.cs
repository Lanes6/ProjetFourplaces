using System.Threading.Tasks;
using System.Windows.Input;
using fourplaces.Models;
using MonkeyCache.SQLite;
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
            FirstName = "Prénom: " + Barrel.Current.Get<UserItem>(key: "Client").FirstName;
            LastName = "Nom: " + Barrel.Current.Get<UserItem>(key: "Client").LastName;
            Mail = "EMail: " + Barrel.Current.Get<UserItem>(key: "Client").Email;
            ImageUrl = App.URI_BASE + App.URI_GET_IMAGE + Barrel.Current.Get<UserItem>(key: "Client").ImageId;
        }
    }
}