using MediAid.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xamarin.Forms;
using MediAid.Views;
using System.IO;
using System.Diagnostics;
using SQLite;
using SQLiteNetExtensions.Extensions;

namespace MediAid.Helpers
{

    public class StoreDictionaryHandler
    {

        public readonly SQLiteConnection db;

        /// <summary>
        /// Init the database and must be called before retrieving the data
        /// </summary>
        /// <param name="path">Path to load the datas</param>
        public StoreDictionaryHandler(string path)
        {
            if (!File.Exists(path))
            {
                Directory.CreateDirectory(path.Substring(0, path.LastIndexOf("/")));
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

    public class SettingsStoreDictionary : StoreDictionary<Settings, string>
    {
        protected override void Init(SQLiteConnection db, Dictionary<Settings, string> dictionary)
        {
            db.CreateTable<Settings>();

            if (db.Table<Settings>().Count() == 0)
            {
                //No Settings so we will create one
                var settings = new Settings();
                db.Insert(settings);
            }


            MessagingCenter.Subscribe<Settings, Settings>(this, "UpdateSettings", (obj, settings) => {
                db.Update(settings);
            });


        }
    }

    public class RemindersStoreDictionary : StoreDictionary<Reminder, string>
    {
        protected override void Init(SQLiteConnection db, Dictionary<Reminder, string> remindersPath)
        {

            db.CreateTable<Reminder>();
            db.CreateTable<ReminderDrug>();


            var list = db.Table<Reminder>().ToList();

           
            list.ForEach(reminder => {

                //Load reminders to dictionary
                remindersPath.Add(reminder, reminder.Id);

            });

            //Subscribe to store data
            MessagingCenter.Subscribe <RecordReminder, Reminder>(this, "AddReminder", (obj, reminder) => {
                db.Insert(reminder);
                remindersPath.Add(reminder, reminder.Id);

                //Update Relations
                db.UpdateWithChildren(reminder);
            });

            MessagingCenter.Subscribe<ReminderDetails, Reminder>(this, "RemoveReminder", (obj, reminder) => {
                //Removing Relationship
                reminder.Drugs = null;
                db.UpdateWithChildren(reminder);

                db.Delete(reminder);
                remindersPath.Remove(reminder);
            });

            MessagingCenter.Subscribe<ReminderDetails, Reminder>(this, "UpdateReminder", (obj, reminder) => {
                db.Update(reminder);
                //remindersPath[reminder.Id] = reminder;

                //Update Relations
                db.UpdateWithChildren(reminder);
            });

        }
    }

    public class DrugsStoreDictionary : StoreDictionary<Drug, string>
    {
        protected override void Init(SQLiteConnection db, Dictionary<Drug, string> drugsPath)
        {
            db.CreateTable<Drug>();

            //Load reminders to dictionary
            db.Table<Drug>().ToList().ForEach(drug => drugsPath.Add(drug, ""));


            //Subscribe to store data
            MessagingCenter.Subscribe<AddDrug, Drug>(this, "AddDrug", (obj, drug) => {
                db.Insert(drug);
                drugsPath.Add(drug, drug.Id);
            });
            MessagingCenter.Subscribe<PillDetails, Drug>(this, "RemoveDrug", (obj, drug) => {
                //Remove Reminder Relation
                db.Table<ReminderDrug>().Where(record => record.DatabaseId == drug.DatabaseId).ToList().ForEach(record => db.Delete(record));
                

                db.Delete(drug);
                drugsPath.Remove(drug);



            });
        }



    }



}
