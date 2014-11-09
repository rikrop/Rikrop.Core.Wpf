using System;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;

namespace Rikrop.Core.Wpf.Collections
{
    public class ServiceExecutorItemLoaderBuilder3<TService, TServiceItem, TItem>
    {
        private readonly ObservableCollection<TItem> _targetCollection;
        private readonly bool _isAutoLoading;
        private readonly ServiceItemLoader<TService, TServiceItem> _itemLoader;
        private readonly Func<TServiceItem, TItem> _itemConverterFunc;

        internal ServiceExecutorItemLoaderBuilder3(ObservableCollection<TItem> targetCollection,
                                                   bool isAutoLoading,
                                                   ServiceItemLoader<TService, TServiceItem> itemLoader,
                                                   Func<TServiceItem, TItem> itemConverterFunc)
        {
            Contract.Requires<ArgumentNullException>(itemConverterFunc != null);

            _targetCollection = targetCollection;
            _isAutoLoading = isAutoLoading;
            _itemLoader = itemLoader;
            _itemConverterFunc = itemConverterFunc;
        }

        public CollectionManager<TItem> CreateCollection()
        {
            var itemLoader = new ItemLoaderWithConverter<TServiceItem, TItem>(_itemLoader, _itemConverterFunc);
            return new CollectionManager<TItem>(_targetCollection, itemLoader, _isAutoLoading);
        }
    }
}