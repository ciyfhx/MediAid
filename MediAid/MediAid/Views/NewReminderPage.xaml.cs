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

        private const string RECORD_TEXT = "Record a Reminder";
        private const string STOP_RECORD_TEXT = "Stop Recording";

        public string ButtonText { get; set; }

        AudioHandler audioHandler;

        public NewReminderPage()
		{
			InitializeComponent();
            InitAudioHandler();

            ButtonText = RECORD_TEXT;

            Reminder = new Reminder
            {
				Name = "",
                Hours = 1
			};

			BindingContext = this;

		}

		async void Save_Clicked(object sender, EventArgs e)
		{
            await Navigation.PushAsync(new PillsSelectListPage(ref reminder));
		}

        void Record_Reminder(object sender, EventArgs e)
        {
            if (!audioHandler.IsRecording)
            {
                audioHandler.StartRecording($"{Reminder.Id}.3gpp");
                ButtonText = STOP_RECORD_TEXT;
            }
            else
            {
                audioHandler.StopRecording();
                ButtonText = RECORD_TEXT;
            }

        }

       

        public void InitAudioHandler()
        {
            string path = EnvironmentUtils.GetPlatformEnironmentPath() + "/Recordings";
            Directory.CreateDirectory(path);
            audioHandler = DependencyService.Get<AudioHandler>();
            audioHandler.Init(path);

        }

    }
}