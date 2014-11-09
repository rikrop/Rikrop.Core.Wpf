using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Rikrop.Core.Wpf.Converters
{
    public class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is string ?
                InvertConverterHelper.InvertValue(!string.IsNullOrEmpty((string)value), Visibility.Visible, Visibility.Collapsed, parameter) :
                InvertConverterHelper.InvertValue(value != null, Visibility.Visible, Visibility.Collapsed, parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
