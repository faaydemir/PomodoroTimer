using PomodoroTimer.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PomodoroTimer
{
    public class TimerTickEventArgs : EventArgs
    {
        public TimeSpan RunTime;
        public TimeSpan RemainigTime { get; set; }
        public TimerState TimerState { get; set; }
    }
    public class TimerCompladedEventArgs : EventArgs
    {
        public TimeSpan RunTime;
    }



    public delegate void TimerTickedEventHandler(object sender, TimerTickEventArgs eventArgs);
    public delegate void TimerComplatedEventHandler(object sender, TimerCompladedEventArgs eventArgs);

    public interface ITimerService
    {
        //event TimerTickedEventHandler TimerTickEvent;
        event TimerComplatedEventHandler TimerComplatedEvent;

        TimeSpan RemainingTime { get; set; }
        TimerState TimerState { get; set; }
        TimeSpan RunningTime { get; set; }
        DateTime StartTime { get; set; }

        void Pause();
        void Continue();
        void Stop();
        void Start(TimeSpan runningTime);
    }
}