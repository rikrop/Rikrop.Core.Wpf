using System.Windows;

namespace Rikrop.Core.Wpf
{
    public class DialogShower : IDialogShower
    {
        private readonly string _caption;

        public DialogShower(string caption)
        {
            _caption = caption;
        }

        public void ShowError(string message)
        {
            MessageBox.Show(message, _caption, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void ShowInfo(string message)
        {
            MessageBox.Show(message, _caption, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public bool Ask(string message)
        {
            return Ask(message, MessageBoxImage.Question);
        }

        private bool Ask(string message, MessageBoxImage image)
        {
            return MessageBox.Show(message, _caption, MessageBoxButton.YesNo, image) == MessageBoxResult.Yes;
        }
    }
}