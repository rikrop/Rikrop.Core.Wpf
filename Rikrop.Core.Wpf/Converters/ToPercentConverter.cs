using System;
using System.Windows.Data;

namespace Rikrop.Core.Wpf.Converters
{
    public class ToPercentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return value;

            if (string.IsNullOrEmpty(value.ToString())) 
                return 0;

            if (value is double) 
                return (double)value * 100;

            if (value is decimal) 
                return (decimal)value * 100;

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return value;

            var strValue = value.ToString();

            if (string.IsNullOrEmpty(strValue)) 
                return 0;

            if (targetType == typeof(double) || targetType == typeof(double?))
            {
                double result;
                if (double.TryParse(strValue, out result))
                    return result / 100;
                else
                    return value;
            }

            if (targetType == typeof(decimal) || targetType == typeof(decimal?))
            {
                decimal result;
                if (decimal.TryParse(strValue, out result))
                    return result / 100;
                else
                    return value;
            }
            return value;
        }
    }
}
