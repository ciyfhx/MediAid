using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MediAid.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RootMasterPageMaster : ContentPage
    {
        public ListView ListView;

        public ObservableCollection<RootMasterPageMenuItem> MenuItems { get => viewModel.MenuItems; }

        private RootMasterPageMasterViewModel viewModel;

        public RootMasterPageMaster()
        {
            InitializeComponent();

            BindingContext = viewModel = new RootMasterPageMasterViewModel();

            ListView = MenuItemsListView;
        }

        class RootMasterPageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<RootMasterPageMenuItem> MenuItems { get; set; }

            //Email
            public string Email { get; set; } = App.firebase.GetEmail();

            public Image ProfilePic { get; set; }



            public RootMasterPageMasterViewModel()
            {
                MenuItems = new ObservableCollection<RootMasterPageMenuItem> {
                    new MainPageMenuItem { Id = 0, Title = "Medications" },
                    //new RootMasterPageMenuItem { Id = 1, Title = "Settings" },
                    new LogOutPageMenuItem { Id = 2, Title = "Log Out" }
                };

            }
            
            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}