using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using Rikrop.Core.Framework;

namespace Rikrop.Core.Wpf.Mvvm.ValueEditing.ValueSource
{
    public class EntityValueSource<TEntity, TValue> : ChangeNotifier, IValueSource<TValue>
    {
        private readonly PropertyEditor<TEntity, TValue> _propertyEditor;

        private TValue _value;

        public TValue Value
        {
            get { return _value; }
            set { SetProperty(ref _value, value); }
        }

        protected void UpdateValueFromEntity()
        {
            Value = _propertyEditor.Value;
        }

        protected void UpdateEntityFromValue()
        {
            _propertyEditor.Value = Value;
        }

        public EntityValueSource(TEntity entity, Expression<Func<TEntity, object>> property)
        {
            Contract.Requires<ArgumentNullException>(property != null);
            _propertyEditor = new PropertyEditor<TEntity, TValue>(entity, property.GetName());
            UpdateValueFromEntity();

            AfterNotify(() => Value)
                .Execute(UpdateEntityFromValue);
        }
    }
}