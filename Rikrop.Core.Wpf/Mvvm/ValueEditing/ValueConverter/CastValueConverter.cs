namespace Rikrop.Core.Wpf.Mvvm.ValueEditing.ValueConverter
{
    public class CastValueConverter<TValue, TEditedValue> : IValueConverter<TValue, TEditedValue>
    {
        public TValue ConvertToValue(TEditedValue editedValue)
        {
            return (TValue)((object) editedValue);
        }

        public TEditedValue ConvertToEditedValue(TValue value)
        {
            return (TEditedValue)((object)value);
        }
    }
}