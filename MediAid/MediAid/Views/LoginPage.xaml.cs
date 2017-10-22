using Firebase.Auth;
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

            //Hyperlink
            var tap = new TapGestureRecognizer();
            tap.Tapped += (s, e) =>
            {
                Navigation.PushAsync(new CreateAccountPage());
            };
            CreateAccountLbl.GestureRecognizers.Add(tap);



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

                App.Settings.SaveCredentials(this.Username, this.Password);

                //settings.Username = this.username;
                //settings.Password = this.Password;

            }
            else
            {
                Warning.Text = "Incorrect username or password!";
            }


        }

        private async Task<bool> LoginAsync(string username, string password)
        {
            try
            {
                bool connected = await App.firebase.LoginUserAsync(username, password);
                return connected;
            }
            catch(FirebaseAuthInvalidCredentialsException e)
            {
                Debug.WriteLine(e.Message);
            }
            return false;
        }


    }
}