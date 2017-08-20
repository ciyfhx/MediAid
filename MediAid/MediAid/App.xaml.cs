using MediAid.Helpers;
using MediAid.Views;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace MediAid
{
    public partial class App : Application
    {

        private StoreDictionaryHandler StoreDictionaryHandler;
        public static readonly RemindersStoreDictionary Reminders = new RemindersStoreDictionary();
        public static readonly DrugsStoreDictionary Drugs = new DrugsStoreDictionary();


        public App()
		{
            //Test
            InitializeComponent();

            StoreDictionaryHandler = new StoreDictionaryHandler(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Recordings/Reminders.db"));
            StoreDictionaryHandler.AddStoreDictionary(Reminders);
            StoreDictionaryHandler.AddStoreDictionary(Drugs);

            //Testing
            Drugs.GetItems().ToList().ForEach(pair =>
            {
                Debug.WriteLine($"{pair.Key}, {pair.Value}");

            });

			SetMainPage();
		}

		public static void SetMainPage()
		{
            Current.MainPage = new TabbedPage
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
        }
	}
}
