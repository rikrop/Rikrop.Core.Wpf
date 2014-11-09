using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Rikrop.Core.Wpf.StyleSelectors
{
    [ContentProperty("Styles")]
    public class KeyStyleSelector : StyleSelector
    {
        private readonly List<KeyStylePair> _styles;

        public Style DefaultStyle { get; set; }

        public List<KeyStylePair> Styles
        {
            get { return _styles; }
        }

        public bool ThrowOnNoKey { get; set; }

        public KeyStyleSelector()
        {
            _styles = new List<KeyStylePair>();
            ThrowOnNoKey = true;
        }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            var tObject = Styles.FirstOrDefault(o => Equals(o.Key, item));

            if (tObject != null)
            {
                return tObject.Style;
            }

            if (ThrowOnNoKey)
            {
                throw new ArgumentException(String.Format("Среди списка заданных значений: {0} не найдено значение {1}",
                    string.Join(", ", Styles.Select(o => o.Key)), item));
            }

            return DefaultStyle;
        }
    }

    public class KeyStylePair
    {
        public Type Key { get; set; }
        public Style Style { get; set; }
    }
}