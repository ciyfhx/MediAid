using MediAid.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Xamarin.Forms.Device;

namespace MediAid.Views.ReminderPage
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ReminderList : ContentPage
	{
        RemindersListPageViewModel viewModel;

        public ReminderList ()
		{
			InitializeComponent ();

            BindingContext = viewModel = new RemindersListPageViewModel();

        }

        async void Handle_ReminderTapped(object sender, SelectedItemChangedEventArgs e)
        {
            
            if (e.SelectedItem == null)
                return;


            await Navigation.PushAsync(new ReminderDetails(viewModel.SelectedItem));


            //Deselect Reminder
            ((ListView)sender).SelectedItem = null;
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }

    }

    

}