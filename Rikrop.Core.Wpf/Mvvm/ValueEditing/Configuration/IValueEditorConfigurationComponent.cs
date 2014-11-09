using System;
using System.Diagnostics.Contracts;
using Rikrop.Core.Wpf.Mvvm.ValueEditing.Configuration.Contracts;

namespace Rikrop.Core.Wpf.Mvvm.ValueEditing.Configuration
{
    [ContractClass(typeof(ContractIValueEditorConfigurationComponent<,>))]
    public interface IValueEditorConfigurationComponent<TValue, TEditedValue>
    {
        void RegisterIn(IValueEditorBuilder<TValue, TEditedValue> editor);
    }

    namespace Contracts
    {
        [ContractClassFor(typeof (IValueEditorConfigurationComponent<,>))]
        public abstract class ContractIValueEditorConfigurationComponent<TValue, TEditedValue> : IValueEditorConfigurationComponent<TValue, TEditedValue>
        {
            public void RegisterIn(IValueEditorBuilder<TValue, TEditedValue> editor)
            {
                Contract.Requires<ArgumentNullException>(editor != null);
                return;
            }
        }
    }
}