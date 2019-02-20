using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
// #laterusable
namespace PomodoroTimer.Services
{
    /// <summary>
    /// Timer for manipulate ui
    /// </summary>

    public class UITimer : IDisposable
    {

        private TimeSpan runTime;
        private TimeSpan frequency;
        private TimeSpan remainningTime;
        private readonly Action onComplated;
        private readonly Action<TimeSpan> onTick;

        public bool IsRunning { get; private set; }

        //TODO change callback metods to event
        public UITimer(Action<TimeSpan> onTickCallback, Action onComplatedCallbak)
        {
            onTick = onTickCallback;
            onComplated = onComplatedCallbak;
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

        private void OnComplated()
        {
            onComplated?.Invoke();
        }

        private void OnTick()
        {
            if (IsRunning)
                onTick?.Invoke(this.remainningTime);
        }

        public void Dispose()
        {
            Stop();
        }

    }
}
