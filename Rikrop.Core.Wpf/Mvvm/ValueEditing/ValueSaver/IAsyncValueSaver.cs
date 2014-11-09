using System.Threading.Tasks;

namespace Rikrop.Core.Wpf.Mvvm.ValueEditing.ValueSaver
{
    public interface IAsyncValueSaver<in TEditedValue>
    {
        Task SaveValueAsync(TEditedValue editedValue);
    }
}