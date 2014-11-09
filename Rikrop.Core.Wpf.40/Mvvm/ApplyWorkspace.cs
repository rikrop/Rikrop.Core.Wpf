using System;
using System.Threading.Tasks;
using System.Windows;
using Rikrop.Core.Wpf.Commands;

namespace Rikrop.Core.Wpf.Mvvm
{
    public abstract class ApplyWorkspace<TView> : Workspace<TView>, IApplyWorkspace where TView : FrameworkElement, new()
    {
        private readonly RelayCommand _applyCommand;
        private bool _isApplied;
        public event EventHandler Applied;

        public bool IsApplied
        {
            get { return _isApplied; }
            protected set {SetProperty(ref _isApplied, value);}
        }

        private bool _isApplying;
        public bool IsApplying
        {
            get { return _isApplying; }
            private set { SetProperty(ref _isApplying, value); }
        }

        public RelayCommand ApplyCommand
        {
            get { return _applyCommand; }
        }

        protected ApplyWorkspace()
        {
            _applyCommand = new RelayCommandBuilder(Apply).AddCanExecute(CanApply).CreateCommand();
        }

        protected abstract Task OnApply();

        private void RaiseApplied()
        {
            var handler = Applied;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        protected virtual bool CanApply()
        {
            return !IsApplying;
        }

        private async void Apply()
        {
            if (IsApplying)
            {
                return;
            }

            try
            {
                IsApplying = true;

                await OnApply();
            }
            finally
            {
                IsApplying = false;
            }
            
            IsApplied = true;
            RaiseApplied();
            Close();
        }
    }
}