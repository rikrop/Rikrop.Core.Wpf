using System.ComponentModel;

namespace Rikrop.Core.Wpf.Async
{
    public interface IBusyItem : INotifyPropertyChanged
    {
        bool IsBusy { get; }
    }
}