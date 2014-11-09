using System;
using System.Globalization;
using System.Windows.Data;

namespace Rikrop.Core.Wpf.Converters
{
    public class AddValueConverterConverter : IValueConverter
    {
        public object DefaultParameter { get; set; }

        public AddValueConverterConverter()
        {
            DefaultParameter = 0;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            double dval;
            dynamic val = value is string && Double.TryParse((string)value, NumberStyles.Any, CultureInfo.InvariantCulture, out dval)
                                    ? dval
                                    : value;

            double dpar;
            dynamic par = parameter == null
                              ? 0
                              : parameter is string && Double.TryParse((string)parameter, NumberStyles.Any, CultureInfo.InvariantCulture, out dpar)
                                    ? dpar
                                    : parameter;

            return val + par;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}