using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Windows.Input;
using Rikrop.Core.Wpf.Helpers.WeekEvents;

namespace Rikrop.Core.Wpf.Commands
{
    public abstract class CommandBase : ChangeNotifier, ICommand
    {
        private readonly Listener<PropertyChangedEventArgs> _weakEventListener;

        public event EventHandler CanExecuteChanged;

        protected CommandBase()
        {
            _weakEventListener = new Listener<PropertyChangedEventArgs>(RequeryCanExecute);
        }

        public abstract bool CanExecute(object parameter);
        public abstract void Execute(object parameter);

        public virtual void InvalidateCommand()
        {
            RaiseCanExecuteChanged();
        }

        protected void AddListenerInternal<TEntity>(TEntity source, string propertyName)
            where TEntity : INotifyPropertyChanged
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(propertyName));
            PropertyChangedEventManager.AddListener(source, _weakEventListener, propertyName);
        }

        private void RaiseCanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            if (handler != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }

        private void RequeryCanExecute(object sender, PropertyChangedEventArgs args)
        {
            InvalidateCommand();
        }
    }
}