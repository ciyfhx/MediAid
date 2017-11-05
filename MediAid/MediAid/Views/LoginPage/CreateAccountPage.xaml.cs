using MediAid.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MediAid.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CreateAccountPage : ContentPage
	{
        private CreateAccountViewModel viewModel;

		public CreateAccountPage ()
		{
            this.BindingContext = viewModel = new CreateAccountViewModel();

            InitializeComponent ();
		}

        public static bool ValidatePasswordLength(string password) => password?.Length > 8;
        public static bool? ValidateConfirmPassword(string password, string confirmPassword) => password?.Equals(confirmPassword);

        private void ValidatePassword(object sender, TextChangedEventArgs e) => Validation();

        private bool Validation()
        {
            string password = viewModel.Password;
            bool validationLength = ValidatePasswordLength(password);

            string confirmPassword = viewModel.ConfirmPassword;

            bool? validationConfirmPassword = ValidateConfirmPassword(password, confirmPassword);

            string text = null;

            if (!validationLength) StringAppend(ref text, "Password length not met!");
            if (validationConfirmPassword.HasValue && !validationConfirmPassword.Value) StringAppend(ref text, "Password and confirm password are not the same!");

            PasswordError.Text = text;

            bool val = !validationLength || (validationConfirmPassword.HasValue && !validationConfirmPassword.Value);

            PasswordError.IsVisible = val;
            return !val;
        }


        public string StringAppend(ref string val, string append)
        {
            if (String.IsNullOrEmpty(val)) return val = append;
            return val += "\n" + append;
        }

        private async void Next(object sender, EventArgs e)
        {
            if (!Validation()) return;
            try
            {
                if (String.IsNullOrEmpty(viewModel.Email) && String.IsNullOrEmpty(viewModel.Password) && String.IsNullOrEmpty(viewModel.ConfirmPassword)) return;
                bool done = await App.firebase.CreateUser(viewModel.Email, viewModel.Password);
                if (done) await Navigation.PushModalAsync(new ToLoginDone());
            }
            catch(Exception)
            {
                PasswordError.Text = "Account already exists or an error has occur";
                PasswordError.IsVisible = true;
            }

        }
    }

    public class ToLoginDone : Done
    {

        public ToLoginDone() : base() => this.Text = "User Created!";

        public override void OnClickTask(object sender, EventArgs e)
        {
            //Cannot work
            //await Navigation.PopToRootAsync();
            App.Current.MainPage = new StartPage();
        }
    }


}