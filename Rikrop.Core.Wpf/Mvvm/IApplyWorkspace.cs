using System;
using Rikrop.Core.Wpf.Commands;

namespace Rikrop.Core.Wpf.Mvvm
{
    public interface IApplyWorkspace : IWorkspace
    {
        event EventHandler Applied;
        bool IsApplied { get; }
        bool IsApplying { get; }
        RelayCommand ApplyCommand { get; }
    }
}