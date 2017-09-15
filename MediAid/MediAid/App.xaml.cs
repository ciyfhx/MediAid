using MediAid.Helpers;
using MediAid.Models;
using MediAid.Services;
using MediAid.Views;
using SQLiteNetExtensions.Extensions;
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
        // Too many stuffs
        //SQL Stores
        public static readonly StoreDictionaryHandler StoreDictionaryHandler = new StoreDictionaryHandler(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Recordings/Reminders.db"));
        public static readonly RemindersStoreDictionary Reminders = new RemindersStoreDictionary();
        public static readonly DrugsStoreDictionary Drugs = new DrugsStoreDictionary();
        public static readonly SettingsStoreDictionary SettingsDictionary = new SettingsStoreDictionary();

        //Dependency Services
        public static readonly FirebaseConnection firebase = DependencyService.Get<FirebaseConnection>();
        public static readonly AudioHandler audioHandler = DependencyService.Get<AudioHandler>();
        public static readonly AlarmHandler alarmHandler = DependencyService.Get<AlarmHandler>();

        //Settings information
        public static Settings Settings { get => settings; }
        private static Settings settings;

        private bool IsRunning = false;

        public App(int ReminderId) : this()
        {
            Debug.WriteLine(ReminderId);

            if (ReminderId != -1)
            {
                //Go to details page
                var rootMasterPage = new RootMasterPage();
                Current.MainPage = rootMasterPage;
                Debug.WriteLine(firebase.IsLogin);
                rootMasterPage.Detail.Navigation.PushAsync(new ReminderDetails(Reminders.GetItems().Keys.First(k => k.ReminderId == ReminderId)));
            }
        }

        public App()
		{
            InitializeComponent();

            StoreDictionaryHandler.AddStoreDictionary(Reminders);
            StoreDictionaryHandler.AddStoreDictionary(Drugs);
            StoreDictionaryHandler.AddStoreDictionary(SettingsDictionary);

            //Set the settings
            settings = StoreDictionaryHandler.db.Table<Settings>().First();

            //Testing
            Reminders.GetItems().ToList().ForEach(pair =>
            {
                Debug.WriteLine($"TEST {pair.Key}, {pair.Value}");
                pair.Key.Drugs = App.StoreDictionaryHandler.db.GetWithChildren<Reminder>(pair.Key.ReminderId).Drugs;
                Debug.WriteLine($"{pair.Key.ToJson()}");

            });
            InitAudioHandler();

            SetPage();
            IsRunning = true;

        }

		public async void SetPage()
		{
            if (!firebase.IsLogin)
            {
                Debug.WriteLine("To Login Page");
                //Let the Login Page handle the login
                Current.MainPage = new NavigationPage(new LoginPage());
            }else
            {
                Debug.WriteLine("Automatically Login");
                await firebase.LoginUserAsync(settings.Username, settings.Password);
            }
        }

        public void InitAudioHandler()
        {
            string path = EnvironmentUtils.GetPlatformEnironmentPath() + "/Recordings";
            Directory.CreateDirectory(path);
            audioHandler.Init(path);

        }



    }
}
