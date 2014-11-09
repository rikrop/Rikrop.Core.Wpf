using System;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using Rikrop.Core.Wpf.Mvvm;

namespace Rikrop.Core.Wpf.Workspace
{
    public class WorkspaceVisualizator : ChangeNotifier, IWorkspaceVisualizator
    {
        private readonly ObservableCollection<IWorkspace> _workspacesInternal;
        private readonly ReadOnlyObservableCollection<IWorkspace> _workspaces;
        private IWorkspace _selectedWorkspace;

        public ReadOnlyObservableCollection<IWorkspace> Workspaces
        {
            get { return _workspaces; }
        }

        public IWorkspace SelectedWorkspace
        {
            get { return _selectedWorkspace; }
            set
            {
                if (value != null && !Workspaces.Contains(value))
                {
                    throw new InvalidOperationException("���������� ���������� ��������� workspace �� �� ���������");
                }

                if(_selectedWorkspace != null)
                {
                    _selectedWorkspace.Deactivate();
                }

                SetProperty(ref _selectedWorkspace, value);

                if (_selectedWorkspace != null)
                {
                    _selectedWorkspace.Activate();
                }
            }
        }

        public WorkspaceVisualizator()
        {
            _workspacesInternal = new ObservableCollection<IWorkspace>();
            _workspaces = new ReadOnlyObservableCollection<IWorkspace>(_workspacesInternal);
        }

        public bool TrySetActive<TIdentifier, TWorkspace>(TIdentifier identifier, Func<TIdentifier, TWorkspace, bool> comparer)
            where TWorkspace : IWorkspace
        {
            var exvm = Workspaces
                .OfType<TWorkspace>()
                .FirstOrDefault(vm => comparer(identifier, vm));

            if (exvm != null)
            {
                SelectedWorkspace = exvm;
                return true;
            }
            return false;
        }

        public bool TrySetActive<TWorkspace>()
            where TWorkspace : IWorkspace
        {
            var exvm = Workspaces
                .OfType<TWorkspace>()
                .FirstOrDefault();

            if (exvm != null)
            {
                SelectedWorkspace = exvm;
                return true;
            }
            return false;
        }

        public void SetActive<TIdentifier, TWorkspace>(TIdentifier identifier, Func<TIdentifier, TWorkspace, bool> comparer) where TWorkspace : IWorkspace
        {
            var exvm = Workspaces
                .OfType<TWorkspace>()
                .First(vm => comparer(identifier, vm));
            SelectedWorkspace = exvm;
        }

        public void SetActive<TWorkspace>() where TWorkspace : IWorkspace
        {
            var exvm = Workspaces
                .OfType<TWorkspace>()
                .First();
            SelectedWorkspace = exvm;
        }

        public void SetActive(IWorkspace workspace)
        {
            var exvm = Workspaces.First(vm => Equals(vm, workspace));
            SelectedWorkspace = exvm;
        }

        public TWorkspace FindWorkspace<TWorkspace>() where TWorkspace : IWorkspace
        {
            var exvm = Workspaces
                .OfType<TWorkspace>()
                .FirstOrDefault();

            return exvm;
        }

        public TWorkspace FindWorkspace<TIdentifier, TWorkspace>(TIdentifier identifier, Func<TIdentifier, TWorkspace, bool> comparer) where TWorkspace : IWorkspace
        {
            var exvm = Workspaces
                .OfType<TWorkspace>()
                .FirstOrDefault(vm => comparer(identifier, vm));

            return exvm;
        }

        public void AddWorkspace(IWorkspace workspace)
        {
            _workspacesInternal.Add(workspace);
            workspace.RequestClose += delegate { _workspacesInternal.Remove(workspace); };
        }
    }
}