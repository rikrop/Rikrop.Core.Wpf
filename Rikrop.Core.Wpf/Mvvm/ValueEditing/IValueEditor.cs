namespace Rikrop.Core.Wpf.Mvvm.ValueEditing
{
    public interface IValueEditor<out TValue, TEditedValue> : IValueEditorHandler
    {
        TEditedValue EditValue { get; set; }
        TValue GetValue();
    }
}