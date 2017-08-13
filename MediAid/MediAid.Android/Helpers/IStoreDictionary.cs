using System.Collections.Generic;
using SQLite;

namespace MediAid.Helpers
{
    public interface IStoreDictionary<K, V>
    {
        void CallInit(SQLiteConnection db);
        Dictionary<K, V> GetItems();
    }
}