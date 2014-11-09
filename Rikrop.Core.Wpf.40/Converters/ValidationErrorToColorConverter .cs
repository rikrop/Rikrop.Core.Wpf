using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Rikrop.Core.Wpf.Converters
{
    public class ValidationErrorToBorderBrushConverter : IValueConverter
    {
        private static Color ConvertStandartValidation(ValidationError validationError)
        {
            return Color.FromArgb(0xFF, 0xFF, 0, 0);
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var validationError = value as ValidationError;
            if (validationError == null)
            {
                return null;
            }

            var color = ConvertStandartValidation(validationError);

            return new SolidColorBrush(color);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    public class ValidationErrorToBackgroundBrushConverter : IValueConverter
    {
        private static Color ConvertStandartValidation(ValidationError validationError)
        {
            return Color.FromArgb(0xFF, 0xFA, 0xCb, 0xD1);
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var validationError = value as ValidationError;
            if (validationError == null)
            {
                return null;
            }

            var color = ConvertStandartValidation(validationError);

            return new SolidColorBrush(color);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}