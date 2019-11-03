using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamarinHelpers.Converters
{
    //TODO naming
    public class NullToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if (value == null)
                    return false;

                if (string.IsNullOrEmpty(value as string))
                    return false;

                return true;
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
