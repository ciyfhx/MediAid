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

        private Reminder reminder;
        private ReminderDrugsListPage viewModel;

        public ReminderDetails(Reminder reminder)
        {
            this.reminder = reminder;
            InitializeComponent();

            //Manually set the data
            Reminder.Text = this.reminder.Name;


            BindingContext = viewModel = new ReminderDrugsListPage(reminder);

        }

         void Handle_ItemTapped(object sender, SelectedItemChangedEventArgs e)
        {

        }

         void Play_Reminder(object sender, EventArgs e)
        {
            App.audioHandler.PlayRecording($"{reminder.RecordId}.3gpp");


        }

        void Remove_Reminder(object sender, EventArgs e)
        {
            App.audioHandler.RemoveRecording($"{reminder.RecordId}.3gpp");

            MessagingCenter.Send(this, "RemoveReminder", reminder);

            Navigation.PopToRootAsync();

        }


        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }

    }
}