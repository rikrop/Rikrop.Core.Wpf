using System;
using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Rikrop.Core.Wpf.Converters
{
    public class CommaVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2)
            {
                return Visibility.Collapsed;
            }
            var items = (IList)values[1];
            return items.IndexOf(values[0]) == items.Count - 1
                       ? Visibility.Collapsed
                       : Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
