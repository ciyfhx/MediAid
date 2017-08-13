using System;
using System.Collections.Generic;
using System.Text;
using MediAid.Models;

namespace MediAid.Models
{
    public enum ReminderType
    {
        RemindOnce, RemindDaily, RemindWeekly
    }

    public sealed class Reminder : BaseModel
    {
        public string Name { get; set; }
        public string Message { get; set; }
        public ReminderType Type { get => type; set => type = value; }

        public DateTime TimeCreated;
        private ReminderType type;

        public Reminder()
        {
            TimeCreated = DateTime.Now;
        }
        



    }

}
