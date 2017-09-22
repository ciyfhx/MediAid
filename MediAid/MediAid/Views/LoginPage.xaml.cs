using MediAid.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MediAid.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
        private string username;

        public string Username { get => username; set => username = value; }

        private string password;

        public string Password { get => password; set => password = value; }


        public LoginPage ()
		{


            Title = "Login";

			InitializeComponent ();

            BindingContext = this;


		}

        //protected async override void OnAppearing()
        //{
        //    var settings = App.Settings;

        //    if (settings.FirstLogin && !String.IsNullOrEmpty(settings.Username) && !String.IsNullOrEmpty(settings.Password))
        //    {
        //        Debug.WriteLine($"Logging in as {settings.Username}");
        //        var connected = await LoginAsync(settings.Username, settings.Password);
        //        if (connected) App.Current.MainPage = new RootMasterPage();
        //    }
        //}

        public async void Login_ToFirebase(object sender, EventArgs e)
        {
            bool connected = await LoginAsync(this.username, this.password);

            if (connected)
            {
                App.Current.MainPage = new RootMasterPage();

                //Set the username and password for next automatic login
                var settings = App.Settings;
                settings.FirstLogin = true;
                settings.Username = this.username;
                settings.Password = this.Password;

            }
            else
            {
                Warning.Text = "Incorrect username or password!";
            }


        }

        private async Task<bool> LoginAsync(string username, string password)
        {
            bool connected = await App.firebase.LoginUserAsync(username, password);
            return connected;

        }


    }
}