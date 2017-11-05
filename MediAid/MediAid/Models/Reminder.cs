using System;
using System.Collections.Generic;
using System.Text;
using MediAid.Models;
using System.ComponentModel;
using SQLiteNetExtensions.Attributes;
using SQLite;
using SQLiteNetExtensions.Extensions;
using Newtonsoft.Json;
using System.IO;
using Xamarin.Forms;

namespace MediAid.Models
{

    public sealed class Reminder : BaseModel, INotifyPropertyChanged
    {
        [PrimaryKey]
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
        private int mins;
        public int Mins
        {
            get => mins;
            set
            {
                mins = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
            }
        }

        public bool IsEnabled { get; set; }
        public DateTime TimeEnabled { get; set; }
        public TimeSpan Time { get; set; }
        [ManyToMany(typeof(ReminderDrug), CascadeOperations = CascadeOperation.All)]
        public List<Drug> Drugs { get; set; }

        //Initiate with AlarmUtils.NextTimeMillis
        public long NextRingMillis = 0;


        private int repeatingCount = 0;
        public int RepeatingCount
        {
            get => repeatingCount;
            set
            {
                repeatingCount = value;
                MessagingCenter.Send(this, "UpdateReminder", this);
            }
        }


        //[OneToMany]
        //public List<Timing> Timings { get; set; }



        public event PropertyChangedEventHandler PropertyChanged;



    }

    public class Timing
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public TimeSpan Time { get; set; }
        public bool IsEnabled { get; set; } = false;
        
    }

    public static class ReminderExtension
    {
        public static string ToJson(this Reminder reminder)
        {
            StringBuilder builder = new StringBuilder();
            StringWriter writer = new StringWriter(builder);
            new JsonSerializer().Serialize(new JsonTextWriter(writer), reminder);
            return writer.ToString();
        }

        public static Reminder FromJson(string json)
        {
            return JsonConvert.DeserializeObject<Reminder>(json);
        }

    }

}
