using System;
using System.Globalization;
using System.Windows.Data;

namespace Rikrop.Core.Wpf.Converters
{
    public class ComparisonConverter : IValueConverter
    {
        public object DefaultValue { get; set; }
        public object DefaultParameter { get; set; }

        public ComparisonConverter()
        {
            DefaultValue = 0;
            DefaultParameter = 0;
        }

        public NumericComparisonMode Mode { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double dval;
            dynamic val = value == null
                              ? DefaultValue
                              : value is string && Double.TryParse((string) value, NumberStyles.Any, CultureInfo.InvariantCulture, out dval)
                                    ? dval
                                    : value;

            double dpar;
            dynamic par = parameter == null
                              ? DefaultParameter
                              : parameter is string && Double.TryParse((string) parameter, NumberStyles.Any, CultureInfo.InvariantCulture, out dpar)
                                    ? dpar
                                    : parameter;

            switch (Mode)
            {
                case NumericComparisonMode.MoreThen:
                    var result = val > par;
                    return result;
                case NumericComparisonMode.MoreThenOrEqual:
                    return val >= par;
                case NumericComparisonMode.LessThen:
                    return val < par;
                case NumericComparisonMode.LessThenOrEqual:
                    return val <= par;
                case NumericComparisonMode.Equal:
                    return val == par;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    public enum NumericComparisonMode
    {
        MoreThen,
        MoreThenOrEqual,
        LessThen,
        LessThenOrEqual,
        Equal,
    }
}