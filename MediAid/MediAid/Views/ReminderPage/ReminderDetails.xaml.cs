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


        public ReminderDetails(Reminder reminder, bool OnAppearToggleFirstFire)
        {

            InitializeComponent();
            if(reminder.NextRingMillis!=0 && reminder.IsEnabled) UpdateAlarmLabel(reminder);

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
            if (viewModel.Reminder.IsEnabled)
            {
                viewModel.Reminder.TimeEnabled = DateTime.Now;

                long millis = AlarmUtils.NextTimeMillis(viewModel.Reminder, DateTime.Now);
                App.alarmHandler.CreateAlarm(viewModel.Reminder, millis);
                viewModel.Reminder.RepeatingCount = 0;
                UpdateAlarmLabel(viewModel.Reminder);
            }
            else
            {
                viewModel.Reminder.RepeatingCount = 0;
                App.alarmHandler.RemoveAlarm(viewModel.Reminder);
                NextAlarm.Text = "";
            }
            MessagingCenter.Send(this, "UpdateReminder", viewModel.Reminder);


        }

        private void UpdateAlarmLabel(Reminder reminder)
        {
            DateTime now = DateTime.Now;


            NextAlarm.Text = $"Next alarm will ring on {now.AddMilliseconds(reminder.NextRingMillis).ToString("d MMM (ddd), h:mm tt")}";
        }

        private async void Edit_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewReminderPage(viewModel.Reminder));
        }

        

    }
}