using System;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace Rikrop.Core.Wpf.Mvvm.ValueEditing.ValueSource
{
    public class PropertyEditor<TEntity, TValue>
    {
        private readonly TEntity _entity;
        private readonly PropertyInfo _propertyInfo;

        public PropertyEditor(TEntity entity, string propertyName)
        {
            Contract.Requires<ArgumentNullException>(!Equals(entity, null));
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(propertyName));
            
            _entity = entity;
            _propertyInfo = _entity.GetType().GetProperty(propertyName);

            if (_propertyInfo == null)
            {
                throw new ArgumentException("Параметр не найден в сущности", "propertyName");
            }
        }

        public TValue Value
        {
            get { return (TValue)_propertyInfo.GetValue(_entity, new object[] { }); }
            set { _propertyInfo.SetValue(_entity, value, new object[] { }); }
        }
    }
}