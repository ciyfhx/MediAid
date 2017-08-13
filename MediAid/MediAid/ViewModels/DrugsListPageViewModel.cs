using MediAid.Helpers;
using MediAid.Models;
using MediAid.Services;
using MediAid.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MediAid.ViewModels
{
    class DrugsListPageViewModel : BaseViewModel
    {

        public ObservableRangeCollection<Drug> Drugs { get; set; }

        public Command LoadDrugsCommand { get; set; }

        public LoadDataStore<Drug> DrugsDataStore = new LoadDataStore<Drug>();

        private List<Drug> LoadDrugs()
        {
            return App.Drugs.GetItems().Keys.ToList();
        }


        public DrugsListPageViewModel()
        {
            //Load Drugs
            DrugsDataStore.LoadDataHandler = LoadDrugs;


            Drugs = new ObservableRangeCollection<Drug>();

            //Get new drug
            MessagingCenter.Subscribe<AddDrug, Drug>(this, "AddDrug", (obj, drug) => {
                Drugs.Add(drug);
            });

            LoadDrugsCommand = new Command(async () => await ExecuteLoadDrugsCommand());

        }


        async Task ExecuteLoadDrugsCommand()
        {
            Debug.WriteLine(IsBusy);
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Drugs.Clear();
                var _Drugs = await DrugsDataStore.GetItemsAsync(true);
                Drugs.ReplaceRange(_Drugs);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessagingCenter.Send(new MessagingCenterAlert
                {
                    Title = "Error",
                    Message = "Unable to load Drugs.",
                    Cancel = "OK"
                }, "message");
            }
            finally
            {
                IsBusy = false;
            }
        }


    }
}
