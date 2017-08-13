using MediAid.Models;
using MediAid.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MediAid.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PillsListPage : ContentPage
    {
        DrugsListPageViewModel viewModel;

        public PillsListPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new DrugsListPageViewModel();

        }

        async void AddMedication_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddDrug());


        }


        async void Handle_ItemTapped(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            //await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Drugs.Count == 0)
                viewModel.LoadDrugsCommand.Execute(null);
        }

    }
}