using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using Rikrop.Core.Framework.Services;
using Rikrop.Core.Wpf.Async;

namespace Rikrop.Core.Wpf.Collections
{
    public class SequentialCollectionManagerBuilder<TItem>
    {
        private readonly SequentialCollectionManagerBuilderArgs<TItem> _args;

        public SequentialCollectionManagerBuilder()
        {
            _args = new SequentialCollectionManagerBuilderArgs<TItem>();
        }

        public SequentialCollectionManagerBuilder<TItem> SetFirstPageSize(int firstPageSize)
        {
            Contract.Requires<ArgumentException>(firstPageSize > 0);
            _args.FirstPageSize = firstPageSize;
            return this;
        }

        public SequentialCollectionManagerBuilder<TItem> SetPageSize(int pageSize)
        {
            Contract.Requires<ArgumentException>(pageSize > 0);
            _args.PageSize = pageSize;
            return this;
        }

        public SequentialCollectionManagerBuilder<TItem> UseTargetCollection(ObservableCollection<TItem> targetCollection)
        {
            _args.TargetCollection = targetCollection;
            return this;
        }

        public SequentialCollectionManagerBuilder<TItem> AllowPageRequestToLoadEmptyCollection()
        {
            _args.PageRequestCanLoadEmptyCollection = true;
            return this;
        }

        public SequentialCollectionManagerBuilder1<TItem> BuildLoader()
        {
            return new SequentialCollectionManagerBuilder1<TItem>(_args);
        }
    }

    public class SequentialCollectionManagerBuilder1<TItem>
    {
        private readonly SequentialCollectionManagerBuilderArgs<TItem> _args;

        public SequentialCollectionManagerBuilder1(
            SequentialCollectionManagerBuilderArgs<TItem> args)
        {
            Contract.Requires<ArgumentNullException>(args != null);
            _args = args;
        }

        public SequentialCollectionManagerBuilder0<TItem> UsePageLoader(IPageLoader<TItem> pageLoader)
        {
            Contract.Requires<ArgumentNullException>(pageLoader != null);
            return new SequentialCollectionManagerBuilder0<TItem>(_args, pageLoader);
        }

        public SequentialCollectionManagerBuilder11<TItem, TService> UseServiceExecutor<TService>(IServiceExecutor<TService> serviceExecutor)
        {
            return new SequentialCollectionManagerBuilder11<TItem, TService>(serviceExecutor, _args);
        }

        public SequentialCollectionManagerBuilder11<TItem, TService> UseServiceExecutor<TService>(IServiceExecutorFactory<TService> serviceExecutorFactory)
        {
            return new SequentialCollectionManagerBuilder11<TItem, TService>(serviceExecutorFactory.CreateSingleCall(), _args);
        }

        public SequentialCollectionManagerBuilder12<TItem, TService, TInnerItem> UseServiceExecutor<TService, TInnerItem>(IServiceExecutor<TService> serviceExecutor)
        {
            return new SequentialCollectionManagerBuilder12<TItem, TService, TInnerItem>(serviceExecutor, _args);
        }

        public SequentialCollectionManagerBuilder12<TItem, TService, TInnerItem> UseServiceExecutor<TService, TInnerItem>(IServiceExecutorFactory<TService> serviceExecutorFactory)
        {
            return new SequentialCollectionManagerBuilder12<TItem, TService, TInnerItem>(serviceExecutorFactory.CreateSingleCall(), _args);
        }
    }

    public class SequentialCollectionManagerBuilder11<TItem, TService>
    {
        private readonly IServiceExecutor<TService> _serviceExecutor;
        private readonly SequentialCollectionManagerBuilderArgs<TItem> _args;

        public SequentialCollectionManagerBuilder11(
            IServiceExecutor<TService> serviceExecutor,
            SequentialCollectionManagerBuilderArgs<TItem> args)
        {
            Contract.Requires<ArgumentNullException>(serviceExecutor != null);
            Contract.Requires<ArgumentNullException>(args != null);
            _serviceExecutor = serviceExecutor;
            _args = args;
        }

        public SequentialCollectionManagerBuilder0<TItem> LoadWithFunc(Func<TService, int, int, Task<IReadOnlyCollection<TItem>>> loadFunc)
        {
            Contract.Requires<ArgumentNullException>(loadFunc != null);
            return new SequentialCollectionManagerBuilder0<TItem>(_args, new ServicePageLoader(_serviceExecutor, loadFunc));
        }

        private class ServicePageLoader : IPageLoader<TItem>
        {
            private readonly IServiceExecutor<TService> _serviceExecutor;
            private readonly Func<TService, int, int, Task<IReadOnlyCollection<TItem>>> _loadFunc;

            public ServicePageLoader(
                IServiceExecutor<TService> serviceExecutor,
                Func<TService, int, int, Task<IReadOnlyCollection<TItem>>> loadFunc)
            {
                Contract.Requires<ArgumentNullException>(serviceExecutor != null);
                Contract.Requires<ArgumentNullException>(loadFunc != null);
                _serviceExecutor = serviceExecutor;
                _loadFunc = loadFunc;
            }

            public Task<IReadOnlyCollection<TItem>> GetPage(int skipItems, int takeItems)
            {
                return _serviceExecutor.Execute(service => _loadFunc(service, skipItems, takeItems));
            }
        }
    }

