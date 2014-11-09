using System.Windows.Data;

namespace Rikrop.Core.Wpf.Converters
{
    public class ScrollbarOnFarLeftConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return false;
            return ((double)value > 0);
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;//throw new System.NotImplementedException();
        }
    }
}
