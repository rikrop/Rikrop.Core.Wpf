using System;
using System.Windows;
using System.Windows.Controls;
using Rikrop.Core.Wpf.Controls.Helpers;

namespace Rikrop.Core.Wpf.Controls
{
    [TemplatePart(Name = "PART_CloseButton", Type = typeof (Button))]
    [TemplatePart(Name = "PART_MinimizeButton", Type = typeof (Button))]
    [TemplatePart(Name = "PART_MaximizeButton", Type = typeof (Button))]
    [TemplatePart(Name = "PART_NormalizeButton", Type = typeof (Button))]
    public class RrcWindowButtons : Control
    {
        public static readonly DependencyProperty CloseButtonVisibilityProperty = DependencyProperty.Register(
            "CloseButtonVisibility",
            typeof (Visibility),
            typeof (RrcWindowButtons),
            new PropertyMetadata(default(Visibility)));

        public static readonly DependencyProperty MaximizeNormalizeButtonsVisibilityProperty = DependencyProperty.
            Register(
                "MaximizeNormalizeButtonsVisibility",
                typeof (Visibility),
                typeof (RrcWindowButtons),
                new PropertyMetadata(default(Visibility)));

        public static readonly DependencyProperty MinimizeButtonVisibilityProperty = DependencyProperty.Register(
            "MinimizeButtonVisibility",
            typeof (Visibility),
            typeof (RrcWindowButtons),
            new PropertyMetadata(default(Visibility)));

        public static readonly DependencyProperty MinimizeButtonStyleProperty = DependencyProperty.Register(
            "MinimizeButtonStyle",
            typeof (Style),
            typeof (RrcWindowButtons),
            new PropertyMetadata(default(Style)));

        public static readonly DependencyProperty MaximizeButtonStyleProperty = DependencyProperty.Register(
            "MaximizeButtonStyle",
            typeof (Style),
            typeof (RrcWindowButtons),
            new PropertyMetadata(default(Style)));

        public static readonly DependencyProperty NormalizeButtonStyleProperty = DependencyProperty.Register(
            "NormalizeButtonStyle",
            typeof (Style),
            typeof (RrcWindowButtons),
            new PropertyMetadata(default(Style)));

        public static readonly DependencyProperty CloseButtonStyleProperty = DependencyProperty.Register(
            "CloseButtonStyle",
            typeof (Style),
            typeof (RrcWindowButtons),
            new PropertyMetadata(default(Style)));

        private Window _parentWindow;
        private Button _normalizeButton;
        private Button _maximizeButton;

        public Visibility CloseButtonVisibility
        {
            get { return (Visibility) GetValue(CloseButtonVisibilityProperty); }
            set { SetValue(CloseButtonVisibilityProperty, value); }
        }

        public Visibility MaximizeNormalizeButtonsVisibility
        {
            get { return (Visibility) GetValue(MaximizeNormalizeButtonsVisibilityProperty); }
            set { SetValue(MaximizeNormalizeButtonsVisibilityProperty, value); }
        }

        public Visibility MinimizeButtonVisibility
        {
            get { return (Visibility) GetValue(MinimizeButtonVisibilityProperty); }
            set { SetValue(MinimizeButtonVisibilityProperty, value); }
        }

        public Style MinimizeButtonStyle
        {
            get { return (Style) GetValue(MinimizeButtonStyleProperty); }
            set { SetValue(MinimizeButtonStyleProperty, value); }
        }

        public Style MaximizeButtonStyle
        {
            get { return (Style) GetValue(MaximizeButtonStyleProperty); }
            set { SetValue(MaximizeButtonStyleProperty, value); }
        }

        public Style NormalizeButtonStyle
        {
            get { return (Style) GetValue(NormalizeButtonStyleProperty); }
            set { SetValue(NormalizeButtonStyleProperty, value); }
        }

        public Style CloseButtonStyle
        {
            get { return (Style) GetValue(CloseButtonStyleProperty); }
            set { SetValue(CloseButtonStyleProperty, value); }
        }

        static RrcWindowButtons()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (RrcWindowButtons),
                                                     new FrameworkPropertyMetadata(typeof (RrcWindowButtons)));
        }

        public RrcWindowButtons()
        {
            Loaded += OnLoaded;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var btnClose = Template.FindName("PART_CloseButton", this) as Button;
            if (btnClose != null)
            {
                btnClose.Click += OnCloseButtonClick;
            }
            var btnMinimize = Template.FindName("PART_MinimizeButton", this) as Button;
            if (btnMinimize != null)
            {
                btnMinimize.Click += OnMinimizeButtonClick;
            }
            _normalizeButton = Template.FindName("PART_NormalizeButton", this) as Button;
            if (_normalizeButton != null)
            {
                _normalizeButton.Click += OnNormalizeButtonClick;
            }
            _maximizeButton = Template.FindName("PART_MaximizeButton", this) as Button;
            if (_maximizeButton != null)
            {
                _maximizeButton.Click += OnMaximizeButtonClick;
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _parentWindow = this.FindVisualParent<Window>();
            if (_parentWindow != null)
            {
                Loaded -= OnLoaded;

                _parentWindow.StateChanged += OnParentWindowStateChanged;

                RefreshButtonsVisibility();
            }
        }

        private void RefreshButtonsVisibility()
        {
            if (_parentWindow == null)
            {
                return;
            }
            if (_parentWindow.WindowState == WindowState.Normal)
            {
                if (MaximizeNormalizeButtonsVisibility == Visibility.Visible)
                {
                    if (_normalizeButton != null)
                    {
                        _normalizeButton.Visibility = Visibility.Collapsed;
                    }
                    if (_maximizeButton != null)
                    {
                        _maximizeButton.Visibility = Visibility.Visible;
                    }
                }
            }
            else
            {
                if (MaximizeNormalizeButtonsVisibility == Visibility.Visible)
                {
                    if (_normalizeButton != null)
                    {
                        _normalizeButton.Visibility = Visibility.Visible;
                    }
                    if (_maximizeButton != null)
                    {
                        _maximizeButton.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        private void OnParentWindowStateChanged(object sender, EventArgs e)
        {
            RefreshButtonsVisibility();
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            if (_parentWindow != null)
            {
                _parentWindow.Close();
            }
        }

        private void OnMinimizeButtonClick(object sender, RoutedEventArgs e)
        {
            if (_parentWindow != null)
            {
                _parentWindow.WindowState = WindowState.Minimized;
            }
        }

        private void OnNormalizeButtonClick(object sender, RoutedEventArgs e)
        {
            if (_parentWindow != null)
            {
                _parentWindow.WindowState = WindowState.Normal;
            }
        }

        private void OnMaximizeButtonClick(object sender, RoutedEventArgs e)
        {
            if (_parentWindow != null)
            {
                _parentWindow.WindowState = WindowState.Maximized;
            }
        }
    }
}