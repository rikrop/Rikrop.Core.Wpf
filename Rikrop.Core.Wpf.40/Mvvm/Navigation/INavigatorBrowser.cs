using System;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using Rikrop.Core.Wpf.Mvvm.Navigation.Contracts;

namespace Rikrop.Core.Wpf.Mvvm.Navigation
{
    [ContractClass(typeof (ContractINavigatorBrowser))]
    public interface INavigatorBrowser
    {
        ReadOnlyObservableCollection<IWorkspace> Workspaces { get; }
        Task NavigateBack(IWorkspace workspace);
    }

    namespace Contracts
    {
        [ContractClassFor(typeof (INavigatorBrowser))]
        public abstract class ContractINavigatorBrowser : INavigatorBrowser
        {
            public ReadOnlyObservableCollection<IWorkspace> Workspaces
            {
                get
                {
                    Contract.Ensures(Contract.Result<ReadOnlyObservableCollection<IWorkspace>>() != null);
                    return default(ReadOnlyObservableCollection<IWorkspace>);
                }
            }

            public Task NavigateBack(IWorkspace workspace)
            {
                Contract.Requires<ArgumentNullException>(workspace != null);
                Contract.Requires<InvalidOperationException>(Workspaces.Any(w => w.Equals(workspace)));
                Contract.Ensures(Contract.Result<Task>() != null);
                return default(Task);
            }
        }
    }
}