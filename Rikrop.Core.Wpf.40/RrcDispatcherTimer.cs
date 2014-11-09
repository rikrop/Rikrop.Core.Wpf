using System;
using System.Windows.Threading;
using Rikrop.Core.Framework;

namespace Rikrop.Core.Wpf
{
    public class RrcDispatcherTimer : ITimer
    {
        private readonly DispatcherTimer _realTimer;

        public event EventHandler Tick;

        public bool IsEnabled
        {
            get { return _realTimer.IsEnabled; }
            set { _realTimer.IsEnabled = value; }
        }

        public TimeSpan Interval
        {
            get { return _realTimer.Interval; }
            set { _realTimer.Interval = value; }
        }

        public RrcDispatcherTimer()
            : this(DispatcherPriority.Background)
        {
        }

        public RrcDispatcherTimer(DispatcherPriority priority)
            : this(priority, Dispatcher.CurrentDispatcher)
        {
        }

        public RrcDispatcherTimer(DispatcherPriority priority, Dispatcher dispatcher)
        {
            _realTimer = new DispatcherTimer(priority, dispatcher);
            _realTimer.Tick += OnRealTimerElapsed;
        }

        public void Start()
        {
            _realTimer.Start();
        }

        public void Stop()
        {
            _realTimer.Stop();
        }

        private void OnRealTimerElapsed(object sender, EventArgs e)
        {
            var handler = Tick;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}