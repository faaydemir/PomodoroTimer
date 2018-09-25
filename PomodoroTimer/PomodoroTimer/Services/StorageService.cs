using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using PomodoroTimer.Models;
using Xamarin.Essentials;
namespace PomodoroTimer.Services
{
    public class StorageModel
    {
        public AplicationUser User { get; set; }
        public AppSettings AppSettings { get; set; }
        public List<PomodoroSession> Sessions { get; set; } = new List<PomodoroSession>();
        public List<UserTask> UserTasks { get; set; } = new List<UserTask>();
    }

    public class StorageService : IStorageService
    {
        private StorageModel StorageModel { get; set; }



        public StorageService()
        {
            if (StorageModel == null)
            {
                StorageModel = Load<StorageModel>() ?? new StorageModel();
            }

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
            return Save<StorageModel>(StorageModel);
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
            return Save<StorageModel>(StorageModel);
        }

        public bool UpdateUserTask(UserTask userTask)
        {
            if (userTask == null)
                return false;

            StorageModel.UserTasks.RemoveAll(x => x.Id == userTask.Id);
            StorageModel.UserTasks.Add(userTask);

            return Save<StorageModel>(StorageModel);
        }

        private bool Save<T>(T model) where T : class
        {
            var serializer = new XmlSerializer(model.GetType());
            var stringWriter = new StringWriter();
            serializer.Serialize(stringWriter, model);
            try
            {
                Preferences.Set("DataStore", stringWriter.ToString());
                return true;
            }
            catch
            {
                return false;
            }
        }

        private T Load<T>() where T : class, new()
        {
            var serializer = new XmlSerializer(typeof(T));
            var dataString = Preferences.Get("DataStore", string.Empty);

            if (dataString == null || dataString == "")
                return new T();

            var stringReader = new StringReader(dataString);

            try
            {
                return (T)serializer.Deserialize(stringReader);
            }
            catch
            {
                return null;
            }
        }
        private void Clear()
        {

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
            return Save(StorageModel);
        }
        public bool ClearStatistics(DateTime startTime, DateTime finishTime)
        {
            StorageModel.Sessions.RemoveAll(session => session.Day >= startTime && session.Day <= finishTime);
            Save(StorageModel);
            return true;
        }
        private void GetStatistics()
        {
            var allFinishedTask = StorageModel.Sessions.SelectMany(x => x.FinishedTaskInfo);
            var yearlyFinishedTask = allFinishedTask.Where(x => x.FinishedTime.Year == DateTime.Now.Year);
            var mountlyFinishedTask = yearlyFinishedTask.Where(x => x.FinishedTime.Month == DateTime.Now.Month);
            var weeklyFinishedTask = mountlyFinishedTask.Where(x => GetIso8601WeekOfYear(x.FinishedTime) == GetIso8601WeekOfYear(DateTime.Now));
            var dailyFinishedTask = mountlyFinishedTask.Where(x => x.FinishedTime.DayOfYear == DateTime.Now.DayOfYear);

            foreach (var userTask in StorageModel.UserTasks)
            {
                var taskStatistic = new FinishedTaskStatistic();
                taskStatistic.DailyFinishedCount = dailyFinishedTask.Where(x => x.TaskId == userTask.Id)?.Count() ?? 0;
                taskStatistic.WeeklyFinishedCount = weeklyFinishedTask.Where(x => x.TaskId == userTask.Id)?.Count() ?? 0;
                taskStatistic.MountlyFinishedCount = mountlyFinishedTask.Where(x => x.TaskId == userTask.Id)?.Count() ?? 0;
                taskStatistic.YearlyFinishedCount = yearlyFinishedTask.Where(x => x.TaskId == userTask.Id)?.Count() ?? 0;
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
            return Save(StorageModel);
        }

        // https://stackoverflow.com/questions/11154673/get-the-correct-week-number-of-a-given-date
        private int GetIso8601WeekOfYear(DateTime time)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
    }
}
