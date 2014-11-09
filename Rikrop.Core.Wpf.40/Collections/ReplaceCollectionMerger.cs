using System;
using System.Collections.Generic;

namespace Rikrop.Core.Wpf.Collections
{
    public class ReplaceCollectionMerger<TItem> : ICollectionMerger<TItem>
    {
        public void MergeLoadedItems(IList<TItem> targetCollection, IList<TItem> sourceCollection)
        {
            if (sourceCollection.Count == 0)
            {
                targetCollection.Clear();
            }
            else
            {
                var oldCount = targetCollection.Count;
                var newCount = sourceCollection.Count;

                var commonCount = Math.Min(oldCount, newCount);
                for (var i = 0; i < commonCount; i++)
                {
                    targetCollection[i] = sourceCollection[i];
                }

                var addcount = newCount - oldCount;
                if (addcount > 0)
                {
                    for (var i = oldCount; i < newCount; i++)
                    {
                        targetCollection.Add(sourceCollection[i]);
                    }
                }
                else
                {
                    for (var i = oldCount; i > newCount; i--)
                    {
                        targetCollection.RemoveAt(i - 1);
                    }
                }
            }
        }
    }
}