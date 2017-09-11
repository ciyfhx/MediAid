using MediAid.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace MediAid.ViewModels
{
    public class AddDrugViewModel : BaseModel
    {

        public Drug Drug { get; set; }
        public string DrugTypeName { get; set; }

        public List<DrugType> DrugTypeValues { get; set; }

        public AddDrugViewModel()
        {
            DrugTypeValues = DrugType.Values.ToList();

            Drug = new Drug { Name = "" };



        }





    }
}
