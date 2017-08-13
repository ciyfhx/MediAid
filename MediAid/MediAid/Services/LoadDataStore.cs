using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MediAid.Models;

using Xamarin.Forms;
using MediAid.Helpers;
using System.Diagnostics;

namespace MediAid.Services
{
	public class LoadDataStore<T> : IDataStore<T> where T : BaseModel
    {
		bool isInitialized;
		List<T> Items;

        public delegate List<T> LoadData();

        public LoadData LoadDataHandler;

        public async Task<bool> AddItemAsync(T Item)
		{
			await InitializeAsync();

			Items.Add(Item);

			return await Task.FromResult(true);
		}

		public async Task<bool> UpdateItemAsync(T Item)
		{
			await InitializeAsync();

			var _Item = Items.Where((T arg) => arg.Id == Item.Id).FirstOrDefault();
			Items.Remove(_Item);
			Items.Add(Item);

			return await Task.FromResult(true);
		}

		public async Task<bool> DeleteItemAsync(T Item)
		{
			await InitializeAsync();

			var _Item = Items.Where((T arg) => arg.Id == Item.Id).FirstOrDefault();
			Items.Remove(_Item);

			return await Task.FromResult(true);
		}

		public async Task<T> GetItemAsync(string id)
		{
			await InitializeAsync();

			return await Task.FromResult(Items.FirstOrDefault(s => s.Id == id));
		}

		public async Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false)
		{
			await InitializeAsync();

			return await Task.FromResult(Items);
		}

		public Task<bool> PullLatestAsync()
		{
			return Task.FromResult(true);
		}


		public Task<bool> SyncAsync()
		{
			return Task.FromResult(true);
		}

		public async Task InitializeAsync()
		{
			if (isInitialized)
				return;

            Items = new List<T>();

            List<T> _Items = LoadDataHandler();

            _Items.ForEach(Item => Items.Add(Item));

			isInitialized = true;
		}

	}
}
