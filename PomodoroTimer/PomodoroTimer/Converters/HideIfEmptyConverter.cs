using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PomodoroTimer.Converters
{
    public class HideIfEmptyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                var stringValue = (string)value;
                if (stringValue == "" || stringValue == null)
                {
                    return false;
                }
                else
                {
                    return true;
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
