using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Rikrop.Core.Wpf.Async;

namespace Rikrop.Core.Wpf.Collections
{
    public class SequentialPageRequester<TItem> : ChangeNotifier
    {
        private readonly IList<TItem> _targetCollection;
        private readonly IPageLoader<TItem> _itemPagesSource;
        private readonly ISequentialCollectionRefreshStrategy<TItem> _sequentialCollectionRefreshStrategy;

        private readonly LastCallResultAwaiter _lastCallResultAwaiter;

        private bool _hasMoreItems;
        private int _addingItems;

        public bool HasMoreItems
        {
            get { return _hasMoreItems; }
            private set { SetProperty(ref _hasMoreItems, value); }
        }

        public SequentialPageRequester(
            IList<TItem> targetCollection,
            IPageLoader<TItem> itemPagesSource,
            ISequentialCollectionRefreshStrategy<TItem> sequentialCollectionRefreshStrategy)
        {
            Contract.Requires<ArgumentNullException>(targetCollection != null);
            Contract.Requires<ArgumentNullException>(itemPagesSource != null);
            Contract.Requires<ArgumentNullException>(sequentialCollectionRefreshStrategy != null);

            _targetCollection = targetCollection;
            _itemPagesSource = itemPagesSource;

            _sequentialCollectionRefreshStrategy = sequentialCollectionRefreshStrategy;

            _lastCallResultAwaiter = new LastCallResultAwaiter();

            HasMoreItems = true;
        }

        public async Task TryRequestNextPage()
        {
            if (!HasMoreItems)
            {
                return;
            }
            if (_addingItems > 0)
            {
                return;
            }
            await RequestNextPageInternal();
        }

        public async Task Refresh()
        {
            await Refresh(_sequentialCollectionRefreshStrategy);
        }

        public async Task Refresh(ISequentialCollectionRefreshStrategy<TItem> sequentialCollectionRefreshStrategy)
        {
            Contract.Requires<ArgumentNullException>(sequentialCollectionRefreshStrategy != null);

            var lst = new List<TItem>();
            ++_addingItems;
            var hasMoreItems = true;
            var isCancelled = false;

            var refreshCount = sequentialCollectionRefreshStrategy.GetRefreshTotalItemsCount(_targetCollection);

            try
            {
                while (lst.Count < refreshCount)
                {
                    var pageSize = sequentialCollectionRefreshStrategy.GetNextPageSize(lst);
                    IReadOnlyCollection<TItem> page;
                    try
                    {
                        page = await GetLastPage(lst.Count, pageSize);
                    }
                    catch (OperationCanceledException)
                    {
                        isCancelled = true;
                        throw;
                    }

                    lst.AddRange(page);

                    if (page.Count < pageSize)
                    {
                        hasMoreItems = false;
                        break;
                    }
                }
            }
            finally
            {
                if (!isCancelled)
                {
                    HasMoreItems = hasMoreItems;
                    sequentialCollectionRefreshStrategy.GetCollectionMerger().MergeLoadedItems(_targetCollection, lst);
                }

                --_addingItems;
            }
        }

        private async Task RequestNextPageInternal()
        {
            ++_addingItems;
            try
            {
                var nextPageSize = _sequentialCollectionRefreshStrategy.GetNextPageSize(_targetCollection);
                var page = await GetLastPage(_targetCollection.Count, nextPageSize);

                foreach (var litem in page)
                {
                    _targetCollection.Add(litem);
                }

                if (page.Count < nextPageSize)
                {
                    HasMoreItems = false;
                }
            }
            finally
            {
                --_addingItems;
            }
        }

        private async Task<IReadOnlyCollection<TItem>> GetLastPage(int skipItems, int takeItems)
        {
            return await _lastCallResultAwaiter.Await(() => _itemPagesSource.GetPage(skipItems, takeItems));
        }
    }
}
