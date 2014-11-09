using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Rikrop.Core.Wpf.Async;

namespace Rikrop.Core.Wpf.Collections
{
    public interface IAsyncCollection : IEnumerable, IBusyItem, INotifyCollectionChanged
    {
        event Action<IAsyncCollection> Loaded;
        void Refresh();
    }

    public interface IAsyncCollection<out T> : IAsyncCollection, IEnumerable<T>
    {
        new event Action<IAsyncCollection<T>> Loaded;
    }
}
