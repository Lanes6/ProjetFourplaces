using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using fourplaces.Models;
using Storm.Mvvm;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace fourplaces.ViewModels
{
    public class DetailViewModel : ViewModelBase
    {
        private int _id;
        private string _msg = "";
        private string _newComment;
        private string _title;
        private string _description;
        private string _imageUrl;
        private double _latitude;
        private double _longitude;
        private double _distanceP;
        private Map _map;
        private List<CommentItem> _commentaires;
        public ICommand SendCommentCommand { protected set; get; }


        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public string Msg
        {
            get => _msg;
            set => SetProperty(ref _msg, value);
        }

        public string NewComment
        {
            get => _newComment;
            set => SetProperty(ref _newComment, value);
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
            set => SetProperty(ref _latitude, value);
        }
        public double Longitude
        {
            get => _longitude;
            set => SetProperty(ref _longitude, value);
        }

        public double DistanceP
        {
            get => _distanceP;
            set => SetProperty(ref _distanceP, value);
        }

        public Map Map
        {
            get => _map;
            set => SetProperty(ref _map, value);
        }

        public List<CommentItem> Commentaires
        {
            get => _commentaires;
            set => SetProperty(ref _commentaires, value);
        }

        public DetailViewModel(PlaceItemSummary lieu)
        {
            _id = lieu.Id;
            _title = lieu.Title;
            _description = lieu.Description;
            _imageUrl = lieu.ImageUrl;
            _latitude = lieu.Latitude;
            _longitude = lieu.Longitude;
            _distanceP = lieu.Distance;
            _map = new Map();
            _commentaires = new List<CommentItem>();
            SendCommentCommand = new Command(async () => { await TrySendComment(); });
            Setup();
        }

        public override async Task OnResume()
        {
            await base.OnResume();
            await Setup();
        }

        public async Task Setup()
        {
            PlaceItem place = await App.SERVICE.GetPlace(Id);
            if (place != null)
            {
                Commentaires = place.Comments;
                Position position_pin = new Position(Latitude, Longitude);
                Map.MoveToRegion(MapSpan.FromCenterAndRadius(position_pin, Distance.FromKilometers(DistanceP * 1.5)));
                var pin = new Pin
                {
                    Position = position_pin,
                    Label = Title
                };
                Map.Pins.Add(pin);
            }
            else
            {
                Msg = "Erreur lors du chargement des détails du lieu";
            }
        }

            public async Task TrySendComment()
        {
            if (NewComment != "" && NewComment!= null)
            {
                if (await App.SERVICE.SendComment(NewComment, Id))
                {
                    NewComment = "";
                    await this.OnResume();
                }
                else
                {
                    Msg = "Erreur lors de l'envoi du commentaire";
                }
            }
            else
            {
                Msg="Commentaire vide";
            }
        }
    }
}
