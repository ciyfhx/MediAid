using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Auth;
using Xamarin.Forms;

namespace MediAid.Models
{
    public class Settings : BaseModel
    {

        public const string APPNAME = "MediAid";

        //Not required but sqlite needs it so
        [PrimaryKey]
        public int SettingsId { get; set; }

        public string Version { get; } = "1.4.0";

        private bool isLogin;

        ////Not the best way to store user sensitive informations
        //private string username;
        //private string password;


        public bool IsLogin {

            get => isLogin;

            set {

                isLogin = value;
                UpdateSettings();

        } }

        public string Username
        {
            get
            {
                var account = AccountStore.Create().FindAccountsForService(APPNAME).FirstOrDefault();
                return (account != null) ? account.Username : null;
            }

        }

        public string Password
        {
            get
            {
                var account = AccountStore.Create().FindAccountsForService(APPNAME).FirstOrDefault();
                return (account != null) ? account.Properties["Password"] : null;
            }

        }


        public void UpdateSettings()
        {
            MessagingCenter.Send(this, "UpdateSettings", this);
        }

        public void SaveCredentials(string username, string password)
        {
            if (!String.IsNullOrWhiteSpace(username) && !String.IsNullOrWhiteSpace(password))
            {
                Account account = new Account
                {
                    Username = username
                };
                account.Properties.Add("Password", password);
                AccountStore.Create().Save(account, APPNAME);
            }
        }

        public void DeleteCredentials()
        {
            var account = AccountStore.Create().FindAccountsForService(APPNAME).FirstOrDefault();
            if (account != null)
            {
                AccountStore.Create().Delete(account, APPNAME);
            }
        }

    }
}
