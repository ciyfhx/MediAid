using MediAid.Helpers;
using MediAid.Models;
using MediAid.Services;
using MediAid.Views;
using SQLiteNetExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MediAid.ViewModels
{
    class ReminderDrugsListPage : LoadData<Drug>
    {
        private Reminder reminder;


        public override List<Drug> GetData()
        {
            //reminder.Drugs.ForEach(drug => list.Add(drug.ToDrug()));
            return App.StoreDictionaryHandler.db.GetWithChildren<Reminder>(reminder.ReminderId).Drugs;
        }


        public ReminderDrugsListPage(Reminder reminder) : base()
        {
            this.reminder = reminder;
        }

    }
}
