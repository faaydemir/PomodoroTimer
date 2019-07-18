using PomodoroTimer.Enums;
using PomodoroTimer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PomodoroTimer.Converters
{
    public class ButtonTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var startText = "Start";
            var pauseText = "Pause";


            if (value == null)
                return startText;
   
            try
            {
                var state = (PomodoroTimerState)value;
                switch (state.TimerState)
                {
                    case TimerState.Paused:
                    case TimerState.Complated:
                    case TimerState.Stoped:
                        return startText;

                    case TimerState.Running:
                        {
                            if (state.PomodoroState == PomodoroState.Pomodoro)
                            {
                                return pauseText;
                            }
                            else
                            {
                                return startText;
                            }
                        }
                    default:
                        return startText;
                }
            }
            catch
            {
                return startText;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return "";
        }
    }
}
