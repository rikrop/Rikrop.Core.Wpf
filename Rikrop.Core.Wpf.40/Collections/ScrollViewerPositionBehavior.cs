using System;
using System.Collections.Specialized;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Rikrop.Core.Wpf.Controls.Helpers;

namespace Rikrop.Core.Wpf.Collections
{
    public static class ScrollViewerPositionBehavior
    {
        public static readonly DependencyProperty BottomReachedCommandProperty =
            DependencyProperty.RegisterAttached("BottomReachedCommand", typeof (ICommand), typeof (ScrollViewerPositionBehavior),
                                                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, OnBottomReachedCommandChangedCallback));

        public static readonly DependencyProperty BottomThresholdProperty =
            DependencyProperty.RegisterAttached("BottomThreshold", typeof (double?), typeof (ScrollViewerPositionBehavior),
                                                new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.None, BottomTresholdPropertyChangedCallback));

        public static readonly DependencyProperty ScrollTopOnResetOfCollectionProperty =
            DependencyProperty.RegisterAttached("ScrollTopOnResetOfCollection", typeof (INotifyCollectionChanged), typeof (ScrollViewerPositionBehavior),
                                                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, ScrollTopOnResetOfCollectionPropertyChangedCallback));

        public static void SetScrollTopOnResetOfCollection(UIElement element, INotifyCollectionChanged value)
        {
            element.SetValue(ScrollTopOnResetOfCollectionProperty, value);
        }

        public static INotifyCollectionChanged GetScrollTopOnResetOfCollection(UIElement element)
        {
            return (INotifyCollectionChanged) element.GetValue(ScrollTopOnResetOfCollectionProperty);
        }

        public static void SetBottomReachedCommand(UIElement element, ICommand value)
        {
            element.SetValue(BottomReachedCommandProperty, value);
        }

        public static ICommand GetBottomReachedCommand(UIElement element)
        {
            return (ICommand) element.GetValue(BottomReachedCommandProperty);
        }

        public static void SetBottomThreshold(UIElement element, double? value)
        {
            element.SetValue(BottomThresholdProperty, value);
        }

        public static double? GetBottomThreshold(UIElement element)
        {
            return (double?) element.GetValue(BottomThresholdProperty);
        }

        private static async void ScrollTopOnResetOfCollectionPropertyChangedCallback(DependencyObject behaviourTarget, DependencyPropertyChangedEventArgs dargs)
        {
            var sv = behaviourTarget as ScrollViewer ?? await GetChildScrollViewerAsync(behaviourTarget);

            if (sv == null)
            {
                return;
            }

            sv.ScrollToHome();
            var ncc = dargs.NewValue as INotifyCollectionChanged;
            if (ncc != null)
            {
                ncc.CollectionChanged += (sender, args) =>
                                             {
                                                 if (args.Action == NotifyCollectionChangedAction.Reset)
                                                 {
                                                     sv.ScrollToHome();
                                                 }
                                             };
            }
        }

        private static async void BottomTresholdPropertyChangedCallback(DependencyObject behaviourTarget, DependencyPropertyChangedEventArgs dargs)
        {
            var isChildScrollViewer = false;
            var sv = behaviourTarget as ScrollViewer;
            if (sv == null)
            {
                isChildScrollViewer = true;
                sv = await GetChildScrollViewerAsync(behaviourTarget);
            }

            if (sv == null)
            {
                return;
            }

            if (isChildScrollViewer)
            {
                SetBottomThreshold(sv, dargs.NewValue as double?);                
            }
            CheckScrollViewerBottomReach(sv);
        }

        private static async void OnBottomReachedCommandChangedCallback(DependencyObject behaviourTarget, DependencyPropertyChangedEventArgs dargs)
        {
            var isChildScrollViewer = false;
            var sv = behaviourTarget as ScrollViewer;
            if (sv == null)
            {
                isChildScrollViewer = true;
                sv = await GetChildScrollViewerAsync(behaviourTarget);
            }

            if (sv == null)
            {
                return;
            }

            var command = dargs.NewValue as ICommand;
            if (isChildScrollViewer)
            {
                SetBottomReachedCommand(sv, command);                
            }

            if (command == null)
            {
                ProcessOldScrollViewer(sv);
            }
            else
            {
                ProcessNewScrollViewer(sv);
            }
        }

        private static Task<ScrollViewer> GetChildScrollViewerAsync(DependencyObject target)
        {
            var tcs = new TaskCompletionSource<ScrollViewer>();

            var sv = target as ScrollViewer;
            if (sv != null)
            {
                tcs.SetResult(sv);
            }
            else
            {
                var fe = target as FrameworkElement;
                if (fe == null)
                {
                    tcs.SetResult(null);
                }
                else
                {
                    if (fe.IsLoaded)
                    {
                        sv = fe.FindVisualChild<ScrollViewer>();
                        tcs.SetResult(sv);
                    }
                    else
                    {
                        RoutedEventHandler handler = null;
                        handler = (sender, args) =>
                                      {
                                          fe.Loaded -= handler;

                                          var sc = fe.FindVisualChild<ScrollViewer>();
                                          tcs.SetResult(sc);
                                      };

                        fe.Loaded += handler;
                    }
                }
            }
            return tcs.Task;
        }

        private static void ProcessNewScrollViewer(ScrollViewer scrollViewer)
        {
            Contract.Requires<ArgumentNullException>(scrollViewer != null);

            CheckScrollViewerBottomReach(scrollViewer);
            scrollViewer.ScrollChanged += ScrollViewerOnScrollChanged;
        }

        private static void ProcessOldScrollViewer(ScrollViewer scrollViewer)
        {
            scrollViewer.ScrollChanged -= ScrollViewerOnScrollChanged;
        }

        private static void ScrollViewerOnScrollChanged(object sender, ScrollChangedEventArgs scrollChangedEventArgs)
        {
            var sv = sender as ScrollViewer;
            if (sv == null)
            {
                return;
            }
            CheckScrollViewerBottomReach(sv);
        }

        private static void CheckScrollViewerBottomReach(ScrollViewer scrollViewer)
        {
            Contract.Requires<ArgumentNullException>(scrollViewer != null);

            var command = GetBottomReachedCommand(scrollViewer);
            if (command == null)
            {
                return;
            }
            var treshhold = GetBottomThreshold(scrollViewer);

            var difference = scrollViewer.ExtentHeight - scrollViewer.VerticalOffset - scrollViewer.ViewportHeight;
            if (treshhold.HasValue
                    ? difference <= treshhold && command.CanExecute(difference)
                    : command.CanExecute(difference))
            {
                command.Execute(difference);
            }
        }
    }

    public interface IScrollPositionRequeser
    {
        event Action RequestScrollToHome;
    }
}