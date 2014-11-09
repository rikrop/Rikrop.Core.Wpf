using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Rikrop.Core.Wpf.Mvvm.Navigation.Contracts;

namespace Rikrop.Core.Wpf.Mvvm.Navigation
{
    [ContractClass(typeof(ContractINavigator))]
    public interface INavigator
    {
        Task NavigateTo(IWorkspace workspace);

        Task StartNewSequenceFrom(IWorkspace workspace);
        Task CompleteCurrentSequence();

        Task BackToRoot();
    }

    namespace Contracts
    {
        [ContractClassFor(typeof(INavigator))]
        public abstract class ContractINavigator : INavigator
        {
            public Task NavigateTo(IWorkspace workspace)
            {
                Contract.Requires<ArgumentNullException>(workspace != null);

                Contract.Ensures(Contract.Result<Task>() != null);
                return default(Task);
            }

            public Task StartNewSequenceFrom(IWorkspace workspace)
            {
                Contract.Requires<ArgumentNullException>(workspace != null);

                Contract.Ensures(Contract.Result<Task>() != null);
                return default(Task);
            }

            public Task CompleteCurrentSequence()
            {
                Contract.Ensures(Contract.Result<Task>() != null);
                return default(Task);
            }

            public Task BackToRoot()
            {
                Contract.Ensures(Contract.Result<Task>() != null);
                return default(Task);
            }
        }
    }
}