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
using Android.Content.Res;
using Firebase.Storage;
using System.IO;
using MediAid.Helpers;

[assembly: Dependency(typeof(AndroidFirebaseConnection))]
namespace MediAid.Droid
{

    public class AndroidFirebaseConnection : FirebaseConnection
    {

        private FirebaseDatabase database;
        private DatabaseReference databaseRef, remindersRef, drugsRef;
        private FirebaseStorage storage;
        private StorageReference storageRef, sRemindersRef, sDrugsRef;

        private FirebaseUser user;

        private AndroidFirebaseValueListener ValueListener;

        private void InitDatabase()
        {
            WriteLine("Initizating Database");
            var user = FirebaseAuth.Instance.CurrentUser;

            database = FirebaseDatabase.Instance;

            //Allow offline cache
            database.SetPersistenceEnabled(true);

            databaseRef = database.GetReference("").Child("users").Child(user.Uid);
            databaseRef.KeepSynced(true);
            remindersRef = databaseRef.Child("reminders");
            remindersRef.KeepSynced(true);
            drugsRef = databaseRef.Child("drugs");
            drugsRef.KeepSynced(true);

            storage = FirebaseStorage.Instance;
            storageRef = storage.GetReferenceFromUrl("gs://mediaid-79290.appspot.com").Child("users").Child(user.Uid);
            sRemindersRef = storageRef.Child("reminders");
            sDrugsRef = storageRef.Child("drugs");

            //Save drugs for later iteration of reminder
            //var listDrugs = new List<Drug>();


            //OnDataChange
            ValueListener = new AndroidFirebaseValueListener();
            ValueListener.AddDrug += (drug) =>
            {
                WriteLine(drug.Name);
                drug.ImageFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), drug.DatabaseId + ".jpg");
                drugs.Add(drug);
            };

            ValueListener.AddReminder += (reminder, list) => 
            {
                WriteLine(reminder.Name);

                //Loop through saved drugs
                if (list != null)
                {

                    //reminder.Drugs = drugs.Where(drug => cList.Any(id => drug.DatabaseId == id)).ToList();

                    reminder.Drugs  = drugs.Where(drug => list.CheckIdContains(drug.DatabaseId)).ToList();

                }
                

                reminders.Add(reminder);
            };

