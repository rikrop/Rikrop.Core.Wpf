using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;

namespace Rikrop.Core.Wpf.Converters
{
    [ContentProperty("Pairs")]
    public class KeyToValueConverter : IValueConverter
    {
        private readonly List<KeyToValuePair> _pairs;

        public object DefaultValue { get; set; }

        public List<KeyToValuePair> Pairs
        {
            get { return _pairs; }
        }

        public bool ThrowOnNoKey { get; set; }

        public KeyToValueConverter()
        {
            _pairs = new List<KeyToValuePair>();
            ThrowOnNoKey = true;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var tObject = _pairs.FirstOrDefault(o => Equals(o.Key, value));

            if (tObject != null)
            {
                return tObject.Value;
            }
            if (ThrowOnNoKey)
            {
                throw new ArgumentException(String.Format("Среди списка заданных значений: {0} не найдено значение {1}",
                    string.Join(", ", Pairs.Select(o => o.Key)), value));
            }
            return DefaultValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    public class KeyToValuePair
    {
        public object Key { get; set; }
        public object Value { get; set; }
    }
}