using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace MediAid.ViewModels
{
	public class CreateAccountViewModel : BaseViewModel
	{

        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public CreateAccountViewModel()
		{
			Title = "Create Account";
		}

	}
}
