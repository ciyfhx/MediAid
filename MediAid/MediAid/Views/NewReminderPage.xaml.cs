using System;

using MediAid.Models;

using MediAid.Services;
using System.IO;
using System.Diagnostics;
using MediAid.Helpers;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace MediAid.Views
{
    public partial class NewReminderPage : ContentPage
    {
        public Reminder Reminder { get; set; }



        public NewReminderPage()
		{

            var temp = new Reminder
            {
                Name = "",
                Hours = 1,
                IsEnabled = false,
                RecordId = Guid.NewGuid().ToString()
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
            await Navigation.PushAsync(new PillsSelectListPage(Reminder));
		}

       
       



    }
}