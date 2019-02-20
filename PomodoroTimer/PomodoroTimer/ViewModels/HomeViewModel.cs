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
using XamarinHelpers.MVVM;

namespace PomodoroTimer.ViewModels
{
    public class HomeViewModel : PageViewModel
    {
        #region fields
        private PomdoroStatus _timerInfo;
        private TimeSpan _pomodoroDuration;
        private TimeSpan _remainningTime;
        private string _taskName;
        private UserTask _activeTask;
        public Chart _chart;
        private ObservableCollection<UserTaskViewModel> _userTask;
        private float _tick = 0;
        private int _tickCount;




        #endregion

        #region props

        private IAppService AppService { get; set; }
        private UITimer UiTimer { get; set; }

        public Microcharts.Entry ProgressValue { get; private set; }
        public float ChartValue { get; set; } = 0;
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
        public float Tick
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

        public PomdoroStatus TimerInfo
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

            PomodoroDuration = AppService.PomodoroSettings.PomodoroDuration;
            ActiveTask = AppService.ActiveTask;

            UserTasks = new ObservableCollection<UserTaskViewModel>(AppService.UserTasks.Select(x => new UserTaskViewModel(x)));
            TimerInfo = appService.PomodoroStatus ?? new PomdoroStatus() { PomodoroState = PomodoroState.Ready, TimerState = TimerState.Stoped };

            AppService.UserTaskRemovedEvent += OnUserTaskRemoved;
            AppService.TimerFinishedEvent += OnTimerFinished;
            AppService.UserTaskModifiedEvent += OnUserTaskUpdate;
            AppService.AppResumedEvent += OnAppResumed;
            AppService.PomodoroTimerStatusChangedEvent += OnPomodoroStatusChanged;

            LoadState(TimerInfo);
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
                        TimerInfo = AppService.PausePomodoro();
                        PauseTimerTick();
                    }
                    else
                    {
                        TimerInfo = AppService.StartPomodoro();
                        StartTimerTick();
                    }

                }
            );

            StopTimer = new Command(
                execute: () =>
                {
                    TimerInfo.TimerState = TimerState.Stoped;
                    TimerInfo.PomodoroState = PomodoroState.Ready;
                    OnPropertyChanged("TimerInfo");

                    RemainingTimeValue = TimeSpan.Zero;
                    Tick = 0;
                    AppService.StopPomodoro();
                    StopTimerTick();
                }
            );
        }

        private void OnPomodoroStatusChanged(object sender, PomodoroTimerStatusChangedEventArgs eventArgs)
        {
            LoadState(eventArgs.NewStatus);
        }

        private void OnAppResumed(object sender, AppResumedEventArgs eventArgs)
        {
            LoadState(eventArgs.AppState);
        }

        public void LoadState(PomdoroStatus state)
        {
            if (state == null)
                return;

            TimerInfo = new PomdoroStatus()
            {
                PomodoroState = state.PomodoroState,
                RunTime = state.RunTime,
                TimerState = state.TimerState,
                StartTime = state.StartTime,
                RemainingTime = state.RemainingTime.Subtract(DateTime.Now.Subtract(state.StartTime))
            };

            OnPropertyChanged("TimerInfo");

            if (TimerInfo.TimerState == TimerState.Running)
                StartTimerTick();
        }

        private void StartTimerTick()
        {
            StopTimerTick();
            UiTimer = new UITimer(OnTick,()=> { });
            UiTimer.Start(TimerInfo.RemainingTime, TimeSpan.FromSeconds(1));
        }

        private void StopTimerTick()
        {
            if (UiTimer != null)
                UiTimer.Stop();
        }

        private void PauseTimerTick()
        {
            StopTimerTick();
        }

        private void OnTick(TimeSpan remaingTime)
        {
            TimerInfo.RemainingTime = remaingTime;
            AppService.SetTimerInfo(TimerInfo);
            RemainingTimeValue = TimerInfo.RemainingTime;
            UpdateChart(TimerInfo.RemainingTime, TimeSpan.Zero, TimerInfo.RunTime);
            OnPropertyChanged("TimerInfo");
        }

        private void OnUserTaskRemoved(object sender, UserTaskModifiedEventArgs args)
        {
            var removedTask = UserTasks.FirstOrDefault(x => x.Id == args.UserTask?.Id);
            if (removedTask != null)
            {
                UserTasks.Remove(removedTask);
            }
        }
        private void OnUserTaskUpdate(object sender, UserTaskModifiedEventArgs args)
        {
            bool contain = false;
            for (int i = 0; i < UserTasks.Count; i++)
            {
                if (UserTasks[i].Id == args.UserTask?.Id)
                {
                    UserTasks[i] = new UserTaskViewModel(args.UserTask);
                    contain = true;
                    return;
                }
            }
            if (!contain)
            {
                UserTasks.Add(new UserTaskViewModel(args.UserTask));
            }
        }

        private void OnTimerFinished(object sender, PomodoroChangedEventArgs args)
        {
            StopTimerTick();

            Device.BeginInvokeOnMainThread(() =>
            {
                RemainingTimeValue = TimeSpan.Zero;
                TimerInfo.TimerState = TimerState.Complated;
                OnPropertyChanged("TimerInfo");
                Tick = 0;
            });
        }

        private void UpdateChart(TimeSpan value, TimeSpan min, TimeSpan max)
        {
            double factor = TickCount / (max.TotalMilliseconds - min.TotalMilliseconds);
            Tick = (float)((max.TotalMilliseconds - value.TotalMilliseconds) * factor);
        }
    }
}
