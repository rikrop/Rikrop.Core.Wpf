using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace Rikrop.Core.Wpf.Mvvm.Navigation
{
    internal class NavigationTask
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

        public NavigationTask(IWorkspace workspace)
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