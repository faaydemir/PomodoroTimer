using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PomodoroTimer.Enums;
using PomodoroTimer.Messaging;
using PomodoroTimer.Services;
using Xamarin.Forms;

namespace PomodoroTimer.Droid.Services
{
    public class AndroidTimer

    {
        public TimeSpan UpdateFrequency { get; set; }
        public TimeSpan RemainingTime { get; set; }
        public TimerState TimerState { get; set; } = TimerState.Stoped;
        public TimeSpan RunningTime { get; set; }

        public AndroidTimer()
        {
            UpdateFrequency = new TimeSpan(0, 0, 0, 1);
        }

        public void Pause()
        {
            TimerState = TimerState.Paused;
            SendTimerStatus();
        }

        public void Start()
        {
            TimerState = TimerState.Running;
            SendTimerStatus();
        }

        public void Start(TimeSpan runningTime)
        {
            if (runningTime != null && runningTime > TimeSpan.Zero)
            {
                RunningTime = runningTime;
                RemainingTime = RunningTime;
            }
            Start();
        }

        public void Stop()
        {
            TimerState = TimerState.Stoped;
            RemainingTime = TimeSpan.Zero;
            SendTimerStatus();
        }
        public async Task Run(CancellationToken token)
        {
            await Task.Run(async () =>
            {
                while (true)
                {
                    try
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(UpdateFrequency);


                        if (TimerState == TimerState.Running)
                        {
                            RemainingTime = RemainingTime.Subtract(UpdateFrequency);
                            if (RemainingTime <= TimeSpan.Zero)
                            {
                                RemainingTime = TimeSpan.Zero;
                                TimerState = TimerState.Complated;
                            }
                            SendTimerStatus();
                        }
                        else if (TimerState == TimerState.Stoped)
                        {
                            ;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessagingCenter.Send<TimerMessage>(new TimerMessage(), AppMessages.TimerMessage);
                        break;
                    }
                }
            });
        }

        private void SendTimerStatus()
        {

            var message = new TimerMessage
            {
                RemainingTime = RemainingTime,
                TimerState = TimerState,
                RunningTime = RunningTime,
            };
            MessagingCenter.Send<TimerMessage>(message, AppMessages.TimerMessage);
        }
    }
}