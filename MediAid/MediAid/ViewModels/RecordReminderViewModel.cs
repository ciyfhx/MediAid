using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace MediAid.ViewModels
{
    class RecordReminderViewModel : INotifyPropertyChanged
    {

        private int seconds = 0;

        private string details = "0s";
        public string Details
        {
            get => details;
            set
            {
                details = value;
                OnPropertyChanged();
            }
        }

        public void UpdateDetails(object source)
        {
            seconds++;
            Details = $"{seconds}s";
        }

        public void Reset()
        {
            seconds = 0;
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
