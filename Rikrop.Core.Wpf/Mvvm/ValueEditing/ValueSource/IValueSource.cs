using System.ComponentModel;

namespace Rikrop.Core.Wpf.Mvvm.ValueEditing.ValueSource
{
    public interface IValueSource<TValue> : INotifyPropertyChanged
    {
        TValue Value { get; set; }
    }
}