using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;

namespace Rikrop.Core.Wpf.Converters
{
    [ContentProperty("Converters")]
    public class ComposingConverter : IValueConverter
    {
        private readonly List<IValueConverter> _converters = new List<IValueConverter>();

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public List<IValueConverter> Converters
        {
            get { return _converters; }
        }

        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            return _converters.Aggregate(value, (current, t) => t.Convert(current, targetType, parameter, culture));
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            for (var i = _converters.Count - 1; i >= 0; i--)
            {
                value = _converters[i].ConvertBack(value, targetType, parameter, culture);
            }

            return value;
        }
    }
}