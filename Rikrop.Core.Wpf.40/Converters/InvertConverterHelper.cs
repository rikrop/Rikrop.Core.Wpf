namespace Rikrop.Core.Wpf.Converters
{
    public static class InvertConverterHelper
    {
        public static bool InvertValue(bool value, object parameter)
        {
            var invertParametr = parameter as string;
            return invertParametr == "Invert" ? !value : value;
        }

        public static object InvertValue(bool value, object originalValue, object invertedValue, object parameter)
        {
            value = InvertValue(value, parameter);
            return value ? originalValue : invertedValue;
        }
    }
}