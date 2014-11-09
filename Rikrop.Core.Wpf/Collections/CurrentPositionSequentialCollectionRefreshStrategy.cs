using System.Collections.Generic;

namespace Rikrop.Core.Wpf.Collections
{
    public class CurrentPositionSequentialCollectionRefreshStrategy<TItem> : ISequentialCollectionRefreshStrategy<TItem>
    {
        private readonly int _firstPageSize;
        private readonly int _commonPageSize;

        public CurrentPositionSequentialCollectionRefreshStrategy(int firstPageSize, int commonPageSize)
        {
            _firstPageSize = firstPageSize;
            _commonPageSize = commonPageSize;
        }

        public CurrentPositionSequentialCollectionRefreshStrategy(int commonPageSize)
            : this(commonPageSize, commonPageSize)
        {
        }

        public int GetNextPageSize(IList<TItem> targetCollection)
        {
            if (targetCollection.Count == 0)
            {
                return _firstPageSize;
            }
            return _commonPageSize;
        }

        public int GetRefreshTotalItemsCount(IList<TItem> targetCollection)
        {
            if (targetCollection.Count == 0)
            {
                return _firstPageSize;
            }
            return targetCollection.Count;
        }

        public ICollectionMerger<TItem> GetCollectionMerger()
        {
            return new ReplaceCollectionMerger<TItem>();
        }
    }
}