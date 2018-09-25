using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PomodoroTimer.Enums;
using PomodoroTimer.Models;

namespace PomodoroTimer.Services
{
    public class PomodoroControlService : IPomodoroControlService
    {
        ITimerService TimerService;

        public event TimerFinishedEventHandler TimerFinishedEvent;
        public event TimerTickEventHandler TimerTickEvent;
        public TimerInfo TimerInfo { get; set; }

        private int FinishedPomodoroCount { get; set; }
        public PomodoroSettings PomodoroSettings { get; set; }
        public PomodoroControlService()
        {
            FinishedPomodoroCount = 0;
            TimerInfo = new TimerInfo();
            TimerService = new TimerController();
            TimerService.TimerTickEvent += OnTimerTick;
        }

        private void OnTimerTick(object sender, TimerTickEventArgs eventArgs)
        {

            switch (eventArgs.TimerState)
            {
                case TimerState.Running:
                case TimerState.Paused:
                    OnRunning(eventArgs.RunTime, eventArgs.RemainigTime, eventArgs.TimerState);
                    break;
                case TimerState.Stoped:
                    OnStopped();
                    break;
                case TimerState.Complated:
                    OnFinished();
                    break;
                default:
                    break;
            }
        }

        private void OnRunning(TimeSpan runTime, TimeSpan remainigTime, TimerState timerState)
        {

            TimerInfo.RemainingTime = remainigTime;
            TimerInfo.RunTime = runTime;
            TimerInfo.TimerState = timerState;

            TimerTickEvent?.Invoke(
                this,
                new PomodoroTimerTickEventArgs()
                {
                    TimerInfo = TimerInfo,
                }
            );
        }

        private void OnFinished()
        {

            var ComplatedState = TimerInfo.PomodoroState;


            switch (TimerInfo.PomodoroState)
            {
                case PomodoroState.Pomodoro:
                    FinishedPomodoroCount++;
                    if (FinishedPomodoroCount >= PomodoroSettings.SessionPomodoroCount)
                    {
                        TimerInfo.PomodoroState = PomodoroState.SessionBreak;
                        FinishedPomodoroCount = 0;
                        StartSessionBreak();
                    }
                    else
                    {
                        TimerInfo.PomodoroState = PomodoroState.PomodoroBreak;
                        StartBreak();
                    }
                    break;
                case PomodoroState.PomodoroBreak:
                case PomodoroState.SessionBreak:
                    TimerInfo.PomodoroState = PomodoroState.Pomodoro;
                    if (PomodoroSettings.AutoContinue)
                    {
                        StartPomodoro();
                    }
                    break;
            }
            var NextState = TimerInfo.PomodoroState;
            TimerFinishedEvent?.Invoke(
                this,
                new PomodoroChangedEventArgs()
                {
                    ComplatedState = ComplatedState,
                    NextState = NextState,

                }
            );
        }

        private void OnStopped()
        {
            TimerFinishedEvent?.Invoke(
            this,
            new PomodoroChangedEventArgs()
            {
                ComplatedState = TimerInfo.PomodoroState,
                NextState = PomodoroState.Ready,

            }
            );

            TimerInfo.PomodoroState = PomodoroState.Ready;
        }

        public void StartBreak()
        {
            TimerInfo.PomodoroState = PomodoroState.PomodoroBreak;
            TimerService.Start(TimeSpan.FromMinutes(PomodoroSettings.PomodoroBreakDuration));
        }

        public void PausePomodoro()
        {
            if (TimerInfo.PomodoroState == PomodoroState.Pomodoro && TimerInfo.TimerState == TimerState.Running)
            {
                TimerService.Pause();
            }
        }
        public void StartPomodoro()
        {
            if (TimerInfo.PomodoroState == PomodoroState.Pomodoro && TimerInfo.TimerState == TimerState.Paused)
            {
                TimerService.Start();
            }
            else
            {
                TimerService.Start(TimeSpan.FromMinutes(PomodoroSettings.PomodoroDuration));
            }
            TimerInfo.PomodoroState = PomodoroState.Pomodoro;
        }

        public void StartSessionBreak()
        {
            TimerInfo.PomodoroState = PomodoroState.SessionBreak;
            TimerService.Start(TimeSpan.FromMinutes(PomodoroSettings.SessionBreakDuration));
        }

        public void StopPomodoro()
        {
            TimerInfo.PomodoroState = PomodoroState.Ready;
            TimerService.Stop();
        }


    }
}
