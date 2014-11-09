using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Rikrop.Core.Wpf.StyleSelectors
{
    [ContentProperty("Styles")]
    public class TypeStyleSelector : StyleSelector
    {
        private readonly List<TypeStylePair> _styles;

        public Style DefaultStyle { get; set; }

        public List<TypeStylePair> Styles
        {
            get { return _styles; }
        }

        public bool ThrowOnNoType { get; set; }

        public TypeStyleSelector()
        {
            _styles = new List<TypeStylePair>();
            ThrowOnNoType = true;
        }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item == null)
            {
                return null;
            }

            // Точное соответствие типа
            var tObject = _styles.FirstOrDefault(o => o.IsValid && o.Type == item.GetType());
            if (tObject != null)
            {
                return tObject.Style;
            }

            // Соответствие базового типа
            var bObject = _styles.FirstOrDefault(o => o.IsValid && o.Type.IsInstanceOfType(item));
            if (bObject != null)
            {
                return bObject.Style;
            }

            if (ThrowOnNoType)
            {
                throw new ArgumentException(String.Format("Среди списка заданных значений: {0} не найдено значение {1}",
                    string.Join(", ", Styles.Select(o => o.Type)), item.GetType()));
            }

            // Умолчание
            if (DefaultStyle != null)
            {
                return DefaultStyle;
            }

            // Первый с пустым типом
            var firstNullType = _styles.FirstOrDefault(o => o.Type == null);
            if (firstNullType != null)
            {
                return firstNullType.Style;
            }

            return base.SelectStyle(item, container);
        }
    }

    public class TypeStylePair
    {
        public Type Type { get; set; }
        public Style Style { get; set; }

        public bool IsValid
        {
            get { return Type != null; }
        }
    }
}