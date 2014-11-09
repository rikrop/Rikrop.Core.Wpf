using System.ComponentModel;
using System.Diagnostics.Contracts;
using Rikrop.Core.Wpf.Mvvm.Navigation.Contracts;

namespace Rikrop.Core.Wpf.Mvvm.Navigation
{
    [ContractClass(typeof(ContractINavigatorSource))]
    public interface INavigatorSource : INotifyPropertyChanged
    {
        IWorkspace Workspace { get; }
    }

    namespace Contracts
    {
        [ContractClassFor(typeof(INavigatorSource))]
        public abstract class ContractINavigatorSource : INavigatorSource
        {
            public abstract event PropertyChangedEventHandler PropertyChanged;
            public IWorkspace Workspace
            {
                get
                {
                    Contract.Assume(Contract.Result<IWorkspace>() != null);
                    
                    return default(IWorkspace);
                }
            }
        }
    }
}