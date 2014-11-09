using System;

namespace Rikrop.Core.Wpf.Async
{
    public interface IBusyItemRemoveStrategy
    {
        event Action RequestRemove;
    }
}