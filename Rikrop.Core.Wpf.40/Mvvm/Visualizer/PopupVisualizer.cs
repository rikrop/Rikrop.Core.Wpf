using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rikrop.Core.Wpf.Mvvm.Visualizer
{
    public class PopupVisualizer : ChangeNotifier, IPopupVisualizer, IPopupSource
    {
        private readonly List<IWorkspace> _showStack;
        private IWorkspace _workspace;

        public IWorkspace Workspace
        {
            get { return _workspace; }
            private set
            {
                DeactivateWorkspace(_workspace);

                SetProperty(ref _workspace, value);

                ActivateWorkspace(_workspace);
            }
        }

        private static void ActivateWorkspace(IWorkspace workspace)
        {
            if (workspace != null)
            {
                workspace.Activate();
            }
        }

        private static void DeactivateWorkspace(IWorkspace workspace)
        {
            if (workspace != null)
            {
                workspace.Deactivate();
            }
        }

        public bool IsOpen
        {
            get { return Workspace != null; }
        }

        public PopupVisualizer()
        {
            _showStack = new List<IWorkspace>();
            AfterNotify(() => Workspace)
                .Notify(() => IsOpen);
        }

        public async Task Show(IWorkspace workspace)
        {
            _showStack.Add(workspace);
            
            await ShowAsync(workspace);
            
            _showStack.Remove(workspace);

            if (Workspace == workspace)
            {
                ShowInternal(_showStack.LastOrDefault());
            }
        }

        public bool Close()
        {
            if (Workspace == null)
            {
                return true;
            }
            return Workspace.Close();
        }

        private Task ShowAsync(IWorkspace workspace)
        {
            var tcs = new TaskCompletionSource<object>();
            workspace.RequestClose += (sender, args) => tcs.TrySetResult(null);
            ShowInternal(workspace);
            return tcs.Task;
        }

        private void ShowInternal(IWorkspace workspace)
        {
            Workspace = workspace;
        }
    }
}