using System.Collections.Generic;
using System.Threading.Tasks;

namespace MediAid.Services
{
	public interface IDataStore<T>
	{
		Task<bool> AddItemAsync(T Item);
		Task<bool> UpdateItemAsync(T Item);
		Task<bool> DeleteItemAsync(T Item);
		Task<T> GetItemAsync(string id);
		Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);

		Task InitializeAsync();
		Task<bool> PullLatestAsync();
		Task<bool> SyncAsync();
	}
}
