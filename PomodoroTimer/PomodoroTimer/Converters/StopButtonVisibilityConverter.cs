using PomodoroTimer.Enums;
using PomodoroTimer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PomodoroTimer.Converters
{
    public class StopButtonVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            try
            {
                var state = (PomodoroTimerState)value;
                if (state.PomodoroState == PomodoroState.Pomodoro && state.TimerState == TimerState.Running)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch
            {
                return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
