using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PomodoroTimer.Enums;
using PomodoroTimer.Models;
using PomodoroTimer.Services;


namespace PomodoroTimer
{
    public class AppMainService : IAppService
    {
        #region singleton
        static IAppService _instance;
        public static IAppService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AppMainService();
                }
                return _instance;
            }
        }
        #endregion

        #region events
        public event TimerTickEventHandler TimerTickEvent;
        public event TimerFinishedEventHandler TimerFinishedEvent;
        public event UserTaskModifiedEventHandler UserTaskModifiedEvent;
        public event UserTaskModifiedEventHandler UserTaskRemovedEvent;
        public event AppResumedEventHandler AppResumedEvent;
        #endregion


        #region services
        IStorageService StorageService;
        AlarmService AlarmService;
        IPomodoroControlService PomodoroControlService;
        INotificationService NotificationService;
        IFlipDetectionService GetFlipDetectionService;
        #endregion
        #region fields
        private PomodoroSettings _pomodoroSettings;

        // AplicationUser User;
        #endregion

        #region props
        public AppSettings AppSettings { get; set; }
        public AplicationUser User { get; set; }
        public List<UserTask> UserTasks { get; set; }
        public PomodoroSession CurrentSession { get; set; }

        public PomodoroSettings PomodoroSettings
        {
            get
            {
                return _pomodoroSettings;
            }
            set
            {
                _pomodoroSettings = value;
                if (PomodoroControlService != null)
                    PomodoroControlService.PomodoroSettings = PomodoroSettings;
            }
        }

        public UserTask ActiveTask { get; set; }
        public PomdoroStatus AppState { get; set; }
        public PomdoroStatus PomodoroStatus => PomodoroControlService.PomodoroStatus;

        public bool IsNotificationEnable { get; private set; } = false;
        #endregion

        AppMainService()
        {
            StorageService = new StorageService();
            AlarmService = new AlarmService();
            NotificationService = new NotificationService();
            PomodoroControlService = new PomodoroControlService(StorageService);

            AppSettings = StorageService.GetAppSettings() ?? AppConstants.DEFAULT_APP_SETTINGS;
            UserTasks = StorageService.GetAllUserTask(User);
            PomodoroSettings = AppSettings.PomodoroSettings;

            User = StorageService.GetUser() ?? AppConstants.DEFAULT_USER;
            ActiveTask = AppConstants.FREE_RUN_TASK;
            CurrentSession = StorageService.GetSession();

            AlarmService.SoundEnable = AppSettings.SoundAlarm;
            AlarmService.VibrationEnable = AppSettings.VibrationAlarm;

            PomodoroControlService.TimerFinishedEvent += OnTimerFinished;
            //PomodoroControlService.TimerTickEvent += OnTimerTick;
        }
        public void SetActiveTask(UserTask selectedUserTask)
        {
            PomodoroControlService.StopPomodoro();
            ActiveTask = selectedUserTask;
            if (ActiveTask.PomodoroSettings != null)
            {
                PomodoroSettings = ActiveTask.PomodoroSettings;
            }
            else
            {
                PomodoroSettings = AppSettings.PomodoroSettings;
            }
        }

        public void ClearStatistics()
        {
            StorageService.ClearStatistics(DateTime.MinValue, DateTime.Now);
        }

        public Task<bool> SaveSettingsAsync(AppSettings settings)
        {
            return Task.Run(
            () =>
            {
                var setResult = StorageService.SetAppSettings(settings);
                if (setResult)
                {
                    AppSettings = settings;
                    AlarmService.SoundEnable = AppSettings.SoundAlarm;
                    AlarmService.VibrationEnable = AppSettings.VibrationAlarm;

                    if (this.ActiveTask.PomodoroSettings == null)
                    {
                        PomodoroSettings = settings.PomodoroSettings;
                    }
                }
                return setResult;
            }
            );
        }


        public Task<bool> AddNewUserTask(UserTask userTask)
        {
            return Task.Run(
            () =>
            {
                var isAdded = StorageService.AddNewUserTask(userTask);
                if (ActiveTask.Id == userTask.Id)
                {
                    ActiveTask = userTask;
                }
                if (isAdded)
                {
                    UserTasks.RemoveAll(x => x.Id == userTask.Id);
                    UserTasks.Insert(0, userTask);
                    UserTaskModifiedEvent?.Invoke(this, new UserTaskModifiedEventArgs() { UserTask = userTask });
                }
                return isAdded;
            }
            );
        }

        public Task<bool> RemoveUserTask(UserTask userTask)
        {
            return Task.Run(
                () =>
                    {
                        var isDeleted = StorageService.RemoveUserTask(userTask);
                        if (isDeleted)
                        {
                            this.UserTasks.Remove(userTask);
                            UserTaskRemovedEvent?.Invoke(this, new UserTaskModifiedEventArgs() { UserTask = userTask });
                        }
                        return isDeleted;
                    }
                );
        }

        public List<TaskStatistic> GetStatisticData(DateTime startTime, DateTime finishTime)
        {
            return StorageService.GetStatisticData(startTime, finishTime);
        }
        public PomdoroStatus PausePomodoro()
        {
            PomodoroControlService.PausePomodoro();
            return PomodoroControlService.PomodoroStatus;
        }
        public PomdoroStatus StartPomodoro()
        {
            PomodoroControlService.StartPomodoro();
            return PomodoroControlService.PomodoroStatus;
        }
        public PomdoroStatus StopPomodoro()
        {
            PomodoroControlService.StopPomodoro();
            return PomodoroControlService.PomodoroStatus;
        }

        public void Export(DateTime startTime, DateTime finishTime)
        {
            throw new NotImplementedException();
        }


        public void EnableNotification()
        {
            IsNotificationEnable = true;
        }
        public void DisableNotification()
        {
            IsNotificationEnable = false;
            NotificationService?.Cancel();
        }

        //private void OnTimerTick(object sender, PomodoroTimerTickEventArgs eventArgs)
        //{
        //    NotificationService?.SetTimerInfo(eventArgs.TimerInfo);
        //    TimerTickEvent?.Invoke(
        //    this,
        //    eventArgs
        //);
        //}

        private void OnTimerFinished(object sender, PomodoroChangedEventArgs eventArgs)
        {
            if (IsNotificationEnable)
                NotificationService.SetFinisedInfo(eventArgs.ComplatedState);

            if (eventArgs.ComplatedState == PomodoroState.Pomodoro)
            {
                OnPomodoroFinished();
            }
            else
            {
                OnBreakFinished();
            }

            TimerFinishedEvent?.Invoke(
            this,
            eventArgs
            );

        }
        private void OnBreakFinished()
        {
            AlarmService.RunAlarm();
        }
        private void OnPomodoroFinished()
        {
            if (ActiveTask.TaskStatistic == null)
                ActiveTask.TaskStatistic = new FinishedTaskStatistic();

            ActiveTask.TaskStatistic.Add(1);

            AlarmService.RunAlarm();

            CurrentSession.FinishedTaskInfo.Add(
                new TaskStatistic()
                {
                    TaskId = ActiveTask.Id,
                    FinishedTime = DateTime.Now,
                    Duration = PomodoroSettings.PomodoroDuration,
                    TaskName = ActiveTask.TaskName
                });

            StorageService.UpdateSessionInfo(CurrentSession);

            if (ActiveTask.Id != AppConstants.FREE_RUN_TASK.Id)
                StorageService.UpdateUserTask(ActiveTask);

            UserTaskModifiedEvent?.Invoke(this, new UserTaskModifiedEventArgs() { UserTask = ActiveTask });
        }
        public void OnSleep()
        {
            StorageService.SaveAppState(AppState);
        }


        public void OnResume()
        {
            AppState = PomodoroControlService.PomodoroStatus;
            AppResumedEvent?.Invoke(this, new AppResumedEventArgs() { AppState = AppState });
        }

        public void OnDestroy()
        {
            StopPomodoro();
        }

        public void SetTimerInfo(PomdoroStatus timerInfo)
        {
            if (IsNotificationEnable)
                NotificationService.SetTimerInfo(timerInfo);
        }
    }
}