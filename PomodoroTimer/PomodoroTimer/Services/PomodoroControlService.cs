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
        private int FinishedPomodoroCount;
        private IStorageService StorageService;
        private ITimerService TimerService;

        public PomdoroStatus PomodoroStatus { get; private set; }
        public PomodoroSettings PomodoroSettings { get; set; }

        public event TimerFinishedEventHandler TimerFinishedEvent;
        public event TimerTickEventHandler TimerTickEvent;

        public PomodoroControlService(IStorageService storageService)
        {
            FinishedPomodoroCount = 0;
            TimerService = new TimerService();
            this.StorageService = storageService;
            TimerService.TimerComplatedEvent += OnTimerCompleted;

            var lastState = this.StorageService.ReadLastState();
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
            if (PomodoroStatus.PomodoroState == PomodoroState.Pomodoro && PomodoroStatus.TimerState == TimerState.Paused) // if paused
            {
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
            PomodoroStatus.TimerState = TimerState.Paused;

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
            PomodoroStatus.PomodoroState = Enums.PomodoroState.PomodoroBreak;
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
        }
        public void StartSessionBreak()
        {
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
        }
        public void StopPomodoro()
        {
            TimerService.Stop();

            PomodoroStatus = new PomdoroStatus()
            {
                PomodoroState = PomodoroState.Ready,
                TimerState = TimerState.Stoped,
            };
            StorageService.SaveAppState(PomodoroStatus);
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
        }


        private void OnTimerCompleted(object sender, TimerCompladedEventArgs eventArgs)
        {
            OnFinished();
        }


        private void OnFinished()
        {

            var ComplatedState = PomodoroStatus.PomodoroState;


            switch (PomodoroStatus.PomodoroState)
            {
                case PomodoroState.Pomodoro:
                    FinishedPomodoroCount++;
                    if (FinishedPomodoroCount == PomodoroSettings.SessionPomodoroCount)
                    {
                        PomodoroStatus.PomodoroState = Enums.PomodoroState.SessionBreak;
                        FinishedPomodoroCount = 0;
                        StartSessionBreak();
                    }
                    else
                    {
                        PomodoroStatus.PomodoroState = Enums.PomodoroState.PomodoroBreak;
                        StartBreak();
                    }
                    break;

                case PomodoroState.PomodoroBreak:
                case PomodoroState.SessionBreak:
                    PomodoroStatus.PomodoroState = Enums.PomodoroState.Pomodoro;
                    if (PomodoroSettings.AutoContinue)
                    {
                        StartPomodoro();
                    }
                    else
                    {
                        WaitForStart();
                    }
                    break;
            }

            var NextState = PomodoroStatus.PomodoroState;

            TimerFinishedEvent?.Invoke(
                this,
                new PomodoroChangedEventArgs()
                {
                    PomodoroStatus= PomodoroStatus,
                    ComplatedState = ComplatedState,
                    NextState = NextState,

                }
            );
        }
    }
}



//private void OnTimerTick(object sender, TimerTickEventArgs eventArgs)
//{
//    if (TimerInfo.TimerState == TimerState.Running)
//        OnRunning(eventArgs.RunTime, eventArgs.RemainigTime, eventArgs.TimerState);
//}
//private void OnRunning(TimeSpan runTime, TimeSpan remainigTime, TimerState timerState)
//{

//    TimerInfo.RemainingTime = remainigTime;
//    TimerInfo.RunTime = runTime;
//    TimerInfo.TimerState = timerState;

//    TimerTickEvent?.Invoke(
//        this,
//        new PomodoroTimerTickEventArgs()
//        {
//            TimerInfo = new TimerInfo
//            {
//                PomodoroState = TimerInfo.PomodoroState,
//                RemainingTime = TimerInfo.RemainingTime,
//                RunTime = TimerInfo.RunTime,
//                TimerState = TimerInfo.TimerState
//            },
//        }
//    );
//}
