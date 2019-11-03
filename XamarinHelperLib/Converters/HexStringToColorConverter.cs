using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamarinHelpers.Converters
{
    public class HexStringToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var stringValue = (string)value;
            if (stringValue == null)
                return Color.Bisque;
            return Color.FromHex(stringValue);

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
