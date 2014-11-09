using System;
using System.Globalization;
using System.Windows.Data;

namespace Rikrop.Core.Wpf.Converters
{
    public class StringIsNullOrEmptyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return InvertConverterHelper.InvertValue(string.IsNullOrEmpty(value as string), parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}