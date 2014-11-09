using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Rikrop.Core.Wpf.Mvvm.Validation;
using Rikrop.Core.Wpf.Mvvm.ValueEditing.ValidationRulesSource;
using Rikrop.Core.Wpf.Mvvm.ValueEditing.ValueConverter;
using Rikrop.Core.Wpf.Mvvm.ValueEditing.ValueSaver;
using Rikrop.Core.Wpf.Mvvm.ValueEditing.ValueSource;

namespace Rikrop.Core.Wpf.Mvvm.ValueEditing
{
    public class ValueEditor<TValue, TEditedValue> : DataValidationInfo, IValueEditor<TValue, TEditedValue>
    {
        private readonly IValueConverter<TValue, TEditedValue> _valueConverter;
        private readonly IAsyncValueSaver<TValue> _asyncValueSaver;
        private readonly IValueSource<TValue> _valueSource;
        private readonly bool _ignorePendingChangesOnSourceChange;

        private TEditedValue _editValue;
        private bool _hasPendingChanges;
        public event Action<ValueEditorCancellationType> Cancelled;

        public TEditedValue EditValue
        {
            get { return _editValue; }
            set { SetProperty(ref _editValue, value); }
        }

        public ValueEditor(IValueConverter<TValue, TEditedValue> valueConverter,
                           IValidationRulesSource<TEditedValue> validationRulesSource,
                           IAsyncValueSaver<TValue> asyncValueSaver,
                           IValueSource<TValue> valueSource,
                           bool ignorePendingChangesOnSourceChange)
        {
            Contract.Requires<ArgumentNullException>(valueConverter != null);
            Contract.Requires<ArgumentNullException>(validationRulesSource != null);
            Contract.Requires<ArgumentNullException>(valueSource != null);

            _valueConverter = valueConverter;
            _asyncValueSaver = asyncValueSaver;
            _valueSource = valueSource;
            _ignorePendingChangesOnSourceChange = ignorePendingChangesOnSourceChange;

            UpdateEditValueFromSource();

            AddValidationRules(validationRulesSource);

            AfterNotify(valueSource, source => source.Value)
                .Execute(OnSourceValueChanged);

            AfterNotify(() => EditValue)
                .Execute(() => _hasPendingChanges = true);
        }

        public TValue GetValue()
        {
            return _valueSource.Value;
        }


        /// <summary>
        /// До окончания вызова EndEditAsync нельзя редактировать EditValue
        /// </summary>
        public async Task EndEditAsync()
        {
            if (HasErrors)
            {
                CancelEdit();
                _hasPendingChanges = false;
                RaiseCancelled(ValueEditorCancellationType.OnSaveValidationRollback);
                return;
            }

            try
            {
                var newValue = GetNewValue();
                if (_asyncValueSaver != null)
                {
                    await _asyncValueSaver.SaveValueAsync(newValue);
                }
                _valueSource.Value = newValue;
            }
            catch
            {
                CancelEdit();
                RaiseCancelled(ValueEditorCancellationType.OnSaveError);
                throw;
            }
            finally
            {
                _hasPendingChanges = false;
            }
        }

        public void CancelEdit()
        {
            UpdateEditValueFromSource();
            RaiseCancelled(ValueEditorCancellationType.Manual);
        }

        private void OnSourceValueChanged()
        {
            if (_ignorePendingChangesOnSourceChange || !_hasPendingChanges)
            {
                UpdateEditValueFromSource();
            }
        }

        private void RaiseCancelled(ValueEditorCancellationType type)
        {
            var handler = Cancelled;
            if (handler != null)
            {
                handler(type);
            }
        }

        private void AddValidationRules(IValidationRulesSource<TEditedValue> validationRulesSource)
        {
            foreach (var rule in validationRulesSource.GetSyncValidationRules())
            {
                var vrule = rule;
                ForProperty(() => EditValue).AddValidationRule(() => vrule(EditValue));
            }

            foreach (var asyncRule in validationRulesSource.GetAsyncValidationRules())
            {
                var vasyncRule = asyncRule;
                ForProperty(() => EditValue).AddAsyncValidationRule(async ct => await vasyncRule(EditValue, ct));
            }
        }

        private void UpdateEditValueFromSource()
        {
            EditValue = _valueConverter.ConvertToEditedValue(_valueSource.Value);
        }

        private TValue GetNewValue()
        {
            return _valueConverter.ConvertToValue(EditValue);
        }
    }
}