namespace Rikrop.Core.Wpf.Async
{
    public interface IBusyTrigger
    {
        void SetBusy();
        void ClearBusy();
    }
}