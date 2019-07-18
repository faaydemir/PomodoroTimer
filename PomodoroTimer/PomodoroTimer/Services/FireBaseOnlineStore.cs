using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using PomodoroTimer.Enums;
using PomodoroTimer.Models;
using PomodoroTimer.Services.Interfaces;

namespace PomodoroTimer.Services
{
    public class FireBaseOnlineStore : IOnlineDataStore
    {
        private AplicationUser user;
        private string BaseUrl = AppKeysAndSecrets.Get("firebaseUrl");
        Dictionary<Type, string> TableNames;

        public FireBaseOnlineStore(AplicationUser user)
        {
            this.user = user;
            TableNames = new Dictionary<Type, string>();
        }


        private string GetTableName(Type type)
        {
            if (!TableNames.ContainsKey(type))
            {
                TableNames[type] = type.Name;
            }

            return TableNames[type];
        }


        public async Task<bool> AddTaskStatisticsAsync(TaskStatistic taskStatistic)
        {
            try
            {
                var store = new FirebaseDataStore<TaskStatistic>(BaseUrl, GetTableName(taskStatistic.GetType()));
                await store.AddItemAsync(taskStatistic);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> AddLog(AppLog appLog)
        {
            try
            {
                var store = new FirebaseDataStore<AppLog>(BaseUrl, GetTableName(appLog.GetType()));
                await store.AddItemAsync(appLog);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
