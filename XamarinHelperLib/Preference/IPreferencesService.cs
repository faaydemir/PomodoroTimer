using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XamarinHelpers.Preference
{
    public interface IPreferencesService
    {
        string Key { get; }
        Task<bool> SaveAsync();
        Task<bool> LoadAsync();
        bool Save();
        bool Load();
        void Clear();
    }
}
