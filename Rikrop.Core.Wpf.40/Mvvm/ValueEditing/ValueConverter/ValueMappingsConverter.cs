using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Rikrop.Core.Wpf.Mvvm.ValueEditing.ValueConverter
{
    public class ValueMappingsConverter<TValue, TEditedValue> : IValueConverter<TValue, TEditedValue>
    {
        private readonly Dictionary<TValue, TEditedValue> _valueMappings;

        private readonly Dictionary<TEditedValue, TValue> _editedMappings;
        private readonly bool _throwOnNoKey;

        public ValueMappingsConverter(
            IEnumerable<ValueMapping<TValue, TEditedValue>> valuesMappings,
            IEnumerable<ValueMapping<TEditedValue, TValue>> editedMappings)
            : this(valuesMappings, editedMappings, true)
        {
        }

        public ValueMappingsConverter(
            IEnumerable<ValueMapping<TValue, TEditedValue>> valuesMappings,
            IEnumerable<ValueMapping<TEditedValue, TValue>> editedMappings,
            bool throwOnNoKey)
        {
            Contract.Requires<ArgumentNullException>(valuesMappings != null);
            Contract.Requires<ArgumentNullException>(editedMappings != null);

            _valueMappings = new Dictionary<TValue, TEditedValue>();
            foreach (var mapping in editedMappings)
            {
                foreach (var from in mapping.FromValues)
                {
                    _valueMappings.Add(from, mapping.ToValue);
                }
            }

            _editedMappings = new Dictionary<TEditedValue, TValue>();
            foreach (var mapping in valuesMappings)
            {
                foreach (var from in mapping.FromValues)
                {
                    _editedMappings.Add(from, mapping.ToValue);
                }
            }

            _throwOnNoKey = throwOnNoKey;
        }

        public TValue ConvertToValue(TEditedValue editedValue)
        {
            TValue value;
            if (_editedMappings.TryGetValue(editedValue, out value))
            {
                return value;
            }
            if (_throwOnNoKey)
            {
                throw new InvalidOperationException(String.Format("Value {0} is not mapped", editedValue));
            }
            return default(TValue);
        }

        public TEditedValue ConvertToEditedValue(TValue value)
        {
            TEditedValue evalue;
            if (_valueMappings.TryGetValue(value, out evalue))
            {
                return evalue;
            }
            if (_throwOnNoKey)
            {
                throw new InvalidOperationException(String.Format("Value {0} is not mapped", value));
            }
            return default(TEditedValue);
        }
    }
}