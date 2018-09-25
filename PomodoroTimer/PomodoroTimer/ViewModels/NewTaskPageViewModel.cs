using PomodoroTimer.Messaging;
using PomodoroTimer.Enums;
using PomodoroTimer.Models;
using PomodoroTimer.Services;
using PomodoroTimer.Validations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace PomodoroTimer.ViewModels
{
    public class NewTaskPageViewModel : PageViewModel
    {
        private readonly string TaskNameValdationMessage = "Task Name could not  be empty.";

        private string _taskName = string.Empty;
        private int _pomodoroCount = 4;
        private int _pomodoroDuration = 20;
        private int _smallBreakDuration = 5;
        private int _sessionPomodoroCount = 5;
        private int _largeBreakDuration = 10;

        internal void UpdateSelectedItem()
        {
            SelectedIndex = 2;
        }

        private int _goalPomodoroCount = 5;
        private int _selectedIndex = 0;

        private bool _isHasGoal = false;
        private string _taskColor;
        private string _validataionMessage;
        private string _goalFrequency;
        private bool _isHaveSettings;

        private bool IsEdit { get; } = false;

        public ObservableCollection<string> Frequencies { get; set; }
        public ICommand SaveCommand { get; set; }

        private ICommand _ok;


        public ICommand Ok
        {
            get { return _ok; }
            set { SetProperty(ref _ok, value); }
        }
        public bool IsHaveSettings 
        {
            get { return _isHaveSettings ; }
            set { SetProperty(ref _isHaveSettings, value); }
        }

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set { SetProperty(ref _selectedIndex, value); }
        }

        public string ValidationMessage
        {
            get { return _validataionMessage; }
            set { SetProperty(ref _validataionMessage, value); }
        }

        public string GoalFrequency
        {
            get { return _goalFrequency; }
            set
            {
                SetProperty(ref _goalFrequency, value);
            }
        }

        public string TaskColor
        {
            get { return _taskColor; }
            set { SetProperty(ref _taskColor, value); }
        }

        public bool IsHasGoal
        {
            get { return _isHasGoal; }
            set
            {
                SetProperty(ref _isHasGoal, value);
            }
        }

        public int GoalPomodoroCount
        {
            get { return _goalPomodoroCount; }
            set { SetProperty(ref _goalPomodoroCount, value); }
        }


        public int LargeBreakDuration
        {
            get { return _largeBreakDuration; }
            set { SetProperty(ref _largeBreakDuration, value); }
        }

        public int SessionPomodoroCount
        {
            get { return _sessionPomodoroCount; }
            set { SetProperty(ref _sessionPomodoroCount, value); }
        }

        public int SmallBreakDuration
        {
            get { return _smallBreakDuration; }
            set { SetProperty(ref _smallBreakDuration, value); }
        }


        public int PomodoroDuration
        {
            get { return _pomodoroDuration; }
            set { SetProperty(ref _pomodoroDuration, value); }
        }

        public int PomodoroCount

        {
            get { return _pomodoroCount; }
            set { SetProperty(ref _pomodoroCount, value); }
        }

        public Guid Id { get; private set; }

        public string TaskName
        {
            get { return _taskName; }
            set { SetProperty(ref _taskName, value); }
        }

        public ObservableCollection<string> Colors { get; set; }

        public NewTaskPageViewModel()
        {
            Id = Guid.NewGuid();

            Colors =  new ObservableCollection<string>
            {
                "#F44336",
                "#D50000",
                "#C51162",
                "#4A148C",
                "#7986CB",
                "#0D47A1",
                "#2962FF",
                "#01579B",
                "#0091EA",
                "#0097A7",
                "#006064",
                "#004D40",
                "#1B5E20",
                "#33691E",
                "#827717",
                "#AEEA00",
                "#E65100",
                "#F4511E",
                "#DD2C00",
                "#78909C",
                "#212121",
                "#3E2723",
                "#795548"
            };

            Title = "New Task";
            Frequencies = new ObservableCollection<string>(Enum.GetNames(typeof(GoalFrequency)).ToList());
            GoalFrequency = Frequencies.ElementAt(0);
            TaskColor = ColorPickService.NextRandom();
            SaveCommand = new Command(
                execute: async (o) =>
                {
                    if (IsBusy)
                        return;

                    IsBusy = true;

                    var userTask = CreateUserTask();
                    if (userTask != null)
                    {
                        MessagingCenter.Send(this, AppMessages.SaveUserTaskMessage, userTask);
                        await Page.Navigation.PopModalAsync();
                    }
                    IsBusy = false;
                }
            );
        }


        public NewTaskPageViewModel(UserTask userTask) : this()
        {
            Id = userTask.Id;
            TaskName = userTask.TaskName;
            TaskColor = userTask.TaskColor;
            if (userTask.PomodoroSettings != null)
            {
                IsHaveSettings  = true;
                PomodoroDuration = userTask.PomodoroSettings.PomodoroDuration;
                SmallBreakDuration = userTask.PomodoroSettings.PomodoroBreakDuration;
                SessionPomodoroCount = userTask.PomodoroSettings.SessionPomodoroCount;
                LargeBreakDuration = userTask.PomodoroSettings.SessionBreakDuration;
            }
            if (userTask.TaskGoal != null)
            {
                IsHasGoal = true;
                GoalFrequency = userTask.TaskGoal.GoalInterval.ToString();
                GoalPomodoroCount = userTask.TaskGoal.PomodoroCount;
            }
            IsEdit = true;
        }

        public UserTask CreateUserTask()
        {
            UserTask userTask = new UserTask
            {
                TaskColor = (string)TaskColor,
                Id = Id
            };
            if (TaskName == null || TaskName == "")
            {
                ValidationMessage = TaskNameValdationMessage;
                return null;
            }

            userTask.TaskName = TaskName;

            if (IsHasGoal)
            {
                var isPomodoroCountValid = IntergerBoundValidationController.Check(1, 1000, this.PomodoroCount, "Pomodoro Count");
                if (!isPomodoroCountValid.result)
                {
                    ValidationMessage = isPomodoroCountValid.message;
                    return null;
                }
                var taskGoal = new TaskGoal()
                {
                    GoalInterval = (GoalFrequency)System.Enum.Parse(typeof(GoalFrequency), GoalFrequency),
                    PomodoroCount = PomodoroCount,
                };
                userTask.TaskGoal = taskGoal;
            }
            if (IsHaveSettings )
            {

                PomodoroSettings pomodoroSettings = new PomodoroSettings();

                var isSmallBreakValid = IntergerBoundValidationController.Check(1, 150, this.SmallBreakDuration, "Small Break Duration");
                var isLargeBreakDurationValid = IntergerBoundValidationController.Check(1, 150, this.LargeBreakDuration, "Large Break Duration");
                var isPomodoroDurationValid = IntergerBoundValidationController.Check(1, 150, this.PomodoroDuration, "Pomodoro Duration");
                var isSessionPomodoroCountValid = IntergerBoundValidationController.Check(1, 150, this.SessionPomodoroCount, "Session Pomodoro Count");

                if (!isPomodoroDurationValid.result)
                {
                    ValidationMessage = isPomodoroDurationValid.message;
                    return null;
                }
                if (!isSmallBreakValid.result)
                {
                    ValidationMessage = isSmallBreakValid.message;
                    return null;
                }
                if (!isLargeBreakDurationValid.result)
                {
                    ValidationMessage = isLargeBreakDurationValid.message;
                    return null;
                }
                if (!isSessionPomodoroCountValid.result)
                {
                    ValidationMessage = isSessionPomodoroCountValid.message;
                    return null;
                }


                pomodoroSettings.AutoContinue = false;
                pomodoroSettings.SessionPomodoroCount = SessionPomodoroCount;
                pomodoroSettings.PomodoroBreakDuration = SmallBreakDuration;
                pomodoroSettings.SessionBreakDuration = LargeBreakDuration;
                pomodoroSettings.PomodoroDuration = PomodoroDuration;
                userTask.PomodoroSettings = pomodoroSettings;
            }

            return userTask;
        }
    }
}
