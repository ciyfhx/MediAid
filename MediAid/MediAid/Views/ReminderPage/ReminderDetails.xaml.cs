using MediAid.Models;
using MediAid.Services;
using MediAid.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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

        public bool OnAppearToggleFirstFire = false;

        private IMediaService mediaService = DependencyService.Get<IMediaService>();


        public ReminderDetails(Reminder reminder)
        {

            InitializeComponent();
            OnAppearToggleFirstFire = !reminder.IsEnabled;
            if (reminder.IsEnabled) UpdateAlarmLabel(reminder);
            
            //Manually set the data
            Reminder.Text = reminder.Name;
            Title = reminder.Name;

            if (!App.audioHandler.RecordingExist($"{reminder.RecordId}.3gpp"))
            {
                PlayReminderBtn.IsEnabled = false;
            }

            PlayVideoReminderBtn.IsEnabled = File.Exists($"/storage/emulated/0/Android/data/com.ciyfhx.MediAid/files/Movies/temp/{reminder.RecordId}.mp4");

            BindingContext = viewModel = new ReminderDrugsListPage(reminder);

            
            
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

        private async void Toggle_Reminder(object sender, ToggledEventArgs e)
        {
            if (!OnAppearToggleFirstFire)
            {
                OnAppearToggleFirstFire = true;
                return;
            }
            if (viewModel.Reminder.IsEnabled)
            {
                if((viewModel.Reminder.Date + viewModel.Reminder.Time) < DateTime.Now)
                {
                    await DisplayAlert("Error", "This alarm has a starting time in the past", "OK");
                    viewModel.Reminder.IsEnabled = false;
                    return;
                }
                if (viewModel.Reminder.RepeatingCount == 0)
                {
                    viewModel.Reminder.TimeEnabled = DateTime.Now;

                    long millis = AlarmUtils.NextTimeMillis(viewModel.Reminder);
                    App.alarmHandler.CreateAlarm(viewModel.Reminder, millis);
                    //viewModel.Reminder.RepeatingCount = 0;
                    UpdateAlarmLabel(viewModel.Reminder);
                }
            }
            else
            {
                viewModel.Reminder.RepeatingCount = 0;
                App.alarmHandler.RemoveAlarm(viewModel.Reminder);
                NextAlarm.Text = "";
            }
            MessagingCenter.Send(this, "UpdateReminder", viewModel.Reminder);


        }

        private async void Play_Video_Reminder(object sender, EventArgs e)
        {
            //path "/storage/emulated/0/Android/data/com.ciyfhx.MediAid/files/Movies/temp/{RecordID}.mp4"
            await mediaService.PlayVideoAsync($"/storage/emulated/0/Android/data/com.ciyfhx.MediAid/files/Movies/temp/{viewModel.Reminder.RecordId}.mp4");
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