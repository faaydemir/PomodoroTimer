using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HelperLib.Log;
using PomodoroTimer.Enums;
using PomodoroTimer.Models;
using XamarinHelperLib.Utils;

namespace PomodoroTimer.Services
{
    public class PomodoroControlService : IPomodoroControlService
    {
        private int FinishedWitoutSessionBreak;
        private readonly IStorageService StorageService;
        private readonly ITimerService TimerService;
        private readonly ILogger Logger;

        public PomodoroTimerState PomodoroStatus { get; private set; }
        public PomodoroSettings PomodoroSettings { get; set; }

        public event TimerFinishedEventHandler TimerFinishedEvent;
        public event PomodoroTimerStateChangedEventHandler PomodoroTimerStateChangedEvent;

        public PomodoroControlService(IStorageService storageService)
        {
            FinishedWitoutSessionBreak = 0;

            TimerService = new TimerService();
            TimerService.TimerComplatedEvent += OnTimerCompleted;
            Logger = new DebugLogger();
            StorageService = storageService;
            LoadLastState();
        }

        private bool IsPomodoroStatusValid(PomodoroTimerState lastState)
        {
            bool state = true;
            // check if null
            if (lastState == null)
                state = false;

            if (lastState.TimerState == TimerState.Running)
            {
                if (lastState.StartTime > DateTime.Now)
                {
                    state = false;
                }
                if (lastState.RunTime == TimeSpan.Zero)
                {
                    state = false;
                }
                if (lastState.RemainingTime <= TimeSpan.Zero)
                {
                    state = false;
                }
                if (lastState.PomodoroState == PomodoroState.Ready)
                {
                    state = false;
                }
            }
            LogState("IsPomodoroStatusValid state== " + state, lastState);

            return state;

        }

        public void StartPomodoro()
        {
            // check if paused
            if (PomodoroStatus.PomodoroState == PomodoroState.Pomodoro &&
                PomodoroStatus.TimerState == TimerState.Paused)
            {
                PomodoroStatus = PomodoroTimerState.Pomodoro(PomodoroStatus.RunTime, PomodoroStatus.RemainingRunTime);
            }
            else
            {
                // start new pomodoro
                PomodoroStatus = PomodoroTimerState.Pomodoro(PomodoroSettings.PomodoroDuration, PomodoroSettings.PomodoroDuration);
            }

            TimerService.Start(PomodoroStatus.RemainingTime);
            StorageService.SaveAppState(PomodoroStatus);
            PomodoroTimerStateChangedEvent?.Invoke(this, new PomodoroTimerStateChangedEventArgs(PomodoroStatus));
            LogState("StartPomodoro");

        }

        public async void PausePomodoro()
        {
            TimerService.Stop();
            PomodoroStatus = PomodoroTimerState.Paused(PomodoroSettings.PomodoroDuration, PomodoroStatus.RemainingTime);
            await StorageService.SaveAppState(PomodoroStatus);
            PomodoroTimerStateChangedEvent?.Invoke(this, new PomodoroTimerStateChangedEventArgs(PomodoroStatus));
            LogState("PausePomodoro");
        }



        public async void StartBreak()
        {
            TimerService.Start(PomodoroSettings.PomodoroBreakDuration);
            PomodoroStatus = PomodoroTimerState.Break(PomodoroSettings.PomodoroBreakDuration);
            await StorageService.SaveAppState(PomodoroStatus);
            PomodoroTimerStateChangedEvent?.Invoke(this, new PomodoroTimerStateChangedEventArgs(PomodoroStatus));
            LogState("StartBreak");
        }

        public async void StartSessionBreak()
        {
            FinishedWitoutSessionBreak = 0;
            TimerService.Start(PomodoroSettings.SessionBreakDuration);

            PomodoroStatus = PomodoroTimerState.SessionBreak(PomodoroSettings.SessionBreakDuration);

            await StorageService.SaveAppState(PomodoroStatus);
            PomodoroTimerStateChangedEvent?.Invoke(this, new PomodoroTimerStateChangedEventArgs(PomodoroStatus));
            LogState("StartSessionBreak");
        }

        public async void StopPomodoro()
        {
            TimerService.Stop();

            PomodoroStatus = PomodoroTimerState.Stopped();

            await StorageService.SaveAppState(PomodoroStatus);
            PomodoroTimerStateChangedEvent?.Invoke(this, new PomodoroTimerStateChangedEventArgs(PomodoroStatus));
            LogState("StopPomodoro");
        }

        private async void WaitForStart()
        {
            PomodoroStatus = PomodoroTimerState.Ready(PomodoroSettings.PomodoroDuration);
            await StorageService.SaveAppState(PomodoroStatus);
            PomodoroTimerStateChangedEvent?.Invoke(this, new PomodoroTimerStateChangedEventArgs(PomodoroStatus));
            LogState("WaitForStart");
        }

        private void OnTimerCompleted(object sender, TimerCompladedEventArgs eventArgs)
        {
            OnFinished();
        }

        private void OnFinished()
        {
            Logger.Log("Finished\n\n");
            PomodoroState complatedState = PomodoroStatus.PomodoroState;

            TimerFinishedEvent?.Invoke(this, new PomodoroChangedEventArgs(complatedState));

            switch (complatedState)
            {
                case PomodoroState.Pomodoro:
                    FinishedWitoutSessionBreak++;
                    if (FinishedWitoutSessionBreak == PomodoroSettings.SessionPomodoroCount)
                    {
                        StartSessionBreak();
                    }
                    else
                    {
                        StartBreak();
                    }
                    break;
                case PomodoroState.PomodoroBreak:
                case PomodoroState.SessionBreak:
                    if (PomodoroSettings.AutoContinue)
                    {
                        StartPomodoro();
                    }
                    else
                    {
                        WaitForStart();
                    }
                    break;
                default:
                    WaitForStart();
                    break;
            }
        }

        public void LoadLastState()
        {
            var lastState = StorageService.GetLastState();
            // check if last state is valid
            if (IsPomodoroStatusValid(lastState))
            {
                PomodoroStatus = lastState;
            }
            else
            {
                PomodoroStatus = new PomodoroTimerState();
            }
            LogState("Loaded");
        }

        private void LogState(string message = null, PomodoroTimerState state = null)
        {
#if DEBUG
            state = state ?? PomodoroStatus;
            message = message ?? message + '\t';

            string pStateString =
                message +
                state.PomodoroState.ToString() + '\t' +
                state.TimerState.ToString() + '\t' +
                state.StartTime.ToString() + '\t' +
                state.RunTime.TotalSeconds.ToString() + '\t' +
                state.RemainingRunTime.TotalSeconds.ToString() + '\t' +
                state.RemainingTime.TotalSeconds.ToString() + '\t'
                ;
            Logger.Log(LogLevelEnum.INFO, pStateString);
#endif

        }
    }
}


