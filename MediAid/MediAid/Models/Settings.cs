using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MediAid.Models
{
    public class Settings : BaseModel
    {

        //Not required but sqlite needs it so
        [PrimaryKey]
        public int SettingsId { get; set; }

        private bool firstLogin;
        //Not the best way to store user sensitive informations
        private string username;
        private string password;


        public bool FirstLogin {

            get => firstLogin;

            set {

                firstLogin = value;
                UpdateSettings();

        } }

        public string Username
        {
            get => username;

            set
            {
                username = value;
                UpdateSettings();
            }

        }

        public string Password
        {
            get => password;

            set
            {
                password = value;
                UpdateSettings();
            }

        }


        public void UpdateSettings()
        {
            MessagingCenter.Send(this, "UpdateSettings", this);
        }

    }
}
