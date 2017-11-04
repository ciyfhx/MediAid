using MediAid.Models;
using MediAid.Services;
using MediAid.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MediAid.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ReminderDetails : ContentPage
	{
        private ReminderDrugsListPage viewModel;

        //Toggle will not fire first time if the value is false
        public bool OnAppearToggleFirstFire = false;

        public ReminderDetails(Reminder reminder, bool OnAppearToggleFirstFire)
        {
            this.OnAppearToggleFirstFire = OnAppearToggleFirstFire;
            //check whether the reminder is enabled and if the value is already true we dont need to change it
            if (!OnAppearToggleFirstFire) OnAppearToggleFirstFire = !reminder.IsEnabled;
            else if (reminder.IsEnabled) App.alarmHandler.RemoveAlarm(reminder);

            //if (OnAppearToggleFirstFire && ) 

            InitializeComponent();

            //Manually set the data
            Reminder.Text = reminder.Name;

            if (!App.audioHandler.RecordingExist($"{reminder.RecordId}.3gpp"))
            {
                PlayReminderBtn.IsEnabled = false;
            }

            BindingContext = viewModel = new ReminderDrugsListPage(reminder);

          
            
        }

        public ReminderDetails(Reminder reminder) : this(reminder, false)
        {
        }

        async void To_PillDetails(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            await Navigation.PushAsync(new PillDetails(e.SelectedItem as Drug));

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        void Play_Reminder(object sender, EventArgs e)
        {
            App.audioHandler.PlayRecording($"{viewModel.Reminder.RecordId}.3gpp");


        }

        void Remove_Reminder(object sender, EventArgs e)
        {
            App.audioHandler.RemoveRecording($"{viewModel.Reminder.RecordId}.3gpp");

            MessagingCenter.Send(this, "RemoveReminder", viewModel.Reminder);

            Navigation.PopToRootAsync();

        }


        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }

        private void Toggle_Reminder(object sender, ToggledEventArgs e)
        {
            //Hack-around to stop the event from fired from first launch if reminder is enabled is true
            if (!OnAppearToggleFirstFire)
            {
                OnAppearToggleFirstFire = true;
                return;
            }
            if (viewModel.Reminder.IsEnabled)
            {
                viewModel.Reminder.TimeEnabled = DateTime.Now;
                App.alarmHandler.CreateAlarm(viewModel.Reminder);
                viewModel.Reminder.RepeatingCount++;
            }
            else
            {
                viewModel.Reminder.RepeatingCount = 0;
                App.alarmHandler.RemoveAlarm(viewModel.Reminder);
            }
            MessagingCenter.Send(this, "UpdateReminder", viewModel.Reminder);


        }

        private void Edit_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NewReminderPage(viewModel.Reminder));
        }
    }
}