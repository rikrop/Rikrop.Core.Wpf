using System;
using System.Windows;
using System.Windows.Input;
using Rikrop.Core.Wpf.Commands;

namespace Rikrop.Core.Wpf.Mvvm
{
    public abstract class Workspace : ViewModel, IWorkspace
    {
        private RelayCommand _closeCommand;
        private bool _isClosing;
        private bool _isActive;
        public event EventHandler RequestClose;
        public abstract string DisplayName { get; }

        protected bool IsActive
        {
            get { return _isActive; }
        }

        public ICommand CloseCommand
        {
            get { return _closeCommand ?? (_closeCommand = new RelayCommand(CloseInternal)); }
        }

        public void Activate()
        {
            if (_isActive)
            {
                return;
            }
            _isActive = true;

            OnActivate();
        }

        public void Deactivate()
        {
            if (!_isActive)
            {
                return;
            }
            _isActive = false;
            
            OnDeactivate();
        }

        public bool Close()
        {
            if (CloseCommand.CanExecute(null))
            {
                CloseCommand.Execute(null);
                return true;
            }

            return false;
        }

        protected virtual void OnActivate()
        {
        }

        protected virtual void OnDeactivate()
        {
        }

        protected virtual void OnClose()
        {
        }

        private void RaiseRequestClose()
        {
            var handler = RequestClose;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void CloseInternal()
        {
            if (_isClosing)
            {
                return;
            }

            try
            {
                _isClosing = true;

                Deactivate();
                OnClose();
                RaiseRequestClose();
            }
            finally
            {
                _isClosing = false;
            }
        }
    }

    public abstract class Workspace<TView> : Workspace
        where TView : FrameworkElement, new()
    {
        public TView TypedContent
        {
            get { return (TView) Content; }
        }

        public override FrameworkElement Content
        {
            get { return base.Content ?? (base.Content = new TView()); }
            set { base.Content = value; }
        }
    }
}