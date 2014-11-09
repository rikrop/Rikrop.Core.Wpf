using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Rikrop.Core.Wpf.Collections.Contracts;

namespace Rikrop.Core.Wpf.Collections
{
    [ContractClass(typeof(ContractICollectionMerger<>))]
    public interface ICollectionMerger<TItem>
    {
        void MergeLoadedItems(IList<TItem> targetCollection, IReadOnlyList<TItem> sourceCollection);
    }

    namespace Contracts
    {
        [ContractClassFor(typeof(ICollectionMerger<>))]
        public abstract class ContractICollectionMerger<TItem> :ICollectionMerger<TItem>
        {
            public void MergeLoadedItems(IList<TItem> targetCollection, IReadOnlyList<TItem> sourceCollection)
            {
                Contract.Requires<ArgumentNullException>(targetCollection != null);
                Contract.Requires<ArgumentNullException>(sourceCollection != null);
            }
        }
    }
}