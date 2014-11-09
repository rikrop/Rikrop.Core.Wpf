using System.Collections.Generic;

namespace Rikrop.Core.Wpf.Collections
{
    public class ResetCollectionMerger<TItem> : ICollectionMerger<TItem>
    {
        public void MergeLoadedItems(IList<TItem> targetCollection, IList<TItem> sourceCollection)
        {
            targetCollection.Clear();

            foreach (var item in sourceCollection)
            {
                targetCollection.Add(item);
            }
        }
    }
}