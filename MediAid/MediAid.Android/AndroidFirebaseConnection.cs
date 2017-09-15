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

        private void InitDatabase()
        {

            var user = FirebaseAuth.Instance.CurrentUser;

            database = FirebaseDatabase.Instance;
            databaseRef = database.GetReference("").Child("users").Child(user.Uid);
        }

        public override void SetData(string json, params string[] childs)
        {
            var subDatabaseRef = databaseRef;
            childs.ToList().ForEach(child => subDatabaseRef = subDatabaseRef.Child(child));
            subDatabaseRef?.SetValue(json);
        }
        public override void SetData(IDictionary dictionary, params string[] childs)
        {
            var subDatabaseRef = databaseRef;
            childs.ToList().ForEach(child => subDatabaseRef = subDatabaseRef.Child(child));
            subDatabaseRef?.SetValue(new HashMap(dictionary));
        }

        public override void AddReminder(Reminder reminder)
        {
            databaseRef.Child("reminders").SetValue(reminder.ToMap());
        }


        public override async Task<bool> LoginUserAsync(string username, string password)
        {
            IAuthResult result = await FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(username, password);
            var user = result.User;
            IsLogin = true;
            InitDatabase();
            return user!=null;
        }

        public override void LoginUser(string username, string password)
        {
            var task = FirebaseAuth.Instance.SignInWithEmailAndPassword(username, password);
            IsLogin = true;
            InitDatabase();
        }

        public override async Task<bool> CreateUser(string username, string password)
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
            //map.Put("Drugs", new JavaList(reminder.Drugs));

            return map;
        }
    }

}