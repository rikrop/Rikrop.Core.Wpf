using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Rikrop.Core.Wpf.Collections.Contracts;

namespace Rikrop.Core.Wpf.Collections
{
    [ContractClass(typeof (ContractIPageLoader<>))]
    public interface IPageLoader<TItem>
    {
        Task<ICollection<TItem>> GetPage(int skipItems, int takeItems);
    }

    namespace Contracts
    {
        [ContractClassFor(typeof (IPageLoader<>))]
        public abstract class ContractIPageLoader<TItem> : IPageLoader<TItem>
        {
            public Task<ICollection<TItem>> GetPage(int skipItems, int takeItems)
            {
                Contract.Requires<ArgumentException>(skipItems >= 0);
                Contract.Requires<ArgumentException>(takeItems >= 0);

                Contract.Assume(Contract.Result<Task<ICollection<TItem>>>() != null);

                return default(Task<ICollection<TItem>>);
            }
        }
    }
}