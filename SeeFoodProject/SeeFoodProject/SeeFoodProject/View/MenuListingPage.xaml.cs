using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using SeeFoodProject.Model;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SeeFoodProject
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuListingPage : ContentPage
    {

        public MenuListingPage()
        {
            InitializeComponent();

        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        private void MyListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }
    }
}
