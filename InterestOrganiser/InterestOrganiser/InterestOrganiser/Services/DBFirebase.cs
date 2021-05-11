using Firebase.Database;
using InterestOrganiser.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using Firebase.Database.Query;

namespace InterestOrganiser.Services
{
    public class DBFirebase : IDBFirebase
    {
        private FirebaseClient client;
        public DBFirebase()
        {
            client = new FirebaseClient("https://fir-database-aff8d-default-rtdb.firebaseio.com/");
        }

        public async Task AddItem(FirebaseItem item)
        {
            await client.Child("Items").PostAsync(JsonConvert.SerializeObject(item));
        }

        public async Task DeleteItem(string key)
        {
            await client.Child("Items").Child(key).DeleteAsync();
        }

        public async Task<FirebaseItem> CheckItem(string owner, string id)
        {
            var getItem = await client.Child("Items").OnceAsync<FirebaseItem>();
            if(getItem != null)
            {
                var item = getItem.FirstOrDefault(x => x.Object.Owner == owner && x.Object.ID == id);
                if (item != null)
                {
                    return item.Object;
                }
                else
                {
                    return new FirebaseItem();
                }
            }
            return new FirebaseItem();
        }

        public async Task<List<FirebaseItem>> GetItemsForUser(string owner)
        {
            var getItems = await client.Child("Items").OnceAsync<FirebaseItem>();
            if(getItems != null)
            {
                var items = getItems.Where(x => x.Object.Owner == owner).Select(y => y.Object).ToList();
                return items;
            }
            return new List<FirebaseItem>();
        }

        public async Task UpdateItem(FirebaseItem item)
        {
            var itemToUpdate = await client.Child("Items").OnceAsync<FirebaseItem>();
            if(itemToUpdate != null)
            {
                var specificItem = itemToUpdate.FirstOrDefault(x => x.Object.Owner == item.Owner && x.Object.ID == item.ID);
                if(specificItem != null)
                {
                    if(item.Realised == false && item.ToRealise == false)
                    {
                        await DeleteItem(specificItem.Key);
                    }
                    else
                    {
                        await client.Child("Items").Child(specificItem.Key).PutAsync(JsonConvert.SerializeObject(item));
                    }
                    
                }
                else
                {
                    await AddItem(item);
                }
            }
            
        }
    }
}
