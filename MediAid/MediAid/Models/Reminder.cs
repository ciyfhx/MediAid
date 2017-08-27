using System;
using System.Collections.Generic;
using System.Text;
using MediAid.Models;
using System.ComponentModel;

namespace MediAid.Models
{
    //public enum ReminderType
    //{
    //    RemindOnce, RemindDaily, RemindWeekly
    //}

    public sealed class Reminder : BaseModel, INotifyPropertyChanged
    {
        public string Name { get; set; }
        //public ReminderType Type { get => type; set => type = value; }

        private int hours;
        public int Hours
        {
            get => hours;
            set
            {
                hours = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
            }
        }

        public DateTime TimeCreated;
        //private ReminderType type;

        public event PropertyChangedEventHandler PropertyChanged;

        public Reminder()
        {
            TimeCreated = DateTime.Now;
        }
        



    }

}
