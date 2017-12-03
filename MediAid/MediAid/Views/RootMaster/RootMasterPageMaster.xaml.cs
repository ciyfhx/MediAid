using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
            //Causing alarm restarting issue
            public string Email { get;  } = App.firebase.GetEmail() + ((App.firebase.IsLogin)? "" : "(Not login)");

            //Profile Picture
            public ImageSource ProfilePicture { get {
                    string uri = App.firebase.GetProfilePicture();
                    return !String.IsNullOrEmpty(uri) ? ImageSource.FromUri(new Uri(uri)) : ImageSource.FromResource("profile.jpg");
             } }



            public RootMasterPageMasterViewModel()
            {
                MenuItems = new ObservableCollection<RootMasterPageMenuItem> {
                    new MainPageMenuItem { Id = 0, Title = "Medications" },
                    new SettingsPageMenuItem { Id = 1, Title = "Settings" }
                    
                };
                if (App.firebase.IsLogin)
                {
                    MenuItems.Add(new LogOutPageMenuItem { Id = 2, Title = "Logout" });
                }
                else
                {
                    MenuItems.Add(new LogInPageMenuItem { Id = 2, Title = "Login" });
                }

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