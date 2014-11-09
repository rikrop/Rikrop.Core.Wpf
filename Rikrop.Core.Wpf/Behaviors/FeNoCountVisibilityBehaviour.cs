using System;
using System.Diagnostics.Contracts;
using System.Windows;

namespace Rikrop.Core.Wpf.Behaviors
{
    public static class FeNoCountVisibilityBehaviour
    {
        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.RegisterAttached("IsBusy", typeof (bool?), typeof (FeNoCountVisibilityBehaviour), new PropertyMetadata(null, IsBusyPropertyChangedCallback));

        public static readonly DependencyProperty CountProperty =
            DependencyProperty.RegisterAttached("Count", typeof (int?), typeof (FeNoCountVisibilityBehaviour), new PropertyMetadata(null, CountPropertyChangedCallback));

        public static void SetIsBusy(UIElement element, bool? value)
        {
            element.SetValue(IsBusyProperty, value);
        }

        public static bool? GetIsBusy(UIElement element)
        {
            return (bool?) element.GetValue(IsBusyProperty);
        }

        public static void SetCount(UIElement element, int? value)
        {
            element.SetValue(CountProperty, value);
        }

        public static int? GetCount(UIElement element)
        {
            return (int?) element.GetValue(CountProperty);
        }

        private static void IsBusyPropertyChangedCallback(DependencyObject dobj, DependencyPropertyChangedEventArgs dargs)
        {
            Contract.Assume(dobj is FrameworkElement);
            var fe = (FrameworkElement) dobj;

            SetVisibility(fe);
        }

        private static void CountPropertyChangedCallback(DependencyObject dobj, DependencyPropertyChangedEventArgs dargs)
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
            var c = GetCount(fe);

            if (b.HasValue && c.HasValue)
            {
                return !b.Value && c.Value == 0;
            }

            if (c.HasValue)
            {
                return c.Value > 0;
            }

            return false;
        }
    }
}