using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Windows.Input;
using Rikrop.Core.Wpf.Mvvm.PopupManagement.Contracts;

namespace Rikrop.Core.Wpf.Mvvm.PopupManagement
{
    [ContractClass(typeof(ContractIPopupWorkspaceManager))]
    public interface IPopupWorkspaceManager : INotifyPropertyChanged
    {
        bool IsOpen { get; set; }
        IWorkspace Workspace { get; }
        ICommand HideWorkspaceCommand { get; }
    }

    namespace Contracts
    {
        [ContractClassFor(typeof(IPopupWorkspaceManager))]
        public abstract class ContractIPopupWorkspaceManager : IPopupWorkspaceManager
        {
            public abstract event PropertyChangedEventHandler PropertyChanged;
            public bool IsOpen { get; set; }
            public abstract IWorkspace Workspace { get; }
            public ICommand HideWorkspaceCommand
            {
                get
                {
                    Contract.Assume(Contract.Result<ICommand>() != null);
                    return default(ICommand);
                }
            }
        }
    }
}