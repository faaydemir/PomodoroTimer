using System;
using Xamarin.Essentials;

namespace PomodoroTimer
{
    public class VibrationAlarmService
    {
        public void Vibrate(int duration)
        {
            try
            {
                Vibration.Vibrate();
                var vibrationduration = TimeSpan.FromSeconds(duration);
                Vibration.Vibrate(vibrationduration);
            }
            catch (FeatureNotSupportedException ex)
            {
                // Feature not supported on device
            }
        }
    }
}