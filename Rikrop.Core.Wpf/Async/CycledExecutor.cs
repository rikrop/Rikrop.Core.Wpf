using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Rikrop.Core.Wpf.Async
{
    public class CycledExecutor
    {
        private readonly Func<Task> _executeAction;
        private readonly DispatcherTimer _timer;
        private bool _canStart;

        public CycledExecutor(Func<Task> executeAction, TimeSpan betweenExecuteTimeout)
        {
            Contract.Requires<ArgumentNullException>(executeAction != null);

            _executeAction = executeAction;
            _timer = new DispatcherTimer
                         {
                             Interval = betweenExecuteTimeout,
                         };
            _timer.Tick += TimerOnTick;
        }

        public void Stop()
        {
            _timer.Stop();
            _canStart = false;
        }

        public void Start()
        {
            _timer.Start();
            _canStart = true;
        }

        public Task ForceExecute()
        {
            return Execute();
        }

        private async void TimerOnTick(object sender, EventArgs eventArgs)
        {
            await Execute();
        }

        private async Task Execute()
        {
            _timer.Stop();
            try
            {
                await _executeAction();
            }
            finally
            {
                if (_canStart)
                {
                    _timer.Start();
                }
            }
        }
    }
}