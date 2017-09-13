using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace MediAid.Views
{
    public class MainPage : RootMasterPageMenuItem
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
}
