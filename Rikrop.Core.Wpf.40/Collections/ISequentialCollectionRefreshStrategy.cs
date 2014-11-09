using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Rikrop.Core.Wpf.Collections.Contracts;

namespace Rikrop.Core.Wpf.Collections
{
    [ContractClass(typeof (ContractISequentialCollectionRefreshStrategy<>))]
    public interface ISequentialCollectionRefreshStrategy<TItem>
    {
        int GetNextPageSize(IList<TItem> targetCollection);
        int GetRefreshTotalItemsCount(IList<TItem> targetCollection);
        ICollectionMerger<TItem> GetCollectionMerger();
    }

    namespace Contracts
    {
        [ContractClassFor(typeof (ISequentialCollectionRefreshStrategy<>))]
        public abstract class ContractISequentialCollectionRefreshStrategy<TItem> : ISequentialCollectionRefreshStrategy<TItem>
        {
            public int GetNextPageSize(IList<TItem> targetCollection)
            {
                Contract.Requires<ArgumentNullException>(targetCollection != null);
                Contract.Assume(Contract.Result<int>() >= 0);

                return default(int);
            }

            public int GetRefreshTotalItemsCount(IList<TItem> targetCollection)
            {
                Contract.Requires<ArgumentNullException>(targetCollection != null);
                Contract.Assume(Contract.Result<int>() >= 0);

                return default(int);
            }

            public int GetRefreshPageSize(IList<TItem> targetCollection)
            {
                Contract.Requires<ArgumentNullException>(targetCollection != null);
                Contract.Assume(Contract.Result<int>() >= 0);

                return default(int);
            }

            public ICollectionMerger<TItem> GetCollectionMerger()
            {
                Contract.Assume(Contract.Result<ICollectionMerger<TItem>>() != null);
                return default(ICollectionMerger<TItem>);
            }
        }
    }
}