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
        public int ReminderId { get; set; }

        public StartPage() : this(-1)
        {

        }

        public StartPage(int reminderId) : base()
        {
            this.LoadingText = "Welcome to MediAid, Your own medications reminder.";
            this.ReminderId = reminderId;
        }

        public async override void LoadingTask()
        {
            var firebase = App.firebase;
            var settings = App.Settings;
            //There is login information for us to do automatic login
            if (settings.IsLogin && !String.IsNullOrEmpty(settings.Username) && !String.IsNullOrEmpty(settings.Password))
            {
                Debug.WriteLine($"Logging in as {settings.Username}");
                bool connected = (!App.firebase.IsLogin) ? await firebase.LoginUserAsync(settings.Username, settings.Password) : true;
                if (connected)
                {
                    var rootMasterPage = new RootMasterPage();
                    App.Current.MainPage = rootMasterPage;

                    if (ReminderId != -1)
                    {
                        //Update recreate our reminder by firing the event
                        var alarmReminderPage = new AlarmReminderPage(App.Reminders.GetItems().Keys.First(k => k.ReminderId == ReminderId));
                        await rootMasterPage.Detail.Navigation.PushModalAsync(alarmReminderPage);
                    }
                }//if(connected) NavigationPage.SetHasNavigationBar(this, false);
            }
            else
            {
                Debug.WriteLine("To Login Page");
                //Let the Login Page handle the login
                App.Current.MainPage = new NavigationPage(new LoginPage());
                //Navigation.PushAsync(new LoginPage());

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
            App.Settings.IsLogin = false;
            App.Settings.DeleteCredentials();

            //To Login Page
            App.Current.MainPage = new StartPage();

        }
    }

}