using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Rikrop.Core.Wpf.Mvvm.ValueEditing.Configuration.Contracts;

namespace Rikrop.Core.Wpf.Mvvm.ValueEditing.Configuration
{
    [ContractClass(typeof(ContractIValueEditorConfiguration<,>))]
    public interface IValueEditorConfiguration<TValue, TEditedValue>
    {
        IEnumerable<IValueEditorConfigurationComponent<TValue, TEditedValue>> GetConfigurationComponents();
    }

    namespace Contracts
    {
        [ContractClassFor(typeof(IValueEditorConfiguration<,>))]
        public abstract class ContractIValueEditorConfiguration<TValue, TEditedValue> :IValueEditorConfiguration<TValue, TEditedValue>
        {
            public IEnumerable<IValueEditorConfigurationComponent<TValue, TEditedValue>> GetConfigurationComponents()
            {
                Contract.Ensures(Contract.Result<IEnumerable<IValueEditorConfigurationComponent<TValue, TEditedValue>>>() != null);
                return default(IEnumerable<IValueEditorConfigurationComponent<TValue, TEditedValue>>);
            }
        }
    }
}