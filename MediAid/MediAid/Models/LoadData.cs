using MediAid.Helpers;
using MediAid.Services;
using MediAid.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MediAid.Models
{
    public abstract class LoadData<T> : BaseViewModel where T : BaseModel
    {
        public ObservableRangeCollection<T> Items { get; set; }

        public Command LoadItemsCommand { get; set; }

        protected LoadDataStore<T> ItemsDataStore = new LoadDataStore<T>();

        public abstract List<T> GetData();

        public LoadData()
        {
            ItemsDataStore.LoadDataHandler = GetData;

            Items = new ObservableRangeCollection<T>();

            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

        }

        public async void RemoveItem(T item)
        {
            Items.Remove(item);
            await ItemsDataStore.DeleteItemAsync(item);
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var _Items = await ItemsDataStore.GetItemsAsync(true);
                Items.ReplaceRange(_Items);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessagingCenter.Send(new MessagingCenterAlert
                {
                    Title = "Error",
                    Message = $"Unable to load items of type {typeof(T)}",
                    Cancel = "OK"
                }, "message");
            }
            finally
            {
                IsBusy = false;
            }
        }

    }
}
