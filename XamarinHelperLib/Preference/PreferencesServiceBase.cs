using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace XamarinHelpers.Preference
{
    public abstract class PreferencesServiceBase<TPreferenceModel> : IPreferencesService where TPreferenceModel : new()
    {
        public TPreferenceModel StorageModel { get; set; }
        public string Key { get; }
        public PreferencesServiceBase(string key = null)
        {
            Key = key ?? $"{GetType().Name.ToLower()}_datastore";
        }

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
                Preferences.Set(Key, jsonString);
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
                var dataString = Preferences.Get(Key, string.Empty);
                StorageModel = JsonConvert.DeserializeObject<TPreferenceModel>(dataString);
                return StorageModel != null;
            }
            catch
            {
                StorageModel = new TPreferenceModel();
                return false;
            }
        }
        public void Clear()
        {
            Preferences.Set(Key, "");
        }
    }
}
