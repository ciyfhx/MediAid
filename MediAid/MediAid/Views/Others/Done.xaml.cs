using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MediAid.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public abstract partial class Done : ContentPage, INotifyPropertyChanged
	{
        private string text;
        public string Text
        {
            get => text;
            set
            {

                text = value;
                OnPropertyChanged("Text");

            }
        }


        public abstract void OnClickTask(object sender, EventArgs e);

        public Done()
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

    }
}