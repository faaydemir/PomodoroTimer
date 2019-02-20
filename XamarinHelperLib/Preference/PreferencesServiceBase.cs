using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace XamarinHelpers.Preference
{
    public abstract class PreferencesServiceBase<TPreferenceModel> : IPreferencesService
    {
        public TPreferenceModel StorageModel { get; set; }

        public Task<bool> SaveAsync()
        {
            return Task.Run(() => { return Save(); });
        }
        public Task<bool> LoadAsync()
        {
            return Task.Run(() => { return Load(); });
        }
        public bool Save()
        {
            try
            {
                var jsonString = JsonConvert.SerializeObject(StorageModel);
                Preferences.Set("DataStore", jsonString);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Load()
        {
            try
            {
                var dataString = Preferences.Get("DataStore", string.Empty);
                StorageModel = JsonConvert.DeserializeObject<TPreferenceModel>(dataString);
                return StorageModel != null;
            }
            catch
            {
                return false;
            }
        }
        public void Clear()
        {
            Preferences.Set("DataStore", "");
        }
    }
}
