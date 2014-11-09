using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Rikrop.Core.Wpf.Commands;

namespace Rikrop.Core.Wpf.TestApplication
{
    public class MainViewModel : ChangeNotifier
    {
        private readonly RelayCommand<bool> _command;    

        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set { SetProperty(ref _isActive, value); }
        }

        public ICommand Command
        {
            get { return _command; }
        }

        public MainViewModel()
        {
            IsActive = true;

            _command = new RelayCommandBuilder<bool>(DoWork)
                .ListenCommandManager()
                .AddCanExecute(() => IsActive)
                .AddCanExecute(b => b)
                .InvalidateOnNotify(this, () => IsActive)
                .CreateCommand();
        }

        private void DoWork(bool item)
        {
            
        }
    }
}
