using System;
using System.Globalization;
using System.Windows.Data;

namespace Rikrop.Core.Wpf.Converters
{
    public class EnumToStringValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Enum)
            {
                return ProcessParameter(EnumToStringConverter.ConvertToString((Enum) value), parameter);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        private string ProcessParameter(string value, object parameter)
        {
            if (parameter != null && parameter.ToString() == "ToLowerCase")
            {
                return value.ToLower();
            }
            return value;
        }
    }
}