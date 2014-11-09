using System.Diagnostics.Contracts;
using Rikrop.Core.Wpf.Mvvm.Visualizer.Contracts;

namespace Rikrop.Core.Wpf.Mvvm.Visualizer
{
    [ContractClass(typeof (ContractIPopupSource))]
    public interface IPopupSource
    {
        IWorkspace Workspace { get; }
        bool IsOpen { get; }
    }

    namespace Contracts
    {
        [ContractClassFor(typeof (IPopupSource))]
        public abstract class ContractIPopupSource : IPopupSource
        {
            public IWorkspace Workspace
            {
                get
                {
                    Contract.Ensures((Contract.Result<IWorkspace>() != null && IsOpen) || (Contract.Result<IWorkspace>() == null && !IsOpen));
                    return default(IWorkspace);
                }
            }

            public bool IsOpen
            {
                get
                {
                    Contract.Ensures((Contract.Result<bool>() && Workspace != null) || (!Contract.Result<bool>() && Workspace == null));
                    return default(bool);
                }
            }
        }
    }
}