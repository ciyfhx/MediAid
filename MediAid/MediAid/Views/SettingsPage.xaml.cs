using System;
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

        public bool IsAccountVerified { get; } = App.firebase.IsAccountVerified();

        public ImageSource ProfilePicture
        {
            get
            {
                string uri = App.firebase.GetProfilePicture();
                return !String.IsNullOrEmpty(uri) ? ImageSource.FromUri(new Uri(uri)) : ImageSource.FromResource("profile.jpg");
            }
        }

        public SettingsPage ()
		{
			InitializeComponent ();

            this.BindingContext = this;

        }

        protected override void OnAppearing()
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

        }

        private void Restore(object sender, EventArgs e)
        {
            //Remove all recording
            DirectoryInfo di = new DirectoryInfo(App.audioHandler.RecordingPath);
            //di.GetFiles().ToList().ForEach(file => file.Delete());
            //Directory.Delete(App.audioHandler.RecordingPath);
            //Remove all image
            App.Drugs.GetItems().Select(kp => kp.Key).ToList().ForEach(drug => File.Delete(drug.ImageFile));

            //Remove all data
            MessagingCenter.Send(this, "ClearReminder");
            MessagingCenter.Send(this, "ClearDrug");

        }
    }

}