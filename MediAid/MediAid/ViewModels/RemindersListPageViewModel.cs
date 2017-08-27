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
    class RemindersListPageViewModel : BaseViewModel
    {

        public ObservableRangeCollection<Reminder> Reminders { get; set; }
        public Command LoadRemindersCommand { get; set; }

        public LoadDataStore<Reminder> RemindersDataStore = new LoadDataStore<Reminder>();

        private List<Reminder> LoadReminders()
        {
            return App.Reminders.GetItems().Keys.ToList();
        }

        public RemindersListPageViewModel()
        {
            //Load Reminders
            RemindersDataStore.LoadDataHandler = LoadReminders;


            Title = "Reminders";
            Reminders = new ObservableRangeCollection<Reminder>();

            //Get new reminder
            MessagingCenter.Subscribe<PillsSelectListPage, Reminder>(this, "AddReminder", (obj, reminder) => {
                Reminders.Add(reminder);
            });

            LoadRemindersCommand = new Command(async () => await ExecuteLoadRemindersCommand());

        }

        async Task ExecuteLoadRemindersCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Reminders.Clear();
                var _Reminders = await RemindersDataStore.GetItemsAsync(true);
                Reminders.ReplaceRange(_Reminders);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessagingCenter.Send(new MessagingCenterAlert
                {
                    Title = "Error",
                    Message = "Unable to load Reminders.",
                    Cancel = "OK"
                }, "message");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }

}

