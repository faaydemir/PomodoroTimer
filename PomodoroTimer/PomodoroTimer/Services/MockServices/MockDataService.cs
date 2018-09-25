using System;
using PomodoroTimer.Enums;
using PomodoroTimer.Models;

namespace PomodoroTimer
{
    internal class MockDataService
    {
        private static int createdTaskCount = 0;
        public static UserTask CreateUserTask()
        {
            UserTask userTask = new UserTask()
            {
                Id = Guid.NewGuid(),
                TaskName = "Task" + ++createdTaskCount,
                SubTasks = new System.Collections.Generic.List<UserTask>(),
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