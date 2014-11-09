using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Rikrop.Core.Wpf.Controls.Helpers;

namespace Rikrop.Core.Wpf.Helpers
{
    public class ScrollViewerHelper
    {
        public static readonly DependencyProperty MouseWheelHelpProperty = DependencyProperty.RegisterAttached(
            "MouseWheelHelp",
            typeof (bool),
            typeof (ScrollViewerHelper),
            new UIPropertyMetadata(false, MouseWheelHelpChangedCallback));

        public static bool GetMouseWheelHelp(FrameworkElement obj)
        {
            return (bool) obj.GetValue(MouseWheelHelpProperty);
        }

        public static void SetMouseWheelHelp(FrameworkElement obj, bool value)
        {
            obj.SetValue(MouseWheelHelpProperty, value);
        }

        public static void OnScrollViewerPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            //Спасибо: http://serialseb.blogspot.com/2007/09/wpf-tips-6-preventing-scrollviewer-from.html

            var scrollControl = sender as ScrollViewer ?? ((FrameworkElement) sender).FindVisualChild<ScrollViewer>();
            if (!e.Handled && scrollControl != null)
            {
                if ((e.Delta > 0 && scrollControl.VerticalOffset == 0)
                    ||
                    (e.Delta <= 0 &&
                     scrollControl.VerticalOffset >= scrollControl.ExtentHeight - scrollControl.ViewportHeight))
                {
                    e.Handled = true;
                    var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
                                       {
                                           RoutedEvent = UIElement.MouseWheelEvent,
                                           Source = sender
                                       };
                    var parent = scrollControl.FindVisualParent<UIElement>();
                    if (parent != null)
                    {
                        parent.RaiseEvent(eventArg);
                    }
                }
            }
        }

        private static void MouseWheelHelpChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var frameworkElement = (FrameworkElement) d;
            if ((bool) e.NewValue)
            {
                if (d is ScrollViewer)
                {
                    ((ScrollViewer) d).PreviewMouseWheel += OnScrollViewerPreviewMouseWheel;
                }
                else
                {
                    frameworkElement.Loaded += OnFrameworkElementLoaded;
                }
            }
            else
            {
                frameworkElement.Loaded -= OnFrameworkElementLoaded;
                if (d is ScrollViewer)
                {
                    ((ScrollViewer) d).PreviewMouseWheel -= OnScrollViewerPreviewMouseWheel;
                }
            }
        }

        private static void OnFrameworkElementLoaded(object sender, RoutedEventArgs e)
        {
            var frameworkElement = (FrameworkElement) sender;
            frameworkElement.Loaded -= OnFrameworkElementLoaded;

            if (!(sender is ScrollViewer))
            {
                var scrollViewer = frameworkElement.FindVisualChild<ScrollViewer>();
                if (scrollViewer != null)
                {
                    scrollViewer.PreviewMouseWheel += OnScrollViewerPreviewMouseWheel;
                }
                else
                {
                    frameworkElement.PreviewMouseWheel += OnScrollViewerPreviewMouseWheel;
                }
            }
        }
    }
}