using System;
using System.Collections.Generic;
using System.Text;
using PomodoroTimer.Enums;
using PomodoroTimer.Models;

namespace PomodoroTimer.Services
{
    public class PomodoroChangedEventArgs : EventArgs
    {
        public PomodoroState ComplatedState { get; set; }
        public bool IsCanceled { get; set; } = false;

        public PomodoroChangedEventArgs(PomodoroState complatedState)
        {
            ComplatedState = complatedState;
        }
    }

    public class PomodoroTimerStateChangedEventArgs : EventArgs
    {
        public PomodoroTimerState NewState { get; set; }

        public PomodoroTimerStateChangedEventArgs(PomodoroTimerState pomodoroStatus)
        {
            this.NewState = pomodoroStatus;
        }
    }

    public delegate void TimerFinishedEventHandler(object sender, PomodoroChangedEventArgs eventArgs);
    public delegate void PomodoroTimerStateChangedEventHandler(object sender, PomodoroTimerStateChangedEventArgs eventArgs);

    public interface IPomodoroControlService
    {
        event TimerFinishedEventHandler TimerFinishedEvent;
        event PomodoroTimerStateChangedEventHandler PomodoroTimerStateChangedEvent;

        PomodoroTimerState PomodoroStatus { get; }
        PomodoroSettings PomodoroSettings { get; set; }
        void StartPomodoro();
        void StopPomodoro();
        void LoadLastState();
        void StartBreak();
        void StartSessionBreak();
        void PausePomodoro();
    }
}
