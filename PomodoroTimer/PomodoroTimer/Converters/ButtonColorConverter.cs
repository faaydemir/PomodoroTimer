using PomodoroTimer.Enums;
using PomodoroTimer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PomodoroTimer.Converters
{
    public class ButtonColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Color startColor = Color.FromHex("#27CC4C");
            Color pausedColor = Color.FromHex("#27A1A1");
            if (value == null)
                return startColor;
            try
            {
                var state = (TimerInfo)value;
                switch (state.TimerState)
                {

                    case TimerState.Paused:
                    case TimerState.Complated:
                    case TimerState.Stoped:
                        return startColor;

                    case TimerState.Running:
                        return pausedColor;

                    default:
                        return startColor;
                }
            }
            catch
            {
                return startColor;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
