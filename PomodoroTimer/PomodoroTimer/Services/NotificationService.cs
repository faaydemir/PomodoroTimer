
using Plugin.LocalNotifications;
using PomodoroTimer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PomodoroTimer.Services
{
    public class NotificationService : INotificationService
    {

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
            CrossLocalNotifications.Current.Cancel(101);
        }
        public void SetTimerInfo(TimerInfo timerInfo)
        {
            if (IsEnable)
                CrossLocalNotifications.Current.Show(nameof(timerInfo.PomodoroState), timerInfo.RemainingTime.ToString());
        }


    }
}
