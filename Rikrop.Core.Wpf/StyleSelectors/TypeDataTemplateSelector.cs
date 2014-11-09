using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Rikrop.Core.Wpf.StyleSelectors
{
    [ContentProperty("DataTemplates")]
    public class TypeDataTemplateSelector : DataTemplateSelector
    {
        private readonly List<TypeDataTemplatePair> _dataTemplates;

        public DataTemplate DefaultTemplate { get; set; }

        public List<TypeDataTemplatePair> DataTemplates
        {
            get { return _dataTemplates; }
        }

        public bool ThrowOnNoType { get; set; }

        public TypeDataTemplateSelector()
        {
            _dataTemplates = new List<TypeDataTemplatePair>();
            ThrowOnNoType = true;
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null)
            {
                return null;
            }

            // Точное соответствие типа
            var tObject = _dataTemplates.FirstOrDefault(o => o.IsValid && o.Type == item.GetType());
            if (tObject != null)
            {
                return tObject.Template;
            }

            // Соответствие базового типа
            var bObject = _dataTemplates.FirstOrDefault(o => o.IsValid && o.Type.IsInstanceOfType(item));
            if (bObject != null)
            {
                return bObject.Template;
            }

            if (ThrowOnNoType)
            {
                throw new ArgumentException(String.Format("Среди списка заданных значений: {0} не найдено значение {1}",
                    string.Join(", ", DataTemplates.Select(o => o.Type)), item.GetType()));
            }

            // Умолчание
            if (DefaultTemplate != null)
            {
                return DefaultTemplate;
            }

            // Первый с пустым типом
            var firstNullType = _dataTemplates.FirstOrDefault(o => o.Type == null);
            if (firstNullType != null)
            {
                return firstNullType.Template;
            }

            return base.SelectTemplate(item, container);
        }
    }

    public class TypeDataTemplatePair
    {
        public Type Type { get; set; }
        public DataTemplate Template { get; set; }

        public bool IsValid
        {
            get { return Type != null; }
        }
    }
}