using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Rikrop.Core.Wpf.Converters
{
    public class BoolToEnabledMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var result = values.Any(value => (bool) value);
            return InvertConverterHelper.InvertValue(result, parameter);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
