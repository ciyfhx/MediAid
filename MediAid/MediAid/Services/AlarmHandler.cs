using MediAid.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediAid.Services
{
    public interface AlarmHandler
    {

        bool CreateAlarm(Reminder reminder, long millis);
        bool RemoveAlarm(Reminder reminder);
    }

    public class AlarmUtils
    {
        /// <summary>
        /// Get next ring in millis second base on repeating count
        /// First repeating count will return base on the starting time
        /// Second repeating and onward will return base interval
        /// </summary>
        /// <param name="reminder"></param>
        /// <param name="fromTime"></param>
        /// <returns>millis second for the next alarm rings</returns>
        public static long NextTimeMillis(Reminder reminder, DateTime fromTime)
        {

            int cHours = 0, cMins = 0;

            if (reminder.RepeatingCount == 0)
            {
                int diffHours = reminder.Time.Hours - fromTime.Hour;
                int diffMins = reminder.Time.Minutes - fromTime.Minute;


                cHours = (diffHours <= 0 && diffMins < 0) ? (24 + diffHours) : diffHours;

                cMins = (diffMins < 0) ? (60 + diffMins) : diffMins;

                if (cMins > 0 && cHours > 0) cHours--;
            }

#if DEBUG
            reminder.NextRingMillis =  1000 * reminder.Hours;

#else
            reminder.NextRingMillis = (((cHours + ((reminder.RepeatingCount > 0) ? reminder.Hours : 0))) * 1000 * 60 * 60) + ((cMins + ((reminder.RepeatingCount > 0) ? reminder.Mins : 0)) * 60 * 1000);
#endif
            return reminder.NextRingMillis;


        }
    }

}
