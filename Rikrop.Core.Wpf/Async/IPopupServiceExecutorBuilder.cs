using System;
using System.Diagnostics.Contracts;
using Rikrop.Core.Wpf.Async.Contracts;
using Rikrop.Core.Wpf.Mvvm;
using Rikrop.Core.Wpf.Mvvm.Visualizer;

namespace Rikrop.Core.Wpf.Async
{
    [ContractClass(typeof (ContractIPopupServiceExecutorBuilder<>))]
    public interface IPopupServiceExecutorBuilder<out TService>
    {
        IBusyServiceExecutor<TService> Create(IPopupVisualizer popupVisualizer);

        IPopupServiceExecutorBuilder<TService> WithWorkspace(IWorkspace workspace);
        IPopupServiceExecutorBuilder<TService> WithTitle(string title);
        IPopupServiceExecutorBuilder<TService> WithDescription(string description);
        IPopupServiceExecutorBuilder<TService> WithDisplayTimeout(TimeSpan displayTimeout);
    }

    namespace Contracts
    {
        [ContractClassFor(typeof(IPopupServiceExecutorBuilder<>))]
        public abstract class ContractIPopupServiceExecutorBuilder<TService> : IPopupServiceExecutorBuilder<TService>
        {
            public IBusyServiceExecutor<TService> Create(IPopupVisualizer popupVisualizer)
            {
                Contract.Requires<ArgumentNullException>(popupVisualizer != null);
                Contract.Assume(Contract.Result<IBusyServiceExecutor<TService>>() != null);
                return default(IBusyServiceExecutor<TService>);
            }

            public IPopupServiceExecutorBuilder<TService> WithWorkspace(IWorkspace workspace)
            {
                Contract.Requires<ArgumentNullException>(workspace != null);
                Contract.Assume(Contract.Result<IPopupServiceExecutorBuilder<TService>>() != null);
                return default(IPopupServiceExecutorBuilder<TService>);
            }

            public IPopupServiceExecutorBuilder<TService> WithTitle(string title)
            {
                Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(title));
                Contract.Assume(Contract.Result<IPopupServiceExecutorBuilder<TService>>() != null);
                return default(IPopupServiceExecutorBuilder<TService>);
            }

            public IPopupServiceExecutorBuilder<TService> WithDescription(string description)
            {
                Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(description));
                Contract.Assume(Contract.Result<IPopupServiceExecutorBuilder<TService>>() != null);
                return default(IPopupServiceExecutorBuilder<TService>);
            }

            public IPopupServiceExecutorBuilder<TService> WithDisplayTimeout(TimeSpan displayTimeout)
            {
                Contract.Assume(Contract.Result<IPopupServiceExecutorBuilder<TService>>() != null);
                return default(IPopupServiceExecutorBuilder<TService>);
            }
        }
    }
}