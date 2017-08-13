using MediAid.Models;
using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using System.Linq;
using Xamarin.Forms;
using MediAid.Views;
using System.IO;

namespace MediAid.Helpers
{

    public class StoreDictionaryHandler
    {

        SQLiteConnection db;

        /// <summary>
        /// Init the database and must be called before retrieving the data
        /// </summary>
        /// <param name="path">Path to load the datas</param>
        public StoreDictionaryHandler(string path)
        {
            if (!File.Exists(path))
            {
                File.Create(path);
            }
            db = new SQLiteConnection(path);
        }

        private List<Store> stores = new List<Store>();

        public void AddStoreDictionary<K, V>(StoreDictionary<K, V> storeDictionary)
        {
            storeDictionary.CallInit(db);
            stores.Add(storeDictionary);
        }




    }

    public abstract class StoreDictionary<K, V> : Store
    {
        Dictionary<K, V> dictionary = new Dictionary<K, V>();

        public void CallInit(SQLiteConnection db)
        {
            this.Init(db, dictionary);
        }

        protected abstract void Init(SQLiteConnection db, Dictionary<K, V> dictionary);

        ///<summary>
        /// Return a dictionary of the data items
        ///</summary>
        public Dictionary<K, V> GetItems()
        {
            return dictionary;
        }
    }


    public class Store {


        internal Store()
        {

        }

    }

    public class RemindersStoreDictionary : StoreDictionary<Reminder, string>
    {
        protected override void Init(SQLiteConnection db, Dictionary<Reminder, string> remindersPath)
        {

            db.CreateTable<Reminder>();

            //Load reminders to dictionary
            db.Table<Reminder>().ToList().ForEach(reminder => remindersPath.Add(reminder, reminder.Id));

            //Subscribe to store data
            MessagingCenter.Subscribe<NewReminderPage, Reminder>(this, "AddReminder", (obj, reminder) => {
                db.Insert(reminder);
                remindersPath.Add(reminder, reminder.Id);
            });
        }
    }

    public class DrugsStoreDictionary : StoreDictionary<Drug, string>
    {
        protected override void Init(SQLiteConnection db, Dictionary<Drug, string> drugsPath)
        {
            db.CreateTable<DrugData>();

            //Load reminders to dictionary
            db.Table<DrugData>().ToList().ForEach(drug => drugsPath.Add(drug.ToDrug(), ""));


            //Subscribe to store data
            MessagingCenter.Subscribe<AddDrug, Drug>(this, "AddDrug", (obj, drug) => {
                db.Insert(drug.ToDrugData());
                drugsPath.Add(drug, drug.Id);
            });
        }
    }



}
