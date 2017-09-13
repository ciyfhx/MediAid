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
    public partial class RootMasterPage : MasterDetailPage
    {
        public RootMasterPage()
        {
            InitializeComponent();
            MasterPage.ListView.ItemSelected += ListView_ItemSelected;

            SetPage(MasterPage.MenuItems.First());

        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as RootMasterPageMenuItem;
            if (item == null)
                return;

            SetPage(item);
        }

        private void SetPage(RootMasterPageMenuItem item)
        {
            var page = item.CreatePage();
            page.Title = item.Title;

            Detail = new NavigationPage(page);
            IsPresented = false;

            MasterPage.ListView.SelectedItem = null;
        }

    }
}