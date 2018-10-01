using PomodoroTimer.Enums;
using PomodoroTimer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PomodoroTimer.Services
{
    public interface INotificationService
    {
         void DisableNotification();
         void EnableNotifications();
         void SetTimerInfo(TimerInfo timerInfo);
        void SetFinisedInfo(PomodoroState complatedState);
    }
}
