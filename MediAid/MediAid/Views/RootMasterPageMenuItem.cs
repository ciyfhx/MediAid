using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MediAid.Views
{

    public abstract class RootMasterPageMenuItem
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public virtual Page CreatePage() {
            return new ContentPage();

        }

    }
}