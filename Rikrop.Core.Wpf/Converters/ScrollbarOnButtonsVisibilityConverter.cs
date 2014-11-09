using System;
using System.Windows.Data;
using System.Windows;

namespace Rikrop.Core.Wpf.Converters
{
    public class ScrollbarOnButtonsVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values == null) return false;
            if (values[0] == null || values[1] == null) return false;
            if (values[0].Equals(double.NaN) || values[1].Equals(double.NaN)) return false;

            double dblViewportWidth = 0;
            double dblExtentWidth = 0;

            double.TryParse(values[0].ToString(), out dblViewportWidth);
            double.TryParse(values[1].ToString(), out dblExtentWidth);

            bool flag = Math.Round((dblViewportWidth), 2) < Math.Round(dblExtentWidth, 2);

            return InvertConverterHelper.InvertValue(flag, Visibility.Visible, Visibility.Collapsed, string.Empty);
        }

        public object[] ConvertBack(object value, System.Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}
