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

        public static readonly string DATABASE = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Recordings/Reminders.db");

        // Too many stuffs
        //SQL Stores
        public static readonly StoreDictionaryHandler StoreDictionaryHandler = new StoreDictionaryHandler(DATABASE);
        public static readonly RemindersStoreDictionary Reminders = new RemindersStoreDictionary();
        //public static readonly TimingStoreDictionary Timing = new TimingStoreDictionary();
        public static readonly DrugsStoreDictionary Drugs = new DrugsStoreDictionary();
        public static readonly SettingsStoreDictionary SettingsDictionary = new SettingsStoreDictionary();

        //Dependency Services
        public static readonly FirebaseConnection firebase = DependencyService.Get<FirebaseConnection>();
        public static readonly AudioHandler audioHandler = DependencyService.Get<AudioHandler>();
        public static readonly AlarmHandler alarmHandler = DependencyService.Get<AlarmHandler>();

        //Settings information
        public static Settings Settings { get => settings; }
        private static Settings settings;

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
            //StoreDictionaryHandler.AddStoreDictionary(Timing);
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

            MainPage = new StartPage();

        }



        public void InitAudioHandler()
        {
            string path = EnvironmentUtils.GetPlatformEnironmentPath() + "/Recordings";
            Directory.CreateDirectory(path);
            audioHandler.Init(path);

        }



    }
}
