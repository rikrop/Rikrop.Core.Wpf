using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Windows.Input;
using Rikrop.Core.Wpf.Async;
using Rikrop.Core.Wpf.Collections.Contracts;

namespace Rikrop.Core.Wpf.Collections
{
    [ContractClass(typeof (ContractISequentialCollectionManager<>))]
    public interface ISequentialCollectionManager<TItem> : ICollectionManager<TItem>
    {
        ICommand RequestNextPageCommand { get; }
        IBusyItem PageLoadingBusyItem { get; }
    }

    namespace Contracts
    {
        [ContractClassFor(typeof (ISequentialCollectionManager<>))]
        public abstract class ContractISequentialCollectionManager<TItem> : ISequentialCollectionManager<TItem>
        {
            public abstract event PropertyChangedEventHandler PropertyChanged;
            public abstract IBusyItem InitializeBusyItem { get; }
            public abstract ReadOnlyObservableCollection<TItem> Items { get; }

            public ICommand RequestNextPageCommand
            {
                get
                {
                    Contract.Assume(Contract.Result<ICommand>() != null);
                    return default(ICommand);
                }
            }

            public IBusyItem PageLoadingBusyItem
            {
                get
                {
                    Contract.Assume(Contract.Result<IBusyItem>() != null);
                    return default(IBusyItem);
                }
            }
        }
    }
}