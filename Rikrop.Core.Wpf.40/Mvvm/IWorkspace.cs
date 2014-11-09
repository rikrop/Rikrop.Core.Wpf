using System;
using System.Windows.Input;

namespace Rikrop.Core.Wpf.Mvvm
{
    public interface IWorkspace : IViewModel
    {
        string DisplayName { get; }
        ICommand CloseCommand { get; }
        
        void Activate();
        void Deactivate();
        bool Close();
        
        event EventHandler RequestClose;
    }
}