﻿using System;
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


    }
}