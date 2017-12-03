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
    public partial class RemindersListPage : ContentPage
    {
        RemindersListPageViewModel viewModel;

        public RemindersListPage()
        {
            Title = "Reminders";

            InitializeComponent();

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


        async void AddReminder_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewReminderPage());
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
            viewModel.LoadItemsCommand.Execute(null);

            if (!viewModel.Items.Any())
            {
                NoReminders.IsVisible = true;
                NoReminders.Opacity = 0;
                NoReminders.FadeTo(1, 700, easing: Easing.SinIn);
            }
            else
            {
                NoReminders.IsVisible = false;
            }

        }

    }
}