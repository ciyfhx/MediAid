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
    class DrugsListPageViewModel : LoadData<Drug>
    {

        public override List<Drug> GetData()
        {
            return App.Drugs.GetItems().Keys.ToList();
        }


        public DrugsListPageViewModel() : base()
        {

            //Get new drug
            MessagingCenter.Subscribe<AddDrug, Drug>(this, "AddDrug", (obj, drug) => {
                Items.Add(drug);
            });
            //Remove drug
            MessagingCenter.Subscribe<PillDetails, Drug>(this, "RemoveDrug", (obj, drug) => {
                Items.Remove(drug);
            });


        }


        
    }
}
