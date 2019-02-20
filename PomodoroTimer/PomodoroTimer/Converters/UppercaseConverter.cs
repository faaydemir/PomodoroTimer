using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
// #laterusable
namespace PomodoroTimer.Converters
{
    public class UppercaseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var stringValue = value as string;

            if (value == null)
                return null;
            else
                return stringValue.ToUpper();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
