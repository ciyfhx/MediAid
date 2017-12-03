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
using MediAid.Helpers;
using System.Runtime.CompilerServices;

namespace MediAid.Models
{

    public sealed class Reminder : BaseModel, INotifyPropertyChanged
    {
        [PrimaryKey]
        public int ReminderId { get; set; }

        private string name;
        public string Name {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        public string RecordId { get; set; }

        //public ReminderType Type { get => type; set => type = value; }

        private int hours;
        public int Hours
        {
            get => hours;
            set
            {
                hours = value;
                OnPropertyChanged();
            }
        }
        private int mins;
        public int Mins
        {
            get => mins;
            set
            {
                mins = value;
                OnPropertyChanged();
            }
        }

        public bool IsEnabled { get; set; }
        private DateTime timeEnabled;
        public DateTime TimeEnabled { get => timeEnabled; set {

                timeEnabled = value;
                OnPropertyChanged();

            } }
        public TimeSpan Time { get; set; }
        [ManyToMany(typeof(ReminderDrug), CascadeOperations = CascadeOperation.All)]
        public List<Drug> Drugs { get; set; }


        public DateTime Date { get; set; }


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

        #region INotifyPropertyChanged
        /// <summary>
        /// Raises the property changed event.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        public void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        } 
        #endregion



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
