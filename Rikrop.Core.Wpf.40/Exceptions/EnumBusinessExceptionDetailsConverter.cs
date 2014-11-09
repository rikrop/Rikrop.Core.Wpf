namespace Rikrop.Core.Wpf.Exceptions
{
    public class EnumBusinessExceptionDetailsConverter : IBusinessExceptionDetailsConverter
    {
        private readonly EnumToStringConverter _converter;

        public EnumBusinessExceptionDetailsConverter()
        {
            _converter = new EnumToStringConverter();
        }

        public bool CanConvert(object details)
        {
            return details != null && details.GetType().IsEnum;
        }

        public string Convert(object details)
        {
            return _converter.ConvertToString(details);
        }
    }
}