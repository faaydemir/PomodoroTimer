
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
        private readonly string FinishedText = "Finished";
        private readonly string PomodoroText = "Pomodoro";
        private readonly string PomodoroBreakText = "Break";
        private readonly string SessionBreakText = "Session Break";

        public NotificationService()
        {

        }
        public void Cancel()
        {
            CrossLocalNotifications.Current.Cancel(1);
        }
        public void SetTimerInfo(PomodoroTimerState timerInfo)
        {
            CrossLocalNotifications.Current.Show(GetStateName(timerInfo.PomodoroState), TimeSpanFormatter(timerInfo.RemainingTime), 1);
        }

        public void SetFinisedInfo(PomodoroState complatedState)
        {
            CrossLocalNotifications.Current.Show(GetStateName(complatedState), FinishedText, 1);
        }
        private string TimeSpanFormatter(TimeSpan timeSpan)
        {
            string value = "";
            if (timeSpan.Hours != 0)
                value += timeSpan.Hours;

            if (timeSpan.Minutes < 10)
                value += "0";

            value += timeSpan.Minutes + ":";

            if (timeSpan.Seconds < 10)
                value += "0";

            value += timeSpan.Seconds;
            return value;
        }
        private string GetStateName(PomodoroState complatedState)
        {
            switch (complatedState)
            {
                case PomodoroState.Pomodoro:
                    return PomodoroText;
                case PomodoroState.PomodoroBreak:
                    return PomodoroBreakText;
                case PomodoroState.SessionBreak:
                    return SessionBreakText;
                default:
                    return "";
            }
        }
    }
}
