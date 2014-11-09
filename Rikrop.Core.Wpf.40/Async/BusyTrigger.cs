using System.Threading;

namespace Rikrop.Core.Wpf.Async
{
    public sealed class BusyTrigger : ChangeNotifier, IBusyItem, IBusyTrigger
    {
        private bool _isBusy;
        private int _counter;

        public bool IsBusy
        {
            get { return _isBusy; }
            private set { SetProperty(ref _isBusy, value); }
        }

        public void SetBusy()
        {
            var currentCount = Interlocked.Increment(ref _counter);
            if (currentCount == 1)
            {
                IsBusy = true;
            }
        }

        public void ClearBusy()
        {
            var currentCount = Interlocked.Decrement(ref _counter);
            if (currentCount == 0)
            {
                IsBusy = false;
            }
        }
    }
}