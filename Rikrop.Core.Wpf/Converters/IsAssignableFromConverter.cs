using System;
using System.Globalization;
using System.Windows.Data;

namespace Rikrop.Core.Wpf.Converters
{
    public class IsAssignableFromConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter != null && value != null)
            {
                Type type;
                if(parameter is Type)
                {
                    type = (Type) parameter;
                }
                else
                {
                    type = parameter.GetType();
                }
                return type.IsAssignableFrom(value.GetType());
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}