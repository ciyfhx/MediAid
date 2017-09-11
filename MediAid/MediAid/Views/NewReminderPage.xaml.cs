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
        private Reminder reminder;
        public Reminder Reminder { get => reminder; set => reminder = value; }



        public NewReminderPage()
		{
			InitializeComponent();

            Reminder = new Reminder
            {
                Name = "",
                Hours = 1,
                TimeCreated = DateTime.Now,
                RecordId = Guid.NewGuid().ToString()
			};

			BindingContext = this;

		}

		async void Next_Clicked(object sender, EventArgs e)
		{
            await Navigation.PushAsync(new PillsSelectListPage(ref reminder));
		}

       
       



    }
}