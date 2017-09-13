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

[assembly: Dependency(typeof(AndroidFirebaseConnection))]
namespace MediAid.Droid
{

    public class AndroidFirebaseConnection : FirebaseConnection
    {

        private FirebaseDatabase database;

        public void Init()
        {
            //FirebaseApp.InitializeApp(Android.App.Application.Context);
            database = FirebaseDatabase.Instance;
            DatabaseReference databaseRef = database.GetReference("");




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
            return user!=null;
        }

        public async Task<bool> CreateUser(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}