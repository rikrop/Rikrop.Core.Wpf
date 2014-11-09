using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Rikrop.Core.Framework;

namespace Rikrop.Core.Wpf.Themes
{
    internal static class ValidationToolTipHelper
    {
        public static readonly DependencyProperty ToolTipProperty = DependencyProperty.RegisterAttached(
            "ToolTip",
            typeof (object),
            typeof (ValidationToolTipHelper),
            new PropertyMetadata(default(object), ToolTipChangedCallback));

        public static readonly DependencyProperty AdornedElementToolTipTemplateProperty =
            DependencyProperty.RegisterAttached("AdornedElementToolTipTemplate", typeof (DataTemplate),
                                                typeof (ValidationToolTipHelper),
                                                new PropertyMetadata(null, AdornedElementToolTipTemplatePropertyChangedCallback));

        public static object GetToolTip(AdornedElementPlaceholder obj)
        {
            return obj.GetValue(ToolTipProperty);
        }

        public static void SetToolTip(AdornedElementPlaceholder obj, object value)
        {
            obj.SetValue(ToolTipProperty, value);
        }

        public static void ToolTipChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var placeholder = (AdornedElementPlaceholder) d;
            ((FrameworkElement) placeholder.AdornedElement).ToolTip = e.NewValue;
        }

        public static void SetAdornedElementToolTipTemplate(UIElement element, DataTemplate value)
        {
            element.SetValue(AdornedElementToolTipTemplateProperty, value);
        }

        public static DataTemplate GetAdornedElementToolTipTemplate(UIElement element)
        {
            return (DataTemplate) element.GetValue(AdornedElementToolTipTemplateProperty);
        }

        private static void AdornedElementToolTipTemplatePropertyChangedCallback(DependencyObject dobj,
                                                                   DependencyPropertyChangedEventArgs dargs)
        {
            var placeholder = dobj as AdornedElementPlaceholder;
            if(placeholder == null)
            {
                return;
            }

            var adornedElement = placeholder.AdornedElement as FrameworkElement;
            if (adornedElement == null)
            {
                return;
            }

            var itemTemplate = dargs.NewValue as DataTemplate;
            if (itemTemplate == null)
            {
                adornedElement.ToolTip = null;
                return;
            }

            var t = CreateToolTip();
            var ic = CreateItemsControl(itemTemplate);

            t.Content = ic;
            adornedElement.ToolTip = t;
        }

        private static ItemsControl CreateItemsControl(DataTemplate itemTempalte)
        {
            var ic = new ItemsControl
                {
                    ItemTemplate = itemTempalte
                };

            var isc = new Binding("(Validation.Errors)");
            ic.SetBinding(ItemsControl.ItemsSourceProperty, isc);

            return ic;
        }

        private static ToolTip CreateToolTip()
        {
            var t = new ToolTip();

            var dcb = new Binding(ExpressionHelper.GetName<ToolTip>(o => o.PlacementTarget))
                {
                    RelativeSource = new RelativeSource(RelativeSourceMode.Self),
                };

            t.SetBinding(FrameworkElement.DataContextProperty, dcb);

            return t;
        }
    }
}