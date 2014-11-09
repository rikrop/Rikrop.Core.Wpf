using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Rikrop.Core.Wpf.StyleSelectors
{
    [ContentProperty("DataTemplates")]
    public class KeyDataTemplateSelector : DataTemplateSelector
    {
        private readonly List<KeyDataTemplatePair> _dataTemplates;

        public DataTemplate DefaultTemplate { get; set; }

        public List<KeyDataTemplatePair> DataTemplates
        {
            get { return _dataTemplates; }
        }

        public KeyDataTemplateSelector()
        {
            _dataTemplates = new List<KeyDataTemplatePair>();
            ThrowOnNoKey = true;
        }

        public bool ThrowOnNoKey { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var tObject = _dataTemplates.FirstOrDefault(o => Equals(o.Key, item));

            if (tObject != null)
            {
                return tObject.Template;
            }

            if (ThrowOnNoKey)
            {
                throw new ArgumentException(String.Format("Среди списка заданных значений: {0} не найдено значение {1}",
                    string.Join(", ", DataTemplates.Select(o => o.Key)), item));
            }

            return DefaultTemplate;
        }
    }

    public class KeyDataTemplatePair
    {
        public object Key { get; set; }
        public DataTemplate Template { get; set; }
    }
}