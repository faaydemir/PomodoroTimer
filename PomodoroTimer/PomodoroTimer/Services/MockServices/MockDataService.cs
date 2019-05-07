using System;
using System.Linq;
using System.Collections.Generic;
using PomodoroTimer.Enums;
using PomodoroTimer.Models;
using PomodoroTimer.Services;
using PomodoroTimer.ViewModels.ObjectViewModel;
using Helper.Services;

namespace PomodoroTimer
{
    internal class MockDataService
    {
        private static int createdTaskCount = 0;
        public static List<PomodoroSession> CreateStatisticData(int taskCount, int dailypomodoroCount, DateTime startTime, DateTime finishTime)
        {
            List<PomodoroSession> sessions = new List<PomodoroSession>();
            List<TaskStatistic> statistics = new List<TaskStatistic>();
            var userTasks = new List<UserTask>();
            for (int i = 0; i < 100; i++)
                userTasks.Add(MockDataService.CreateUserTask());

            var dayCount = (finishTime - startTime).TotalDays;

            var r = new Random();
            int startCount = dailypomodoroCount / 2;

            for (int d = 0; d < dayCount; d++)
            {

                var session = new PomodoroSession() { Day = startTime.AddDays(d), Id = Guid.NewGuid() };
                for (int i = 0; i < startCount; i++)
                {
                    var taskIndex = new Random().Next() % userTasks.Count;
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
                if(haveGoal)
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
            UserTask userTask = new UserTask()
            {
                Id = Guid.NewGuid(),
                TaskName = "Task" + ++createdTaskCount,
                TaskGoal = new TaskGoal()
                {
                    GoalInterval = GoalFrequency.Daily,
                    PomodoroCount = 15,
                },

            };
            return userTask;
        }
    }
}