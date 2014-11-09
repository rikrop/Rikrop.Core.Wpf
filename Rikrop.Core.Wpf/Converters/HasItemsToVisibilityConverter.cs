using System;
using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Rikrop.Core.Wpf.Converters
{
    public class HasItemsToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is IList))
                return InvertConverterHelper.InvertValue(false, Visibility.Visible, Visibility.Collapsed, parameter);

            return InvertConverterHelper.InvertValue(((IList)value).Count > 0, Visibility.Visible, Visibility.Collapsed, parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
