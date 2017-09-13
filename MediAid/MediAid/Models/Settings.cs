using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MediAid.Models
{
    public class Settings : BaseModel
    {

        public bool FirstLogin {

            get => FirstLogin;

            set {

                FirstLogin = value;
                UpdateSettings();

            } }

        public void UpdateSettings()
        {
            MessagingCenter.Send(this, "UpdateSettings", this);
        }

    }
}
