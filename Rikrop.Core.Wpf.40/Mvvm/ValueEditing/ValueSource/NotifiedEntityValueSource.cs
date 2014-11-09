using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;

namespace Rikrop.Core.Wpf.Mvvm.ValueEditing.ValueSource
{
    public class NotifiedEntityValueSource<TEntity, TValue> : EntityValueSource<TEntity, TValue>
        where TEntity : INotifyPropertyChanged
    {
        public NotifiedEntityValueSource(TEntity entity, Expression<Func<TEntity, object>> property)
            :base(entity, property)
        {
            Contract.Requires<ArgumentNullException>(property != null);

            AfterNotify(entity, property)
                .Execute(UpdateValueFromEntity);
        }
    }
}