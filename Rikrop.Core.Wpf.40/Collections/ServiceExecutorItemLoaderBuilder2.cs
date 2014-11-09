using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Rikrop.Core.Framework.Services;

namespace Rikrop.Core.Wpf.Collections
{
    public class ServiceExecutorItemLoaderBuilder2<TService, TItem>
    {
        private readonly IServiceExecutor<TService> _serviceExecutor;
        private readonly ObservableCollection<TItem> _targetCollection;
        private readonly bool _isAutoLoading;
        private readonly ServiceItemLoader<TService, TItem> _itemLoader;

        internal ServiceExecutorItemLoaderBuilder2(IServiceExecutor<TService> serviceExecutor,
                                                   ObservableCollection<TItem> targetCollection,
                                                   bool isAutoLoading,
                                                   Func<TService, Task<IList<TItem>>> itemsLoaderFunc)
        {
            Contract.Requires<ArgumentNullException>(itemsLoaderFunc != null);

            _serviceExecutor = serviceExecutor;
            _targetCollection = targetCollection;
            _isAutoLoading = isAutoLoading;

            _itemLoader = new ServiceItemLoader<TService, TItem>(_serviceExecutor, itemsLoaderFunc);
        }

        public CollectionManager<TItem> CreateCollection()
        {
            return new CollectionManager<TItem>(_targetCollection, _itemLoader, _isAutoLoading);
        }
    }

    public class ServiceExecutorItemLoaderBuilder2<TService, TServiceItem, TItem>
    {
        private readonly IServiceExecutor<TService> _serviceExecutor;
        private readonly ObservableCollection<TItem> _targetCollection;
        private readonly bool _isAutoLoading;
        private readonly ServiceItemLoader<TService, TServiceItem> _itemLoader;

        internal ServiceExecutorItemLoaderBuilder2(IServiceExecutor<TService> serviceExecutor,
                                                   ObservableCollection<TItem> targetCollection,
                                                   bool isAutoLoading,
                                                   Func<TService, Task<IList<TServiceItem>>> itemsLoaderFunc)
        {
            Contract.Requires<ArgumentNullException>(itemsLoaderFunc != null);

            _serviceExecutor = serviceExecutor;
            _targetCollection = targetCollection;
            _isAutoLoading = isAutoLoading;

            _itemLoader = new ServiceItemLoader<TService, TServiceItem>(_serviceExecutor, itemsLoaderFunc);
        }

        public ServiceExecutorItemLoaderBuilder3<TService, TServiceItem, TItem> ConvertTo(Func<TServiceItem, TItem> itemConverterFunc)
        {
            return new ServiceExecutorItemLoaderBuilder3<TService, TServiceItem, TItem>(_targetCollection, _isAutoLoading, _itemLoader, itemConverterFunc);
        }
    }
}