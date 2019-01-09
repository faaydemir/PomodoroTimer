using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PomodoroTimer.Models;
using PomodoroTimer.Utils;
using Xamarin.Essentials;
namespace PomodoroTimer.Services
{
    public class StorageModel
    {
        public AplicationUser User { get; set; }
        public AppSettings AppSettings { get; set; }
        public List<PomodoroSession> Sessions { get; set; } = new List<PomodoroSession>();
        public List<UserTask> UserTasks { get; set; } = new List<UserTask>();
        public PomdoroStatus AppState { get; internal set; }
    }
    public interface IPreferencesService
    {
        bool Save();
        bool Load();
        void Clear();
    }
    public abstract class PreferencesServiceBase<TPreferenceModel> : IPreferencesService
    {
        public TPreferenceModel StorageModel { get; set; }
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

    public class StorageService : PreferencesServiceBase<StorageModel>, IStorageService
    {
        public StorageService()
        {
            if (!Load())
                StorageModel = new StorageModel();
        }

        public bool AddNewUserTask(UserTask userTask)
        {
            if (StorageModel.UserTasks == null)
                StorageModel.UserTasks = new List<UserTask>();
            if (StorageModel.UserTasks.FirstOrDefault(x => x.Id == userTask.Id) == null)
            {
                StorageModel.UserTasks.Add(userTask);
            }
            else
            {
                StorageModel.UserTasks.RemoveAll(x => x.Id == userTask.Id);
                StorageModel.UserTasks.Add(userTask);
            }
            return Save();
        }

        public List<UserTask> GetAllUserTask(AplicationUser user)
        {
            GetStatistics();
            return StorageModel.UserTasks;

        }

        public AplicationUser GetUser()
        {
            return StorageModel.User;
        }

        public PomodoroSession GetSession()
        {
            var currentItem = StorageModel.Sessions.SingleOrDefault((x) => x.Day == DateTime.Today) ?? new PomodoroSession() { Day = DateTime.Today };
            return currentItem;
        }

        public bool RemoveUserTask(UserTask userTask)
        {
            if (userTask == null)
            {
                return false;
            }
            StorageModel.UserTasks.RemoveAll(x => x.Id == userTask.Id);
            return Save();
        }

        public bool UpdateUserTask(UserTask userTask)
        {
            if (userTask == null)
                return false;

            StorageModel.UserTasks.RemoveAll(x => x.Id == userTask.Id);
            StorageModel.UserTasks.Add(userTask);

            return Save();
        }

        public AppSettings GetAppSettings()
        {
            return StorageModel.AppSettings;
        }

        public bool SetAppSettings(AppSettings settings)
        {
            if (settings == null)
                return false;

            StorageModel.AppSettings = settings;

            return Save();
        }

        public bool ClearStatistics(DateTime startTime, DateTime finishTime)
        {

            StorageModel.Sessions.RemoveAll(session => session.Day >= startTime && session.Day <= finishTime);
            Save();

            return true;
        }

        private void GetStatistics()
        {
            var allFinishedTask = StorageModel.Sessions.SelectMany(x => x.FinishedTaskInfo);
            var yearlyFinishedTask = allFinishedTask.Where(x => x.FinishedTime.Year == DateTime.Now.Year);
            var mountlyFinishedTask = yearlyFinishedTask.Where(x => x.FinishedTime.Month == DateTime.Now.Month);
            var weeklyFinishedTask = allFinishedTask.Where(x => x.FinishedTime.Iso8601WeekOfYear() == DateTime.Now.Iso8601WeekOfYear());
            var dailyFinishedTask = weeklyFinishedTask.Where(x => x.FinishedTime.DayOfYear == DateTime.Now.DayOfYear);

            foreach (var userTask in StorageModel.UserTasks)
            {
                var taskStatistic = new FinishedTaskStatistic
                {
                    DailyFinishedCount = dailyFinishedTask.Where(x => x.TaskId == userTask.Id)?.Count() ?? 0,
                    WeeklyFinishedCount = weeklyFinishedTask.Where(x => x.TaskId == userTask.Id)?.Count() ?? 0,
                    MountlyFinishedCount = mountlyFinishedTask.Where(x => x.TaskId == userTask.Id)?.Count() ?? 0,
                    YearlyFinishedCount = yearlyFinishedTask.Where(x => x.TaskId == userTask.Id)?.Count() ?? 0
                };
                userTask.TaskStatistic = taskStatistic;
            }
        }

        public List<TaskStatistic> GetStatisticData(DateTime startTime, DateTime finishTime)
        {
            List<TaskStatistic> statistics = new List<TaskStatistic>();

            foreach (var session in StorageModel.Sessions)
            {
                if (session.Day >= startTime && session.Day <= finishTime)
                {
                    if (session.FinishedTaskInfo != null)
                        statistics.AddRange(session.FinishedTaskInfo);
                }
            }

            return statistics;
        }

        public bool UpdateSessionInfo(PomodoroSession currentSession)
        {
            var preValue = StorageModel.Sessions.SingleOrDefault((x) => x.Id == currentSession.Id);

            StorageModel.Sessions.RemoveAll((x) => x.Id == currentSession.Id);

            StorageModel.Sessions.Add(currentSession);

            return Save();
        }

        public PomdoroStatus ReadLastState()
        {
            return StorageModel.AppState;
        }

        public void SaveAppState(PomdoroStatus appState)
        {
            StorageModel.AppState = appState;
            Save();
        }
    }
}
