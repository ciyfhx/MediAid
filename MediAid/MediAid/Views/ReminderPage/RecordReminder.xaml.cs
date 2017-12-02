using MediAid.Helpers;
using MediAid.Models;
using MediAid.Services;
using MediAid.ViewModels;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MediAid.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RecordReminder : ContentPage
    {
        private const string RECORD_TEXT = "Record a Reminder";
        private const string STOP_RECORD_TEXT = "Stop Recording";

        private Timer timer;

        private Reminder reminder;

        private RecordReminderViewModel viewModel;



        public RecordReminder (Reminder reminder)
		{
            Title = "Record Reminder";
            this.reminder = reminder;
			InitializeComponent ();

            BindingContext = viewModel = new RecordReminderViewModel();

            RecordVideoBtn.Text = "Record a Video Reminder";

            RecordAudioBtn.Text = RECORD_TEXT;


        }

        async void Record_Video_Reminder(object sender, EventArgs e)
        {
            DeleteVideo(reminder.RecordId);
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakeVideoSupported)
            {
                Debug.WriteLine("Unable to take video");
                await DisplayAlert("Error", "Video taking is not supported", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakeVideoAsync(new StoreVideoOptions
            {
                Directory = "temp",
                Name = $"{reminder.RecordId}.mp4"
            });

        }

        void DeleteVideo(string file)
        {
            if (File.Exists(file))
            {
                File.Delete(file);
            }
        }

        void Record_Audio_Reminder(object sender, EventArgs e)
        {
            if (!App.audioHandler.IsRecording)
            {
                RecordAudioBtn.Text = STOP_RECORD_TEXT;
                Debug.WriteLine(reminder.RecordId);

                viewModel.Reset();
                timer = new Timer(new TimerCallback(viewModel.UpdateDetails), null, 1000, 1000);
                App.audioHandler.StartRecording($"{reminder.RecordId}.3gpp");
            }
            else
            {
                RecordAudioBtn.Text = RECORD_TEXT;
                App.audioHandler.StopRecording();
                timer?.Dispose();

            }

        }



        async void Done()
        {
            if (App.audioHandler.IsRecording) App.audioHandler.StopRecording();
            //Check if reminder exists
            if (!App.Reminders.GetItems().Keys.Any(cReminder => cReminder.ReminderId == reminder.ReminderId))
                MessagingCenter.Send(this, "AddReminder", reminder);
            else
            {
                MessagingCenter.Send(this, "UpdateReminder", reminder);
                if (reminder.IsEnabled)
                {
                    App.alarmHandler.RemoveAlarm(reminder);
                    //reminder.RepeatingCount = 0;

                    App.alarmHandler.CreateAlarm(reminder, AlarmUtils.NextTimeMillis(reminder));
                }
            }
            //AlarmHandler handler = DependencyService.Get<AlarmHandler>();

            //handler.CreateAlarm(reminder);
            await Navigation.PopToRootAsync();
            await Navigation.PushAsync(new ReminderDetails(reminder));
        }



    }
}