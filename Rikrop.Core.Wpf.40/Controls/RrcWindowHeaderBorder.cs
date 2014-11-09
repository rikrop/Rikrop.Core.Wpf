using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Rikrop.Core.Wpf.Controls.Helpers;

namespace Rikrop.Core.Wpf.Controls
{
    public class RrcWindowHeaderBorder : ContentControl
    {
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            "CornerRadius",
            typeof (CornerRadius),
            typeof (RrcWindowHeaderBorder),
            new PropertyMetadata(default(CornerRadius)));

        public static readonly DependencyProperty CanResizeProperty = DependencyProperty.RegisterAttached(
            "CanResize",
            typeof (bool),
            typeof (RrcWindowHeaderBorder),
            new PropertyMetadata(true));

        private Point? _dragStartPosition;

        private Window _parentWindow;

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius) GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        static RrcWindowHeaderBorder()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (RrcWindowHeaderBorder),
                                                     new FrameworkPropertyMetadata(typeof (RrcWindowHeaderBorder)));
        }

        public RrcWindowHeaderBorder()
        {
            Loaded += OnLoaded;
        }

        public static bool GetCanResize(FrameworkElement element)
        {
            return (bool) element.GetValue(CanResizeProperty);
        }

        public static void SetCanResize(FrameworkElement element, bool value)
        {
            element.SetValue(CanResizeProperty, value);
        }

        protected virtual void OnDetachWindow(Window window)
        {
            var relPos = Mouse.GetPosition(window);
            var curPos = window.PointToScreen(relPos);

            var oldWidth = window.ActualWidth;

            if (oldWidth == 0)
            {
                return;
            }

            _parentWindow.WindowState = WindowState.Normal;

            var newRelX = relPos.X*(window.ActualWidth/oldWidth);

            var curRelPos = Mouse.GetPosition(window);

            window.Top = curPos.Y - curRelPos.Y;
            window.Left = curPos.X - newRelX;
        }

        protected virtual void OnMouseDoubleClick(Window window)
        {
            if (!GetCanResize(_parentWindow))
            {
                return;
            }

            if (_parentWindow.WindowState == WindowState.Maximized)
            {
                _parentWindow.WindowState = WindowState.Normal;
            }
            else if (_parentWindow.WindowState == WindowState.Normal)
            {
                _parentWindow.WindowState = WindowState.Maximized;
            }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            if (_parentWindow == null)
            {
                return;
            }

            if (e.ClickCount == 2)
            {
                OnMouseDoubleClick(_parentWindow);
            }
            else if (e.ClickCount == 1)
            {
                _dragStartPosition = Mouse.GetPosition(_parentWindow);
            }
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            _dragStartPosition = null;
        }

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            base.OnPreviewMouseMove(e);

            if (_parentWindow == null)
            {
                return;
            }

            if (_dragStartPosition != null)
            {
                var currentPosition = e.GetPosition(_parentWindow);
                if ((Math.Abs(currentPosition.X - _dragStartPosition.Value.X) >
                     SystemParameters.MinimumHorizontalDragDistance ||
                     Math.Abs(currentPosition.Y - _dragStartPosition.Value.Y) >
                     SystemParameters.MinimumVerticalDragDistance)
                    && Mouse.LeftButton == MouseButtonState.Pressed)
                {
                    if (_parentWindow.WindowState == WindowState.Maximized)
                    {
                        OnDetachWindow(_parentWindow);
                    }
                    WindowDragMove(_parentWindow);
                    _dragStartPosition = null;
                }
            }
        }


        protected virtual void WindowDragMove(Window window)
        {
            try
            {
                if (Mouse.LeftButton == MouseButtonState.Pressed)
                {
                    window.DragMove();
                }
            }
            catch
            {
                // Каким-то чудесным образом Mouse.LeftButton перестала быть Pressed и это событие не было отловлено.
                // В этом случае окошко просто не переместится
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _parentWindow = this.FindVisualParent<Window>();
            if (_parentWindow != null)
            {
                Loaded -= OnLoaded;
            }
        }
    }
}