namespace Rikrop.Core.Wpf.Mvvm.ValueEditing.ValueConverter
{
    public interface IValueConverter<TValue, TEditedValue>
    {
        TValue ConvertToValue(TEditedValue editedValue);
        TEditedValue ConvertToEditedValue(TValue value);
    }
}