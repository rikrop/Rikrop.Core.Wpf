using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rikrop.Core.Wpf.Async
{
    public sealed class LastCallResultAwaiter
    {
        private CancellationTokenSource _cts;

        public LastCallResultAwaiter()
        {
            RenewCancellationTokenSource();
        }

        public void Cancel()
        {
            _cts.Cancel();
            RenewCancellationTokenSource();
        }

        private void RenewCancellationTokenSource()
        {
            _cts = new CancellationTokenSource();
        }

        public async Task Await(Func<Task> taskCreator)
        {
            Cancel();
            var ct = _cts.Token;
            await taskCreator();
            ct.ThrowIfCancellationRequested();
        }

        public async Task<TResult> Await<TResult>(Func<Task<TResult>> taskCreator)
        {
            Cancel();
            var ct = _cts.Token;
            var result = await taskCreator();
            ct.ThrowIfCancellationRequested();
            return result;
        }

        public async Task Await(Func<CancellationToken, Task> taskCreator)
        {
            Cancel();
            var ct = _cts.Token;
            await taskCreator(ct);
            ct.ThrowIfCancellationRequested();
        }

        public async Task<TResult> Await<TResult>(Func<CancellationToken, Task<TResult>> taskCreator)
        {
            Cancel();
            var ct = _cts.Token;
            var result = await taskCreator(ct);
            ct.ThrowIfCancellationRequested();
            return result;
        }
    }
}