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

[assembly: Dependency(typeof(AndroidFirebaseConnection))]
namespace MediAid.Droid
{

    public class AndroidFirebaseConnection : FirebaseConnection
    {

        private FirebaseDatabase database;
        private DatabaseReference databaseRef;

        private FirebaseUser user;

        public void Init()
        {
            //FirebaseApp.InitializeApp(Android.App.Application.Context);



        }

        private void InitDatabase()
        {

            var user = FirebaseAuth.Instance.CurrentUser;

            database = FirebaseDatabase.Instance;
            databaseRef = database.GetReference("").Child("users").Child(user.Uid);
        }

        public void SetData(string json, params string[] childs)
        {
            var subDatabaseRef = databaseRef;
            childs.ToList().ForEach(child => subDatabaseRef = subDatabaseRef.Child(child));
            subDatabaseRef?.SetValue(json);
        }
        public void SetData(IDictionary dictionary, params string[] childs)
        {
            var subDatabaseRef = databaseRef;
            childs.ToList().ForEach(child => subDatabaseRef = subDatabaseRef.Child(child));
            subDatabaseRef?.SetValue(new HashMap(dictionary));
        }

        public void AddReminder(Reminder reminder)
        {
            databaseRef.Child("reminders").SetValue(reminder.ToMap());
        }

        private async void OnFirstConnect()
        {
            FirebaseAuth auth = FirebaseAuth.Instance;
            var user = auth.CurrentUser;

            if (user is null)
            {

                //
                //await auth.CreateUserWithEmailAndPasswordAsync();
            }

        }
        
        public void Connect()
        {

        }


        public async Task<bool> LoginUser(string username, string password)
        {
            await FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(username, password);
            var user = FirebaseAuth.Instance.CurrentUser;
            InitDatabase();
            return user!=null;
        }

        public async Task<bool> CreateUser(string username, string password)
        {
            throw new NotImplementedException();
        }

  

    }

    static class Extensions
    {
        public static HashMap ToMap(this Reminder reminder)
        {
            HashMap map = new HashMap();
            map.Put("Uid", reminder.ReminderId);
            map.Put("Name", reminder.Name);
            map.Put("Hours", reminder.Hours);
            map.Put("IsEnabled", reminder.IsEnabled);
            map.Put("TimeEnabled", reminder.TimeEnabled.ToLongTimeString());
            map.Put("Drugs", new JavaList(reminder.Drugs));

            return map;
        }
    }

}