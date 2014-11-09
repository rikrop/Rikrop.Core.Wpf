using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Rikrop.Core.Wpf.Converters
{
    public sealed class BoolToVisibilityConverter : IValueConverter
    {
        public bool Inverse { get; set; }
        public bool Hide { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Inverse)
            {
                return (bool) value
                           ? GetNonvisibility()
                           : Visibility.Visible;
            }
            return (bool) value
                       ? Visibility.Visible
                       : GetNonvisibility();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        private Visibility GetNonvisibility()
        {
            return Hide
                       ? Visibility.Hidden
                       : Visibility.Collapsed;
        }
    }
}