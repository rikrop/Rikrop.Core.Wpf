using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Rikrop.Core.Wpf.Mvvm.ValueEditing.ValueSource;

namespace Rikrop.Core.Wpf.Mvvm.ValueEditing.ValueSaver
{
    public class FilterNotChangedValueSaver<TEditedValue> : IAsyncValueSaver<TEditedValue>
    {
        private readonly IAsyncValueSaver<TEditedValue> _saver;
        private readonly IValueSource<TEditedValue> _valueSource;

        public FilterNotChangedValueSaver(IAsyncValueSaver<TEditedValue> saver, IValueSource<TEditedValue> valueSource)
        {
            Contract.Requires<ArgumentNullException>(saver != null);
            Contract.Requires<ArgumentNullException>(valueSource != null);

            _saver = saver;
            _valueSource = valueSource;
        }

        public async Task SaveValueAsync(TEditedValue editedValue)
        {
            if (Equals(editedValue, _valueSource.Value))
            {
                return;
            }

            await _saver.SaveValueAsync(editedValue);
        }
    }
}