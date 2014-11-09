using System;
using System.Windows.Data;

namespace Rikrop.Core.Wpf.Converters
{
    public class IntToMonthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DateTime dt = new DateTime(1900, (int)value, 1);
            return dt.ToString("MMMM");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
