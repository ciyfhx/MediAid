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



        public bool CreateAlarm(Reminder reminder, long millis)
        {
            Context context = Android.App.Application.Context;
            AlarmManager alarmManager = context.GetSystemService(Context.AlarmService) as AlarmManager;
            Intent intent = new Intent(context, typeof(Services.RingtoneService));

            intent.PutExtra("Title", reminder.Name);
            intent.PutExtra("ReminderId", reminder.ReminderId);
            intent.PutExtra("PlayOrEnd", true);

            PendingIntent pendingIntent = PendingIntent.GetService(context, reminder.ReminderId, intent, PendingIntentFlags.UpdateCurrent);

            DateTime now = DateTime.Now;

            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
            {
                alarmManager.SetExactAndAllowWhileIdle(AlarmType.RtcWakeup, Java.Lang.JavaSystem.CurrentTimeMillis() + millis, pendingIntent);
            }
            else
            {
                alarmManager.Set(AlarmType.RtcWakeup, Java.Lang.JavaSystem.CurrentTimeMillis() + millis, pendingIntent);
            }

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