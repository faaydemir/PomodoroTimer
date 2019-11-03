using System;
using System.Linq;
using System.Collections.Generic;
using PomodoroTimer.Enums;
using PomodoroTimer.Models;
using PomodoroTimer.Services;
using PomodoroTimer.ViewModels.ObjectViewModel;
using Helper.Services;
using System.Threading.Tasks;

namespace PomodoroTimer
{
    internal class MockDataService : IStorageService
    {
        private static int createdTaskCount = 0;
        private List<UserTask> UserTasks;
        ApplicationThema ApplicationThema;
        private List<PomodoroSession> Sessions { get; set; }

        private static Random Random = new Random();
        public static List<PomodoroSession> CreateStatisticData(List<UserTask> userTasks, int dailypomodoroCount, DateTime startTime, DateTime finishTime)
        {
            List<PomodoroSession> sessions = new List<PomodoroSession>();
            if (userTasks == null)
            {
                userTasks = new List<UserTask>();
                for (int i = 0; i < 10; i++)
                    userTasks.Add(MockDataService.CreateUserTask());
            }
            var dayCount = (finishTime - startTime).TotalDays;

            int startCount = dailypomodoroCount / 2;

            for (int d = 0; d < dayCount; d++)
            {

                var session = new PomodoroSession() { Day = startTime.AddDays(d), Id = Guid.NewGuid() };
                for (int i = 0; i < startCount; i++)
                {
                    var taskIndex = Random.Next() % userTasks.Count;
                    TaskStatistic s = new TaskStatistic();
                    s.Duration = TimeSpan.FromMinutes(20);
                    s.Id = Guid.NewGuid();
                    s.TaskId = userTasks[taskIndex].Id;
                    s.TaskName = userTasks[taskIndex].TaskName;
                    s.FinishedTime = startTime.AddDays(d);
                    session.FinishedTaskInfo.Add(s);
                }

                if (startCount > dailypomodoroCount)
                    startCount = dailypomodoroCount / 2;
                startCount++;
                sessions.Add(session);
            }

            return sessions;
        }
        public static List<UserTaskViewModel> CreateUserTaskVM(int count)
        {
            var userTaskVMList = new List<UserTaskViewModel>();
            bool haveGoal = false;
            var intervals = Enum.GetValues(typeof(GoalFrequency))
                        .Cast<GoalFrequency>()
                        .Select(v => v.ToString())
                        .ToList();
            var random = new Random();
            for (int i = 1; i <= count; i++)
            {
                haveGoal = !haveGoal;
                int GoalPomodoroCount = 0;
                int frequncyIndex = random.Next(0, intervals.Count - 1);
                int FinishedPomodoroCount = random.Next(0, 5 * (frequncyIndex + 3));
                if (haveGoal)
                    GoalPomodoroCount = random.Next(5 * (frequncyIndex + 1), 5 * (frequncyIndex + 3));

                var a = new UserTaskViewModel()
                {
                    FinishedPomodoroCount = FinishedPomodoroCount,
                    GoalPomodoroCount = GoalPomodoroCount,
                    HaveGoal = haveGoal,
                    GoalInterval = intervals[frequncyIndex],
                    IsGaolAchived = FinishedPomodoroCount >= GoalPomodoroCount,
                    TaskName = "Task" + i,
                    TaskColor = ColorPickService.GetRandom(),
                };



                userTaskVMList.Add(a);
            }
            return userTaskVMList;
        }
        public static UserTask CreateUserTask()
        {
            var userTaskVMList = new List<UserTaskViewModel>();
            bool haveGoal = false;
            var intervals = Enum.GetValues(typeof(GoalFrequency))
                        .Cast<GoalFrequency>()
                        .Select(v => v.ToString())
                        .ToList();
   

            haveGoal = !haveGoal;
            int GoalPomodoroCount = 0;
            int frequncyIndex = Random.Next(0, intervals.Count - 1);
            int FinishedPomodoroCount = Random.Next(0, 5 * (frequncyIndex + 3));
            if (haveGoal)
                GoalPomodoroCount = Random.Next(5 * (frequncyIndex + 1), 5 * (frequncyIndex + 3));

            UserTask userTask = new UserTask()
            {
                Id = Guid.NewGuid(),
                TaskName = "Task" + ++createdTaskCount,
                TaskColor = ColorPickService.GetRandom(),
                TaskStatistic = new FinishedTaskStatistic()
                {
                    DailyFinishedCount=FinishedPomodoroCount,
                    MonthlyFinishedCount = FinishedPomodoroCount,
                    WeeklyFinishedCount = FinishedPomodoroCount,
                    YearlyFinishedCount = FinishedPomodoroCount,
                },
                TaskGoal = new TaskGoal()
                {
                    GoalInterval = (GoalFrequency)frequncyIndex,
                    PomodoroCount = GoalPomodoroCount,
                },

            };
            return userTask;
        }

        public MockDataService()
        {
            UserTasks = new List<UserTask>();
            for (int i = 0; i < 11; i++)
            {
                UserTasks.Add(CreateUserTask());
            }
            Sessions = CreateStatisticData(UserTasks, 20, DateTime.Now.AddDays(-360), DateTime.Now);
            ApplicationThema = ApplicationThema.NightThema;
        }

        public AplicationUser GetUser()
        {
            return AppConstants.DEFAULT_USER;
        }

        public AppSettings GetAppSettings()
        {
            return AppConstants.DEFAULT_APP_SETTINGS;
        }

        public PomodoroSession GetSession()
        {
            return new PomodoroSession();
        }

        public List<UserTask> GetAllUserTask(AplicationUser user)
        {
            return UserTasks;
        }

        public List<TaskStatistic> GetStatisticData(DateTime startTime, DateTime finishTime)
        {
            List<TaskStatistic> statistics = new List<TaskStatistic>();

            foreach (var session in Sessions)
            {
                if (session.Day >= startTime && session.Day <= finishTime)
                {
                    if (session.FinishedTaskInfo != null)
                        statistics.AddRange(session.FinishedTaskInfo);
                }
            }

            return statistics;
        }

        public PomodoroTimerState GetLastState()
        {
            return new PomodoroTimerState();
        }

        public Task<bool> SaveAppState(PomodoroTimerState appState)
        {
            return Task.FromResult(true);
        }

        public Task<bool> UpdateUserTask(UserTask task)
        {
            return Task.FromResult(true);
        }

        public Task<bool> AddNewUserTaskAsync(UserTask userTask)
        {
            return Task.FromResult(true);
        }

        public Task<bool> RemoveUserTask(UserTask userTask)
        {
            return Task.FromResult(true);
        }

        public Task<bool> SetAppSettings(AppSettings settings)
        {
            return Task.FromResult(true);
        }

        public Task<bool> UpdateSessionInfo(PomodoroSession currentSession)
        {
            return Task.FromResult(true);
        }

        public Task<bool> ClearStatistics(DateTime startTime, DateTime finishTime)
        {
            return Task.FromResult(true);
        }

        public Task<bool> SaveAppThema(ApplicationThema applicationThema)
        {
            ApplicationThema = applicationThema;
            return Task.FromResult(true);
        }

        public Task<ApplicationThema> GetAppThema()
        {
            return Task.FromResult(ApplicationThema);
        }
    }
}