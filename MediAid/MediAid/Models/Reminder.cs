using System;
using System.Collections.Generic;
using System.Text;
using MediAid.Models;
using System.ComponentModel;
using SQLiteNetExtensions.Attributes;
using SQLite;
using SQLiteNetExtensions.Extensions;

namespace MediAid.Models
{
    //public enum ReminderType
    //{
    //    RemindOnce, RemindDaily, RemindWeekly
    //}

    public sealed class Reminder : BaseModel, INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement]
        public int ReminderId { get; set; }

        public string Name { get; set; }

        public string RecordId { get; set; }

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

        public DateTime TimeCreated { get; set; }
        [ManyToMany(typeof(ReminderDrug), CascadeOperations = CascadeOperation.All)]
        public List<Drug> Drugs { get; set; }
        //private ReminderType type;



        public event PropertyChangedEventHandler PropertyChanged;



    }
}
