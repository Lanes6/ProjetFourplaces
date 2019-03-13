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
	public partial class UserEditPage : BaseContentPage
    {
		public UserEditPage()
		{
			InitializeComponent ();
            BindingContext = new UserEditViewModel(Navigation);
        }
	}
}