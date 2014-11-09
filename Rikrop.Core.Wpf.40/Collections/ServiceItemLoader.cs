using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Rikrop.Core.Framework.Services;

namespace Rikrop.Core.Wpf.Collections
{
    public class ServiceItemLoader<TService, TItem> : IItemLoader<IList<TItem>>
    {
        private readonly IServiceExecutor<TService> _serviceExecutor;
        private readonly Func<TService, Task<IList<TItem>>> _itemsLoader;

        public ServiceItemLoader(IServiceExecutor<TService> serviceExecutor, Func<TService, Task<IList<TItem>>> itemsLoader)
        {
            Contract.Requires<ArgumentNullException>(serviceExecutor != null);
            Contract.Requires<ArgumentNullException>(itemsLoader != null);

            _serviceExecutor = serviceExecutor;
            _itemsLoader = itemsLoader;
        }

        public Task<IList<TItem>> GetItem()
        {
            return _serviceExecutor.Execute(_itemsLoader);
        }
    }
}