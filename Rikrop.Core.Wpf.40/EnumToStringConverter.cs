using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace Rikrop.Core.Wpf
{
    public class EnumToStringConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || sourceType.IsEnum;
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if(value is Enum)
            {
                return GetEnumDescription((Enum) value);
            }
            return null;
        }

        private static string GetEnumDescription(Enum value)
        {            
            FieldInfo fi = value.GetType().GetField(value.ToString());
            if (fi == null) return value.ToString();
            var attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes
                                            (typeof(DescriptionAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
        }

        public static string ConvertToString(Enum value)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(value.GetType());
            return converter.ConvertToString(value);
        }
    }
}
