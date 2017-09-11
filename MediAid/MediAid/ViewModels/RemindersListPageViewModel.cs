using MediAid.Helpers;
using MediAid.Models;
using MediAid.Services;
using MediAid.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MediAid.ViewModels
{



    class RemindersListPageViewModel : LoadData<Reminder>
    {

        public Reminder SelectedItem { get; set; }

        public RemindersListPageViewModel()
        {

            Title = "Reminders";
            //Get new reminder
            MessagingCenter.Subscribe<RecordReminder, Reminder>(this, "AddReminder", (obj, reminder) => {
                Items.Add(reminder);
            });
            //Remove reminder
            MessagingCenter.Subscribe<ReminderDetails, Reminder>(this, "RemoveReminder", (obj, reminder) => {
                RemoveItem(reminder);
            });
        }

        public override List<Reminder> GetData()
        {
            return App.Reminders.GetItems().Keys.ToList();
        }
    }

}

