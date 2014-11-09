using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Rikrop.Core.Wpf.Converters
{
    public class NumberToVisibiltyConverter : IValueConverter
    {
        public Visibility VisibilityOnFalse { get; set; }

        public NumberToVisibiltyConverter()
        {
            VisibilityOnFalse = Visibility.Hidden;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var count = System.Convert.ToInt32(value);
                return InvertConverterHelper.InvertValue(count > 0, parameter) 
                           ? Visibility.Visible
                           : VisibilityOnFalse;
            }
            catch
            {
                return VisibilityOnFalse;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
