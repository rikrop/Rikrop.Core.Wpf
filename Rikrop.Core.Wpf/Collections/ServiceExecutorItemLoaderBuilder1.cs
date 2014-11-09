using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Rikrop.Core.Framework.Services;

namespace Rikrop.Core.Wpf.Collections
{
    public class ServiceExecutorItemLoaderBuilder1<TService, TItem>
    {
        private readonly IServiceExecutor<TService> _serviceExecutor;
        private readonly ObservableCollection<TItem> _targetCollection;
        private readonly bool _isAutoLoading;

        internal ServiceExecutorItemLoaderBuilder1(IServiceExecutor<TService> serviceExecutor, ObservableCollection<TItem> targetCollection, bool isAutoLoading)
        {
            _serviceExecutor = serviceExecutor;
            _targetCollection = targetCollection;
            _isAutoLoading = isAutoLoading;
        }

        public ServiceExecutorItemLoaderBuilder2<TService, TItem> LoadWith(Func<TService, Task<IReadOnlyList<TItem>>> itemsLoaderFunc)
        {
            return new ServiceExecutorItemLoaderBuilder2<TService, TItem>(_serviceExecutor, _targetCollection, _isAutoLoading, itemsLoaderFunc);
        }
    }

    public class ServiceExecutorItemLoaderBuilder1<TService, TServiceItem, TItem>
    {
        private readonly IServiceExecutor<TService> _serviceExecutor;
        private readonly ObservableCollection<TItem> _targetCollection;
        private readonly bool _isAutoLoading;

        internal ServiceExecutorItemLoaderBuilder1(IServiceExecutor<TService> serviceExecutor, ObservableCollection<TItem> targetCollection, bool isAutoLoading)
        {
            _serviceExecutor = serviceExecutor;
            _targetCollection = targetCollection;
            _isAutoLoading = isAutoLoading;
        }

        public ServiceExecutorItemLoaderBuilder2<TService, TServiceItem, TItem> LoadWith(Func<TService, Task<IReadOnlyList<TServiceItem>>> itemsLoaderFunc)
        {
            return new ServiceExecutorItemLoaderBuilder2<TService, TServiceItem, TItem>(_serviceExecutor, _targetCollection, _isAutoLoading, itemsLoaderFunc);
        }
    }
}