using Microcharts;
using Plugin.LocalNotifications;
using PomodoroTimer.Enums;
using PomodoroTimer.Messaging;
using PomodoroTimer.Models;
using PomodoroTimer.Services;
using PomodoroTimer.ViewModels.ObjectViewModel;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace PomodoroTimer.ViewModels
{
    public class HomeViewModel : PageViewModel
    {
        #region fields
        private TimerInfo _timerInfo;
        private TimeSpan _pomodoroDuration;
        private TimeSpan _remainningTime;
        private string _taskName;
        private UserTask _activeTask;
        public Chart _chart;
        private ObservableCollection<UserTaskViewModel> _userTask;
        private int _tick = 0;
        private int _tickCount;

        public Microcharts.Entry ProgressValue { get; private set; }
        public float ChartValue { get; set; } = 0;

        #endregion

        #region props
        private PomodoroSession PomodoroSession { get; set; }
        private IAppService AppService { get; set; }

        public ICommand SetTimerStatus { get; set; }
        public ICommand StopTimer { get; set; }
        public ICommand SetTask { get; set; }
        public ICommand ChangeTask { get; set; }

        public Chart Chart
        {
            get { return _chart; }
            set { SetProperty(ref _chart, value); }
        }
        public int TickCount
        {
            get { return _tickCount; }
            set { SetProperty(ref _tickCount, value); }
        }

        public int Tick
        {
            get { return _tick; }
            set { SetProperty(ref _tick, value); }
        }

        public string TaskName
        {
            get { return _taskName; }
            set { SetProperty(ref _taskName, value); }
        }

        public string RemainingTime
        {
            get
            {
                string value = "";
                if (RemainingTimeValue.Hours != 0)
                    value += RemainingTimeValue.Hours;

                if (RemainingTimeValue.Minutes < 10)
                    value += "0";

                value += RemainingTimeValue.Minutes + ":";

                if (RemainingTimeValue.Seconds < 10)
                    value += "0";

                value += RemainingTimeValue.Seconds;
                return value;
            }
        }

        public TimeSpan RemainingTimeValue
        {
            get { return _remainningTime; }
            set
            {
                SetProperty(ref _remainningTime, value);
                OnPropertyChanged("RemainingTime");
            }
        }

        public TimeSpan PomodoroDuration
        {
            get { return _pomodoroDuration; }
            set { SetProperty(ref _pomodoroDuration, value); }
        }

        public TimerInfo TimerInfo
        {
            get { return _timerInfo; }
            set { SetProperty(ref _timerInfo, value); }
        }

        public UserTask ActiveTask
        {
            get { return _activeTask; }
            set
            {
                SetProperty(ref _activeTask, value);
                TaskName = ActiveTask.TaskName;
            }
        }

        public ObservableCollection<UserTaskViewModel> UserTasks
        {
            get
            {
                return _userTask;
            }
            set
            {
                SetProperty(ref _userTask, value);
            }
        }
        #endregion

        public HomeViewModel(IAppService appService)
        {
            AppService = appService;
            TickCount = 90;
            PomodoroDuration = TimeSpan.FromMinutes(AppService.PomodoroSettings.PomodoroDuration);
            ActiveTask = AppService.ActiveTask;  
            UserTasks = new ObservableCollection<UserTaskViewModel>(AppService.UserTasks.Select(x => new UserTaskViewModel(x)));
            UserTasks.Insert(0, new UserTaskViewModel(AppConstants.FREE_RUN_TASK));
            TimerInfo = new TimerInfo() { PomodoroState = PomodoroState.Ready, TimerState = TimerState.Stoped };

            AppService.UserTaskRemovedEvent += OnUserTaskRemoved;
            AppService.TimerFinishedEvent += OnTimerFinished;
            AppService.UserTaskModifiedEvent += OnUserTaskModified;
            AppService.TimerTickEvent += OnTimerTick;

            ChangeTask = new Command(
                execute: async (o) =>
                {
                    if (o is UserTaskViewModel userTaskViewModel)
                    {
                        if (userTaskViewModel.Id == ActiveTask.Id)
                            return;

                        if (TimerInfo.TimerState == TimerState.Running)
                        {

                            var displayAlert = new DialogService(Page);
                            var changeTask = await displayAlert.DisplayAlert("Change Task", "Pomodoro will be cancelled. Did you want to continue", "ok", "cancel");
                            if (!changeTask)
                                return;
                        }

                        ActiveTask = userTaskViewModel.UserTask;
                        AppService.SetActiveTask(userTaskViewModel.UserTask);

                        RemainingTimeValue = TimeSpan.Zero;
                        TimerInfo.PomodoroState = PomodoroState.Ready;
                        OnPropertyChanged("TimerInfo");

                    }
                }
            );

            SetTimerStatus = new Command(
                execute: () =>
                {
                    if (TimerInfo.PomodoroState == PomodoroState.Pomodoro && TimerInfo.TimerState == TimerState.Running)
                    {
                        AppService.PausePomodoro();
                    }
                    else
                    {
                        AppService.StartPomodoro();
                    }
                }
            );

            StopTimer = new Command(
                execute: () =>
                {
                    TimerInfo.TimerState = TimerState.Stoped;
                    TimerInfo.PomodoroState = PomodoroState.Ready;
                    OnPropertyChanged("TimerInfo");
                    AppService.StopPomodoro();
                }
            );

        }

        private void OnUserTaskRemoved(object sender, UserTaskModifiedEventArgs args)
        {
            var removedTask = UserTasks.FirstOrDefault(x => x.Id == args.UserTask?.Id);
            if (removedTask != null)
            {
                UserTasks.Remove(removedTask);
            }
        }

        private void OnTimerTick(object sender, PomodoroTimerTickEventArgs args)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                RemainingTimeValue = args.TimerInfo.RemainingTime;
                TimerInfo = args.TimerInfo;
                OnPropertyChanged("TimerInfo");
                UpdateChart(args.TimerInfo.RemainingTime, TimeSpan.Zero, args.TimerInfo.RunTime);
            });
        }
        private void OnUserTaskModified(object sender, UserTaskModifiedEventArgs args)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                bool contain = false;
                for(int i = 0; i< UserTasks.Count;i++)
                {
                    if(UserTasks[i].Id == args.UserTask?.Id)
                    {
                        UserTasks[i] = new UserTaskViewModel(args.UserTask);
                        contain = true;
                        return;
                    }
                }
                if(!contain)
                {
                    UserTasks.Add(new UserTaskViewModel(args.UserTask));
                }
            });
        }
        private void OnTimerFinished(object sender, PomodoroChangedEventArgs args)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                RemainingTimeValue = TimeSpan.Zero;
                TimerInfo.PomodoroState = args.NextState;
                TimerInfo.TimerState = TimerState.Complated;
                OnPropertyChanged("TimerInfo");
                Tick = 0;
            });
        }
        private void UpdateChart(TimeSpan value, TimeSpan min, TimeSpan max)
        {
            double factor = TickCount / (max.TotalMilliseconds - min.TotalMilliseconds);
            Tick = (int)((max.TotalMilliseconds - value.TotalMilliseconds) * factor);
        }
    }
}
