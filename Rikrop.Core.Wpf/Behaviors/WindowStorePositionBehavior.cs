using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Windows;

namespace Rikrop.Core.Wpf.Behaviors
{
    public static class WindowStorePositionBehavior
    {
        public static readonly DependencyProperty ProviderProperty =
            DependencyProperty.RegisterAttached("Provider",
                                                typeof (IWindowPositionProvider),
                                                typeof (WindowStorePositionBehavior),
                                                new PropertyMetadata(default(IWindowPositionProvider), OnProviderPropertyChangedCallback));

        public static void SetProvider(UIElement element, IWindowPositionProvider value)
        {
            element.SetValue(ProviderProperty, value);
        }

        public static IWindowPositionProvider GetProvider(UIElement element)
        {
            return (IWindowPositionProvider) element.GetValue(ProviderProperty);
        }

        public static void TrackWindow(Window w, IWindowPositionProvider provider)
        {
            Contract.Requires<ArgumentNullException>(w != null);
            Contract.Requires<ArgumentNullException>(provider != null);

            SetSize(w, provider);
            w.SizeChanged += WindowOnSizeChanged;
            w.Closing += WindowOnClosing;
        }

        public static void LeaveWindow(Window w)
        {
            Contract.Requires<ArgumentNullException>(w != null);

            w.SizeChanged -= WindowOnSizeChanged;
            w.Closing -= WindowOnClosing;
            w.Loaded -= WindowOnLoaded;
        }

        public static void SetSize(Window w, IWindowPositionProvider provider)
        {
            Contract.Requires<ArgumentNullException>(w != null);
            Contract.Requires<ArgumentNullException>(provider != null);

            var rect = GetPositionRect(provider);
            Contract.Assume(!rect.IsEmpty);

            var we = SystemParameters.WorkArea;

            var s = GetWorkAreaRoundedSize(rect.Size);

            if (w.WindowStartupLocation == WindowStartupLocation.Manual)
            {
                w.Top = rect.Top + s.Height > we.Top + we.Height
                            ? Math.Max(we.Top, (we.Height/2) - (s.Height/2) + we.Top)
                            : rect.Top;

                w.Left = rect.Left + s.Width > we.Left + we.Width
                             ? Math.Max(we.Left, (we.Width/2) - (s.Width/2) + we.Left)
                             : rect.Left;
            }
            else
            {
                w.Top = Math.Max(we.Top, (we.Height/2) - (s.Height/2) + we.Top);
                w.Left = Math.Max(we.Left, (we.Width/2) - (s.Width/2) + we.Left);
            }

            w.Height = s.Height;
            w.Width = s.Width;

            //Восстановили развернутое/нормальное положение окна
            w.WindowState = provider.IsMaximazed
                                ? WindowState.Maximized
                                : WindowState.Normal;
        }

        private static Size GetWorkAreaRoundedSize(Size s)
        {
            var we = SystemParameters.WorkArea;
            var height = Math.Min(s.Height, we.Height);
            var width = Math.Min(s.Width, we.Width);
            return new Size(width, height);
        }

        private static Rect GetPositionRect(IWindowPositionProvider provider)
        {
            Contract.Requires<ArgumentNullException>(provider != null);

            var rect = provider.Position;

            //Если ничего подходящего из настроек не загружено, устанвливаем Значения ширины и высоты окна
            if (rect.IsEmpty)
            {
                return new Rect(0, 0, 1280, 1024);
            }
            return rect;
        }

        private static void OnProviderPropertyChangedCallback(DependencyObject dobj, DependencyPropertyChangedEventArgs dargs)
        {
            Contract.Assume(dobj is Window);
            var w = (Window) dobj;

            var provider = dargs.NewValue as IWindowPositionProvider;
            if (provider != null)
            {
                if (w.IsLoaded)
                {
                    TrackWindow(w, provider);
                }
                else
                {
                    w.Loaded += WindowOnLoaded;
                }
            }
            else
            {
                LeaveWindow(w);
            }
        }

        private static void WindowOnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var w = (Window) sender;

            var provider = GetProvider(w);
            if (provider != null)
            {
                TrackWindow(w, provider);
            }
        }

        private static void WindowOnClosing(object sender, CancelEventArgs eventArgs)
        {
            var w = (Window) sender;

            LeaveWindow(w);

            var provider = GetProvider(w);
            if (provider != null)
            {
                SaveState(w, provider);
            }
        }

        private static void WindowOnSizeChanged(object sender, SizeChangedEventArgs args)
        {
            var w = (Window) sender;

            var provider = GetProvider(w);
            if (provider != null)
            {
                SaveState(w, provider);
            }
            else
            {
                LeaveWindow(w);
            }
        }

        private static async void SaveState(Window w, IWindowPositionProvider provider)
        {
            Contract.Requires<ArgumentNullException>(w != null);
            Contract.Requires<ArgumentNullException>(provider != null);

            provider.Position = w.RestoreBounds;
            provider.IsMaximazed = w.WindowState == WindowState.Maximized;
            await provider.SaveSettings();
        }
    }

    public interface IWindowPositionProvider
    {
        Rect Position { get; set; }
        bool IsMaximazed { get; set; }
        Task SaveSettings();
    }
}