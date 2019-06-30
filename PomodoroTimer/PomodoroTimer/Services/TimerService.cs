using PomodoroTimer.Enums;
using PomodoroTimer.Messaging;
using PomodoroTimer.Services;
using System;
using Xamarin.Forms;

namespace PomodoroTimer
{
    public class TimerService : ITimerService
    {
        //TODO delete alarm service from timerservice
        private IDeviceAlarmService DeviceAlarmService;

        public TimeSpan RemainingTime { get;  set; }
        public TimerState TimerState { get;  set; } = TimerState.Stoped;
        public TimeSpan RunningTime { get;  set; }
        public DateTime StartTime { get;  set; }

        public event TimerComplatedEventHandler TimerComplatedEvent;

        public TimerService()
        {
            try
            {
                DeviceAlarmService = DependencyService.Get<IDeviceAlarmService>();
                DeviceAlarmService.AlarmEvent += OnAlarmEvent;
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
            DeviceAlarmService.SetAlarm(RemainingTime);
        }

        public void Start(TimeSpan runningTime)
        {
            DeviceAlarmService.CancelAlarm();
            StartTime = DateTime.Now;

            if (runningTime != null && runningTime > TimeSpan.Zero)
            {
                RunningTime = runningTime;
                RemainingTime = runningTime;
            }
            TimerState = TimerState.Running;

            DeviceAlarmService.SetAlarm(runningTime);
        }

        public void Stop()
        {
            DeviceAlarmService.CancelAlarm();
            TimerState = TimerState.Stoped;
            RemainingTime = TimeSpan.Zero;
        }

        public void Pause()
        {
            RemainingTime = RemainingTime - (DateTime.Now - StartTime);
            TimerState = TimerState.Paused;

            DeviceAlarmService.CancelAlarm();
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
