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

[assembly: Dependency(typeof(AndroidAlarmHandler))]
namespace MediAid.Droid
{
    [BroadcastReceiver(Name = "com.ciyfhx.MediAid.receiver", Enabled = true)]
    public class AlarmReceiver : BroadcastReceiver
    {
        private Intent ringIntent;

        public override void OnReceive(Context context, Intent intent)
        {
            //System.Diagnostics.Debug.WriteLine("Ringing");

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


    public class AndroidAlarmHandler : AlarmHandler
    {


       

        public bool CreateAlarm(Reminder reminder)
        {
            Context context = Android.App.Application.Context;
            AlarmManager alarmManager = context.GetSystemService(Context.AlarmService) as AlarmManager;
            Intent intent = new Intent(context, Class.FromType(typeof(AlarmReceiver)));

            intent.PutExtra("Title", reminder.Name);
            intent.PutExtra("ReminderId", reminder.ReminderId);
            intent.PutExtra("PlayOrEnd", true);

            PendingIntent pendingIntent = PendingIntent.GetBroadcast(context, reminder.ReminderId, intent, PendingIntentFlags.UpdateCurrent);

            alarmManager.SetExactAndAllowWhileIdle(AlarmType.RtcWakeup, reminder.Hours * 10, pendingIntent);
            System.Diagnostics.Debug.WriteLine($"Alarm Created {reminder.Name}, {reminder.ReminderId}");

            return true;

        }

        public bool RemoveAlarm(Reminder reminder)
        {
            Context context = Android.App.Application.Context;
            AlarmManager alarmManager = context.GetSystemService(Context.AlarmService) as AlarmManager;
            Intent intent = new Intent(context, Class.FromType(typeof(AlarmReceiver)));
            intent.PutExtra("PlayOrEnd", false);
            context.SendBroadcast(intent);

            PendingIntent pendingIntent = PendingIntent.GetBroadcast(
                context, reminder.ReminderId, intent, PendingIntentFlags.UpdateCurrent);

            alarmManager.Cancel(pendingIntent);
            System.Diagnostics.Debug.WriteLine($"Alarm Removed {reminder.Name}, {reminder.ReminderId}");
            return true;
        }
    }

    [Service]
    public class RingtoneService : Service
    {

        public MediaPlayer MediaPlayer;


        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            Context context = Android.App.Application.Context;
            if (intent.GetBooleanExtra("PlayOrEnd", false) && MediaPlayer == null)
            {
                var title = intent.GetStringExtra("Title");
                var ReminderId = intent.GetIntExtra("ReminderId", -1);
                App.ShowReminder = ReminderId;
                Notification.Builder builder = new Notification.Builder(context)
                    .SetContentTitle(title)
                    .SetOngoing(true)
                    .SetAutoCancel(true)
                    .SetContentText("Time to take your medications!" + title)
                    .SetSmallIcon(Resource.Drawable.icon);

                Notification notification = builder.Build();

                Intent launchIntent = new Intent(context, typeof(MainActivity));
                notification.ContentIntent = PendingIntent.GetActivity(context, 0, launchIntent, 0);

                NotificationManager notificationManager =
                    context.GetSystemService(Context.NotificationService) as NotificationManager;


                const int notificationId = 0;
                notificationManager.Notify(notificationId, notification);


                MediaPlayer = MediaPlayer.Create(this, Resource.Raw.ringtone);
                MediaPlayer.Looping = true;
                MediaPlayer.Start();

            }
            else if (MediaPlayer != null)
            {
                System.Diagnostics.Debug.WriteLine("Stop");
                MediaPlayer.Pause();
                MediaPlayer.Stop();
                MediaPlayer.Reset();
                MediaPlayer = null;
            }
            return StartCommandResult.Sticky;

        }

        public override IBinder OnBind(Intent intent)
        {

            return null;
        }
    }
}