            remindersRef.AddValueEventListener(ValueListener);
            drugsRef.AddValueEventListener(ValueListener);


        }


        public override void AddReminder(Reminder reminder)
        {
            if (!IsLogin) return; 
            var reminderRef = remindersRef.Child(reminder.ReminderId.ToString());
            reminderRef.SetValue(reminder.ToMap());

            var sReminderRef = sRemindersRef.Child(reminder.ReminderId.ToString());

            string path = Path.Combine(App.audioHandler.RecordingPath, reminder.RecordId + ".3gpp");
            if(!String.IsNullOrEmpty(path))sReminderRef.PutFile(Android.Net.Uri.FromFile(new Java.IO.File(path)));

        }

        public override void AddDrug(Drug drug)
        {
            if (!IsLogin) return;
            var drugRef = drugsRef.Child(drug.DatabaseId.ToString());
            drugRef.SetValue(drug.ToMap());

            var sDrugRef = sDrugsRef.Child(drug.DatabaseId.ToString());

            if(!String.IsNullOrEmpty(drug.ImageFile)) sDrugRef.PutFile(Android.Net.Uri.FromFile(new Java.IO.File(drug.ImageFile)));

        }

        public override void RemoveReminder(Reminder reminder)
        {
            if (!IsLogin) return;
            remindersRef.Child(reminder.ReminderId.ToString()).RemoveValue();
            sRemindersRef.Child(reminder.ReminderId.ToString());
        }

        public override void RemoveDrug(Drug drug)
        {
            if (!IsLogin) return;
            drugsRef.Child(drug.DatabaseId.ToString()).RemoveValue();
            sDrugsRef.Child(drug.DatabaseId.ToString()).Delete();
        }

        public override async Task<bool> LoginUserAsync(string username, string password)
        {
            try
            {
                IAuthResult result = await FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(username, password);
                user = result.User;
                IsLogin = true;
                InitDatabase();
                return user != null;
            }
            catch (FirebaseNetworkException)
            {
                return false;
            }

        }

        public override void SignOut()
        {
            if (user == null) throw new InvalidOperationException("Not Login");
            FirebaseAuth.Instance.SignOut();
            IsLogin = false;
        }

        public override async Task<bool> ResetEmail(string email)
        {
            try
            {
                await FirebaseAuth.Instance.SendPasswordResetEmailAsync(email);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public override async Task<bool> CreateUser(string username, string password)
        {
            IAuthResult result = await FirebaseAuth.Instance.CreateUserWithEmailAndPasswordAsync(username, password);
            var tuser = result.User;
            tuser.SendEmailVerification();

            return tuser != null;
        }

        public override string GetEmail()
        {
            return user?.Email?? "";
        }

        public override string GetProfilePicture()
        {
            return user?.PhotoUrl?.ToString();
        }

        public override bool IsAccountVerified()
        {
            if (user == null) throw new InvalidOperationException("Not Login");
            return user.IsEmailVerified;
        }

        public override void RedownloadFiles()
        {
            WriteLine(sDrugsRef.Path);
            drugs.ForEach(drug => sDrugsRef.Child(drug.DatabaseId.ToString()).GetFile(Android.Net.Uri.FromFile(new Java.IO.File(drug.ImageFile))));
            reminders.ForEach(reminder => sRemindersRef.Child(reminder.ReminderId.ToString()).GetFile(Android.Net.Uri.FromFile(new Java.IO.File(Path.Combine(App.audioHandler.RecordingPath, reminder.RecordId + ".3gpp")))));

        }

    }

    static class Extensions
    {
        public static HashMap ToMap(this Reminder reminder)
        {
            HashMap map = new HashMap();
            map.Put("Name", reminder.Name);
            map.Put("Hours", reminder.Hours);
            map.Put("Mins", reminder.Mins);
            map.Put("IsEnabled", reminder.IsEnabled);
            map.Put("Time", reminder.Time.ToString());
            map.Put("Date", reminder.Date.ToString());
            map.Put("RecordId", reminder.RecordId);

            var list = new JavaList();
            reminder.Drugs?.ToList().ForEach(drug => list.Add(drug.DatabaseId));

            map.Put("Drugs", list);

            return map;
        }

        public static Reminder FromMapToReminder(IDictionary dictionary, int id)
        {
            return new Reminder { ReminderId = id,
                Name = dictionary["Name"].ToString(),
                Hours = Convert.ToInt32(dictionary["Hours"]),
                Mins = Convert.ToInt32(dictionary["Mins"]),
                IsEnabled = Boolean.Parse(dictionary["IsEnabled"].ToString()),
                Time = TimeSpan.Parse(dictionary["Time"].ToString()),
                Date = DateTime.Parse(dictionary["Date"].ToString()),
                RecordId = dictionary["RecordId"].ToString()
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

        public static bool CheckIdContains(this JavaList list, int id)
        {
            foreach (long item in list) if (item == id) return true;
            return false;
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
            dynamic refList = snapshot.Value as JavaDictionary;
            if (refName.Equals("reminders"))
            {
                if (refList == null)
                {
                    refList = snapshot.Value as JavaList;
                    if (refList == null) return;
                    for (int i = 0; i < refList.Count; i++)
                    {
                        JavaDictionary dictionary = refList[i] as JavaDictionary;
                        if (dictionary != null)
                            AddReminder(Extensions.FromMapToReminder(dictionary, Convert.ToInt32(i)), dictionary["Drugs"] as JavaList);
                    }
                }
                else
                {
                    foreach (DictionaryEntry entry in refList)
                    {
                        JavaDictionary dictionary = entry.Value as JavaDictionary;
                        if (dictionary != null)
                            AddReminder(Extensions.FromMapToReminder(dictionary, Convert.ToInt32(entry.Key)), dictionary["Drugs"] as JavaList);
                    }
                }
   
            }
            else if (refName.Equals("drugs"))
            {
                if (refList == null)
                {
                    refList = snapshot.Value as JavaList;
                    if (refList == null) return;
                    for (int i = 0; i < refList.Count; i++)
                    {
                        JavaDictionary dictionary = refList[i] as JavaDictionary;
                        if (dictionary != null)
                            AddDrug(Extensions.FromMapToDrug(dictionary, Convert.ToInt32(i)));
                    }

                }
                else
                {
                    foreach (DictionaryEntry entry in refList)
                    {
                        JavaDictionary dictionary = entry.Value as JavaDictionary;
                        if (dictionary != null)
                            AddDrug(Extensions.FromMapToDrug(dictionary, Convert.ToInt32(entry.Key)));
                    }
                }


            }
        }

    }


}