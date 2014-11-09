namespace Rikrop.Core.Wpf
{
    public interface IDialogShower
    {
        void ShowError(string message);

        void ShowInfo(string message);

        bool Ask(string message);
    }
}