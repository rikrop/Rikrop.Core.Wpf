using System.Threading.Tasks;

namespace Rikrop.Core.Wpf.Collections
{
    public interface IItemLoader<TItem>
    {
        Task<TItem> GetItem();
    }
}