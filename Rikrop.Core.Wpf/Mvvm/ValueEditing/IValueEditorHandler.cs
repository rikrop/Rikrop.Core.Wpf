using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Rikrop.Core.Wpf.Mvvm.ValueEditing.Contracts;

namespace Rikrop.Core.Wpf.Mvvm.ValueEditing
{
    [ContractClass(typeof (ContractIValueEditorHandler))]
    public interface IValueEditorHandler : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        event Action<ValueEditorCancellationType> Cancelled;
        Task EndEditAsync();
        void CancelEdit();
    }

    namespace Contracts
    {
        [ContractClassFor(typeof (IValueEditorHandler))]
        public abstract class ContractIValueEditorHandler : IValueEditorHandler
        {
            public abstract event PropertyChangedEventHandler PropertyChanged;
            public abstract event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
            public abstract event Action<ValueEditorCancellationType> Cancelled;
            public abstract bool HasErrors { get; }
            public abstract IEnumerable GetErrors(string propertyName);

            public Task EndEditAsync()
            {
                Contract.Ensures(Contract.Result<Task>() != null);
                return default(Task);
            }

            public abstract void CancelEdit();
        }
    }
}