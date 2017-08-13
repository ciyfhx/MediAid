using MediAid.Models;
using MediAid.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

            //await DisplayAlert("Reminder Tapped", "An Reminder was tapped.", "OK");




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

            if (viewModel.Reminders.Count == 0)
                viewModel.LoadRemindersCommand.Execute(null);
        }
    }
}