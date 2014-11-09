using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using Rikrop.Core.Framework;

namespace Rikrop.Core.Wpf.Async
{
    public class BusyItemTracker : ChangeNotifier, IBusyItem
    {
        private readonly List<LoadingItemWrapper> _trackingItems;
        private bool _isBusy;

        public bool IsBusy
        {
            get { return _isBusy; }
            private set { SetProperty(ref _isBusy, value); }
        }

        public IEnumerable<IBusyItem> TrackingItems
        {
            get { return _trackingItems.Select(o => o.Item); }
        }

        public BusyItemTracker()
        {
            _trackingItems = new List<LoadingItemWrapper>();
        }

        public IRemoveStrategyAdder AddTrackingItem(IBusyItem item)
        {
            var wrapper = _trackingItems.FirstOrDefault(o => o.Item == item);

            if (wrapper == null)
            {
                wrapper = CreateWrapper(item);
                _trackingItems.Add(wrapper);
                item.PropertyChanged += OnItemPropertyChanged;
                UpdateIsBusy();
            }

            return new RemoveStrategyAdder(this, wrapper);
        }

        public void RemoveTrackingItem(IBusyItem item)
        {
            var exitem = _trackingItems.FirstOrDefault(o => o.Item == item);

            if (exitem == null)
            {
                return;
            }
            _trackingItems.Remove(exitem);
            item.PropertyChanged -= OnItemPropertyChanged;
            exitem.RequestRemove -= OnWrapperRequestRemove;
            UpdateIsBusy();
        }

        protected void UpdateIsBusy()
        {
            IsBusy = CalcIsBusy();
        }

        protected virtual bool CalcIsBusy()
        {
            return _trackingItems.Any(i => i.Item.IsBusy);
        }

        private LoadingItemWrapper CreateWrapper(IBusyItem item)
        {
            var wrapper = new LoadingItemWrapper(item);
            wrapper.RequestRemove += OnWrapperRequestRemove;

            return wrapper;
        }

        private void OnWrapperRequestRemove(LoadingItemWrapper loadingItemWrapper)
        {
            Contract.Requires<ArgumentNullException>(loadingItemWrapper != null);
            RemoveTrackingItem(loadingItemWrapper.Item);
        }

        private void OnItemPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (ExpressionHelper.GetName<IBusyItem>(o => o.IsBusy) == args.PropertyName)
            {
                UpdateIsBusy();
            }
        }

        private void SetItemRemoveStrategy(LoadingItemWrapper wrapper, IBusyItemRemoveStrategy strategy)
        {
            Contract.Requires<ArgumentNullException>(wrapper != null);
            wrapper.SetDeleteStategy(strategy);
        }

        public interface IRemoveStrategyAdder
        {
            void WithRemoveStrategy(IBusyItemRemoveStrategy strategy);
        }

        private class LoadingItemWrapper
        {
            private readonly IBusyItem _item;
            private IBusyItemRemoveStrategy _deleteStrategy;

            public event Action<LoadingItemWrapper> RequestRemove;

            public IBusyItem Item
            {
                get { return _item; }
            }

            public LoadingItemWrapper(IBusyItem item)
            {
                Contract.Requires<ArgumentNullException>(item != null);
                _item = item;
            }

            public void SetDeleteStategy(IBusyItemRemoveStrategy strategy)
            {
                if (_deleteStrategy != null)
                {
                    _deleteStrategy.RequestRemove -= RaizeRequestRemove;
                }
                _deleteStrategy = strategy;

                if (_deleteStrategy != null)
                {
                    _deleteStrategy.RequestRemove += RaizeRequestRemove;
                }
            }

            private void RaizeRequestRemove()
            {
                var handler = RequestRemove;
                if (handler != null)
                {
                    handler(this);
                }
            }
        }

        private class RemoveStrategyAdder : IRemoveStrategyAdder
        {
            private readonly BusyItemTracker _tracker;
            private readonly LoadingItemWrapper _wrapper;

            public RemoveStrategyAdder(BusyItemTracker tracker, LoadingItemWrapper wrapper)
            {
                Contract.Requires<ArgumentNullException>(tracker != null);
                Contract.Requires<ArgumentNullException>(wrapper != null);
                _tracker = tracker;
                _wrapper = wrapper;
            }

            public void WithRemoveStrategy(IBusyItemRemoveStrategy strategy)
            {
                _tracker.SetItemRemoveStrategy(_wrapper, strategy);
            }
        }
    }
}