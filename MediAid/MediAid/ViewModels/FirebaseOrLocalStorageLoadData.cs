using MediAid.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediAid.ViewModels
{
    public class LoadDataReminders : LoadData<Reminder>
    {
        public override List<Reminder> GetData()
        {
            throw new NotImplementedException();
        }
    }

    public class LoadDataDrugs : LoadData<Drug>
    {
        public override List<Drug> GetData()
        {
            throw new NotImplementedException();
        }
    }

}
