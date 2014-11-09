using System.ComponentModel;
using System.Windows;

namespace Rikrop.Core.Wpf.Mvvm
{
    public interface IViewModel : INotifyPropertyChanged
    {
        FrameworkElement Content { get; set; }
    }
}