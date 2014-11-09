using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace Rikrop.Core.Wpf.Collections
{
    public class ItemLoaderWithConverter<TItem, TModelItem> : IItemLoader<IReadOnlyList<TModelItem>>
    {
        private readonly IItemLoader<IReadOnlyList<TItem>> _itemLoader;
        private readonly Func<TItem, TModelItem> _itemConverterFunc;

        public ItemLoaderWithConverter(IItemLoader<IReadOnlyList<TItem>> itemLoader, Func<TItem, TModelItem> itemConverterFunc)
        {
            Contract.Requires<ArgumentNullException>(itemLoader != null);
            Contract.Requires<ArgumentNullException>(itemConverterFunc != null);

            _itemLoader = itemLoader;
            _itemConverterFunc = itemConverterFunc;
        }

        public async Task<IReadOnlyList<TModelItem>> GetItem()
        {
            var items = await _itemLoader.GetItem();

            return items.Select(o => _itemConverterFunc(o)).ToList();
        }
    }
}