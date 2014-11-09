using System;
using System.Globalization;
using System.Windows.Data;

namespace Rikrop.Core.Wpf.Converters
{
    public class CommentsCountToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double commentsCount = System.Convert.ToDouble(value);

            if (commentsCount >= 1000)
            {
                var thousandsCount = (int) (commentsCount/1000.0);
                return thousandsCount + "т";
            }

            return commentsCount;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}