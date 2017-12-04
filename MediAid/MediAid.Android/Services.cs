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
using Android.Media;

namespace MediAid.Droid
{
    class Services
    {


        [Service]
        public class RingtoneService : Service
        {

            public MediaPlayer MediaPlayer;
            public NotificationManager notificationManager;

            public Vibrator vibrator;

            private const int notificationId = 0;


            public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
            {
                Context context = Android.App.Application.Context;
                if (intent.GetBooleanExtra("PlayOrEnd", false) && MediaPlayer == null)
                {
                    var title = intent.GetStringExtra("Title");
                    var ReminderId = intent.GetIntExtra("ReminderId", -1);

                    Notification.Builder builder = new Notification.Builder(context)
                        .SetContentTitle(title)
                        .SetOngoing(true)
                        .SetAutoCancel(true)
                        .SetContentText("Time to take your medications!")
                        .SetSmallIcon(Resource.Drawable.icon);

                    Notification notification = builder.Build();

                    Intent launchIntent = new Intent(context, typeof(MainActivity));
                    launchIntent.PutExtra("ReminderId", ReminderId);
                    notification.ContentIntent = PendingIntent.GetActivity(context, 0, launchIntent, 0);

                    notificationManager =
                        context.GetSystemService(Context.NotificationService) as NotificationManager;



                    notificationManager.Notify(notificationId, notification);

                    long[] pattern = { 0, 400, 1000 };

                    vibrator = (Vibrator)context.GetSystemService(Context.VibratorService);
                    vibrator.Vibrate(pattern, 0);

                    MediaPlayer = MediaPlayer.Create(this, Resource.Raw.ringtone);
                    MediaPlayer.Looping = true;
                    MediaPlayer.Start();

                }

                return StartCommandResult.Sticky;

            }

            public override void OnDestroy()
            {
                if (MediaPlayer != null)
                {
                    System.Diagnostics.Debug.WriteLine("Stop");
                    MediaPlayer.Pause();
                    MediaPlayer.Stop();
                    MediaPlayer.Reset();
                    MediaPlayer = null;
                    vibrator.Cancel();
                    notificationManager.Cancel(notificationId);
                }
            }

            public override IBinder OnBind(Intent intent)
            {

                return null;
            }
        }


    }
}