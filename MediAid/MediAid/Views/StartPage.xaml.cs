using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Xamarin.Forms;
using System.Diagnostics;

namespace MediAid.Views
{
    public  partial class StartPage : LoadingPage
    {

        public StartPage() : base()
        {
            
        }

        public override void LoadingTask()
        {

        }


        public async void SetPage_AndLogin()
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
}