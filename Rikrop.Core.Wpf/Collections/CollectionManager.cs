using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;
using Rikrop.Core.Wpf.Async;

namespace Rikrop.Core.Wpf.Collections
{
    public class CollectionManager<TItem> : ChangeNotifier, ICollectionManager<TItem>
    {
        private readonly ObservableCollection<TItem> _targetCollection;
        private readonly IItemLoader<IReadOnlyList<TItem>> _loader;
        private readonly ReadOnlyObservableCollection<TItem> _items;
        private readonly ReplaceCollectionMerger<TItem> _replaceCollectionMerger = new ReplaceCollectionMerger<TItem>();

        private readonly TaskExecutionNotifier _taskExecutionNotifier;
        private readonly BusyTrigger _busyTrigger;

        private readonly LastCallResultAwaiter _lastCallResultAwaiter;
        private bool _isAutoLoading;

        public IBusyItem InitializeBusyItem
        {
            get { return _busyTrigger; }
        }

        public ReadOnlyObservableCollection<TItem> Items
        {
            get
            {
                if (_isAutoLoading)
                {
                    _isAutoLoading = false;
                    Refresh();
                }
                return _items;
            }
        }

        public CollectionManager(ObservableCollection<TItem> targetCollection, IItemLoader<IReadOnlyList<TItem>> loader, bool isAutoLoading)
        {
            Contract.Requires<ArgumentNullException>(targetCollection != null);
            Contract.Requires<ArgumentNullException>(loader != null);

            _targetCollection = targetCollection;
            _loader = loader;
            _isAutoLoading = isAutoLoading;
            _items = new ReadOnlyObservableCollection<TItem>(_targetCollection);

            _busyTrigger = new BusyTrigger();
            _taskExecutionNotifier = new TaskExecutionNotifier(_busyTrigger);
            _lastCallResultAwaiter = new LastCallResultAwaiter();
        }

        public static CollectionManagerBuilder<TItem> GetBuilder()
        {
            return new CollectionManagerBuilder<TItem>();
        }

        public async void Refresh()
        {
            await RefreshTask(_replaceCollectionMerger);
        }

        public Task RefreshTask()
        {
            return RefreshTask(_replaceCollectionMerger);
        }

        public Task RefreshTask(ICollectionMerger<TItem> merger)
        {
            Contract.Requires<ArgumentNullException>(merger != null);

            return _taskExecutionNotifier.TrackExecution(() => _lastCallResultAwaiter.Await(ct => RefreshWithMergerCore(merger, ct)));
        }

        private async Task RefreshWithMergerCore(ICollectionMerger<TItem> merger, CancellationToken ct)
        {
            var result = await _loader.GetItem();
            ct.ThrowIfCancellationRequested();
            merger.MergeLoadedItems(_targetCollection, result);
        }
    }
}