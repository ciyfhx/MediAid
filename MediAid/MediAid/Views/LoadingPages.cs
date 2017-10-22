using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Xamarin.Forms;
using System.Diagnostics;

namespace MediAid.Views
{
    public class StartPage : LoadingPage
    {

        public StartPage() : base() => this.LoadingText = "Welcome to MediAid, Your own medications reminder.";

        public async override void LoadingTask()
        {
            var firebase = App.firebase;
            var settings = App.Settings;
            if (!firebase.IsLogin)
            {

                //There is login information for us to do automatic login
                if (settings.FirstLogin && !String.IsNullOrEmpty(settings.Username) && !String.IsNullOrEmpty(settings.Password))
                {
                    Debug.WriteLine($"Logging in as {settings.Username}");
                    var connected = await firebase.LoginUserAsync(settings.Username, settings.Password);
                    if (connected) App.Current.MainPage = new RootMasterPage();
                    //if(connected) NavigationPage.SetHasNavigationBar(this, false);
                }
                else
                {
                    Debug.WriteLine("To Login Page");
                    //Let the Login Page handle the login
                    App.Current.MainPage = new NavigationPage(new LoginPage());
                    //Navigation.PushAsync(new LoginPage());

                }

            }
            //else
            //{
            //    Debug.WriteLine("Automatically Login");
            //    await firebase.LoginUserAsync(settings.Username, settings.Password);
            //}
        }
    }

    public class LogOutPage : LoadingPage
    {
        public LogOutPage() : base() => this.LoadingText = "Logging Out";

        public override void LoadingTask() {
            App.firebase.SignOut();
            App.Settings.FirstLogin = false;
            App.Settings.DeleteCredentials();

            //To Login Page
            App.Current.MainPage = new StartPage();

        }
    }

}