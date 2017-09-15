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
            

            alarmManager.SetExactAndAllowWhileIdle(AlarmType.ElapsedRealtimeWakeup, reminder.Hours * 1000, pendingIntent);
            //alarmManager.SetRepeating(AlarmType.Rtc, reminder.Hours* 1000, AlarmManager.IntervalDay, pendingIntent);
            System.Diagnostics.Debug.WriteLine($"Alarm Created {reminder.Name}, {reminder.ReminderId}");

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

            System.Diagnostics.Debug.WriteLine($"Alarm Removed {reminder.Name}, {reminder.ReminderId}");
            return true;
        }
    }

}