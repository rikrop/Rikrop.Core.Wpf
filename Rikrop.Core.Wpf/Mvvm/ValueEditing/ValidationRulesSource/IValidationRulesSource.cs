using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;
using Rikrop.Core.Wpf.Mvvm.ValueEditing.ValidationRulesSource.Contracts;

namespace Rikrop.Core.Wpf.Mvvm.ValueEditing.ValidationRulesSource
{
    [ContractClass(typeof(ContractIValidationRulesSource<>))]
    public interface IValidationRulesSource<in TEditedValue>
    {
        IEnumerable<Func<TEditedValue, object>> GetSyncValidationRules();
        IEnumerable<Func<TEditedValue, CancellationToken, Task<object>>> GetAsyncValidationRules();
    }

    namespace Contracts
    {
        [ContractClassFor(typeof(IValidationRulesSource<>))]
        public abstract class ContractIValidationRulesSource<TEditedValue> : IValidationRulesSource<TEditedValue>
        {
            public IEnumerable<Func<TEditedValue, object>> GetSyncValidationRules()
            {
                Contract.Ensures(Contract.Result<IEnumerable<Func<TEditedValue, object>>>() != null);
                return default(IEnumerable<Func<TEditedValue, object>>);
            }

            public IEnumerable<Func<TEditedValue, CancellationToken, Task<object>>> GetAsyncValidationRules()
            {
                Contract.Ensures(Contract.Result<IEnumerable<Func<TEditedValue, CancellationToken, Task<object>>>>() != null);
                return default(IEnumerable<Func<TEditedValue, CancellationToken, Task<object>>>);
            }
        }
    }
}