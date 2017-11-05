using MediAid.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MediAid.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PillDetails : ContentPage
    {
        public Drug Drug { get; set; }

        public PillDetails(Drug drug)
        {
            this.Drug = drug;
            InitializeComponent();

            BindingContext = this;

        }

        async void Remove_Medication(object sender, EventArgs e)
        {
            if (CheckRelations(Drug))
            {
                //Have relations

                //Prompt user
                var result = await DisplayAlert("Warning", "This Medication is listed in one of the reminder, Do you still want to proceed", "Continue", "Cancel");

                if (result) RemoveDrug(Drug);

            }
            else RemoveDrug(Drug);

        }

        private void RemoveDrug(Drug drug)
        {
            MessagingCenter.Send(this, "RemoveDrug", drug);

            //Remove image
            try
            {
                File.Delete(drug.ImageFile);
            }
            catch (Exception)
            {

            }

            Navigation.PopToRootAsync();
        }


        /// <summary>
        /// Check whether if the Drug have any relations
        /// </summary>
        /// <param name="drug"></param>
        /// <returns></returns>
        private bool CheckRelations(Drug drug)
        {

            foreach(ReminderDrug checkDrug in App.StoreDictionaryHandler.db.Table<ReminderDrug>())
            {
                if (drug.DatabaseId == checkDrug.DatabaseId) return true;
            }
            return false;

        }




    }
}
