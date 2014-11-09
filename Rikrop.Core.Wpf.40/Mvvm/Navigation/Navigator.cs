using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace Rikrop.Core.Wpf.Mvvm.Navigation
{
    public class Navigator : ChangeNotifier, INavigator, INavigatorSource, INavigatorBrowser
    {
        private readonly List<NavigationSequence> _navigationSequences = new List<NavigationSequence>();
        private readonly ObservableCollection<IWorkspace> _workspacesInternal;
        private readonly ReadOnlyObservableCollection<IWorkspace> _workspaces;
        private NavigationSequence _navigationSequence;

        private IWorkspace _workspace;

        public IWorkspace Workspace
        {
            get { return _workspace; }
            private set
            {
                if (_workspace == value)
                {
                    return;
                }

                DeactivateWorkspace(_workspace);
                _workspace = value;
                ActivateWorkspace(_workspace);

                NotifyPropertyChanged(() => Workspace);
            }
        }

        public ReadOnlyObservableCollection<IWorkspace> Workspaces
        {
            get { return _workspaces; }
        }

        public Navigator()
        {
            _workspacesInternal = new ObservableCollection<IWorkspace>();
            _workspaces = new ReadOnlyObservableCollection<IWorkspace>(_workspacesInternal);
        }

        private void ActivateWorkspace(IWorkspace workspace)
        {
            if (workspace != null)
            {
                workspace.Activate();
            }
        }

        private void DeactivateWorkspace(IWorkspace workspace)
        {
            if (workspace != null)
            {
                workspace.Deactivate();
            }
        }

        public async Task NavigateTo(IWorkspace workspace)
        {
            if (_navigationSequence == null)
            {
                await StartNewSequenceFrom(workspace);
                return;
            }

            var oldWorkspace = Workspace;        

            bool isCompleted;
            _workspacesInternal.Add(workspace);
            try
            {
                Workspace = workspace;
                isCompleted = await _navigationSequence.Navigate(new NavigationTask(workspace));
            }
            finally
            {
                _workspacesInternal.Remove(workspace);
            }

            if (!isCompleted)
            {
                return;
            }

            Workspace = oldWorkspace;
        }

        public async Task StartNewSequenceFrom(IWorkspace workspace)
        {
            var oldNavigationSequence = _navigationSequence;

            _navigationSequence = new NavigationSequence();
            _navigationSequences.Insert(0, _navigationSequence);

            await NavigateTo(workspace);

            _navigationSequences.Remove(_navigationSequence);
            _navigationSequence = oldNavigationSequence;
        }

        public async Task CompleteCurrentSequence()
        {
            if (_navigationSequence != null)
            {
                await _navigationSequence.Complete();
            }
        }

        public Task BackToRoot()
        {
            Contract.Assume(Workspaces.Any());

            return NavigateBack(workspace: Workspaces.First());
        }

        public async Task NavigateBack(IWorkspace workspace)
        {
            foreach (var sequence in _navigationSequences.ToArray())
            {
                if (await sequence.TryNavigateBack(workspace))
                {
                    Workspace = workspace;
                    break;
                }
            }
        }
    }
}