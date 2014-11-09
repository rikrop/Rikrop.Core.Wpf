using System.Collections.Generic;

namespace Rikrop.Core.Wpf.Collections
{
    public class FirstPageSequentialCollectionRefreshStrategy<TItem> : ISequentialCollectionRefreshStrategy<TItem>
    {
        private readonly int _firstPageSize;
        private readonly int _commonPageSize;

        public FirstPageSequentialCollectionRefreshStrategy(int firstPageSize, int commonPageSize)
        {
            _firstPageSize = firstPageSize;
            _commonPageSize = commonPageSize;
        }

        public FirstPageSequentialCollectionRefreshStrategy(int commonPageSize)
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
            return _firstPageSize;
        }

        public ICollectionMerger<TItem> GetCollectionMerger()
        {
            return new ResetCollectionMerger<TItem>();
        }
    }
}