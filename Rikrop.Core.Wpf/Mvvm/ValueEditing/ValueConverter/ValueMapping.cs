using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Rikrop.Core.Wpf.Mvvm.ValueEditing.ValueConverter
{
    public class ValueMapping<TToValue, TFromValue>
    {
        private readonly TToValue _toValue;
        private readonly IReadOnlyCollection<TFromValue> _fromValues;

        public TToValue ToValue
        {
            get { return _toValue; }
        }

        public IReadOnlyCollection<TFromValue> FromValues
        {
            get { return _fromValues; }
        }

        public ValueMapping(TToValue toValue, TFromValue fromValue)
            : this(toValue, new[] {fromValue})
        {
        }

        public ValueMapping(TToValue toValue, IEnumerable<TFromValue> fromValues)
        {
            Contract.Requires<ArgumentNullException>(fromValues != null);
            _toValue = toValue;
            _fromValues = fromValues.ToList().AsReadOnly();
        }
    }
}