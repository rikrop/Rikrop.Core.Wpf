using System;
using System.Diagnostics.Contracts;
using System.Windows;
using System.Windows.Shell;
using Rikrop.Core.Wpf.Controls.Helpers;

namespace Rikrop.Core.Wpf.Behaviors
{
    public class RoundWindowChromeCaptionHeightToElementHeight
    {
        public static readonly DependencyProperty RoundProperty =
            DependencyProperty.RegisterAttached("Round", typeof (bool), typeof (RoundWindowChromeCaptionHeightToElementHeight), new PropertyMetadata(false, RoundPropertyChangedCallback));

        public static void SetRound(UIElement element, bool value)
        {
            element.SetValue(RoundProperty, value);
        }

        public static bool GetRound(UIElement element)
        {
            return (bool) element.GetValue(RoundProperty);
        }

        private static void RoundPropertyChangedCallback(DependencyObject dobj, DependencyPropertyChangedEventArgs dargs)
        {
            Contract.Assume(dobj is FrameworkElement);
            var fe = (FrameworkElement) dobj;

            if ((bool) dargs.NewValue)
            {
                StartSizeTracking(fe);
            }
            else
            {
                StopSizeTracking(fe);
            }
        }

        private static void StartSizeTracking(FrameworkElement fe)
        {
            Contract.Requires<ArgumentNullException>(fe != null);

            SizeWindowChromeToHeigth(fe);
            fe.SizeChanged += FeOnSizeChanged;
        }

        private static void FeOnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            SizeWindowChromeToHeigth((FrameworkElement) sender);
        }

        private static void StopSizeTracking(FrameworkElement fe)
        {
            Contract.Requires<ArgumentNullException>(fe != null);
            fe.SizeChanged -= FeOnSizeChanged;
        }

        private static void SizeWindowChromeToHeigth(FrameworkElement fe)
        {
            Contract.Requires<ArgumentNullException>(fe != null);
            var w = fe.FindVisualParent<Window>();
            if (w == null)
            {
                return;
            }
            // Unsupported in 4.0
            //var wch = WindowChrome.GetWindowChrome(w);
            //if (wch != null)
            //{
            //    wch.CaptionHeight = fe.ActualHeight;
            //}
        }
    }
}