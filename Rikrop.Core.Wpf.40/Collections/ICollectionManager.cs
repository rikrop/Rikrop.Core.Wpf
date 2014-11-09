using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using Rikrop.Core.Wpf.Async;
using Rikrop.Core.Wpf.Collections.Contracts;

namespace Rikrop.Core.Wpf.Collections
{
    [ContractClass(typeof (ContractICollectionManager<>))]
    public interface ICollectionManager<TItem> : INotifyPropertyChanged
    {
        IBusyItem InitializeBusyItem { get; }
        ReadOnlyObservableCollection<TItem> Items { get; }
    }

    namespace Contracts
    {
        [ContractClassFor(typeof (ICollectionManager<>))]
        public abstract class ContractICollectionManager<TItem> : ICollectionManager<TItem>
        {
            public abstract event PropertyChangedEventHandler PropertyChanged;

            public IBusyItem InitializeBusyItem
            {
                get
                {
                    Contract.Assume(Contract.Result<IBusyItem>() != null);
                    return default(IBusyItem);
                }
            }

            public ReadOnlyObservableCollection<TItem> Items
            {
                get
                {
                    Contract.Assume(Contract.Result<ReadOnlyObservableCollection<TItem>>() != null);
                    return default(ReadOnlyObservableCollection<TItem>);
                }
            }
        }
    }
}