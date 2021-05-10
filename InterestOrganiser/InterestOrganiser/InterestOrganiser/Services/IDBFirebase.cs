using InterestOrganiser.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InterestOrganiser.Services
{
    public interface IDBFirebase
    {
        Task<List<FirebaseItem>> GetItemsForUser(string owner);
        Task<FirebaseItem> CheckItem(string owner, string id);
        Task AddItem(FirebaseItem item);
        Task UpdateItem(FirebaseItem item);
        Task DeleteItem(string key);
    }
}
