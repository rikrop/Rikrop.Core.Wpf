using System;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace Rikrop.Core.Wpf.Async
{
    public class TaskExecutionNotifier
    {
        private readonly IBusyTrigger _viewer;
        private int _concatenateCounter;

        public TaskExecutionNotifier(IBusyTrigger viewer)
        {
            Contract.Requires<ArgumentNullException>(viewer != null);
            _viewer = viewer;
        }

        public async Task TrackExecution(Func<Task> taskCreator)
        {
            var currentCounter = Interlocked.Increment(ref _concatenateCounter);
            if (currentCounter == 1)
            {
                _viewer.SetBusy();
            }
            try
            {
                await taskCreator();
            }
            finally
            {
                currentCounter = Interlocked.Decrement(ref _concatenateCounter);
                if (currentCounter == 0)
                {
                    _viewer.ClearBusy();
                }
            }
        }

        public async Task<TResult> TrackExecution<TResult>(Func<Task<TResult>> taskCreator)
        {
            TResult result;

            var currentCounter = Interlocked.Increment(ref _concatenateCounter);
            if (currentCounter == 1)
            {
                _viewer.SetBusy();
            }
            try
            {
                result = await taskCreator();
            }
            finally
            {
                currentCounter = Interlocked.Decrement(ref _concatenateCounter);
                if (currentCounter == 0)
                {
                    _viewer.ClearBusy();
                }
            }
            return result;
        }
    }
}