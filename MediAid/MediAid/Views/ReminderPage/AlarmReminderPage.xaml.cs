using MediAid.Models;
using MediAid.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MediAid.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AlarmReminderPage : ContentPage
	{

        public Reminder Reminder { get; set; }

		public AlarmReminderPage (Reminder reminder)
		{
            this.Reminder = reminder;

            App.alarmHandler.RemoveAlarm(reminder);

            this.BindingContext = this;


            InitializeComponent ();
		}

        private async void NextReminder(object sender, EventArgs e)
        {
            if (Reminder.IsEnabled)
            {
                Reminder.RepeatingCount++;
                long millis = AlarmUtils.NextTimeMillis(Reminder);
                App.alarmHandler.CreateAlarm(Reminder, millis);
                MessagingCenter.Send(this, "UpdateReminder", Reminder);
            }



            await (App.Current.MainPage as RootMasterPage).Detail.Navigation.PopModalAsync();
            await (App.Current.MainPage as RootMasterPage).Detail.Navigation.PushAsync(new ReminderDetails(Reminder));
            //Navigation.RemovePage(this.Navigation.NavigationStack[this.Navigation.NavigationStack.Count - 1]);
        }

        void Play_Reminder(object sender, EventArgs e)
        {
            App.audioHandler.PlayRecording($"{Reminder.RecordId}.3gpp");


        }

    }
}