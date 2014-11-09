using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Rikrop.Core.Framework.Services;
using Rikrop.Core.Wpf.Mvvm;
using Rikrop.Core.Wpf.Mvvm.Visualizer;

namespace Rikrop.Core.Wpf.Async
{
    public class ServiceExecutorFactory<TService> : IServiceExecutorFactory<TService>
    {
        private readonly Func<IServiceExecutor<TService>> _serviceExecutorCreator;

        public ServiceExecutorFactory(Func<IServiceExecutor<TService>> serviceExecutorCreator)
        {
            Contract.Requires<ArgumentNullException>(serviceExecutorCreator != null);
            _serviceExecutorCreator = serviceExecutorCreator;
        }

        public IBusyServiceExecutor<TService> CreateBusyMultipleCall(IBusyTrigger busyTrigger)
        {
            return new MultipleCallBusyServiceExecutor(_serviceExecutorCreator, busyTrigger);
        }

        public IBusyServiceExecutor<TService> CreateBusyMultipleCall()
        {
            return new MultipleCallBusyServiceExecutor(_serviceExecutorCreator);
        }

        public IBusyServiceExecutor<TService> CreateBusySingleCall(IBusyTrigger busyTrigger)
        {
            return new SingleCallBusyServiceExecutor(_serviceExecutorCreator, busyTrigger);
        }

        public IBusyServiceExecutor<TService> CreateBusySingleCall()
        {
            return new SingleCallBusyServiceExecutor(_serviceExecutorCreator);
        }

        public IServiceExecutor<TService> CreateMultipleCall()
        {
            return _serviceExecutorCreator();
        }

        public IServiceExecutor<TService> CreateSingleCall()
        {
            return new SingleCallServiceExecutor(_serviceExecutorCreator);
        }

        public IPopupServiceExecutorBuilder<TService> GetPopupSingleCallBuilder()
        {
            return new SingleCallBusyServiceExecutorWithPopup.SingleCallServiceExecutorWithPopupBuilder(_serviceExecutorCreator);
        }

        private abstract class BusyServiceExecutorBase : ChangeNotifier, IBusyServiceExecutor<TService>, IBusyTrigger
        {
            private bool _isBusy;

            public bool IsBusy
            {
                get { return _isBusy; }
                private set { SetProperty(ref _isBusy, value); }
            }

            public abstract Task Execute(Func<TService, Task> action);

            public abstract Task<TResult> Execute<TResult>(Func<TService, Task<TResult>> func);

            void IBusyTrigger.SetBusy()
            {
                IsBusy = true;
            }

            void IBusyTrigger.ClearBusy()
            {
                IsBusy = false;
            }
        }

        private class MultipleCallBusyServiceExecutor : BusyServiceExecutorBase
        {
            private readonly Func<IServiceExecutor<TService>> _serviceExecutorCreator;
            private readonly TaskExecutionNotifier _taskExecutionNotifier;

            public MultipleCallBusyServiceExecutor(Func<IServiceExecutor<TService>> serviceExecutorCreator, IBusyTrigger trigger)
            {
                Contract.Requires<ArgumentNullException>(serviceExecutorCreator != null);
                Contract.Requires<ArgumentNullException>(trigger != null);

                _serviceExecutorCreator = serviceExecutorCreator;
                _taskExecutionNotifier = new TaskExecutionNotifier(new CompositeBusyTrigger(new[] {trigger, this}));
            }

            public MultipleCallBusyServiceExecutor(Func<IServiceExecutor<TService>> serviceExecutorCreator)
            {
                Contract.Requires<ArgumentNullException>(serviceExecutorCreator != null);

                _serviceExecutorCreator = serviceExecutorCreator;
                _taskExecutionNotifier = new TaskExecutionNotifier(this);
            }

            public override Task Execute(Func<TService, Task> action)
            {
                return _taskExecutionNotifier.TrackExecution(() => _serviceExecutorCreator().Execute(action));
            }

            public override Task<TResult> Execute<TResult>(Func<TService, Task<TResult>> func)
            {
                return _taskExecutionNotifier.TrackExecution(() => _serviceExecutorCreator().Execute(func));
            }
        }

        private class SingleCallBusyServiceExecutor : BusyServiceExecutorBase
        {
            private readonly Func<IServiceExecutor<TService>> _serviceExecutorCreator;
            private readonly TaskExecutionNotifier _taskExecutionNotifier;
            private readonly LastCallResultAwaiter _lastCallResultAwaiter;

            public SingleCallBusyServiceExecutor(Func<IServiceExecutor<TService>> serviceExecutorCreator, IBusyTrigger trigger)
            {
                Contract.Requires<ArgumentNullException>(serviceExecutorCreator != null);
                Contract.Requires<ArgumentNullException>(trigger != null);

                _serviceExecutorCreator = serviceExecutorCreator;
                _taskExecutionNotifier = new TaskExecutionNotifier(new CompositeBusyTrigger(new[] {trigger, this}));
                _lastCallResultAwaiter = new LastCallResultAwaiter();
            }

            public SingleCallBusyServiceExecutor(Func<IServiceExecutor<TService>> serviceExecutorCreator)
            {
                Contract.Requires<ArgumentNullException>(serviceExecutorCreator != null);

                _serviceExecutorCreator = serviceExecutorCreator;
                _taskExecutionNotifier = new TaskExecutionNotifier(this);
                _lastCallResultAwaiter = new LastCallResultAwaiter();
            }

