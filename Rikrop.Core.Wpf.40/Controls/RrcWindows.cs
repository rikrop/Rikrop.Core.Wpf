using System;
using System.Diagnostics.Contracts;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shell;

namespace Rikrop.Core.Wpf.Controls
{
    public class RrcWindow : Window
    {
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            "Header",
            typeof (object),
            typeof (RrcWindow),
            new PropertyMetadata(null));

        public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(
            "HeaderTemplate",
            typeof (DataTemplate),
            typeof (RrcWindow),
            new PropertyMetadata(null));

        public static readonly DependencyProperty HeaderTemplateSelectorProperty = DependencyProperty.Register(
            "HeaderTemplateSelector",
            typeof (DataTemplateSelector),
            typeof (RrcWindow),
            new PropertyMetadata(null));

        public static readonly DependencyProperty WindowButtonsStyleProperty = DependencyProperty.Register(
            "WindowButtonsStyle",
            typeof (Style),
            typeof (RrcWindow),
            new PropertyMetadata(null));

        public static readonly DependencyProperty HeaderBorderThicknessProperty = DependencyProperty.Register(
            "HeaderBorderThickness",
            typeof (Thickness),
            typeof (RrcWindow),
            new PropertyMetadata(default(Thickness)));

        public static readonly DependencyProperty HeaderBorderBrushProperty = DependencyProperty.Register(
            "HeaderBorderBrush",
            typeof (Brush),
            typeof (RrcWindow),
            new PropertyMetadata(default(Brush)));

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            "CornerRadius",
            typeof (CornerRadius),
            typeof (RrcWindow),
            new PropertyMetadata(default(CornerRadius)));

        public static readonly DependencyProperty ThumbSizeProperty = DependencyProperty.Register(
            "ThumbSize",
            typeof (double),
            typeof (RrcWindow),
            new PropertyMetadata(default(double), ThumbSizePropertyChangedCallback));

        public double ThumbSize
        {
            get { return (double) GetValue(ThumbSizeProperty); }
            set { SetValue(ThumbSizeProperty, value); }
        }

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius) GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public Brush HeaderBorderBrush
        {
            get { return (Brush) GetValue(HeaderBorderBrushProperty); }
            set { SetValue(HeaderBorderBrushProperty, value); }
        }

        public Thickness HeaderBorderThickness
        {
            get { return (Thickness) GetValue(HeaderBorderThicknessProperty); }
            set { SetValue(HeaderBorderThicknessProperty, value); }
        }

        public object Header
        {
            get { return GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate) GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        public DataTemplateSelector HeaderTemplateSelector
        {
            get { return (DataTemplateSelector) GetValue(HeaderTemplateSelectorProperty); }
            set { SetValue(HeaderTemplateSelectorProperty, value); }
        }

        public Style WindowButtonsStyle
        {
            get { return (Style) GetValue(WindowButtonsStyleProperty); }
            set { SetValue(WindowButtonsStyleProperty, value); }
        }

        static RrcWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (RrcWindow), new FrameworkPropertyMetadata(typeof (RrcWindow)));
        }

        private static void ThumbSizePropertyChangedCallback(DependencyObject dobj, DependencyPropertyChangedEventArgs dargs)
        {
            Contract.Assume(dobj is RrcWindow);

            var w = dobj as RrcWindow;
            w.SetWindowChromeResizeBorderThicknessIfPossible();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            SetWindowChromeResizeBorderThicknessIfPossible();
        }

        private void SetWindowChromeResizeBorderThicknessIfPossible()
        {
            // Unsupported in 4.0
            //var wch = WindowChrome.GetWindowChrome(this);
            //if (wch == null)
            //{
            //    return;
            //}

            //wch.ResizeBorderThickness = new Thickness(ThumbSize);
        }
    }
}