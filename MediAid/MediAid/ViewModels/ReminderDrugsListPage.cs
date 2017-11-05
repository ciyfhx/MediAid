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
        public Reminder Reminder { get; set; }

        public override List<Drug> GetData()
        {
            //reminder.Drugs.ForEach(drug => list.Add(drug.ToDrug()));
            return Reminder.Drugs;
        }


        public ReminderDrugsListPage(Reminder reminder) : base()
        {
            this.Reminder = reminder;
        }

    }
}