            public override Task Execute(Func<TService, Task> action)
            {
                return _taskExecutionNotifier.TrackExecution(() => _lastCallResultAwaiter.Await(() => _serviceExecutorCreator().Execute(action)));
            }

            public override Task<TResult> Execute<TResult>(Func<TService, Task<TResult>> func)
            {
                return _taskExecutionNotifier.TrackExecution(() => _lastCallResultAwaiter.Await(() => _serviceExecutorCreator().Execute(func)));
            }
        }

        private class SingleCallBusyServiceExecutorWithPopup : BusyServiceExecutorBase
        {
            private readonly Func<IServiceExecutor<TService>> _serviceExecutorCreator;
            private readonly PopupTaskExecutionNotifier _popupTaskExecutionNotifier;

            private SingleCallBusyServiceExecutorWithPopup(Func<IServiceExecutor<TService>> serviceExecutorCreator, IPopupVisualizer popupVisualizer, IWorkspace workspace, TimeSpan displayTimeout)
            {
                Contract.Requires<ArgumentNullException>(serviceExecutorCreator != null);
                Contract.Requires<ArgumentNullException>(popupVisualizer != null);
                Contract.Requires<ArgumentNullException>(workspace != null);

                _serviceExecutorCreator = serviceExecutorCreator;

                _popupTaskExecutionNotifier = new PopupTaskExecutionNotifier(popupVisualizer, this, workspace, displayTimeout);
            }

            public override Task Execute(Func<TService, Task> action)
            {
                return _popupTaskExecutionNotifier.TrackExecution(() => _serviceExecutorCreator().Execute(action));
            }

            public override Task<TResult> Execute<TResult>(Func<TService, Task<TResult>> func)
            {
                return _popupTaskExecutionNotifier.TrackExecution(() => _serviceExecutorCreator().Execute(func));
            }

            public class SingleCallServiceExecutorWithPopupBuilder : IPopupServiceExecutorBuilder<TService>
            {
                private const int DefaultDisplayTimeout = 300;

                private readonly Func<IServiceExecutor<TService>> _serviceExecutorCreator;

                private TimeSpan? _delayTime;
                private IWorkspace _workspace;
                private string Title { get; set; }
                private string Description { get; set; }


                private TimeSpan DisplayTimeout
                {
                    get { return (_delayTime ?? (_delayTime = TimeSpan.FromMilliseconds(DefaultDisplayTimeout))).Value; }
                    set { _delayTime = value; }
                }

                private IWorkspace Workspace
                {
                    get { return _workspace ?? (_workspace = GetDefaultModel()); }
                    set { _workspace = value; }
                }

                public SingleCallServiceExecutorWithPopupBuilder(Func<IServiceExecutor<TService>> serviceExecutorCreator)
                {
                    _serviceExecutorCreator = serviceExecutorCreator;
                }

                public IBusyServiceExecutor<TService> Create(IPopupVisualizer popupVisualizer)
                {
                    return new SingleCallBusyServiceExecutorWithPopup(_serviceExecutorCreator, popupVisualizer, Workspace, DisplayTimeout);
                }

                public IPopupServiceExecutorBuilder<TService> WithWorkspace(IWorkspace workspace)
                {
                    Workspace = workspace;
                    return this;
                }

                public IPopupServiceExecutorBuilder<TService> WithTitle(string title)
                {
                    Title = title;
                    Workspace = null;
                    return this;
                }

                public IPopupServiceExecutorBuilder<TService> WithDescription(string description)
                {
                    Description = description;
                    Workspace = null;
                    return this;
                }

                public IPopupServiceExecutorBuilder<TService> WithDisplayTimeout(TimeSpan displayTimeout)
                {
                    DisplayTimeout = displayTimeout;
                    return this;
                }

                private IWorkspace GetDefaultModel()
                {
                    var vm = new BusyPopupWorkspace();
                    if (!string.IsNullOrWhiteSpace(Title))
                    {
                        vm.Title = Title;
                    }

                    if (!string.IsNullOrWhiteSpace(Description))
                    {
                        vm.Description = Description;
                    }

                    return vm;
                }
            }
        }

        private class SingleCallServiceExecutor : IServiceExecutor<TService>
        {
            private readonly Func<IServiceExecutor<TService>> _serviceExecutorCreator;
            private readonly LastCallResultAwaiter _lastCallResultAwaiter;

            public SingleCallServiceExecutor(Func<IServiceExecutor<TService>> serviceExecutorCreator)
            {
                Contract.Requires<ArgumentNullException>(serviceExecutorCreator != null);

                _serviceExecutorCreator = serviceExecutorCreator;
                _lastCallResultAwaiter = new LastCallResultAwaiter();
            }

            public Task Execute(Func<TService, Task> action)
            {
                return _lastCallResultAwaiter.Await(() => _serviceExecutorCreator().Execute(action));
            }

            public Task<TResult> Execute<TResult>(Func<TService, Task<TResult>> func)
            {
                return _lastCallResultAwaiter.Await(() => _serviceExecutorCreator().Execute(func));
            }
        }
    }
}