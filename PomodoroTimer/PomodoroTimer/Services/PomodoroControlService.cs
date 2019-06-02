using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PomodoroTimer.Enums;
using PomodoroTimer.Models;
using XamarinHelperLib.Utils;

namespace PomodoroTimer.Services
{
    public class PomodoroControlService : IPomodoroControlService
    {
        private PersistentObject<PomdoroStatus> _pomodoroStatusPersistent;
        private int FinishedWitoutSessionBreak;
        private IStorageService StorageService;
        private ITimerService TimerService;

        public PomdoroStatus PomodoroStatus { get; private set; }
        public PomodoroSettings PomodoroSettings { get; set; }

        public event TimerFinishedEventHandler TimerFinishedEvent;
        public event PomodoroTimerStatusChangedEventHandler PomodoroTimerStatusChangedEvent;

        public PomodoroControlService(IStorageService storageService)
        {
            FinishedWitoutSessionBreak = 0;
            TimerService = new TimerService();
            StorageService = storageService;
            TimerService.TimerComplatedEvent += OnTimerCompleted;

            LoadLastState();
        }

        private bool IsPomodoroStatusValid(PomdoroStatus lastState)
        {
            // check if null
            if (lastState == null)
                return false;

            if (lastState.TimerState == TimerState.Running)
            {
                // check if timer expired

                if (lastState.RunTime == null ||
                    lastState.RemainingTime == null ||
                    lastState.StartTime == null ||
                    lastState.RunTime == TimeSpan.Zero)
                    return false;

                if (DateTime.Now.Subtract(lastState.StartTime) > lastState.RemainingTime)
                    return false;

                // check if timer run incorrect pomodoro state

                if (lastState.PomodoroState == PomodoroState.Ready)
                    return false;
            }

            return true;

        }

        public void StartPomodoro()
        {
            // check if paused
            if (PomodoroStatus.PomodoroState == PomodoroState.Pomodoro &&
                PomodoroStatus.TimerState == TimerState.Paused)
            {
                // if paused continue pomodoro
                TimerService.Continue();

                PomodoroStatus = new PomdoroStatus()
                {
                    PomodoroState = PomodoroState.Pomodoro,
                    RemainingTime = TimerService.RemainingTime,
                    RunTime = PomodoroSettings.PomodoroDuration,
                    StartTime = TimerService.StartTime,
                    TimerState = TimerState.Running,
                };
            }
            else
            {
                // start new pomodoro
                TimerService.Start(PomodoroSettings.PomodoroDuration);

                PomodoroStatus = new PomdoroStatus()
                {
                    PomodoroState = PomodoroState.Pomodoro,
                    RemainingTime = PomodoroSettings.PomodoroDuration,
                    RunTime = PomodoroSettings.PomodoroDuration,
                    StartTime = TimerService.StartTime,
                    TimerState = TimerState.Running,
                };
            }


            StorageService.SaveAppState(PomodoroStatus);

        }

        public void PausePomodoro()
        {
            TimerService.Pause();

            PomodoroStatus = new PomdoroStatus()
            {
                PomodoroState = PomodoroState.Pomodoro,
                RemainingTime = TimerService.RemainingTime,
                RunTime = PomodoroSettings.PomodoroDuration,
                StartTime = DateTime.Now,
                TimerState = TimerState.Paused,
            };

            StorageService.SaveAppState(PomodoroStatus);
        }

        public void StartBreak()
        {
            PomodoroStatus.PomodoroState = PomodoroState.PomodoroBreak;
            TimerService.Start(PomodoroSettings.PomodoroBreakDuration);

            PomodoroStatus = new PomdoroStatus()
            {
                PomodoroState = PomodoroState.PomodoroBreak,
                RunTime = PomodoroSettings.PomodoroBreakDuration,
                RemainingTime = PomodoroSettings.PomodoroBreakDuration,
                StartTime = DateTime.Now,
                TimerState = TimerState.Running,

            };

            StorageService.SaveAppState(PomodoroStatus);
            PomodoroTimerStatusChangedEvent?.Invoke(this, new PomodoroTimerStatusChangedEventArgs(PomodoroStatus));
        }

        public void StartSessionBreak()
        {
            FinishedWitoutSessionBreak = 0;
            TimerService.Start(PomodoroSettings.SessionBreakDuration);

            PomodoroStatus = new PomdoroStatus()
            {
                PomodoroState = PomodoroState.SessionBreak,
                RunTime = PomodoroSettings.SessionBreakDuration,
                RemainingTime = PomodoroSettings.SessionBreakDuration,
                StartTime = DateTime.Now,
                TimerState = TimerState.Running,
            };
            StorageService.SaveAppState(PomodoroStatus);
            PomodoroTimerStatusChangedEvent?.Invoke(this, new PomodoroTimerStatusChangedEventArgs(PomodoroStatus));
        }

        public void StopPomodoro()
        {
            TimerService.Stop();

            PomodoroStatus = new PomdoroStatus()
            {
                RemainingTime = TimeSpan.Zero,
                RunTime = TimeSpan.Zero,
                StartTime = DateTime.Now,
                PomodoroState = PomodoroState.Ready,
                TimerState = TimerState.Stoped,
            };

            StorageService.SaveAppState(PomodoroStatus);
            PomodoroTimerStatusChangedEvent?.Invoke(this, new PomodoroTimerStatusChangedEventArgs(PomodoroStatus));
        }

        private void WaitForStart()
        {
            PomodoroStatus = new PomdoroStatus()
            {
                PomodoroState = PomodoroState.Pomodoro,
                TimerState = TimerState.Stoped,
                RunTime = PomodoroSettings.PomodoroDuration,
                RemainingTime = PomodoroSettings.PomodoroDuration,
                StartTime = DateTime.Now
            };

            StorageService.SaveAppState(PomodoroStatus);
            PomodoroTimerStatusChangedEvent?.Invoke(this, new PomodoroTimerStatusChangedEventArgs(PomodoroStatus));
        }

        private void OnTimerCompleted(object sender, TimerCompladedEventArgs eventArgs)
        {
            OnFinished();
        }

        private void OnFinished()
        {
            PomodoroState complatedState = PomodoroStatus.PomodoroState;

            TimerFinishedEvent?.Invoke(this, new PomodoroChangedEventArgs(complatedState));

            if (complatedState == PomodoroState.Pomodoro)
            {
                FinishedWitoutSessionBreak++;
                if (FinishedWitoutSessionBreak == PomodoroSettings.SessionPomodoroCount)
                    StartSessionBreak();
                else
                    StartBreak();
            }
            else if (complatedState == PomodoroState.PomodoroBreak || complatedState == PomodoroState.SessionBreak)
            {
                if (PomodoroSettings.AutoContinue)
                {
                    StartPomodoro();
                }
            }
            else
                WaitForStart();

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
                PomodoroStatus = new PomdoroStatus
                {
                    PomodoroState = PomodoroState.Ready,
                    TimerState = TimerState.Stoped,
                };
            }

            if (PomodoroStatus != null)
            {
                TimerService.StartTime = PomodoroStatus.StartTime;
                TimerService.RunningTime = PomodoroStatus.RunTime;
                TimerService.RemainingTime = PomodoroStatus.RemainingTime;
                TimerService.TimerState = PomodoroStatus.TimerState;
            }
        }
    }
}


