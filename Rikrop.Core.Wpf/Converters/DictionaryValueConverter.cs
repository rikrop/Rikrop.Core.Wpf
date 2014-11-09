using System;
using System.Collections;
using System.Windows.Data;

namespace Rikrop.Core.Wpf.Converters
{
    public class DictionaryValueConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var key = parameter as string;

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("parameter");
            }

            var dictionary = value as IDictionary;

            if (dictionary == null)
            {
                throw new ArgumentException("value");
            }

            return dictionary[key];
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
