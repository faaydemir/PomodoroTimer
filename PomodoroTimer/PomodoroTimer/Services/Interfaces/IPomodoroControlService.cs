using System;
using System.Collections.Generic;
using System.Text;
using PomodoroTimer.Models;

namespace PomodoroTimer.Services
{


    public interface IPomodoroControlService
    {
        event TimerFinishedEventHandler TimerFinishedEvent;
        event TimerTickEventHandler TimerTickEvent;

        TimerInfo TimerInfo { get; }
        PomodoroSettings PomodoroSettings { get; set; }

        void StartPomodoro();
        void StopPomodoro();
        void StartBreak();
        void StartSessionBreak();
        void PausePomodoro();
    }
}
