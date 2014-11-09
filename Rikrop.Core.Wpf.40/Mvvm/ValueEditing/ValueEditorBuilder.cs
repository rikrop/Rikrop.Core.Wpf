using Rikrop.Core.Wpf.Mvvm.ValueEditing.Configuration;
using Rikrop.Core.Wpf.Mvvm.ValueEditing.ValidationRulesSource;
using Rikrop.Core.Wpf.Mvvm.ValueEditing.ValueConverter;
using Rikrop.Core.Wpf.Mvvm.ValueEditing.ValueSaver;
using Rikrop.Core.Wpf.Mvvm.ValueEditing.ValueSource;

namespace Rikrop.Core.Wpf.Mvvm.ValueEditing
{
    public class ValueEditorBuilder<TValue, TEditedValue> : IValueEditorBuilder<TValue, TEditedValue>
    {
        private IValueSource<TValue> _valueSource;
        private IValidationRulesSource<TEditedValue> _validationRulesSource;
        private IValueConverter<TValue, TEditedValue> _valueConverter;

        private IValueSource<TValue> ValueSource
        {
            get { return _valueSource ?? (_valueSource = new SelfValueSource<TValue>(default(TValue))); }
            set { _valueSource = value; }
        }

        private IAsyncValueSaver<TValue> AsyncValueSaver { get; set; }

        private IValidationRulesSource<TEditedValue> ValidationRulesSource
        {
            get { return _validationRulesSource ?? (_validationRulesSource = new ValidationRulesSourceBuider<TEditedValue>().CreateSource()); }
            set { _validationRulesSource = value; }
        }

        private IValueConverter<TValue, TEditedValue> ValueConverter
        {
            get { return _valueConverter ?? (_valueConverter = new CastValueConverter<TValue, TEditedValue>()); }
            set { _valueConverter = value; }
        }

        private bool _ignorePendingChangesOnSourceChange;

        public ValueEditor<TValue, TEditedValue> Create()
        {
            return new ValueEditor<TValue, TEditedValue>(ValueConverter, ValidationRulesSource, AsyncValueSaver, ValueSource, _ignorePendingChangesOnSourceChange);
        }

        public IValueEditorBuilder<TValue, TEditedValue> WithConfiguration(IValueEditorConfiguration<TValue, TEditedValue> configuration)
        {
            foreach (var component in configuration.GetConfigurationComponents())
            {
                component.RegisterIn(this);
            }

            return this;
        }

        public IValueEditorBuilder<TValue, TEditedValue> IgnorePendingChangesOnSourceChange()
        {
            _ignorePendingChangesOnSourceChange = true;
            return this;
        }

        public IValueEditorBuilder<TValue, TEditedValue> WithValueSource(IValueSource<TValue> valueSource)
        {
            ValueSource = valueSource;
            return this;
        }

        public IValueEditorBuilder<TValue, TEditedValue> WithValueSaver(IAsyncValueSaver<TValue> valueSaver)
        {
            AsyncValueSaver = valueSaver;
            return this;
        }

        public IValueEditorBuilder<TValue, TEditedValue> WithValueConverter(IValueConverter<TValue, TEditedValue> valueConverter)
        {
            ValueConverter = valueConverter;
            return this;
        }

        public IValueEditorBuilder<TValue, TEditedValue> WithValidationRuleSource(IValidationRulesSource<TEditedValue> validationRulesSource)
        {
            ValidationRulesSource = validationRulesSource;
            return this;
        }
    }
}