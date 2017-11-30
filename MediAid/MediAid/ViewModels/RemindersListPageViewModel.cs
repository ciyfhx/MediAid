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
using System.Windows.Input;
using Xamarin.Forms;

namespace MediAid.ViewModels
{



    class RemindersListPageViewModel : LoadData<Reminder>
    {

        public bool IsRefreshing { get; set; }

        public ICommand RefreshCommand
        {
            get
            {
                return new Command(() =>
                {
                    IsRefreshing = true;

                    LoadItemsCommand.Execute(null);

                    IsRefreshing = false;
                });
            }
        }


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

            //Clear reminder
            MessagingCenter.Subscribe<SettingsPage>(this, "ClearReminder", (obj) =>
            {
                Items.Clear();
            });
            MessagingCenter.Subscribe<RecordReminder, Reminder>(this, "UpdateReminder", (obj, reminder) => {
                OnPropertyChanged();
                reminder.OnPropertyChanged();
                
            });

        }


        /// <summary>
        /// We need to do a hackaround in order to update the list view since Xamarin does not support nested list view
        /// </summary>
        private void UpdateListViewReminder(Reminder reminder)
        {
            //Create a new list
            int i = Items.IndexOf(reminder);
            Items.RemoveAt(i);
            Items.Insert(i, reminder);

            //Items.Remove(reminder);
           // Items.Add(reminder);


        }

        public override IEnumerable<Reminder> GetData()
        {
            return App.Reminders.GetItems().Keys.ToList();
        }
    }

}

