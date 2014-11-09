using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Rikrop.Core.Wpf.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public BooleanToVisibilityConverter()
        {
            VisibilityOnFalse = Visibility.Collapsed;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var flag = false;
            if (value is bool)
            {
                flag = (bool)value;
            }
            else if (value is bool?)
            {
                var nullable = (bool?)value;
                flag = nullable.Value;
            }
            return InvertConverterHelper.InvertValue(flag, Visibility.Visible, VisibilityOnFalse, parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var convertBack = ((value is Visibility) && (((Visibility)value) == Visibility.Visible));
            return InvertConverterHelper.InvertValue(convertBack, parameter);
        }

        public Visibility VisibilityOnFalse { get; set; }
    }
}
