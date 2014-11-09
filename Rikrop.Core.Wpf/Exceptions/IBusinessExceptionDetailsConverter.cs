namespace Rikrop.Core.Wpf.Exceptions
{
    public interface IBusinessExceptionDetailsConverter
    {
        bool CanConvert(object details);
        string Convert(object details);
    }
}