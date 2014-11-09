using System;
using System.Diagnostics.Contracts;
using System.Windows.Threading;

namespace Rikrop.Core.Wpf.Async
{
    public class DelayedExecutor
    {
        private readonly Action _execute;

        private readonly DispatcherTimer _timer;

        public DelayedExecutor(Action execute)
            : this(execute, TimeSpan.FromMilliseconds(500))
        {
        }

        public DelayedExecutor(Action execute, TimeSpan delay)
        {
            Contract.Requires<ArgumentNullException>(execute != null);
            _execute = execute;

            _timer = new DispatcherTimer
                         {
                             Interval = delay
                         };
            _timer.Tick += OnTimerTick;
        }

        public void Touch()
        {
            ResetTimer();
        }

        public void Force()
        {
            _timer.Stop();
            _execute();
        }

        private void ResetTimer()
        {
            _timer.Stop();
            _timer.Start();
        }

        private void OnTimerTick(object sender, EventArgs eventArgs)
        {
            Force();
        }
    }
}