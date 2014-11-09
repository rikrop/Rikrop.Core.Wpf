using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Windows.Threading;
using Rikrop.Core.Wpf.Mvvm;
using Rikrop.Core.Wpf.Mvvm.Visualizer;

namespace Rikrop.Core.Wpf.Async
{
    public class PopupTaskExecutionNotifier
    {
        private readonly IPopupVisualizer _visualizer;
        private readonly IBusyTrigger _busyTrigger;
        private readonly DispatcherTimer _displayAfterTimer;
        private readonly IWorkspace _popupWorkspace;

        private readonly LastCallResultAwaiter _lastCallResultAwaiter;

        private bool _isExecuting;

        public PopupTaskExecutionNotifier(IPopupVisualizer visualizer, IBusyTrigger busyTrigger, IWorkspace popupWorkspace, TimeSpan displayAfter)
        {
            Contract.Requires<ArgumentNullException>(visualizer != null);
            Contract.Requires<ArgumentNullException>(busyTrigger != null);
            Contract.Requires<ArgumentNullException>(popupWorkspace != null);

            _visualizer = visualizer;
            _busyTrigger = busyTrigger;
            _popupWorkspace = popupWorkspace;

            _displayAfterTimer = new DispatcherTimer {Interval = displayAfter};
            _displayAfterTimer.Tick += DisplayAfterTimerElapsed;

            _lastCallResultAwaiter = new LastCallResultAwaiter();
        }

        public async Task TrackExecution(Func<Task> taskCreator)
        {
            BeginExecution();
            
            bool wasCanceled = false;
            try
            {
                await _lastCallResultAwaiter.Await(taskCreator);
            }
            catch (OperationCanceledException)
            {
                wasCanceled = true;
                throw;
            }
            finally
            {
                if (!wasCanceled)
                {
                    EndExecution();
                }
            }
        }

        public async Task<TResult> TrackExecution<TResult>(Func<Task<TResult>> taskCreator)
        {
            BeginExecution();

            bool wasCanceled = false;
            try
            {
                return await _lastCallResultAwaiter.Await(taskCreator);
            }
            catch (OperationCanceledException)
            {
                wasCanceled = true;
                throw;
            }
            finally
            {
                if (!wasCanceled)
                {
                    EndExecution();
                }
            }
        }

        private void BeginExecution()
        {
            StopTimer();
            StopExecution();

            _busyTrigger.SetBusy();

            _isExecuting = true;

            if (_displayAfterTimer.Interval == TimeSpan.Zero)
            {
                ShowPopup();
            }
            else
            {
                _displayAfterTimer.Start();
            }
        }

        private void EndExecution()
        {
            StopTimer();
            StopExecution();
            ClosePopup();
        }

        private void DisplayAfterTimerElapsed(object sender, EventArgs eventArgs)
        {
            StopTimer();

            lock (_lastCallResultAwaiter)
            {
                if (_isExecuting)
                {
                    ShowPopup();
                }
            }
        }

        private async void ShowPopup()
        {
            await _visualizer.Show(_popupWorkspace);

            var wasExecution = StopExecution();
            if (wasExecution)
            {
                _lastCallResultAwaiter.Cancel();
            }
        }

        private bool StopExecution()
        {
            lock (_lastCallResultAwaiter)
            {
                var wasExecution = _isExecuting;
                
                if (_isExecuting)
                {
                    _busyTrigger.ClearBusy();
                }
                _isExecuting = false;

                return wasExecution;
            }
        }

        private void StopTimer()
        {
            _displayAfterTimer.Stop();
        }

        private void ClosePopup()
        {
            if (_popupWorkspace.CloseCommand.CanExecute((null)))
            {
                _popupWorkspace.CloseCommand.Execute(null);
            }
        }
    }
}