using PomodoroTimer.Models;
using PomodoroTimer.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PomodoroTimer.ViewModels.ObjectViewModel
{
    public class UserTaskViewModel : BaseViewModel
    {
        private string _taskName;
        private string _taskColor;
        private string _taskIcon;
        private string _goalInterval;
        private int _goalPomodoroCount;
        private int _finishedPomodoroCount;
        private bool _haveGoal = false;
        private bool _isGaolAchived = false;

        public Guid Id { get; set; }
        public UserTask UserTask { get; private set; }
        public bool IsGaolAchived { get { return _isGaolAchived; } set { SetProperty(ref _isGaolAchived, value); } }
        public bool HaveGoal { get { return _haveGoal; } set { SetProperty(ref _haveGoal, value); } }
        public string TaskName { get { return _taskName; } set { SetProperty(ref _taskName, value); } }
        public string TaskColor { get { return _taskColor; } set { SetProperty(ref _taskColor, value); } }
        public string TaskIcon { get { return _taskIcon; } set { SetProperty(ref _taskIcon, value); } }
        public string GoalInterval { get { return _goalInterval; } set { SetProperty(ref _goalInterval, value); } }
        public int GoalPomodoroCount { get { return _goalPomodoroCount; } set { SetProperty(ref _goalPomodoroCount, value); } }
        public int FinishedPomodoroCount
        {
            get { return _finishedPomodoroCount; }
            set
            {
                SetProperty(ref _finishedPomodoroCount, value);
                if (HaveGoal)
                {
                    if (FinishedPomodoroCount >= GoalPomodoroCount)
                        IsGaolAchived = true;
                }
            }
        }

        public UserTaskViewModel()
        {

        }
        public UserTaskViewModel(UserTask userTask)
        {
            Init(userTask);
        }
        public void Init(UserTask userTask)
        {
            this.UserTask = userTask;
            Id = userTask.Id;
            TaskName = userTask.TaskName;
            TaskColor = userTask.TaskColor;
            TaskIcon = userTask.TaskIcon;
            if (userTask.TaskGoal != null)
            {
                HaveGoal = true;
                GoalInterval = userTask.TaskGoal.GoalInterval.ToString();
                GoalPomodoroCount = userTask.TaskGoal.PomodoroCount;
                if (userTask.TaskStatistic != null)
                    switch (userTask.TaskGoal.GoalInterval)
                    {
                        case GoalFrequency.Daily:
                            FinishedPomodoroCount = userTask.TaskStatistic.DailyFinishedCount;
                            break;
                        case GoalFrequency.Weekly:
                            FinishedPomodoroCount = userTask.TaskStatistic.WeeklyFinishedCount;
                            break;
                        case GoalFrequency.Mountly:
                            FinishedPomodoroCount = userTask.TaskStatistic.MountlyFinishedCount;
                            break;
                        case GoalFrequency.Yearly:
                            FinishedPomodoroCount = userTask.TaskStatistic.YearlyFinishedCount;
                            break;
                        default:
                            break;
                    }
                if (FinishedPomodoroCount >= GoalPomodoroCount)
                    IsGaolAchived = true;
            }
            else
            {
                HaveGoal = false;
                GoalInterval = "Yearly";
                if (userTask.TaskStatistic != null)
                {
                    FinishedPomodoroCount = userTask.TaskStatistic.YearlyFinishedCount;
                }
            }

        }

    }
}
