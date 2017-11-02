using System;

using MediAid.Models;

using MediAid.Services;
using System.IO;
using System.Diagnostics;
using MediAid.Helpers;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using System.Collections.Generic;

namespace MediAid.Views
{
    public partial class NewReminderPage : ContentPage
    {
        public Reminder Reminder { get; set; }

        //TimePicker
        //public TimeSpan Time { get; set; }


        public NewReminderPage()
		{

            var temp = new Reminder
            {
                Name = "",
                Hours = 1,
                IsEnabled = false,
                RecordId = Guid.NewGuid().ToString(),
                Time = new TimeSpan(12, 0, 0)
			};

            Init(temp);
        }

        public NewReminderPage(Reminder reminder)
        {
            Init(reminder);
        }

        private void Init(Reminder reminder)
        {
            InitializeComponent();

            Reminder = reminder;

            BindingContext = this;
        }

        async void Next_Clicked(object sender, EventArgs e)
		{
            //MessagingCenter.Send(this, "AddTiming", new Timing { Time = this.Time});
            //Debug.WriteLine($"{picker.Time.Hours}, {picker.Time.TotalHours}");
            if(Validation())
            await Navigation.PushAsync(new PillsSelectListPage(Reminder));
		}

        public bool Validation()
        {
            return !String.IsNullOrEmpty(Reminder.Name);
        }
       



    }
}