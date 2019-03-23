using fourplaces.ViewModels;
using Storm.Mvvm.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace fourplaces
{
    public partial class DetailPage : TabbedPage
    {
        public DetailPage(Models.PlaceItemSummary lieu)
        {
            InitializeComponent();
            BindingContext = new DetailViewModel(lieu);
        }
    }
}