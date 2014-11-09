using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Rikrop.Core.Framework.Services;

namespace Rikrop.Core.Wpf.Mvvm.ValueEditing.ValueSaver
{
    public class ServiceValueSaver<TService, TEditedValue> : IAsyncValueSaver<TEditedValue>
    {
        private readonly IServiceExecutor<TService> _serviceExecutor;
        private readonly Func<TService, TEditedValue, Task> _func;

        public ServiceValueSaver(IServiceExecutor<TService> serviceExecutor, Func<TService, TEditedValue, Task> func)
        {
            Contract.Requires<ArgumentNullException>(func != null);
            _serviceExecutor = serviceExecutor;
            _func = func;
        }

        public Task SaveValueAsync(TEditedValue editedValue)
        {
            return _serviceExecutor.Execute(service => _func(service, editedValue));
        }
    }
}