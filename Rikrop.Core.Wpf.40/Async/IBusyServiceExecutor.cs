using Rikrop.Core.Framework.Services;

namespace Rikrop.Core.Wpf.Async
{
    public interface IBusyServiceExecutor<out TService> : IServiceExecutor<TService>, IBusyItem
    {
    }
}