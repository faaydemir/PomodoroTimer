using PomodoroTimer.Enums;
using PomodoroTimer.Messaging;
using System;
using Xamarin.Forms;

namespace PomodoroTimer
{
    public class TimerMessage
    {
        public TimerState TimerState { get; set; }
        public TimeSpan RemainingTime { get; set; }
        public TimeSpan RunningTime { get; set; }
    }
    public class TimerControlMessage
    {
        public TimerAction TimerAction { get; set; }
        public TimeSpan RunningTime { get; set; }
    }
    public class TimerController : ITimerService
    {

        public TimerController()
        {
            MessagingCenter.Subscribe<TimerMessage>(this, AppMessages.TimerMessage, message =>
            {
                OnMessageReceived(message);
            });
        }
        public TimerController(TimeSpan runningTime) : this()
        {
            this.RunningTime = runningTime;
        }

        public TimeSpan RemainingTime { get; private set; }
        public TimerState TimerState { get; private set; }
        public TimeSpan RunningTime { get; private set; }
        public event TimerTickedEventHandler TimerTickEvent;

        public void Start()
        {
            TimerState = TimerState.Running;
            SendStatusMessage(TimerAction.Continue);
        }

        public void Start(TimeSpan runningTime)
        {
            RunningTime = runningTime;
            TimerState = TimerState.Running;
            SendStatusMessage(TimerAction.Run);
        }

        public void Stop()
        {
            TimerState = TimerState.Stoped;
            SendStatusMessage(TimerAction.Stop);
        }
        public void Pause()
        {
            TimerState = TimerState.Paused;
            SendStatusMessage(TimerAction.Pause);
        }
        private void SendStatusMessage(TimerAction control)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                MessagingCenter.Send<TimerControlMessage>(new TimerControlMessage() { TimerAction = control, RunningTime = RunningTime }, AppMessages.TimerControlMessage);
            });
        }

        private void OnMessageReceived(TimerMessage message)
        {
            try
            {
                TimerState = message.TimerState;
                RemainingTime = message.RemainingTime;
                TimerTickEvent?.Invoke(this, new TimerTickEventArgs() { RemainigTime = message.RemainingTime, RunTime = RunningTime, TimerState = message.TimerState });
            }
            catch (Exception ex)
            {

            }
        }

    }
}