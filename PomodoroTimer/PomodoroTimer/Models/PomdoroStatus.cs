using PomodoroTimer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroTimer.Models
{
    public class PomodoroTimerState
    {
        public DateTime StartTime { get; set; }
        public PomodoroState PomodoroState { get; set; }
        public TimeSpan RunTime { get; set; }
        public TimeSpan RemainingRunTime { get; set; }

        public TimeSpan RemainingTime
        {
            get
            {
                if (TimerState == TimerState.Running)
                {
                    var elapsedFromStart = DateTime.Now.Subtract(StartTime);
                    var remaingTime = RemainingRunTime.Subtract(elapsedFromStart);
                    return remaingTime <= TimeSpan.Zero ? TimeSpan.Zero : remaingTime;
                }
                else
                {
                    return RemainingRunTime;
                }
            }
        }
        public TimerState TimerState { get; set; }

        public PomodoroTimerState()
        {
            PomodoroState = PomodoroState.Ready;
            RunTime = TimeSpan.Zero;
            RemainingRunTime = TimeSpan.Zero;
            TimerState = TimerState.Stoped;
        }

        public static PomodoroTimerState Paused(TimeSpan pomodoroDuration, TimeSpan remainingRunTime)
        {
            var pomodoroStatus = new PomodoroTimerState()
            {
                PomodoroState = PomodoroState.Pomodoro,
                RemainingRunTime = remainingRunTime,
                RunTime = pomodoroDuration,
                StartTime = DateTime.Now,
                TimerState = TimerState.Paused,
            };
            return pomodoroStatus;
        }

        public static PomodoroTimerState Break(TimeSpan pomodoroBreakDuration)
        {
            return new PomodoroTimerState()
            {
                PomodoroState = PomodoroState.PomodoroBreak,
                RunTime = pomodoroBreakDuration,
                RemainingRunTime = pomodoroBreakDuration,
                StartTime = DateTime.Now,
                TimerState = TimerState.Running,
            };
        }

        public static PomodoroTimerState SessionBreak(TimeSpan sessionBreakDuration)
        {
           return new PomodoroTimerState()
           {
               PomodoroState = PomodoroState.SessionBreak,
               RunTime = sessionBreakDuration,
               RemainingRunTime = sessionBreakDuration,
               StartTime = DateTime.Now,
               TimerState = TimerState.Running,
           };
        }

        public static PomodoroTimerState Stopped()
        {
            return new PomodoroTimerState()
            {
                RemainingRunTime = TimeSpan.Zero,
                RunTime = TimeSpan.Zero,
                StartTime = DateTime.Now,
                PomodoroState = PomodoroState.Ready,
                TimerState = TimerState.Stoped,
            };
        }

        public static PomodoroTimerState Ready(TimeSpan pomodotoDuration)
        {
            return new PomodoroTimerState()
            {
                PomodoroState = PomodoroState.Pomodoro,
                TimerState = TimerState.Stoped,
                RunTime = pomodotoDuration,
                RemainingRunTime = pomodotoDuration,
                StartTime = DateTime.Now
            };
        }

        public static PomodoroTimerState Pomodoro(TimeSpan runTime, TimeSpan remainingRunTime)
        {
            return new PomodoroTimerState()
            {
                PomodoroState = PomodoroState.Pomodoro,
                RemainingRunTime = remainingRunTime,
                RunTime = runTime,
                StartTime = DateTime.Now,
                TimerState = TimerState.Running,
            };
        }
    }
}
