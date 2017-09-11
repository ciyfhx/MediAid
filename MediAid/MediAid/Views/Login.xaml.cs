using MediAid.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MediAid.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Login : ContentPage
	{
        private string username;

        public string Username { get => username; set => username = value; }

        private string password;

        public string Password { get => password; set => password = value; }


        public Login ()
		{
            Title = "Login";

			InitializeComponent ();

            BindingContext = this;

		}

        public async void Login_ToFirebase(object sender, EventArgs e)
        {



            bool connected = await App.firebase.LoginUser("test@testing.com", "password");


            if (connected)
            {
                App.Current.MainPage = new TabbedPage
                {
                    Children =
                {
                    new NavigationPage(new RemindersListPage())
                    {
                        Title = "Reminders",
                        Icon = Device.OnPlatform<string>("tab_feed.png",null,null)
                   },
                    new NavigationPage(new PillsListPage())
                   {
                        Title = "Pills",
                        Icon = Device.OnPlatform<string>("tab_about.png",null,null)
                    },
                    new NavigationPage(new AboutPage())
                    {
                        Title = "About",
                        Icon = Device.OnPlatform<string>("tab_about.png",null,null)
                    },
                }
                };
            }else
            {
                Warning.Text = "Incorrect username or password!";
            }


        }

	}
}