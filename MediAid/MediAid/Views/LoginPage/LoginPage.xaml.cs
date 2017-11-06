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
                if(s.Equals(CreateAccountLbl))Navigation.PushAsync(new CreateAccountPage());
                else if(s.Equals(ForgotPasswordLbl)) Navigation.PushAsync(new ForgotPasswordPage());
            };
            CreateAccountLbl.GestureRecognizers.Add(tap);
            ForgotPasswordLbl.GestureRecognizers.Add(tap);



        }

        public bool Validation()
        {
            return !(String.IsNullOrEmpty(username) && String.IsNullOrEmpty(password));
        }

        public async void Login_ToFirebase(object sender, EventArgs e)
        { 
            if (!Validation()) return;
            bool connected = await LoginAsync(this.username.Trim(), this.password);

            if (connected)
            {
                App.Current.MainPage = new RootMasterPage();

                //Set the username and password for next automatic login
                var settings = App.Settings;
                settings.IsLogin = true;

                App.Settings.SaveCredentials(this.Username.Trim(), this.Password);

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
                Warning.Text = "";
                bool connected = await App.firebase.LoginUserAsync(username.Trim(), password);
                return connected;
            }
            catch (Exception)
            {

                return false;
            }
        }




    }
}