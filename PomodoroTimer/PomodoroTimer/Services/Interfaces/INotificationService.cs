using PomodoroTimer.Enums;
using PomodoroTimer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PomodoroTimer.Services
{
    public interface INotificationService
    {
        void Cancel();
        void SetTimerInfo(PomodoroTimerState timerInfo);
        void SetFinisedInfo(PomodoroState complatedState);
    }
}
