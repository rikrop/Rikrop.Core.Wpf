using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using Rikrop.Core.Framework;
using Rikrop.Core.Wpf.Async;
using Rikrop.Core.Wpf.Mvvm.Validation;

namespace Rikrop.Core.Wpf.Commands
{
    public class RelayCommandBuilder
    {
        private readonly Action _action;
        private readonly List<Func<bool>> _canExecuteCollection;
        private readonly List<Tuple<INotifyPropertyChanged, string>> _invalidatorsCollection;

        public RelayCommandBuilder(Action action)
        {
            _action = action;
            Contract.Requires<ArgumentNullException>(action != null);
            _canExecuteCollection = new List<Func<bool>>();
            _invalidatorsCollection = new List<Tuple<INotifyPropertyChanged, string>>();
        }

        public RelayCommandBuilder AddCanExecute(Func<bool> canExecute)
        {
            Contract.Requires<ArgumentNullException>(canExecute != null);
            _canExecuteCollection.Add(canExecute);
            return this;
        }

        public RelayCommandBuilder InvalidateOnNotify<TEntity>(TEntity entity, Expression<Func<TEntity, object>> property)
            where TEntity : INotifyPropertyChanged
        {
            _invalidatorsCollection.Add(new Tuple<INotifyPropertyChanged, string>(entity, property.GetName()));
            return this;
        }

        public RelayCommandBuilder InvalidateOnNotify<TEntity>(TEntity entity, Expression<Func<object>> property)
            where TEntity : INotifyPropertyChanged
        {
            _invalidatorsCollection.Add(new Tuple<INotifyPropertyChanged, string>(entity, property.GetName()));
            return this;
        }

        public RelayCommandBuilder AddBlocker(DataValidationInfo dataValidationInfo)
        {
            Contract.Requires<ArgumentNullException>(dataValidationInfo != null);
            return AddCanExecute(() => !dataValidationInfo.HasErrors)
                .InvalidateOnNotify(dataValidationInfo, info => info.HasErrors);
        }

        public RelayCommandBuilder AddBlocker(IBusyItem busyItem)
        {
            Contract.Requires<ArgumentNullException>(busyItem != null);
            return AddCanExecute(() => !busyItem.IsBusy)
                .InvalidateOnNotify(busyItem, item => item.IsBusy);
        }

        public RelayCommand CreateCommand()
        {
            return new RelayCommand(_action, _canExecuteCollection, _invalidatorsCollection);
        }
    }

    public class RelayCommandBuilder<T>
    {
        private readonly Action<T> _action;
        private readonly List<Func<T, bool>> _canExecuteCollection;
        private readonly List<Tuple<INotifyPropertyChanged, string>> _invalidatorsCollection;
        private bool _listenCommandManager;

        public RelayCommandBuilder(Action<T> action)
        {
            Contract.Requires<ArgumentNullException>(action != null);
            _action = action;
            _canExecuteCollection = new List<Func<T, bool>>();
            _invalidatorsCollection = new List<Tuple<INotifyPropertyChanged, string>>();
        }

        public RelayCommandBuilder<T> ListenCommandManager()
        {
            _listenCommandManager = true;
            return this;
        }

        public RelayCommandBuilder<T> AddCanExecute(Func<T, bool> canExecute)
        {
            Contract.Requires<ArgumentNullException>(canExecute != null);
            _canExecuteCollection.Add(canExecute);
            return this;
        }

        public RelayCommandBuilder<T> AddCanExecute(Func<bool> canExecute)
        {
            Contract.Requires<ArgumentNullException>(canExecute != null);
            _canExecuteCollection.Add(par => canExecute());
            return this;
        }

        public RelayCommandBuilder<T> InvalidateOnNotify<TEntity>(TEntity entity, Expression<Func<TEntity, object>> property)
            where TEntity : INotifyPropertyChanged
        {
            _invalidatorsCollection.Add(new Tuple<INotifyPropertyChanged, string>(entity, property.GetName()));
            return this;
        }

        public RelayCommandBuilder<T> InvalidateOnNotify<TEntity>(TEntity entity, Expression<Func<object>> property)
            where TEntity : INotifyPropertyChanged
        {
            _invalidatorsCollection.Add(new Tuple<INotifyPropertyChanged, string>(entity, property.GetName()));
            return this;
        }

        public RelayCommandBuilder<T> AddBlocker(DataValidationInfo dataValidationInfo)
        {
            Contract.Requires<ArgumentNullException>(dataValidationInfo != null);
            return AddCanExecute(par => !dataValidationInfo.HasErrors)
                .InvalidateOnNotify(dataValidationInfo, info => info.HasErrors);
        }

        public RelayCommandBuilder<T> AddBlocker(IBusyItem busyItem)
        {
            Contract.Requires<ArgumentNullException>(busyItem != null);
            return AddCanExecute(par => !busyItem.IsBusy)
                .InvalidateOnNotify(busyItem, item => item.IsBusy);
        }

        public RelayCommand<T> CreateCommand()
        {
            return new RelayCommand<T>(_action, _canExecuteCollection, _invalidatorsCollection, _listenCommandManager);
        }
    }
}