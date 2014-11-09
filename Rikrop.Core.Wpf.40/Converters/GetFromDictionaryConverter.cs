using System;
using System.Collections;
using System.Windows.Data;

namespace Rikrop.Core.Wpf.Converters
{
    public class GetFromDictionaryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
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

            return dictionary.Contains(key)
                       ? dictionary[key]
                       : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
