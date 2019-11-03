using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroTimer.Services
{
    public class FirebaseDataStore<T>
            where T : class
    {
        private string BaseUrl;
        private readonly ChildQuery _query;

        public FirebaseDataStore(string baseurl, string path)
        {
            BaseUrl = baseurl;
            _query = new FirebaseClient(BaseUrl).Child(path);
        }

        public async Task<bool> AddItemAsync(T item)
        {
            try
            {
                await _query
                    .PostAsync(item);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> UpdateItemAsync(string id, T item)
        {
            try
            {
                await _query
                    .Child(id)
                    .PutAsync(item);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            try
            {
                await _query
                    .Child(id)
                    .DeleteAsync();
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public async Task<T> GetItemAsync(string id)
        {
            try
            {
                return await _query
                    .Child(id)
                    .OnceSingleAsync<T>();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false)
        {
            try
            {
                var firebaseObjects = await _query
                    .OnceAsync<T>();

                return firebaseObjects
                    .Select(x => x.Object);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
