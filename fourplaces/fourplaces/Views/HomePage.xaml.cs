using fourplaces.Models;
using fourplaces.ViewModels;
using Storm.Mvvm.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace fourplaces
{
    public partial class HomePage : BaseContentPage
    {
        public HomePage()
        {
            InitializeComponent();
            BindingContext = new HomeViewModel(Navigation);
            ListeLieux.ItemTapped += ListeLieux_ItemTapped;
        }

        private void ListeLieux_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            PlaceItemSummary lieu = (PlaceItemSummary)e.Item;
            Navigation.PushAsync(new DetailPage(lieu));
        }
    }
}