using PomodoroTimer.Enums;
using PomodoroTimer.Messaging;
using PomodoroTimer.Services;
using System;
using Xamarin.Forms;

namespace PomodoroTimer
{
    public class TimerService : ITimerService
    {

        private IAlarmService AlarmService;
        public TimeSpan RemainingTime { get; set; }
        public TimerState TimerState { get; set; } = TimerState.Stoped;
        public TimeSpan RunningTime { get; set; }

        public event TimerComplatedEventHandler TimerComplatedEvent;
        public DateTime StartTime { get; set; }


        public TimerService()
        {
            try
            {
                AlarmService = DependencyService.Get<IAlarmService>();
                AlarmService.AlarmEvent += OnAlarmEvent;
            }
            catch (Exception ex)
            {

            }
        }

        private void OnAlarmEvent(object sender, AlarmEventArgs args)
        {
            RemainingTime = TimeSpan.Zero;
            TimerState = TimerState.Complated;
            RiseTimerComplatedEvent();
        }

        public void Continue()
        {
            StartTime = DateTime.Now;
            TimerState = TimerState.Running;
            AlarmService.SetAlarm(RemainingTime);

        }


        public void Start(TimeSpan runningTime)
        {
            AlarmService.CancelAlarm();
            StartTime = DateTime.Now;

            if (runningTime != null && runningTime > TimeSpan.Zero)
            {
                RunningTime = runningTime;
                RemainingTime = RunningTime;
            }
            TimerState = TimerState.Running;

            AlarmService.SetAlarm(runningTime);
        }

        public void Stop()
        {
            AlarmService.CancelAlarm();
            TimerState = TimerState.Stoped;
            RemainingTime = TimeSpan.Zero;
        }

        public void Pause()
        {
            RemainingTime = RemainingTime - (DateTime.Now - StartTime);
            TimerState = TimerState.Paused;

            AlarmService.CancelAlarm();
        }

        //private void RiseTimerEvent()
        //{
        //    if (TimerState == TimerState.Running)
        //        TimerTickEvent?.Invoke(this, new TimerTickEventArgs() { RemainigTime = RemainingTime, RunTime = RunningTime, TimerState = TimerState });
        //}


        private void RiseTimerComplatedEvent()
        {
            TimerComplatedEvent?.Invoke(this, new TimerCompladedEventArgs() { RunTime = RunningTime });

        }

    }
}
