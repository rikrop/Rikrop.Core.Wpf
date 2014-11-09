using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rikrop.Core.Wpf.Mvvm.Navigation
{
    internal class NavigationSequence
    {
        private readonly List<NavigationTask> _showStack;
        private readonly List<Task> _navigatedTasks;

        public NavigationSequence()
        {
            _showStack = new List<NavigationTask>();
            _navigatedTasks = new List<Task>();
        }

        public async Task<bool> Navigate(NavigationTask navigationTask)
        {
            var task = NavigateInternal(navigationTask);

            _navigatedTasks.Insert(0, task);
            var result = await task;
            _navigatedTasks.Remove(task);

            return result;
        }

        private async Task<bool> NavigateInternal(NavigationTask navigationTask)
        {
            _showStack.Insert(0, navigationTask);

            var result = await navigationTask.Task;

            _showStack.Remove(navigationTask);
            return result;
        }

        public Task Complete()
        {
            return CompleteTo(_showStack.Count - 1);
        }

        public Task Interrupt()
        {
            return Interrupt(interruptionCount: _showStack.Count);
        }

        private async Task Interrupt(int interruptionCount)
        {
            if (_showStack.Count == 0)
            {
                return;
            }

            var tasksToAwait = _navigatedTasks.Take(interruptionCount).ToList();

            foreach (var navigateTask in _showStack.Take(interruptionCount).ToList())
            {
                navigateTask.Interrupt();
            }

            await TaskEx.WhenAll(tasksToAwait);
        }

        public async Task<bool> TryNavigateBack(IWorkspace workspace)
        {
            if (_showStack.Count == 0)
            {
                return false;
            }

            var navigationTask = _showStack.FirstOrDefault(w => w.Workspace == workspace);
            if (navigationTask != null)
            {
                var indexOfWorkspace = _showStack.IndexOf(navigationTask);
                if (indexOfWorkspace > 0)
                {
                    await Interrupt(interruptionCount: indexOfWorkspace);                    
                }
                return true;
            }

            await Interrupt();
            return false;
        }

        private async Task CompleteTo(int completeIndex)
        {
            if (_showStack.Count == 0)
            {
                return;
            }

            var tasksToAwait = _navigatedTasks.Take(completeIndex + 1).ToList();
            var completeTask = _showStack[completeIndex];

            foreach (var navigateTask in _showStack.Take(completeIndex).ToList())
            {
                navigateTask.Interrupt();
            }

            completeTask.Complete();

            await TaskEx.WhenAll(tasksToAwait);
        }
    }
}