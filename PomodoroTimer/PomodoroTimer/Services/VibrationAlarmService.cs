using System;
using Xamarin.Essentials;

// #laterusable
namespace PomodoroTimer
{
    public class VibrationAlarmService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="milisecond">duration as milisecond</param>
        public void Vibrate(int milisecond)
        {
            try
            {
                Vibration.Vibrate();
                var vibrationduration = TimeSpan.FromMilliseconds(milisecond);
                Vibration.Vibrate(vibrationduration);
            }
            catch (FeatureNotSupportedException ex)
            {
                // Feature not supported on device
            }
        }
    }
}