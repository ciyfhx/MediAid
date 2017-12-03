using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace MediAid.Views
{
    public class MainPageMenuItem : RootMasterPageMenuItem
    {
        public override Page CreatePage()
        {
            return new TabbedPage
            {
                Children =
                {
                   new RemindersListPage()
                    {
                        Title = "Reminders",
                        Icon = Device.OnPlatform<string>("tab_feed.png",null,null)
                   },
                   new PillsListPage()
                   {
                        Title = "Pills",
                        Icon = Device.OnPlatform<string>("tab_about.png",null,null)
                    },
                    new AboutPage()
                    {
                        Title = "About",
                        Icon = Device.OnPlatform<string>("tab_about.png",null,null)
                    },
                }
            };
        }
    }

    public class LogInPageMenuItem : RootMasterPageMenuItem
    {
        public override Page CreatePage()
        {
            App.Current.MainPage = new NavigationPage(new LoginPage());
            return null;
        }
    }

    public class LogOutPageMenuItem : RootMasterPageMenuItem
    {
        public override Page CreatePage()
        {
            App.Current.MainPage = new LogOutPage();
            return null;
        }
    }
    public class SettingsPageMenuItem : RootMasterPageMenuItem
    {
        public override Page CreatePage()
        {
            return new SettingsPage();
        }
    }


}
