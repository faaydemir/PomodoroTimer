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

    public class PomodoroTimerStatusChangedEventArgs : EventArgs
    {
        public PomdoroStatus NewStatus { get; set; }

        public PomodoroTimerStatusChangedEventArgs(PomdoroStatus pomodoroStatus)
        {
            this.NewStatus = pomodoroStatus;
        }
    }

    public delegate void TimerFinishedEventHandler(object sender, PomodoroChangedEventArgs eventArgs);
    public delegate void PomodoroTimerStatusChangedEventHandler(object sender, PomodoroTimerStatusChangedEventArgs eventArgs);

    public interface IPomodoroControlService
    {
        event TimerFinishedEventHandler TimerFinishedEvent;
        event PomodoroTimerStatusChangedEventHandler PomodoroTimerStatusChangedEvent;

        PomdoroStatus PomodoroStatus { get; }
        PomodoroSettings PomodoroSettings { get; set; }
        void StartPomodoro();
        void StopPomodoro();
        void LoadLastState();
        void StartBreak();
        void StartSessionBreak();
        void PausePomodoro();
    }
}