    public class SequentialCollectionManagerBuilder12<TItem, TService, TInnerItem>
    {
        private readonly IServiceExecutor<TService> _serviceExecutor;
        private readonly SequentialCollectionManagerBuilderArgs<TItem> _args;

        public SequentialCollectionManagerBuilder12(
            IServiceExecutor<TService> serviceExecutor,
            SequentialCollectionManagerBuilderArgs<TItem> args)
        {
            Contract.Requires<ArgumentNullException>(serviceExecutor != null);
            Contract.Requires<ArgumentNullException>(args != null);
            _serviceExecutor = serviceExecutor;
            _args = args;
        }

        public SequentialCollectionManagerBuilder121<TItem, TService, TInnerItem> LoadWithFunc(Func<TService, int, int, Task<IReadOnlyCollection<TInnerItem>>> loadFunc)
        {
            Contract.Requires<ArgumentNullException>(loadFunc != null);
            return new SequentialCollectionManagerBuilder121<TItem, TService, TInnerItem>(_serviceExecutor, loadFunc, _args);
        }
    }

    public class SequentialCollectionManagerBuilder121<TItem, TService, TInnerItem>
    {
        private readonly IServiceExecutor<TService> _serviceExecutor;
        private readonly Func<TService, int, int, Task<IReadOnlyCollection<TInnerItem>>> _loadFunc;
        private readonly SequentialCollectionManagerBuilderArgs<TItem> _args;

        public SequentialCollectionManagerBuilder121(
            IServiceExecutor<TService> serviceExecutor,
            Func<TService, int, int, Task<IReadOnlyCollection<TInnerItem>>> loadFunc,
            SequentialCollectionManagerBuilderArgs<TItem> args)
        {
            Contract.Requires<ArgumentNullException>(serviceExecutor != null);
            Contract.Requires<ArgumentNullException>(loadFunc != null);
            Contract.Requires<ArgumentNullException>(args != null);
            _serviceExecutor = serviceExecutor;
            _loadFunc = loadFunc;
            _args = args;
        }

        public SequentialCollectionManagerBuilder0<TItem> ConvertWith(Func<TInnerItem, TItem> converter)
        {
            Contract.Requires<ArgumentNullException>(converter != null);
            return new SequentialCollectionManagerBuilder0<TItem>(_args, new ServiceWithConverterPageLoader(_serviceExecutor, _loadFunc, converter));
        }

        private class ServiceWithConverterPageLoader : IPageLoader<TItem>
        {
            private readonly IServiceExecutor<TService> _serviceExecutor;
            private readonly Func<TService, int, int, Task<IReadOnlyCollection<TInnerItem>>> _loadFunc;
            private readonly Func<TInnerItem, TItem> _converter;

            public ServiceWithConverterPageLoader(
                IServiceExecutor<TService> serviceExecutor,
                Func<TService, int, int, Task<IReadOnlyCollection<TInnerItem>>> loadFunc,
                Func<TInnerItem, TItem> converter)
            {
                Contract.Requires<ArgumentNullException>(serviceExecutor != null);
                Contract.Requires<ArgumentNullException>(loadFunc != null);
                Contract.Requires<ArgumentNullException>(loadFunc != null);
                _serviceExecutor = serviceExecutor;
                _loadFunc = loadFunc;
                _converter = converter;
            }

            public async Task<IReadOnlyCollection<TItem>> GetPage(int skipItems, int takeItems)
            {
                var data = await _serviceExecutor.Execute(service => _loadFunc(service, skipItems, takeItems));
                return data.Select(i => _converter(i)).ToArray();
            }
        }
    }

    public class SequentialCollectionManagerBuilder0<TItem>
    {
        private const int DefaultPageSize = 100;
        private readonly SequentialCollectionManagerBuilderArgs<TItem> _args;
        private readonly IPageLoader<TItem> _pageLoader;

        public SequentialCollectionManagerBuilder0(
            SequentialCollectionManagerBuilderArgs<TItem> args,
            IPageLoader<TItem> pageLoader)
        {
            Contract.Requires<ArgumentNullException>(args != null);
            Contract.Requires<ArgumentNullException>(pageLoader != null);
            _args = args;
            _pageLoader = pageLoader;
        }

        public SequentialCollectionManager<TItem> Create()
        {
            var cps = _args.PageSize.HasValue
                          ? _args.PageSize.Value
                          : DefaultPageSize;

            var fps = _args.FirstPageSize.HasValue
                          ? _args.FirstPageSize.Value
                          : cps;

            return new SequentialCollectionManager<TItem>(
                targetCollection: _args.TargetCollection ?? new ObservableCollection<TItem>(),
                pageLoader: _pageLoader,
                defaultSequentialCollectionRefreshStrategy: new CurrentPositionSequentialCollectionRefreshStrategy<TItem>(firstPageSize: fps, commonPageSize: cps),
                sequentialCollectionRefreshToStartStrategy: new FirstPageSequentialCollectionRefreshStrategy<TItem>(firstPageSize: fps, commonPageSize: cps),
                pageRequestCanLoadEmptyCollection: _args.PageRequestCanLoadEmptyCollection);
        }
    }

    public class SequentialCollectionManagerBuilderArgs<TItem>
    {
        public int? FirstPageSize { get; set; }

        public int? PageSize { get; set; }

        public ObservableCollection<TItem> TargetCollection { get; set; }

        public bool PageRequestCanLoadEmptyCollection { get; set; }
    }
}