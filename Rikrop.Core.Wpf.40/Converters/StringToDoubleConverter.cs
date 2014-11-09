using System;
using System.Globalization;
using System.Windows.Data;

namespace Rikrop.Core.Wpf.Converters
{
    public class StringToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is double)
            {
                return ConvertBack(value, targetType, parameter, culture);
            }

            var str = value as string;
            if(str == null)
            {
                return value;
            }
            double val;
            if(!double.TryParse(str, out val))
            {
                return value;
            }
            return val;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null)
            {
                return null;
            }
            if(value is string)
            {
                return Convert(value, targetType, parameter, culture);
            }
            return value.ToString();
        }
    }
}