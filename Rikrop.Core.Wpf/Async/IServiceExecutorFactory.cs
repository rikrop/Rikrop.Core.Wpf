using System;
using System.Diagnostics.Contracts;
using Rikrop.Core.Framework.Services;
using Rikrop.Core.Wpf.Async.Contracts;

namespace Rikrop.Core.Wpf.Async
{
    [ContractClass(typeof (ContractIServiceExecutorFactory<>))]
    public interface IServiceExecutorFactory<out TService>
    {
        IBusyServiceExecutor<TService> CreateBusyMultipleCall(IBusyTrigger busyTrigger);
        IBusyServiceExecutor<TService> CreateBusyMultipleCall();

        IBusyServiceExecutor<TService> CreateBusySingleCall(IBusyTrigger busyTrigger);
        IBusyServiceExecutor<TService> CreateBusySingleCall();

        IServiceExecutor<TService> CreateMultipleCall();
        IServiceExecutor<TService> CreateSingleCall();
        
        IPopupServiceExecutorBuilder<TService> GetPopupSingleCallBuilder();
    }

    namespace Contracts
    {
        [ContractClassFor(typeof (IServiceExecutorFactory<>))]
        public abstract class ContractIServiceExecutorFactory<TService> : IServiceExecutorFactory<TService>
        {
            public IBusyServiceExecutor<TService> CreateBusyMultipleCall(IBusyTrigger busyTrigger)
            {
                Contract.Requires<ArgumentNullException>(busyTrigger != null);
                Contract.Assume(Contract.Result<IBusyServiceExecutor<TService>>() != null);
                return default(IBusyServiceExecutor<TService>);
            }

            public IBusyServiceExecutor<TService> CreateBusyMultipleCall()
            {
                Contract.Assume(Contract.Result<IBusyServiceExecutor<TService>>() != null);
                return default(IBusyServiceExecutor<TService>);
            }

            public IServiceExecutor<TService> CreateMultipleCall()
            {
                Contract.Assume(Contract.Result<IServiceExecutor<TService>>() != null);
                return default(IServiceExecutor<TService>);
            }

            public IServiceExecutor<TService> CreateSingleCall()
            {
                Contract.Assume(Contract.Result<IServiceExecutor<TService>>() != null);
                return default(IServiceExecutor<TService>);
            }

            public IBusyServiceExecutor<TService> CreateBusySingleCall(IBusyTrigger busyTrigger)
            {
                Contract.Requires<ArgumentNullException>(busyTrigger != null);
                Contract.Assume(Contract.Result<IBusyServiceExecutor<TService>>() != null);
                return default(IBusyServiceExecutor<TService>);
            }

            public IBusyServiceExecutor<TService> CreateBusySingleCall()
            {
                Contract.Assume(Contract.Result<IBusyServiceExecutor<TService>>() != null);
                return default(IBusyServiceExecutor<TService>);
            }

            public IPopupServiceExecutorBuilder<TService> GetPopupSingleCallBuilder()
            {
                Contract.Assume(Contract.Result<IPopupServiceExecutorBuilder<TService>>() != null);
                return default(IPopupServiceExecutorBuilder<TService>);
            }
        }
    }
}