using System;
using System.Globalization;
using System.Windows.Data;

namespace Rikrop.Core.Wpf.Converters
{
    public class NumericConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null
                       ? ""
                       : string.Format("{0:0.##}", value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}