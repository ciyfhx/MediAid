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
        public static readonly StoreDictionaryHandler StoreDictionaryHandler = new StoreDictionaryHandler(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Recordings/Reminders.db"));
        public static readonly RemindersStoreDictionary Reminders = new RemindersStoreDictionary();
        public static readonly DrugsStoreDictionary Drugs = new DrugsStoreDictionary();
        //public static readonly SettingsStoreDictionary SettingsDictionary = new SettingsStoreDictionary();

        public static readonly FirebaseConnection firebase = DependencyService.Get<FirebaseConnection>();
        public static readonly AudioHandler audioHandler = DependencyService.Get<AudioHandler>();
        public static readonly AlarmHandler alarmHandler = DependencyService.Get<AlarmHandler>();

        public static Settings Settings { get => settings; }
        private static Settings settings;

        public static int ShowReminder { get; set; } = -1;


        public App()
		{
            InitializeComponent();

            StoreDictionaryHandler.AddStoreDictionary(Reminders);
            StoreDictionaryHandler.AddStoreDictionary(Drugs);
            //StoreDictionaryHandler.AddStoreDictionary(SettingsDictionary);

            //Set the settings
            //settings = StoreDictionaryHandler.db.Table<Settings>().First();

            //Testing
            Reminders.GetItems().ToList().ForEach(pair =>
            {
                Debug.WriteLine($"TEST {pair.Key}, {pair.Value}");
                pair.Key.Drugs = App.StoreDictionaryHandler.db.GetWithChildren<Reminder>(pair.Key.ReminderId).Drugs;
                Debug.WriteLine($"{pair.Key.ToJson()}");

            });
            InitAudioHandler();
            firebase.Init();

			SetMainPage();
		}

		public void SetMainPage()
		{
            //Current.MainPage = new NavigationPage(new Login());
            var masterDetail = new RootMasterPage();

            Current.MainPage = masterDetail;

            Debug.WriteLine(ShowReminder);
            if (ShowReminder != -1)
            {
                Current.MainPage.Navigation.PushAsync(new ReminderDetails(Reminders.GetItems().Keys.First(k => k.ReminderId == ShowReminder)));
                ShowReminder = -1;
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
