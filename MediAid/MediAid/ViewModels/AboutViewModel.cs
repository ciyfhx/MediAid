using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace MediAid.ViewModels
{
	public class AboutViewModel : BaseViewModel
	{

		public AboutViewModel()
		{
			Title = "About";

			OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://medi-aid.tumblr.com/")));
		}

        /// <summary>
        /// Command to open browser to https://medi-aid.tumblr.com/
        /// </summary>
        public ICommand OpenWebCommand { get; }
	}
}
