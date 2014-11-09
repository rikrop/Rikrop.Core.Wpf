using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Rikrop.Core.Wpf.Controls.RrcPopupDialog
{
    public class PopupDialogStyleSelector : StyleSelector
    {
        private readonly List<StyleSelectionCondition> _styleSelectors;

        public Style DefaultStyle { get; set; }

        public List<StyleSelectionCondition> StyleSelectors
        {
            get { return _styleSelectors; }
        }

        public PopupDialogStyleSelector()
        {
            _styleSelectors = new List<StyleSelectionCondition>();
        }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item == null)
            {
                return null;
            }

            var sObject = _styleSelectors.FirstOrDefault(o => o.Type == item.GetType());
            return sObject != null
                       ? sObject.StyleForPopupDialog
                       : DefaultStyle;
        }
    }
}