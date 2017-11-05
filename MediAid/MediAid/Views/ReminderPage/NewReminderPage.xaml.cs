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
                Time = DateTime.Now.TimeOfDay,
                Date = DateTime.Now,
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

            DatePicker.MinimumDate = DateTime.Now;

            Reminder = reminder;

            BindingContext = this;
        }

        async void Next_Clicked(object sender, EventArgs e)
		{
            //MessagingCenter.Send(this, "AddTiming", new Timing { Time = this.Time});
            //Debug.WriteLine($"{picker.Time.Hours}, {picker.Time.TotalHours}");
            if ((Reminder.Date - Reminder.Date.TimeOfDay + Reminder.Time) < DateTime.Now)
            {
                await DisplayAlert("Error" , "Cannot set alarm in the past", "OK");
                return;
            }
            if (Validation())
            {
                await Navigation.PushAsync(new PillsSelectListPage(Reminder));
            }

		}

        public bool Validation()
        {
            return !String.IsNullOrEmpty(Reminder.Name);
        }
       



    }
}