using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Rikrop.Core.Framework;

namespace Rikrop.Core.Wpf.Mvvm.Validation
{
    public abstract class DataValidationInfo : ChangeNotifier, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, Validator> _propertyValidators;
        private readonly Validator _objectValidator;

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                return _objectValidator.GetErrors();
            }
            Validator validator;
            return _propertyValidators.TryGetValue(propertyName, out validator)
                       ? validator.GetErrors()
                       : null;
        }

        private bool _hasErrors;
        public bool HasErrors
        {
            get { return _hasErrors; }
            private set
            {
                if (_hasErrors == value)
                {
                    return;
                }
                _hasErrors = value;
                NotifyPropertyChanged(() => HasErrors);
            }
        }

        private void UpdateHasErrors()
        {
            HasErrors = _propertyValidators.Values.Any(pv => pv.HasErrors) || _objectValidator.HasErrors;
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public void RaiseErrorsChanged(string propertyName)
        {
            var handler = ErrorsChanged;
            if (handler == null)
            {
                return;
            }
            var args = new DataErrorsChangedEventArgs(propertyName);
            handler(this, args);
        }

        protected DataValidationInfo()
        {
            _propertyValidators = new Dictionary<string, Validator>();
            _objectValidator = CreateNewValidator(null);
        }

        protected IValidationRuleAdder ForProperty(Expression<Func<object>> property)
        {
            Contract.Requires<ArgumentNullException>(property != null);

            AfterNotify(property)
                .Execute(() => Validate(property));

            return new ValidationRuleAdder(this, property);
        }

        protected IObjectValidationRuleAdder ForObject()
        {
            return new ObjectValidationRuleAdder(this);
        }

        protected void Validate(Expression<Func<object>> property)
        {
            Contract.Requires<ArgumentNullException>(property != null);

            var propertyName = property.GetName();
            Validate(propertyName);
        }

        protected void Validate()
        {
            _objectValidator.BeginValidation();
        }

        private void Validate(string propertyName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(propertyName));
            Validator validator;
            if (!_propertyValidators.TryGetValue(propertyName, out validator))
            {
                return;
            }
            validator.BeginValidation();
        }

        private void AddRule(string propertyName, Func<object> rule)
        {
            var validator = GetOrCreatePropertyValidator(propertyName);
            validator.AddRule(rule);
            validator.BeginValidation();
        }

        private void AddRule(string propertyName, Func<CancellationToken, Task<object>> rule)
        {
            var validator = GetOrCreatePropertyValidator(propertyName);
            validator.AddRule(rule);
            validator.BeginValidation();
        }

        private void AddObjectRule(Func<object> rule)
        {
            _objectValidator.AddRule(rule);
            _objectValidator.BeginValidation();
        }

        private void AddObjectRule(Func<CancellationToken, Task<object>> rule)
        {
            _objectValidator.AddRule(rule);
            _objectValidator.BeginValidation();
        }

        private void AlsoValidate(Expression<Func<object>> forProperty, Expression<Func<object>> alsoValidateProperty)
        {
            AfterNotify(forProperty)
                .Execute(() => Validate(alsoValidateProperty));
        }

        private Validator GetOrCreatePropertyValidator(string propertyName)
        {
            Contract.Requires<ArgumentNullException>(propertyName != null);

            Validator validator;
            if (!_propertyValidators.TryGetValue(propertyName, out validator))
            {
                validator = CreateNewValidator(propertyName);
                _propertyValidators.Add(propertyName, validator);
            }
            return validator;
        }

        private Validator CreateNewValidator(string propertyName)
        {
            var validator = new Validator();
            validator.ValidationChanged += () => RaisePropertyErrorChanged(propertyName);
            return validator;
        }

        private void RaisePropertyErrorChanged(string propertyName)
        {
            UpdateHasErrors();
            RaiseErrorsChanged(propertyName);
        }

        private class ValidationRuleAdder : IValidationRuleAdder
        {
            private readonly DataValidationInfo _dataValidationInfo;
            private readonly Expression<Func<object>> _property;
            private readonly string _propertyName;

            public ValidationRuleAdder(DataValidationInfo dataValidationInfo, Expression<Func<object>> property)
            {
                Contract.Requires<ArgumentNullException>(dataValidationInfo != null);
                Contract.Requires<ArgumentNullException>(property != null);

                _dataValidationInfo = dataValidationInfo;
                _property = property;

                _propertyName = _property.GetName();
                Contract.Assume(!string.IsNullOrWhiteSpace(_propertyName));
            }

            public IValidationRuleAdder AddValidationRule(Func<object> rule)
            {
                _dataValidationInfo.AddRule(_propertyName, rule);
                return this;
            }

            public IValidationRuleAdder AddAsyncValidationRule(Func<CancellationToken, Task<object>> asyncRule)
            {
                _dataValidationInfo.AddRule(_propertyName, asyncRule);
                return this;
            }

            public IValidationRuleAdder AlsoValidate(Expression<Func<object>> propertyName)
            {
                _dataValidationInfo.AlsoValidate(_property, propertyName);
                return this;
            }
        }

        private class ObjectValidationRuleAdder : IObjectValidationRuleAdder
        {
            private readonly DataValidationInfo _dataValidationInfo;

            public ObjectValidationRuleAdder(DataValidationInfo dataValidationInfo)
            {
                Contract.Requires<ArgumentNullException>(dataValidationInfo != null);
                _dataValidationInfo = dataValidationInfo;
            }

            public IObjectValidationRuleAdder AddValidationRule(Func<object> rule)
            {
                _dataValidationInfo.AddObjectRule(rule);
                return this;
            }

            public IObjectValidationRuleAdder AddAsyncValidationRule(Func<CancellationToken, Task<object>> asyncRule)
            {
                _dataValidationInfo.AddObjectRule(asyncRule);
                return this;
            }
        }

        [ContractClass(typeof(ContractValidationRuleAdder))]
        protected interface IValidationRuleAdder
        {
            IValidationRuleAdder AddValidationRule(Func<object> rule);
            IValidationRuleAdder AddAsyncValidationRule(Func<CancellationToken, Task<object>> asyncRule);
            IValidationRuleAdder AlsoValidate(Expression<Func<object>> propertyName);
        }

        [ContractClassFor(typeof(IValidationRuleAdder))]
        protected abstract class ContractValidationRuleAdder : IValidationRuleAdder
        {
            public IValidationRuleAdder AddValidationRule(Func<object> rule)
            {
                Contract.Requires<ArgumentNullException>(rule != null);

                Contract.Ensures(Contract.Result<IValidationRuleAdder>() != null);
                return default(IValidationRuleAdder);
            }

            public IValidationRuleAdder AddAsyncValidationRule(Func<CancellationToken, Task<object>> asyncRule)
            {
                Contract.Requires<ArgumentNullException>(asyncRule != null);

                Contract.Ensures(Contract.Result<IValidationRuleAdder>() != null);
                return default(IValidationRuleAdder);
            }

            public IValidationRuleAdder AlsoValidate(Expression<Func<object>> propertyName)
            {
                Contract.Requires<ArgumentNullException>(propertyName != null);

                Contract.Ensures(Contract.Result<IValidationRuleAdder>() != null);
                return default(IValidationRuleAdder);
            }
        }

        [ContractClass(typeof(ContractObjectValidationRuleAdder))]
        protected interface IObjectValidationRuleAdder
        {
            IObjectValidationRuleAdder AddValidationRule(Func<object> rule);
            IObjectValidationRuleAdder AddAsyncValidationRule(Func<CancellationToken, Task<object>> asyncRule);
        }

        [ContractClassFor(typeof(IObjectValidationRuleAdder))]
        protected abstract class ContractObjectValidationRuleAdder : IObjectValidationRuleAdder
        {
            public IObjectValidationRuleAdder AddValidationRule(Func<object> rule)
            {
                Contract.Requires<ArgumentNullException>(rule != null);

                Contract.Ensures(Contract.Result<IObjectValidationRuleAdder>() != null);
                return default(IObjectValidationRuleAdder);
            }

            public IObjectValidationRuleAdder AddAsyncValidationRule(Func<CancellationToken, Task<object>> asyncRule)
            {
                Contract.Requires<ArgumentNullException>(asyncRule != null);

                Contract.Ensures(Contract.Result<IObjectValidationRuleAdder>() != null);
                return default(IObjectValidationRuleAdder);
            }
        }

        private class Validator
        {
            private readonly List<Func<object>> _validateRules;
            private readonly List<Func<CancellationToken, Task<object>>> _asyncValidateRules;
            private CancellationTokenSource _cts;

            public event Action ValidationChanged;

            private void RaiseValidationChanged()
            {
                var handler = ValidationChanged;
                if (handler != null)
                {
                    handler();
                }
            }

            private void Cancel()
            {
                _cts.Cancel();
                _cts = new CancellationTokenSource();
            }

            private readonly List<object> _errors;

            public void AddRule(Func<object> rule)
            {
                Contract.Requires<ArgumentNullException>(rule != null);
                _validateRules.Add(rule);
            }

            public void AddRule(Func<CancellationToken, Task<object>> rule)
            {
                Contract.Requires<ArgumentNullException>(rule != null);
                _asyncValidateRules.Add(rule);
            }

            public bool HasErrors
            {
                get { return _errors.Any(); }
            }

            public IEnumerable GetErrors()
            {
                return _errors.Any() ? _errors : null;
            }

            public async void BeginValidation()
            {
                Cancel();

                _errors.Clear();
                ValidateSyncRules();
                RaiseValidationChanged();

                try
                {
                    await ValidateAsyncRules(_cts.Token);
                }
                catch (OperationCanceledException)
                {
                }
            }

            private async Task ValidateAsyncRules(CancellationToken ct)
            {
                foreach (var rule in _asyncValidateRules)
                {
                    var validationResult = await rule(ct);

                    ct.ThrowIfCancellationRequested();

                    if (validationResult != null)
                    {
                        _errors.Add(validationResult);
                        RaiseValidationChanged();
                    }
                }
            }

            private void ValidateSyncRules()
            {
                foreach (var rule in _validateRules)
                {
                    var validationResult = rule();
                    if (validationResult != null)
                    {
                        _errors.Add(validationResult);
                    }
                }
            }

            public Validator()
            {
                _validateRules = new List<Func<object>>();
                _asyncValidateRules = new List<Func<CancellationToken, Task<object>>>();
                _errors = new List<object>();
                _cts = new CancellationTokenSource();
            }
        }
    }
}