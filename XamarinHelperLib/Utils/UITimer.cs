using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
// #laterusable
namespace XamarinHelpers.Utils
{

    public class UITimerTickEventArgs : EventArgs
    {
        public TimeSpan RemainningTime { get; set; }
    }

    public class UITimerCompladedEventArgs : EventArgs
    {
    }

    public delegate void UITimerTickedEventHandler(object sender, UITimerTickEventArgs eventArgs);
    public delegate void UITimerComplatedEventHandler(object sender, UITimerCompladedEventArgs eventArgs);


    /// <summary>
    /// Timer for manipulate ui
    /// </summary>
    public class UITimer : IDisposable
    {

        private TimeSpan runTime;
        private TimeSpan frequency;
        private TimeSpan remainningTime;

        public event UITimerTickedEventHandler TimerTickEvent;
        public event UITimerComplatedEventHandler TimerComplatedEvent;

        public bool IsRunning { get; private set; }

        public UITimer()
        {
            IsRunning = false;
        }

        public void Start(TimeSpan runTime, TimeSpan frequency)
        {
            if (IsRunning == true)
                throw new Exception("Timer cant be started more than one");

            IsRunning = true;

            this.remainningTime = runTime;
            this.runTime = runTime;
            this.frequency = frequency;

            DateTime startTime = DateTime.Now;

            Device.StartTimer(frequency, () =>
            {
                remainningTime = runTime.Subtract(DateTime.Now.Subtract(startTime));

                if (remainningTime <= TimeSpan.Zero)
                {
                    remainningTime = TimeSpan.Zero;
                    IsRunning = false;
                    OnComplated();
                }
                OnTick();
                return IsRunning;
            });

        }

        public void Stop()
        {
            IsRunning = false;
        }

        public void Dispose()
        {
            Stop();
        }

        private void OnComplated()
        {
            TimerComplatedEvent?.Invoke(this,new UITimerCompladedEventArgs());
        }

        private void OnTick()
        {
            if (IsRunning)
            {
                TimerTickEvent?.Invoke(this, new UITimerTickEventArgs { RemainningTime = remainningTime });
            }
        }
    }
}
