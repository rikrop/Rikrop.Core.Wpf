using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Rikrop.Core.Wpf.Mvvm.Visualizer.Contracts;

namespace Rikrop.Core.Wpf.Mvvm.Visualizer
{
    [ContractClass(typeof(ContractIPopupVisualizer))]
    public interface IPopupVisualizer
    {
        Task Show(IWorkspace workspace);
    }

    namespace Contracts
    {
        [ContractClassFor(typeof(IPopupVisualizer))]
        public abstract class ContractIPopupVisualizer :IPopupVisualizer
        {
            public Task Show(IWorkspace workspace)
            {
                Contract.Requires<ArgumentNullException>(workspace != null);

                Contract.Assume(Contract.Result<Task>() != null);
                return default(Task);
            }
        }
    }
}