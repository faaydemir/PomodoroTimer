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
        private PersistentObject<PomodoroTimerState> _pomodoroStatusPersistent;
        private int FinishedWitoutSessionBreak;
        private IStorageService StorageService;
        private ITimerService TimerService;
        private ILogger Logger;

        public PomodoroTimerState PomodoroStatus { get; private set; }
        public PomodoroSettings PomodoroSettings { get; set; }

        public event TimerFinishedEventHandler TimerFinishedEvent;
        public event PomodoroTimerStateChangedEventHandler PomodoroTimerStateChangedEvent;

        public PomodoroControlService(IStorageService storageService)
        {
            FinishedWitoutSessionBreak = 0;
            TimerService = new TimerService();
            StorageService = storageService;
            TimerService.TimerComplatedEvent += OnTimerCompleted;
            Logger = new DebugLogger();
            LoadLastState();
        }

        private bool IsPomodoroStatusValid(PomodoroTimerState lastState)
        {
            bool state = true;
            // check if null
            if (lastState == null)
                return false;

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

                if (DateTime.Now.Subtract(lastState.StartTime) > lastState.RemainingTime)
                {
                    state = false;
                }
                // check if timer run incorrect pomodoro state

                if (lastState.PomodoroState == PomodoroState.Ready)
                {
                    state = false;
                }
            }
            LogState("IsPomodoroStatusValid == " + state, lastState);

            return state;

        }

        public void StartPomodoro()
        {
            // check if paused
            if (PomodoroStatus.PomodoroState == PomodoroState.Pomodoro &&
                PomodoroStatus.TimerState == TimerState.Paused)
            {
                // if paused continue pomodoro
                TimerService.Continue();

                PomodoroStatus = new PomodoroTimerState()
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

                PomodoroStatus = new PomodoroTimerState()
                {
                    PomodoroState = PomodoroState.Pomodoro,
                    RemainingTime = PomodoroSettings.PomodoroDuration,
                    RunTime = PomodoroSettings.PomodoroDuration,
                    StartTime = TimerService.StartTime,
                    TimerState = TimerState.Running,
                };
            }


            StorageService.SaveAppState(PomodoroStatus);
            LogState("StartPomodoro");

        }

        public void PausePomodoro()
        {
            TimerService.Pause();

            PomodoroStatus = new PomodoroTimerState()
            {
                PomodoroState = PomodoroState.Pomodoro,
                RemainingTime = TimerService.RemainingTime,
                RunTime = PomodoroSettings.PomodoroDuration,
                StartTime = DateTime.Now,
                TimerState = TimerState.Paused,
            };

            StorageService.SaveAppState(PomodoroStatus);
            LogState("PausePomodoro");
        }



        public async void StartBreak()
        {
            TimerService.Start(PomodoroSettings.PomodoroBreakDuration);

            PomodoroStatus = new PomodoroTimerState()
            {
                PomodoroState = PomodoroState.PomodoroBreak,
                RunTime = PomodoroSettings.PomodoroBreakDuration,
                RemainingTime = PomodoroSettings.PomodoroBreakDuration,
                StartTime = DateTime.Now,
                TimerState = TimerState.Running,
            };

            await StorageService.SaveAppState(PomodoroStatus);
            PomodoroTimerStateChangedEvent?.Invoke(this, new PomodoroTimerStateChangedEventArgs(PomodoroStatus));
            LogState("StartBreak");
        }

        public async void StartSessionBreak()
        {
            FinishedWitoutSessionBreak = 0;
            TimerService.Start(PomodoroSettings.SessionBreakDuration);

            PomodoroStatus = new PomodoroTimerState()
            {
                PomodoroState = PomodoroState.SessionBreak,
                RunTime = PomodoroSettings.SessionBreakDuration,
                RemainingTime = PomodoroSettings.SessionBreakDuration,
                StartTime = DateTime.Now,
                TimerState = TimerState.Running,
            };
            await StorageService.SaveAppState(PomodoroStatus);
            PomodoroTimerStateChangedEvent?.Invoke(this, new PomodoroTimerStateChangedEventArgs(PomodoroStatus));
            LogState("StartSessionBreak");
        }

        public async void StopPomodoro()
        {
            TimerService.Stop();

            PomodoroStatus = new PomodoroTimerState()
            {
                RemainingTime = TimeSpan.Zero,
                RunTime = TimeSpan.Zero,
                StartTime = DateTime.Now,
                PomodoroState = PomodoroState.Ready,
                TimerState = TimerState.Stoped,
            };

            await StorageService.SaveAppState(PomodoroStatus);
            PomodoroTimerStateChangedEvent?.Invoke(this, new PomodoroTimerStateChangedEventArgs(PomodoroStatus));
            LogState("stoped");
        }

        private async void WaitForStart()
        {
            PomodoroStatus = new PomodoroTimerState()
            {
                PomodoroState = PomodoroState.Pomodoro,
                TimerState = TimerState.Stoped,
                RunTime = PomodoroSettings.PomodoroDuration,
                RemainingTime = PomodoroSettings.PomodoroDuration,
                StartTime = DateTime.Now
            };

            await StorageService.SaveAppState(PomodoroStatus);
            PomodoroTimerStateChangedEvent?.Invoke(this, new PomodoroTimerStateChangedEventArgs(PomodoroStatus));
            LogState("waitforstart");
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

                TimerService.StartTime = PomodoroStatus.StartTime;
                TimerService.RunningTime = PomodoroStatus.RunTime;
                TimerService.RemainingTime = PomodoroStatus.RemainingTime;
                TimerService.TimerState = PomodoroStatus.TimerState;
            }
            else
            {
                PomodoroStatus = new PomodoroTimerState();
            }
            LogState("Loaded");
        }

        private void LogState(string message = null, PomodoroTimerState state = null)
        {
            state = state ?? PomodoroStatus;
            message = message ?? message + '\t';

            string pStateString =
                message +
                state.PomodoroState.ToString() + '\t' +
                state.TimerState.ToString() + '\t' +
                state.StartTime.ToString() + '\t' +
                state.RunTime.TotalSeconds.ToString() + '\t' +
                state.RemainingTime.TotalSeconds.ToString() + '\t'
                ;
            Logger.Log(LogLevelEnum.INFO, pStateString);
        }
    }
}


