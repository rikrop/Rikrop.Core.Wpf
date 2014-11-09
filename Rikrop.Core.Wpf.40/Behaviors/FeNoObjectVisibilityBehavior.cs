using System;
using System.Diagnostics.Contracts;
using System.Windows;

namespace Rikrop.Core.Wpf.Behaviors
{
    public static class FeNoObjectVisibilityBehavior
    {
        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.RegisterAttached("IsBusy", typeof (bool?), typeof (FeNoObjectVisibilityBehavior), new PropertyMetadata(null, IsBusyPropertyChangedCallback));

        public static readonly DependencyProperty NoNullObjectProperty =
            DependencyProperty.RegisterAttached("NoNullObject", typeof (object), typeof (FeNoObjectVisibilityBehavior), new PropertyMetadata(null, NotNullObjectPropertyChangedCallback));

        public static void SetIsBusy(UIElement element, bool? value)
        {
            element.SetValue(IsBusyProperty, value);
        }

        public static bool? GetIsBusy(UIElement element)
        {
            return (bool?) element.GetValue(IsBusyProperty);
        }

        public static void SetNoNullObject(UIElement element, object value)
        {
            element.SetValue(NoNullObjectProperty, value);
        }

        public static object GetNoNullObject(UIElement element)
        {
            return element.GetValue(NoNullObjectProperty);
        }

        private static void IsBusyPropertyChangedCallback(DependencyObject dobj, DependencyPropertyChangedEventArgs dargs)
        {
            Contract.Assume(dobj is FrameworkElement);
            var fe = (FrameworkElement) dobj;

            SetVisibility(fe);
        }

        private static void NotNullObjectPropertyChangedCallback(DependencyObject dobj, DependencyPropertyChangedEventArgs dargs)
        {
            Contract.Assume(dobj is FrameworkElement);
            var fe = (FrameworkElement) dobj;

            SetVisibility(fe);
        }

        private static void SetVisibility(FrameworkElement fe)
        {
            Contract.Requires<ArgumentNullException>(fe != null);
            fe.Visibility = IsVisible(fe)
                                ? Visibility.Visible
                                : Visibility.Collapsed;
        }

        private static bool IsVisible(FrameworkElement fe)
        {
            Contract.Requires<ArgumentNullException>(fe != null);
            var b = GetIsBusy(fe);
            var o = GetNoNullObject(fe);

            if (b.HasValue)
            {
                if (!b.Value)
                {
                    return o == null;
                }
                return false;
            }
            return false;
        }
    }
}