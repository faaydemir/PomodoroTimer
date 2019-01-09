using System;
using System.Collections.Generic;
using System.Text;

namespace PomodoroTimer.Services
{
    // will be implemented
    public class AlarmEventArgs : EventArgs
    {
    }
    public delegate void AlarmEventHandler(object sender, AlarmEventArgs args);


    public interface IAlarmService
    {
        event AlarmEventHandler AlarmEvent;
        void SetAlarm(TimeSpan duration);
        void CancelAlarm();
    }
}
