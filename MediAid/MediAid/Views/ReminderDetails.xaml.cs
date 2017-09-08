using MediAid.Models;
using MediAid.ViewModels;
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
	public partial class ReminderDetails : ContentPage
	{
        DrugsListPageViewModel viewModel;

        private Reminder reminder;

        public ReminderDetails(Reminder reminder)
        {
            this.reminder = reminder;
            InitializeComponent();

            //Manually set the data
            Reminder.Text = this.reminder.Name;


            BindingContext = viewModel = new DrugsListPageViewModel();

        }

         void Handle_ItemTapped(object sender, SelectedItemChangedEventArgs e)
        {

        }

         void Play_Reminder(object sender, EventArgs e)
        {
            App.audioHandler.PlayRecording($"{Reminder.Id}.3gpp");


        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Drugs.Count == 0)
                viewModel.LoadDrugsCommand.Execute(null);
        }

    }
}