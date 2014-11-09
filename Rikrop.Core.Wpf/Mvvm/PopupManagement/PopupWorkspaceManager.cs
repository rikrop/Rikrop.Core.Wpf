using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Windows.Input;
using Rikrop.Core.Wpf.Commands;

namespace Rikrop.Core.Wpf.Mvvm.PopupManagement
{
    public class PopupWorkspaceManager : ChangeNotifier, IPopupWorkspaceManager
    {
        private readonly RelayCommand _hideWorkspaceCommand;
        private bool _isOpen;
        private IWorkspace _workspace;
        private WorkspaceShowTask _showTask;

        public bool IsOpen
        {
            get { return _isOpen; }
            set { SetProperty(ref _isOpen, value); }
        }

        public IWorkspace Workspace
        {
            get { return _workspace; }
            private set
            {
                if (_workspace == value)
                {
                    return;
                }
                if (_workspace != null)
                {
                    _workspace.Deactivate();
                }
                _workspace = value;
                if (_workspace != null)
                {
                    _workspace.Activate();
                }
                NotifyPropertyChanged(() => Workspace);
            }
        }

        public ICommand HideWorkspaceCommand
        {
            get { return _hideWorkspaceCommand; }
        }

        public PopupWorkspaceManager()
        {
            _hideWorkspaceCommand = new RelayCommand(HideWorkpspace);

            AfterNotify(() => IsOpen).Execute(
                () =>
                    {
                        if (IsOpen)
                        {
                            Contract.Assume(Workspace != null);
                        }
                        else
                        {
                            Workspace = null;
                            if (_showTask != null)
                            {
                                _showTask.Interrupt();
                                _showTask = null;
                            }
                        }
                    });
        }

        public async Task<bool> ShowWorkspace(IWorkspace workspace)
        {
            Contract.Requires<ArgumentNullException>(workspace != null);

            Contract.Assume(_showTask == null);
            Contract.Assume(Workspace == null);

            _showTask = new WorkspaceShowTask(workspace);
            Workspace = _showTask.Workspace;
            IsOpen = true;
            bool isCompleted;
            try
            {
                isCompleted = await _showTask.Task;
            }
            finally
            {
                IsOpen = false;
            }
            return isCompleted;
        }

        private void HideWorkpspace()
        {
            IsOpen = false;
        }
    }
    
    internal class WorkspaceShowTask
    {
        private readonly TaskCompletionSource<bool> _tcs;
        private readonly IWorkspace _workspace;

        private bool _isInterrupted;

        public Task<bool> Task
        {
            get { return _tcs.Task; }
        }

        public IWorkspace Workspace
        {
            get { return _workspace; }
        }

        public WorkspaceShowTask(IWorkspace workspace)
        {
            Contract.Requires<ArgumentNullException>(workspace != null);

            _tcs = new TaskCompletionSource<bool>();

            _workspace = workspace;
            _workspace.RequestClose += WorkspaceOnRequestClose;
        }

        public void Interrupt()
        {
            _isInterrupted = true;
            Complete();
        }

        public void Complete()
        {
            _workspace.Close();
        }

        private void WorkspaceOnRequestClose(object sender, EventArgs eventArgs)
        {
            _workspace.RequestClose -= WorkspaceOnRequestClose;

            _tcs.TrySetResult(!_isInterrupted);
        }
    }
}