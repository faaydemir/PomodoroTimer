using PomodoroTimer.Enums;
using PomodoroTimer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PomodoroTimer.Converters
{
    public class StateNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var pomodoro = "Pomodoro";
            var pBreak = "Break";
            var sBreak = "Session Break";
            var ready = "Ready";

            if (value == null)
                return ready;
            try
            {
                var state = (PomdoroStatus)value;
                switch (state.PomodoroState)
                {

                    case PomodoroState.Pomodoro:
                        return pomodoro;
                    case PomodoroState.PomodoroBreak:
                        return pBreak;

                    case PomodoroState.SessionBreak:
                        return sBreak;

                    case PomodoroState.Ready:
                        return ready;

                    default:
                        return ready;
                }
            }
            catch
            {
                return ready;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
