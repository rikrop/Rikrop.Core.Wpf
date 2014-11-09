using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Input;
using Rikrop.Core.Framework;
using Rikrop.Core.Wpf.Async;
using Rikrop.Core.Wpf.Mvvm.Validation;

namespace Rikrop.Core.Wpf.Commands
{
    public class RelayCommand : CommandBase
    {
        private readonly IEnumerable<Func<bool>> _canExecuteCollection;
        private readonly Action _execute;

        public RelayCommand(Action execute)
            : this(execute, () => true)
        {
        }

        public RelayCommand(
            Action execute,
            Func<bool> canExecutePredicate)
            : this(execute, new[] {canExecutePredicate}, new Tuple<INotifyPropertyChanged, string>[0])
        {
        }

        public RelayCommand(
            Action execute,
            IEnumerable<Func<bool>> canExecutePredicates,
            IEnumerable<Tuple<INotifyPropertyChanged, string>> invalidators)
        {
            Contract.Requires<ArgumentNullException>(execute != null);
            Contract.Requires<ArgumentNullException>(canExecutePredicates != null);
            Contract.Requires<ArgumentNullException>(invalidators != null);

            _execute = execute;
            _canExecuteCollection = canExecutePredicates;

            foreach (var invalidator in invalidators)
            {
                AddListenerInternal(invalidator.Item1, invalidator.Item2);
            }
        }

        public override sealed bool CanExecute(object parameter)
        {
            return _canExecuteCollection.All(ce => ce());
        }

        public override sealed void Execute(object parameter)
        {
            _execute();
        }
    }

    public class RelayCommand<T> : CommandBase
    {
        private readonly IEnumerable<Func<T, bool>> _canExecuteCollection;
        private readonly Action<T> _execute;

        public RelayCommand(Action<T> execute)
            : this(execute, par => true)
        {
        }

        public RelayCommand(
            Action<T> execute,
            Func<T, bool> canExecutePredicate)
            : this(execute, new[] {canExecutePredicate}, new Tuple<INotifyPropertyChanged, string>[0], false)
        {
        }

        public RelayCommand(
            Action<T> execute,
            IEnumerable<Func<T, bool>> canExecutePredicates,
            IEnumerable<Tuple<INotifyPropertyChanged, string>> invalidators,
            bool listenCommandManager)
        {
            Contract.Requires<ArgumentNullException>(execute != null);
            Contract.Requires<ArgumentNullException>(canExecutePredicates != null);
            Contract.Requires<ArgumentNullException>(invalidators != null);

            _execute = execute;
            _canExecuteCollection = canExecutePredicates;

            foreach (var invalidator in invalidators)
            {
                AddListenerInternal(invalidator.Item1, invalidator.Item2);
            }

            if (listenCommandManager)
            {
                CommandManager.RequerySuggested += (sender, args) => InvalidateCommand();
            }
        }

        public override sealed bool CanExecute(object parameter)
        {
            var par = (T) parameter;
            return _canExecuteCollection.All(ce => ce(par));
        }

        public override sealed void Execute(object parameter)
        {
            _execute((T) parameter);
        }
    }
}