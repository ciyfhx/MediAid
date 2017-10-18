using MediAid.Models;
using MediAid.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        private int seconds = 0;

        private string Details;

        private Reminder reminder;

        public RecordReminder (Reminder reminder)
		{
            this.reminder = reminder;
			InitializeComponent ();

            BindingContext = this;

            RecordBtn.Text = RECORD_TEXT;


        }

        void Record_Reminder(object sender, EventArgs e)
        {
            if (!App.audioHandler.IsRecording)
            {
                RecordBtn.Text = STOP_RECORD_TEXT;
                Debug.WriteLine(reminder.RecordId);

                timer = new Timer(new TimerCallback(UpdateDetails), null, 1000, 1000);
                App.audioHandler.StartRecording($"{reminder.RecordId}.3gpp");
            }
            else
            {
                RecordBtn.Text = RECORD_TEXT;
                App.audioHandler.StopRecording();
                timer?.Dispose();

            }

        }

        private void UpdateDetails(object source)
        {
            Details = $"{seconds}s";
        }

        async void Done()
        {
            if (App.audioHandler.IsRecording) App.audioHandler.StopRecording();
            //Check if reminder exists
            if (!App.Reminders.GetItems().Keys.Any(cReminder => cReminder.ReminderId == reminder.ReminderId))
                MessagingCenter.Send(this, "AddReminder", reminder);
            else MessagingCenter.Send(this, "UpdateReminder", reminder);
            //AlarmHandler handler = DependencyService.Get<AlarmHandler>();

            //handler.CreateAlarm(reminder);

            await Navigation.PopToRootAsync();
        }

    }
}