using System;
using Rikrop.Core.Wpf.Mvvm;

namespace Rikrop.Core.Wpf.Workspace
{
    public interface IWorkspaceVisualizator
    {
        bool TrySetActive<TIdentifier, TWorkspace>(TIdentifier identifier, Func<TIdentifier, TWorkspace, bool> comparer) where TWorkspace : IWorkspace;
        bool TrySetActive<TWorkspace>() where TWorkspace : IWorkspace;
        void SetActive<TIdentifier, TWorkspace>(TIdentifier identifier, Func<TIdentifier, TWorkspace, bool> comparer) where TWorkspace : IWorkspace;
        void SetActive<TWorkspace>() where TWorkspace : IWorkspace;
        void SetActive(IWorkspace workspace);
        TWorkspace FindWorkspace<TWorkspace>() where TWorkspace : IWorkspace;
        TWorkspace FindWorkspace<TIdentifier, TWorkspace>(TIdentifier identifier, Func<TIdentifier, TWorkspace, bool> comparer) where TWorkspace : IWorkspace;
        void AddWorkspace(IWorkspace workspace);
    }
}