using PomodoroTimer.Enums;
using PomodoroTimer.Models;
using PomodoroTimer.Services;
using PomodoroTimer.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PomodoroTimer
{


    public class UserTaskModifiedEventArgs : EventArgs
    {
        public UserTask UserTask { get; set; }
    }
    public class AppResumedEventArgs : EventArgs
    {
        public PomodoroTimerState AppState { get; set; }
    }

    public delegate void UserTaskModifiedEventHandler(object sender, UserTaskModifiedEventArgs eventArgs);
    public delegate void AppResumedEventHandler(object sender, AppResumedEventArgs eventArgs);

    /// <summary>
    /// AppService to handle  viewmodel independent or common in viewmodels operatiton 
    /// </summary>
    public interface IAppService
    {
        IPomodoroControlService PomodoroControlService { get; set; }

        IStorageService StorageService { get; set; }
        AplicationUser User { get; set; }
        UserTask ActiveTask { get; set; }
        List<UserTask> UserTasks { get; set; }
        AppSettings AppSettings { get; set; }
        PomodoroSettings PomodoroSettings { get; set; }
        PomodoroSession CurrentSession { get; set; }
        PomodoroTimerState PomodoroStatus { get; }

        void ChangeAppThema(ApplicationThema AplicationThema);

        event TimerFinishedEventHandler TimerFinishedEvent;
        event PomodoroTimerStateChangedEventHandler PomodoroTimerStateChangedEvent;

        void LogEvent(Exception e);

        event UserTaskModifiedEventHandler UserTaskModifiedEvent;
        event UserTaskModifiedEventHandler UserTaskRemovedEvent;
        event AppResumedEventHandler AppResumedEvent;

        PomodoroTimerState StartPomodoro();
        PomodoroTimerState PausePomodoro();
        PomodoroTimerState StopPomodoro();

        void DisableNotification();
        void EnableNotification();

        void SetActiveTask(UserTask selectedUserTask);

        void ClearStatistics();

        Task<bool> AddNewUserTask(UserTask userTask);
        Task<bool> RemoveUserTask(UserTask userTask);
        Task<bool> SaveSettingsAsync(AppSettings settings);

        //List<TaskStatistic> GetStatisticData(DateTime startTime, DateTime finishTime);
        void LoadTheme();
        void Export(DateTime startTime, DateTime finishTime);
        void OnResume();
        void OnSleep();
        void OnDestroy();
        void SetNotification(PomodoroTimerState timerInfo);
    }
}