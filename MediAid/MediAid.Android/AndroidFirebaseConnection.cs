﻿using System;
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

        private void InitDatabase()
        {
            WriteLine("Initizating Database");
            var user = FirebaseAuth.Instance.CurrentUser;

            database = FirebaseDatabase.Instance;
            databaseRef = database.GetReference("").Child("users").Child(user.Uid);
            remindersRef = databaseRef.Child("reminders");
            drugsRef = databaseRef.Child("drugs");

            //OnDataChange
            remindersRef.AddValueEventListener(this);


        }


        public override void AddReminder(Reminder reminder)
        {
            if (!IsLogin) return; 
            var reminderRef = remindersRef.Child(Convert.ToString(reminder.ReminderId));
            reminderRef.SetValue(reminder.ToMap());
        }

        public override void AddDrug(Drug drug)
        {
            if (!IsLogin) return;
            drugsRef.SetValue(drug.ToMap());
        }

        private void UploadFile()
        {

        }

        public override async Task<bool> LoginUserAsync(string username, string password)
        {
            IAuthResult result = await FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(username, password);
            var user = result.User;
            IsLogin = true;
            InitDatabase();
            return user!=null;
        }

        //public override void SetData(string json, params string[] childs)
        //{
        //    var subDatabaseRef = databaseRef;
        //    childs.ToList().ForEach(child => subDatabaseRef = subDatabaseRef.Child(child));
        //    subDatabaseRef?.SetValue(json);
        //}
        //public override void SetData(IDictionary dictionary, params string[] childs)
        //{
        //    var subDatabaseRef = databaseRef;
        //    childs.ToList().ForEach(child => subDatabaseRef = subDatabaseRef.Child(child));
        //    subDatabaseRef?.SetValue(new HashMap(dictionary));
        //}

        //public override void LoginUser(string username, string password)
        //{
        //    var task = FirebaseAuth.Instance.SignInWithEmailAndPassword(username, password);
        //    IsLogin = true;
        //    InitDatabase();
        //}

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
            //map.Put("Uid", reminder.ReminderId);
            map.Put("Name", reminder.Name);
            map.Put("Hours", reminder.Hours);
            map.Put("IsEnabled", reminder.IsEnabled);
            map.Put("Time", reminder.Time.ToString());
            //map.Put("TimeEnabled", reminder.TimeEnabled.ToLongTimeString());

            var list = new JavaList();
            reminder.Drugs.ForEach(drug => list.Add(drug.DatabaseId));

            map.Put("Drugs", list);

            return map;
        }

        public static Reminder FromMapToReminder(HashMap map, int id)
        {
            return new Reminder { ReminderId = id,
                Name = map.Get("Name").ToString(),
                Hours = Convert.ToInt32(map.Get("Hours")),
                IsEnabled = Boolean.Parse(map.Get("Name").ToString()),
                Time = TimeSpan.Parse(map.Get("Time").ToString())
            };
        }

        public static HashMap ToMap(this Drug drug)
        {
            HashMap map = new HashMap();
            map.Put("Uid", drug.DatabaseId);
            map.Put("Name", drug.Name);
            //map.Put("TimeEnabled", reminder.TimeEnabled.ToLongTimeString());

            return map;
        }

    }

    public class AndroidReminderFirebaseValueListener : IValueEventListener
    {
        public IntPtr Handle => throw new NotImplementedException();

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void OnCancelled(DatabaseError error)
        {
            throw new NotImplementedException();
        }

        public void OnDataChange(DataSnapshot snapshot)
        {
            HashMap map = snapshot.GetValue(true) as HashMap;
            
            foreach (MapEntry entry in map.EntrySet())
            {
                var reminder = Extensions.FromMapToReminder(entry.);
            }

        }
    }


}