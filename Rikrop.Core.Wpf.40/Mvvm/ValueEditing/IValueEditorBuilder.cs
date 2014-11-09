using System;
using System.Diagnostics.Contracts;
using Rikrop.Core.Wpf.Mvvm.ValueEditing.Configuration;
using Rikrop.Core.Wpf.Mvvm.ValueEditing.Contracts;
using Rikrop.Core.Wpf.Mvvm.ValueEditing.ValidationRulesSource;
using Rikrop.Core.Wpf.Mvvm.ValueEditing.ValueConverter;
using Rikrop.Core.Wpf.Mvvm.ValueEditing.ValueSaver;
using Rikrop.Core.Wpf.Mvvm.ValueEditing.ValueSource;

namespace Rikrop.Core.Wpf.Mvvm.ValueEditing
{
    [ContractClass(typeof (ContractIValueEditorBuilder<,>))]
    public interface IValueEditorBuilder<TValue, TEditedValue>
    {
        ValueEditor<TValue, TEditedValue> Create();
        IValueEditorBuilder<TValue, TEditedValue> WithConfiguration(IValueEditorConfiguration<TValue, TEditedValue> configuration);
        IValueEditorBuilder<TValue, TEditedValue> WithValueSource(IValueSource<TValue> valueSource);
        IValueEditorBuilder<TValue, TEditedValue> IgnorePendingChangesOnSourceChange();
        IValueEditorBuilder<TValue, TEditedValue> WithValueSaver(IAsyncValueSaver<TValue> valueSaver);
        IValueEditorBuilder<TValue, TEditedValue> WithValueConverter(IValueConverter<TValue, TEditedValue> valueConverter);
        IValueEditorBuilder<TValue, TEditedValue> WithValidationRuleSource(IValidationRulesSource<TEditedValue> validationRulesSource);
    }

    namespace Contracts
    {
        [ContractClassFor(typeof (IValueEditorBuilder<,>))]
        public abstract class ContractIValueEditorBuilder<TValue, TEditedValue> : IValueEditorBuilder<TValue, TEditedValue>
        {
            public ValueEditor<TValue, TEditedValue> Create()
            {
                Contract.Ensures(Contract.Result<ValueEditor<TValue, TEditedValue>>() != null);
                return default(ValueEditor<TValue, TEditedValue>);
            }

            public IValueEditorBuilder<TValue, TEditedValue> WithConfiguration(IValueEditorConfiguration<TValue, TEditedValue> configuration)
            {
                Contract.Requires<ArgumentNullException>(configuration != null);
                Contract.Ensures(Contract.Result<IValueEditorBuilder<TValue, TEditedValue>>() != null);
                return default(IValueEditorBuilder<TValue, TEditedValue>);
            }

            public IValueEditorBuilder<TValue, TEditedValue> WithValueSource(IValueSource<TValue> valueSource)
            {
                Contract.Requires<ArgumentNullException>(valueSource != null);
                Contract.Ensures(Contract.Result<IValueEditorBuilder<TValue, TEditedValue>>() != null);
                return default(IValueEditorBuilder<TValue, TEditedValue>);
            }

            public IValueEditorBuilder<TValue, TEditedValue> IgnorePendingChangesOnSourceChange()
            {
                Contract.Ensures(Contract.Result<IValueEditorBuilder<TValue, TEditedValue>>() != null);
                return default(IValueEditorBuilder<TValue, TEditedValue>);
            }

            public IValueEditorBuilder<TValue, TEditedValue> WithValueSaver(IAsyncValueSaver<TValue> valueSaver)
            {
                Contract.Requires<ArgumentNullException>(valueSaver != null);
                Contract.Ensures(Contract.Result<IValueEditorBuilder<TValue, TEditedValue>>() != null);
                return default(IValueEditorBuilder<TValue, TEditedValue>);
            }

            public IValueEditorBuilder<TValue, TEditedValue> WithValueConverter(IValueConverter<TValue, TEditedValue> valueConverter)
            {
                Contract.Requires<ArgumentNullException>(valueConverter != null);
                Contract.Ensures(Contract.Result<IValueEditorBuilder<TValue, TEditedValue>>() != null);
                return default(IValueEditorBuilder<TValue, TEditedValue>);
            }

            public IValueEditorBuilder<TValue, TEditedValue> WithValidationRuleSource(IValidationRulesSource<TEditedValue> validationRulesSource)
            {
                Contract.Requires<ArgumentNullException>(validationRulesSource != null);
                Contract.Ensures(Contract.Result<IValueEditorBuilder<TValue, TEditedValue>>() != null);
                return default(IValueEditorBuilder<TValue, TEditedValue>);
            }
        }
    }
}