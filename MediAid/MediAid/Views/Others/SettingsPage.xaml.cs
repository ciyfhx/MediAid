﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediAid.Models;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using System.Diagnostics;

namespace MediAid.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {

        public Settings Settings { get; } = App.Settings;

        public bool IsAccountVerified { get; }

        public ImageSource ProfilePicture
        {
            get
            {
                string uri = App.firebase.GetProfilePicture();
                return !String.IsNullOrEmpty(uri) ? ImageSource.FromUri(new Uri(uri)) : ImageSource.FromResource("profile.jpg");
            }
        }

        public SettingsPage()
        {

            InitializeComponent();

             try
            {
                IsAccountVerified = App.firebase.IsAccountVerified();
            }
            catch (InvalidOperationException)
            {
                IsAccountVerified = false;
                RestoreBtn.IsEnabled = false;
                BackupBtn.IsEnabled = false;
            }

            this.BindingContext = this;

        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            if (IsAccountVerified)
            {
                Verified.Text = "Account is verified";
            }
            else
            {
                Verified.Text = "Account is not verified";
                Verified.TextColor = Color.Red;
            }

            //Not login
            if (!RestoreBtn.IsEnabled)
            {
                await DisplayAlert("Warning", "Some features will not work until you login", "OK");
            }


        }

        private async void Backup(object sender, EventArgs e)
        {
            //Backup data which are not sync with firebase
            App.Drugs.GetItems().Keys.ToList().ForEach(App.firebase.AddDrug);
            App.Reminders.GetItems().Keys.ToList().ForEach(App.firebase.AddReminder);

            await DisplayAlert("Backup", "Backup completed", "OK");

        }

        private async void Restore(object sender, EventArgs e)
        {
            var result = await DisplayAlert("Restoration", "Do you want to restore all your medications and reminders?", "Continue", "Cancel");
            if (!result) return;
            //Remove all recording
            DirectoryInfo di = new DirectoryInfo(App.audioHandler.RecordingPath);
            di.GetFiles().Where(file => file.Name.EndsWith(".3gpp")).ToList().ForEach(file => file.Delete());
            //Directory.Delete(App.audioHandler.RecordingPath);
            //Remove all image
            App.Drugs.GetItems().Select(kp => kp.Key).ToList().ForEach(drug =>
            {
                if (!String.IsNullOrEmpty(drug.ImageFile)) File.Delete(drug.ImageFile);
            });

            //Remove all data
            MessagingCenter.Send(this, "ClearReminder");
            MessagingCenter.Send(this, "ClearDrug");

            //Restoring data
            App.firebase.drugs.ForEach(d => Debug.WriteLine(d.Name));
            App.firebase.drugs.ForEach(drug => MessagingCenter.Send(this, "AddDrug", drug));
            App.firebase.reminders.ForEach(reminder => MessagingCenter.Send(this, "AddReminder", reminder));


            App.firebase.RedownloadFiles();

            await DisplayAlert("Restoration", "Restore completed", "Done");



        }
    }

}