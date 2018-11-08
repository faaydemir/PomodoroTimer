
using Plugin.LocalNotifications;
using PomodoroTimer.Enums;
using PomodoroTimer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PomodoroTimer.Services
{
    public class NotificationService : INotificationService
    {
        private string FinishedText = "Finished";
        private string pomodoroText = "Pomodoro";
        private string pomodoroBreakText = "Break";
        private string sessionBreakText = "Session Break";

        public NotificationService()
        {
            IsEnable = false;
        }
        public bool IsEnable { get; private set; }
        public void EnableNotifications()
        {
            IsEnable = true; ;
        }
        public void DisableNotification()
        {
            IsEnable = false;
            CrossLocalNotifications.Current.Cancel(0);
        }
        public void SetTimerInfo(TimerInfo timerInfo)
        {
            if (IsEnable)
                CrossLocalNotifications.Current.Show(GetStateName(timerInfo.PomodoroState), timerInfo.RemainingTime.ToString());
        }

        public void SetFinisedInfo(PomodoroState complatedState)
        {
            if (IsEnable)
                CrossLocalNotifications.Current.Show(GetStateName(complatedState), FinishedText);
        }

        private string GetStateName(PomodoroState complatedState)
        {
            switch (complatedState)
            {
                case PomodoroState.Pomodoro:
                    return pomodoroText;
                case PomodoroState.PomodoroBreak:
                    return pomodoroBreakText;
                case PomodoroState.SessionBreak:
                    return sessionBreakText;
                default:
                    return "";
            }
        }
    }
}
