using System.Collections.Generic;
using System.Collections.ObjectModel;
using Rikrop.Core.Framework.Services;
using Rikrop.Core.Wpf.Async;

namespace Rikrop.Core.Wpf.Collections
{
    public class CollectionManagerBuilder<TItem>
    {
        private ObservableCollection<TItem> _targetCollection = new ObservableCollection<TItem>();
        private bool _isAutoLoading = true;

        public CollectionManagerBuilder<TItem> WithTargetCollection(ObservableCollection<TItem> targetCollection)
        {
            _targetCollection = targetCollection;
            return this;
        }

        public CollectionManagerBuilder<TItem> DisableAutoLoadOnFirstAccess()
        {
            _isAutoLoading = false;
            return this;
        }

        public ServiceExecutorItemLoaderBuilder1<TService, TItem> UseServiceExecutor<TService>(IServiceExecutorFactory<TService> executorFactory)
        {
            return new ServiceExecutorItemLoaderBuilder1<TService, TItem>(executorFactory.CreateMultipleCall(), _targetCollection, _isAutoLoading);
        }

        public ServiceExecutorItemLoaderBuilder1<TService, TItem> UseServiceExecutor<TService>(IServiceExecutor<TService> executor)
        {
            return new ServiceExecutorItemLoaderBuilder1<TService, TItem>(executor, _targetCollection, _isAutoLoading);
        }

        public ServiceExecutorItemLoaderBuilder1<TService, TServiceItem, TItem> UseServiceExecutor<TService, TServiceItem>(IServiceExecutorFactory<TService> executorFactory)
        {
            return new ServiceExecutorItemLoaderBuilder1<TService, TServiceItem, TItem>(executorFactory.CreateMultipleCall(), _targetCollection, _isAutoLoading);
        }

        public ServiceExecutorItemLoaderBuilder1<TService, TServiceItem, TItem> UseServiceExecutor<TService, TServiceItem>(IServiceExecutor<TService> executor)
        {
            return new ServiceExecutorItemLoaderBuilder1<TService, TServiceItem, TItem>(executor, _targetCollection, _isAutoLoading);
        }

        public CollectionManager<TItem> CreateCollection(IItemLoader<IReadOnlyList<TItem>> loader)
        {
            return new CollectionManager<TItem>(_targetCollection, loader, _isAutoLoading);
        }
    }
}