using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using static MediAid.Droid.Services;
using MediAid.Helpers;
using static System.Diagnostics.Debug;
using Xamarin.Forms;
using MediAid.Services;
using SQLite;
using MediAid.Models;
using System.IO;

namespace MediAid.Droid
{
    class Receivers
    {
        [BroadcastReceiver(Enabled = true)]
        public class AlarmReceiver : BroadcastReceiver
        {
            private Intent ringIntent;

            public override void OnReceive(Context context, Intent intent)
            {
                //Toast.MakeText(context, "Hello", ToastLength.Short).Show();

                ringIntent = new Intent(context, typeof(RingtoneService));
                var playOrEnd = intent.GetBooleanExtra("PlayOrEnd", false);
                ringIntent.PutExtra("PlayOrEnd", playOrEnd);
                if (playOrEnd)
                {
                    var title = intent.GetStringExtra("Title");
                    var ReminderId = intent.GetIntExtra("ReminderId", -1);
                    ringIntent.PutExtra("Title", title);
                    ringIntent.PutExtra("ReminderId", ReminderId);
                }

                context.StartService(ringIntent);


            }

        }
        [BroadcastReceiver(Enabled = true)]
        [IntentFilter(new[] { Android.Content.Intent.ActionBootCompleted })]
        public class BootReceiver : BroadcastReceiver
        {
            //Have to declare our database path here since we cannot access App.xaml.cs
            private string DATABASE = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Recordings/Reminders.db");

            public override void OnReceive(Context context, Intent intent)
            {
                WriteLine("Setting up alarms");

                var db = new SQLiteConnection(DATABASE);

                var alarmHandler = new AndroidAlarmHandler();

                //Restart alarms
                db.Table<Reminder>().ToList().ForEach(reminder => {
                    if (reminder.IsEnabled) alarmHandler.CreateAlarm(reminder);
                });
                WriteLine("Alarm Restart Complete");
            }
        }

    }
}