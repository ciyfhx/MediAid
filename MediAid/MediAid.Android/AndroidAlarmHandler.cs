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

[assembly: Dependency(typeof(AndroidAlarmHandler))]
namespace MediAid.Droid
{
    [BroadcastReceiver]
    public class AndroidAlarmHandler : BroadcastReceiver, AlarmHandler
    {


        public override void OnReceive(Context context, Intent intent)
        {
            Notification.Builder builder = new Notification.Builder(context)
                .SetContentTitle("Sample Notification")
                .SetContentText("Hello World! This is my first notification!")
                .SetSmallIcon(Resource.Drawable.icon);

            Notification notification = builder.Build();

            NotificationManager notificationManager =
                context.GetSystemService(Context.NotificationService) as NotificationManager;

            const int notificationId = 0;
            notificationManager.Notify(notificationId, notification);

            Intent ringIntent = new Intent(context, typeof(RingtoneService));

            context.StartService(ringIntent);


        }

        public bool CreateAlarm(Reminder reminder)
        {
            Context context = Android.App.Application.Context;
            AlarmManager alarmManager = context.GetSystemService(Context.AlarmService) as AlarmManager;
            Intent intent = new Intent(context, this.Class);
            PendingIntent pendingIntent = PendingIntent.GetBroadcast(context, 0, intent, 0);

            alarmManager.SetExactAndAllowWhileIdle(AlarmType.ElapsedRealtimeWakeup, reminder.Hours*10, pendingIntent);

            return true; 

        }

        public bool RemoveAlarm(Reminder reminder)
        {
            throw new NotImplementedException();
        }
    }

    [Service]
    public class RingtoneService : Service
    {

        public MediaPlayer MediaPlayer;



        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            MediaPlayer = MediaPlayer.Create(this, Resource.Raw.ringtone);
            MediaPlayer.Start();

            return StartCommandResult.Sticky;

        }


        public override IBinder OnBind(Intent intent)
        {

            return null;
        }
    }

}