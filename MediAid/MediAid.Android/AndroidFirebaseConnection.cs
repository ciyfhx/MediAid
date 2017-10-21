using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MediAid.Services;
using Firebase.Database;
using Firebase;
using Firebase.Auth;
using Xamarin.Forms;
using MediAid.Droid;
using System.Threading.Tasks;
using System.Collections;
using Java.Util;
using MediAid.Models;
using static System.Diagnostics.Debug;

[assembly: Dependency(typeof(AndroidFirebaseConnection))]
namespace MediAid.Droid
{

    public class AndroidFirebaseConnection : FirebaseConnection
    {

        private FirebaseDatabase database;
        private DatabaseReference databaseRef, remindersRef, drugsRef;

        private FirebaseUser user;

        private AndroidFirebaseValueListener ValueListener;

        private void InitDatabase()
        {
            WriteLine("Initizating Database");
            var user = FirebaseAuth.Instance.CurrentUser;

            database = FirebaseDatabase.Instance;
            databaseRef = database.GetReference("").Child("users").Child(user.Uid);
            remindersRef = databaseRef.Child("reminders");
            drugsRef = databaseRef.Child("drugs");

            //Save drugs for later iteration of reminder
            var listDrugs = new List<Drug>();


            //OnDataChange
            ValueListener = new AndroidFirebaseValueListener();
            ValueListener.AddDrug += (drug) =>
            {
                WriteLine(drug.Name);
                MessagingCenter.Send(typeof(FirebaseConnection), "FirebaseAddDrug", drug);
                listDrugs.Add(drug);
            };

            ValueListener.AddReminder += (reminder, list) => 
            {
                WriteLine(reminder.Name);

                //Loop through saved drugs
                if(list!=null)
                reminder.Drugs = listDrugs.Where(drug => list.Contains(drug.DatabaseId)).ToList();

                MessagingCenter.Send(typeof(FirebaseConnection), "FirebaseAddReminder", reminder);
            };

            remindersRef.AddValueEventListener(ValueListener);
            drugsRef.AddValueEventListener(ValueListener);


        }


        public override void AddReminder(Reminder reminder)
        {
            if (!IsLogin) return; 
            var reminderRef = remindersRef.Child(reminder.ReminderId.ToString());
            reminderRef.SetValue(reminder.ToMap());
        }

        public override void AddDrug(Drug drug)
        {
            if (!IsLogin) return;
            var drugRef = drugsRef.Child(drug.DatabaseId.ToString());
            drugRef.SetValue(drug.ToMap());
        }

        public override void RemoveReminder(Reminder reminder)
        {
            if (!IsLogin) return;
            remindersRef.Child(reminder.ReminderId.ToString()).RemoveValue();
        }

        public override void RemoveDrug(Drug drug)
        {
            if (!IsLogin) return;
            drugsRef.Child(drug.DatabaseId.ToString()).RemoveValue();
        }

        private void UploadFile()
        {
          
        }

        public override async Task<bool> LoginUserAsync(string username, string password)
        {
            IAuthResult result = await FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(username, password);
            user = result.User;
            IsLogin = true;
            InitDatabase();
            return user!=null;
        }

        public override void SignOut()
        {
            if (user == null) throw new InvalidOperationException("Not Login");
            FirebaseAuth.Instance.SignOut();
            IsLogin = false;
        }

        public override async Task<bool> CreateUser(string username, string password)
        {
            throw new NotImplementedException();
        }

        public override string GetEmail()
        {
            return user.Email;
        }

        public override Image GetProfilePicture()
        {
            return new Image {
                Source = ImageSource.FromUri(new Uri(user.PhotoUrl.ToString()))
            };
        }
    }

    static class Extensions
    {
        public static HashMap ToMap(this Reminder reminder)
        {
            HashMap map = new HashMap();
            map.Put("Name", reminder.Name);
            map.Put("Hours", reminder.Hours);
            map.Put("IsEnabled", reminder.IsEnabled);
            map.Put("Time", reminder.Time.ToString());

            var list = new JavaList();
            reminder.Drugs.ForEach(drug => list.Add(drug.DatabaseId));

            map.Put("Drugs", list);

            return map;
        }

        public static Reminder FromMapToReminder(IDictionary dictionary, int id)
        {
            return new Reminder { ReminderId = id,
                Name = dictionary["Name"].ToString(),
                Hours = Convert.ToInt32(dictionary["Hours"]),
                IsEnabled = Boolean.Parse(dictionary["IsEnabled"].ToString()),
                Time = TimeSpan.Parse(dictionary["Time"].ToString())
            };
        }

        public static Drug FromMapToDrug(IDictionary dictionary, int id)
        {
            return new Drug
            {
                DatabaseId = id,
                Name = dictionary["Name"].ToString()
            };
        }

        public static HashMap ToMap(this Drug drug)
        {
            HashMap map = new HashMap();
            map.Put("Name", drug.Name);
            //map.Put("TimeEnabled", reminder.TimeEnabled.ToLongTimeString());

            return map;
        }

    }

    public class AndroidFirebaseValueListener : Java.Lang.Object, IValueEventListener
    {

        public event Action<Reminder, JavaList> AddReminder;
        public event Action<Drug> AddDrug;

        //public delegate void OnReminderAdd(Reminder reminder);

        public void OnCancelled(DatabaseError error)
        {
            WriteLine(error.Message);
        }

        public void OnDataChange(DataSnapshot snapshot)
        { 
            string refName = snapshot.Ref.Key;
            var refDictionary = snapshot.Value as JavaDictionary;
            if (refDictionary == null) return;
            if (refName.Equals("reminders"))
            {
                foreach (DictionaryEntry item in refDictionary)
                {
                    JavaDictionary dictionary = item.Value as JavaDictionary;
                    //WriteLine(dictionary[""]);
                    AddReminder(Extensions.FromMapToReminder(dictionary, Convert.ToInt32(item.Key)), dictionary["Drugs"] as JavaList);
                }
            }
            else if (refName.Equals("drugs"))
            {
                foreach (DictionaryEntry item in refDictionary)
                {
                    JavaDictionary dictionary = item.Value as JavaDictionary;
                    //WriteLine(dictionary[""]);
                    AddDrug(Extensions.FromMapToDrug(dictionary, Convert.ToInt32(item.Key)));
                }
            }
        }

    }


}