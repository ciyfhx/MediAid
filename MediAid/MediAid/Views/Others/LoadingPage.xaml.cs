using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Xamarin.Forms;

namespace MediAid.Views
{
    public abstract partial class LoadingPage : ContentPage, INotifyPropertyChanged
    {
        private string loadingText;
        public string LoadingText { get => loadingText; set {

                loadingText = value;
                OnPropertyChanged("LoadingText");

            }
        }


        public abstract void LoadingTask();

        public LoadingPage()
        {
            this.InitializeComponent();

            this.BindingContext = this;

        }

        #region INotifyPropertyChanged Implementation
        public new event PropertyChangedEventHandler PropertyChanged;
        protected override void OnPropertyChanged(string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        } 
        #endregion

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadingTask();


        }

    }
}
