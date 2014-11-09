using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Rikrop.Core.Wpf.Converters
{
    public class AllTrueVisibilityToBooleanConverter : IMultiValueConverter
    {
        public Visibility VisibilityOnFalse { get; set; }

        public AllTrueVisibilityToBooleanConverter()
        {
            VisibilityOnFalse = Visibility.Collapsed;
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool flag = values.All(v => (bool)v);

            return InvertConverterHelper.InvertValue(flag, Visibility.Visible, VisibilityOnFalse, parameter);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}