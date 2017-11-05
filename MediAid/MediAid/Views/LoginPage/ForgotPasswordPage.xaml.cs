using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MediAid.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ForgotPasswordPage : ContentPage
	{

        public string Email { get; set; }

		public ForgotPasswordPage ()
		{
            Title = "Forgot Password";

            InitializeComponent ();

            this.BindingContext = this;
		}

        private async void Reset(object sender, EventArgs e)
        {
            Info.Text = "";
            bool emailSended = await App.firebase.ResetEmail(Email);
            if (emailSended)
            {
                Info.Text = "Check your email for password reset";
                Info.TextColor = Color.Black;
                Info.IsVisible = true;
            }
            else
            {
                Info.Text = "Error sending reset email";
                Info.TextColor = Color.Red;
                Info.IsVisible = true;
            }

        }
    }
}