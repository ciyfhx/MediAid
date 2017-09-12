using MediAid.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediAid.Services
{
    public interface AlarmHandler
    {

        bool CreateAlarm(Reminder reminder);
        bool RemoveAlarm(Reminder reminder);


    }
}
