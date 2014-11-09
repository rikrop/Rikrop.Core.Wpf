using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace Rikrop.Core.Wpf.Mvvm.ValueEditing.ValidationRulesSource
{
    public class ValidationRulesSourceBuider<TEditedValue>
    {
        private readonly List<Func<TEditedValue, object>> _syncValidationRules;
        private readonly List<Func<TEditedValue, CancellationToken, Task<object>>> _asyncValidationRules;

        public ValidationRulesSourceBuider()
        {
            _syncValidationRules = new List<Func<TEditedValue, object>>();
            _asyncValidationRules = new List<Func<TEditedValue, CancellationToken, Task<object>>>();
        }

        public ValidationRulesSourceBuider<TEditedValue> AddSyncRule(Func<TEditedValue, object> rule)
        {
            Contract.Requires<ArgumentNullException>(rule != null);
            _syncValidationRules.Add(rule);
            return this;
        }

        public ValidationRulesSourceBuider<TEditedValue> AddAsyncRule(Func<TEditedValue, CancellationToken, Task<object>> rule)
        {
            Contract.Requires<ArgumentNullException>(rule != null);
            _asyncValidationRules.Add(rule);
            return this;
        }

        public IValidationRulesSource<TEditedValue> CreateSource()
        {
            return new FuncValidationRulesSource(_syncValidationRules, _asyncValidationRules);
        }


        private class FuncValidationRulesSource : IValidationRulesSource<TEditedValue>
        {
            private readonly IEnumerable<Func<TEditedValue, object>> _rules;
            private readonly IEnumerable<Func<TEditedValue, CancellationToken, Task<object>>> _asyncRules;

            public FuncValidationRulesSource(
                IEnumerable<Func<TEditedValue, object>> rules,
                IEnumerable<Func<TEditedValue, CancellationToken, Task<object>>> asyncRules)
            {
                Contract.Requires<ArgumentNullException>(rules != null);
                Contract.Requires<ArgumentNullException>(asyncRules != null);

                _rules = rules;
                _asyncRules = asyncRules;
            }

            public IEnumerable<Func<TEditedValue, object>> GetSyncValidationRules()
            {
                return _rules;
            }

            public IEnumerable<Func<TEditedValue, CancellationToken, Task<object>>> GetAsyncValidationRules()
            {
                return _asyncRules;
            }
        }
    }
}