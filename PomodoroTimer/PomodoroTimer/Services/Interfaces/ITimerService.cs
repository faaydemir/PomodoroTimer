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

    public delegate void TimerTickedEventHandler(object sender, TimerTickEventArgs eventArgs);

    public interface ITimerService
    {
        event TimerTickedEventHandler TimerTickEvent;

        TimeSpan RemainingTime { get; }
        TimerState TimerState { get; }
        TimeSpan RunningTime { get; }

        void Pause();
        void Start();
        void Stop();
        void Start(TimeSpan runningTime);
    }
}