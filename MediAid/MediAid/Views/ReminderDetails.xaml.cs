using MediAid.Models;
using MediAid.Services;
using MediAid.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
        private ReminderDrugsListPage viewModel;

        public ReminderDetails(Reminder reminder)
        {
            App.alarmHandler.RemoveAlarm(reminder);
            InitializeComponent();

            //Manually set the data
            Reminder.Text = reminder.Name;

            Debug.WriteLine(reminder);
            BindingContext = viewModel = new ReminderDrugsListPage(reminder);

        }

         void Handle_ItemTapped(object sender, SelectedItemChangedEventArgs e)
        {

        }

         void Play_Reminder(object sender, EventArgs e)
        {
            App.audioHandler.PlayRecording($"{viewModel.Reminder.RecordId}.3gpp");


        }

        void Remove_Reminder(object sender, EventArgs e)
        {
            App.audioHandler.RemoveRecording($"{viewModel.Reminder.RecordId}.3gpp");

            MessagingCenter.Send(this, "RemoveReminder", viewModel.Reminder);

            Navigation.PopToRootAsync();

        }


        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }

        private void Toggle_Reminder(object sender, ToggledEventArgs e)
        {
            if (viewModel.Reminder.IsEnabled)
            {
                viewModel.Reminder.TimeEnabled = DateTime.Now;

                App.alarmHandler.CreateAlarm(viewModel.Reminder);
            }
            else
            {
                App.alarmHandler.RemoveAlarm(viewModel.Reminder);
            }


        }
    }
}