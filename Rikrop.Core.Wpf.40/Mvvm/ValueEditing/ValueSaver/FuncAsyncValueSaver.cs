using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace Rikrop.Core.Wpf.Mvvm.ValueEditing.ValueSaver
{
    public class FuncAsyncValueSaver<TEditedValue> : IAsyncValueSaver<TEditedValue>
    {
        private readonly Func<TEditedValue, Task> _func;

        public FuncAsyncValueSaver(Func<TEditedValue, Task> func)
        {
            Contract.Requires<ArgumentNullException>(func != null);
            _func = func;
        }

        public Task SaveValueAsync(TEditedValue editedValue)
        {
            return _func(editedValue);
        }
    }
}