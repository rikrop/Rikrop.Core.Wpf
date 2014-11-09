using System;
using System.Globalization;
using System.Windows.Data;

namespace Rikrop.Core.Wpf.Converters
{
    public class StringToUriConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is Uri)
            {
                return ConvertBack(value, targetType, parameter, culture);
            }
            var str = value as string;
            if(str == null)
            {
                return null;
            }
            return new Uri(str);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            if (value is string)
            {
                return Convert(value, targetType, parameter, culture);
            }
            return value.ToString();
        }
    }
}