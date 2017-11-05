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
        public static long NextTimeMillis(Reminder reminder)
        {
            DateTime nextRing = (reminder.Date - reminder.Date.TimeOfDay).Add(reminder.Time);

            if (reminder.RepeatingCount > 0)
            {

                nextRing = nextRing.AddHours(reminder.Hours * reminder.RepeatingCount);
                nextRing = nextRing.AddMinutes(reminder.Mins * reminder.RepeatingCount);

            }

#if DEBUG
            reminder.NextRingMillis =  1000 * reminder.Hours;

#else
            reminder.NextRingMillis = (long) (nextRing - DateTime.Now).TotalMilliseconds;
#endif
            return reminder.NextRingMillis;


        }
    }

}
