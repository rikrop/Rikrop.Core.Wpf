using System;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Windows.Input;
using Rikrop.Core.Wpf.Async;
using Rikrop.Core.Wpf.Commands;

namespace Rikrop.Core.Wpf.Collections
{
    public class SequentialCollectionManager<TItem> : ChangeNotifier, ISequentialCollectionManager<TItem>
    {
        private readonly ISequentialCollectionRefreshStrategy<TItem> _sequentialCollectionRefreshToStartStrategy;
        private readonly bool _pageRequestCanLoadEmptyCollection;
        private readonly ReadOnlyObservableCollection<TItem> _items;
        private readonly SequentialPageRequester<TItem> _pageLoadMoitor;
        private readonly RelayCommand _requestNextPageCommand;
        private readonly TaskExecutionNotifier _initializeAwaiter;
        private readonly BusyTrigger _initializeAwaiterViewer;
        private readonly TaskExecutionNotifier _pageLoadingAwaiter;
        private readonly BusyTrigger _pageLoadingAwaiterViewer;

        public IBusyItem InitializeBusyItem
        {
            get { return _initializeAwaiterViewer; }
        }

        public IBusyItem PageLoadingBusyItem
        {
            get { return _pageLoadingAwaiterViewer; }
        }

        public ICommand RequestNextPageCommand
        {
            get { return _requestNextPageCommand; }
        }

        public ReadOnlyObservableCollection<TItem> Items
        {
            get { return _items; }
        }

        public SequentialCollectionManager(
            ObservableCollection<TItem> targetCollection,
            IPageLoader<TItem> pageLoader,
            ISequentialCollectionRefreshStrategy<TItem> defaultSequentialCollectionRefreshStrategy,
            ISequentialCollectionRefreshStrategy<TItem> sequentialCollectionRefreshToStartStrategy,
            bool pageRequestCanLoadEmptyCollection)
        {
            Contract.Requires<ArgumentNullException>(targetCollection != null);
            Contract.Requires<ArgumentNullException>(pageLoader != null);
            Contract.Requires<ArgumentNullException>(defaultSequentialCollectionRefreshStrategy != null);
            Contract.Requires<ArgumentNullException>(sequentialCollectionRefreshToStartStrategy != null);

            _sequentialCollectionRefreshToStartStrategy = sequentialCollectionRefreshToStartStrategy;
            _pageRequestCanLoadEmptyCollection = pageRequestCanLoadEmptyCollection;

            _initializeAwaiterViewer = new BusyTrigger();
            _initializeAwaiter = new TaskExecutionNotifier(_initializeAwaiterViewer);
            _pageLoadingAwaiterViewer = new BusyTrigger();
            _pageLoadingAwaiter = new TaskExecutionNotifier(_pageLoadingAwaiterViewer);

            _items = new ReadOnlyObservableCollection<TItem>(targetCollection);
            _pageLoadMoitor = new SequentialPageRequester<TItem>(targetCollection, pageLoader, defaultSequentialCollectionRefreshStrategy);

            _requestNextPageCommand = new RelayCommandBuilder(RequestNextPage)
                .AddCanExecute(() => _pageLoadMoitor.HasMoreItems)
                .InvalidateOnNotify(_pageLoadMoitor, requester => requester.HasMoreItems)
                .CreateCommand();
        }

        private async void RequestNextPage()
        {
            if (_pageRequestCanLoadEmptyCollection || Items.Count > 0)
            {
                await _pageLoadingAwaiter.TrackExecution(() => _pageLoadMoitor.TryRequestNextPage());                
            }
        }

        public async void Refresh()
        {
            await RefreshAsync();
        }

        public async void RefreshToStart()
        {
            await RefreshToStartAsync();
        }

        public async Task RefreshAsync()
        {
            await _initializeAwaiter.TrackExecution(() => _pageLoadMoitor.Refresh());
        }

        public async Task RefreshToStartAsync()
        {
            await _initializeAwaiter.TrackExecution(() => _pageLoadMoitor.Refresh(_sequentialCollectionRefreshToStartStrategy));
        }
    }
}