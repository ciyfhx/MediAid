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
using Xamarin.Forms;
using MediAid.Droid;
using MediAid.Services;
using MediAid.Models;
using Android.Media;
using Android;
using Android.Content.Res;
using MediAid.Views;
using Android.App.Job;
using Java.Lang;
using Android.Support.V4.Content;
using Java.Util;
using MediAid.Helpers;
using static System.Diagnostics.Debug;

[assembly: Dependency(typeof(AndroidAlarmHandler))]
namespace MediAid.Droid
{
    public class AndroidAlarmHandler : AlarmHandler
    {



        public bool CreateAlarm(Reminder reminder)
        {
            Context context = Android.App.Application.Context;
            AlarmManager alarmManager = context.GetSystemService(Context.AlarmService) as AlarmManager;
            Intent intent = new Intent(context, typeof(Services.RingtoneService));

            intent.PutExtra("Title", reminder.Name);
            intent.PutExtra("ReminderId", reminder.ReminderId);
            intent.PutExtra("PlayOrEnd", true);

            PendingIntent pendingIntent = PendingIntent.GetService(context, reminder.ReminderId, intent, PendingIntentFlags.UpdateCurrent);

            DateTime now = DateTime.Now;
            //Get Change in time
            System.Diagnostics.Debug.WriteLine($"{reminder.Time.Hours}, {now.Hour}");

            int cHours = 0, cMins = 0;

            if (reminder.RepeatingCount==0)
            {
                int diffHours = reminder.Time.Hours - now.Hour;
                int diffMins = reminder.Time.Minutes - now.Minute;

                cHours = (diffHours < 0) ? (24 + diffHours) : diffHours;
                cMins = (diffMins < 0) ? (24 + diffMins) : diffMins;
            }

#if DEBUG
            long millis = 1000 * reminder.Hours;

#else
            long millis = ((cHours + reminder.Hours) * 1000 * 60 * 60) + ((cMins + reminder.Mins) * 60 * 1000);
#endif

            alarmManager.SetExactAndAllowWhileIdle(AlarmType.RtcWakeup, Java.Lang.JavaSystem.CurrentTimeMillis() + millis, pendingIntent);

            Toast.MakeText(context, $"Alarm Created On {now.AddMilliseconds(millis).ToString("d MMM (ddd), h:mm tt")}", ToastLength.Long).Show();

            //alarmManager.SetRepeating(AlarmType.Rtc, reminder.Hours* 1000, AlarmManager.IntervalDay, pendingIntent);
            WriteLine($"Alarm Created {reminder.Name}, {reminder.ReminderId}");

            return true;

        }

        public bool RemoveAlarm(Reminder reminder)
        {
            Context context = Android.App.Application.Context;
            AlarmManager alarmManager = context.GetSystemService(Context.AlarmService) as AlarmManager;
            Intent intent = new Intent(context, typeof(Services.RingtoneService));
            intent.PutExtra("Title", reminder.Name);
            context.StopService(intent);

            PendingIntent pendingIntent = PendingIntent.GetService(
                context, reminder.ReminderId, intent, 0);
            

            alarmManager.Cancel(pendingIntent);

            WriteLine($"Alarm Removed {reminder.Name}, {reminder.ReminderId}");
            return true;
        }
    }

    


}