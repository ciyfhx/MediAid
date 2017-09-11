using MediAid.Helpers;
using MediAid.Services;
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

        public static readonly StoreDictionaryHandler StoreDictionaryHandler = new StoreDictionaryHandler(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Recordings/Reminders.db"));
        public static readonly RemindersStoreDictionary Reminders = new RemindersStoreDictionary();
        public static readonly DrugsStoreDictionary Drugs = new DrugsStoreDictionary();

        public static readonly FirebaseConnection firebase = DependencyService.Get<FirebaseConnection>();
        public static readonly AudioHandler audioHandler = DependencyService.Get<AudioHandler>();


        public App()
		{
            InitializeComponent();

            StoreDictionaryHandler.AddStoreDictionary(Reminders);
            StoreDictionaryHandler.AddStoreDictionary(Drugs);

            //Testing
            Reminders.GetItems().ToList().ForEach(pair =>
            {
                Debug.WriteLine($"TEST {pair.Key.Drugs}, {pair.Value}");

            });
            InitAudioHandler();
            firebase.Init();

			SetMainPage();
		}

		public void SetMainPage()
		{
            Current.MainPage = new NavigationPage(new Login());

        }

        public void InitAudioHandler()
        {
            string path = EnvironmentUtils.GetPlatformEnironmentPath() + "/Recordings";
            Directory.CreateDirectory(path);
            audioHandler.Init(path);

        }



    }
}
