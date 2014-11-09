using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Rikrop.Core.Wpf.Controls.Helpers;

namespace Rikrop.Core.Wpf.Controls
{
    [TemplatePart(Name = "PART_LeftThumb", Type = typeof (Thumb))]
    [TemplatePart(Name = "PART_RightThumb", Type = typeof (Thumb))]
    [TemplatePart(Name = "PART_TopThumb", Type = typeof (Thumb))]
    [TemplatePart(Name = "PART_BottomThumb", Type = typeof (Thumb))]
    [TemplatePart(Name = "PART_TopLeftThumb", Type = typeof (Thumb))]
    [TemplatePart(Name = "PART_TopRightThumb", Type = typeof (Thumb))]
    [TemplatePart(Name = "PART_BottomLeftThumb", Type = typeof (Thumb))]
    [TemplatePart(Name = "PART_BottomRightThumb", Type = typeof (Thumb))]
    [TemplatePart(Name = "PART_ContentBorder", Type = typeof (Border))]
    public class RrcWindowResizeBorder : ContentControl
    {
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            "CornerRadius",
            typeof (CornerRadius),
            typeof (RrcWindowResizeBorder),
            new PropertyMetadata(default(CornerRadius)));

        private Window _parentWindow;
        private Border _contentBorder;

        private Thumb _leftThumb;
        private Thumb _topLeftThumb;
        private Thumb _topThumb;
        private Thumb _topRightThumb;
        private Thumb _rightThumb;
        private Thumb _bottomRightThumb;
        private Thumb _bottomThumb;
        private Thumb _bottomLeftThumb;

        public static readonly DependencyProperty ThumbSizeProperty = DependencyProperty.Register(
            "ThumbSize",
            typeof (double),
            typeof (RrcWindowResizeBorder),
            new PropertyMetadata(default(double)));

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

        static RrcWindowResizeBorder()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (RrcWindowResizeBorder),
                                                     new FrameworkPropertyMetadata(typeof (RrcWindowResizeBorder)));
        }

        public RrcWindowResizeBorder()
        {
            Loaded += OnLoaded;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _leftThumb = Template.FindName("PART_LeftThumb", this) as Thumb;
            if (_leftThumb != null)
            {
                _leftThumb.DragDelta += (o, e) => LeftDragDelta(e);
            }
            _topLeftThumb = Template.FindName("PART_TopLeftThumb", this) as Thumb;
            if (_topLeftThumb != null)
            {
                _topLeftThumb.DragDelta += (o, e) => LeftDragDelta(e);
                _topLeftThumb.DragDelta += (o, e) => TopDragDelta(e);
            }
            _topThumb = Template.FindName("PART_TopThumb", this) as Thumb;
            if (_topThumb != null)
            {
                _topThumb.DragDelta += (o, e) => TopDragDelta(e);
            }
            _topRightThumb = Template.FindName("PART_TopRightThumb", this) as Thumb;
            if (_topRightThumb != null)
            {
                _topRightThumb.DragDelta += (o, e) => RightDragDelta(e);
                _topRightThumb.DragDelta += (o, e) => TopDragDelta(e);
            }
            _rightThumb = Template.FindName("PART_RightThumb", this) as Thumb;
            if (_rightThumb != null)
            {
                _rightThumb.DragDelta += (o, e) => RightDragDelta(e);
            }
            _bottomRightThumb = Template.FindName("PART_BottomRightThumb", this) as Thumb;
            if (_bottomRightThumb != null)
            {
                _bottomRightThumb.DragDelta += (o, e) => RightDragDelta(e);
                _bottomRightThumb.DragDelta += (o, e) => BottomDragDelta(e);
            }
            _bottomThumb = Template.FindName("PART_BottomThumb", this) as Thumb;
            if (_bottomThumb != null)
            {
                _bottomThumb.DragDelta += (o, e) => BottomDragDelta(e);
            }
            _bottomLeftThumb = Template.FindName("PART_BottomLeftThumb", this) as Thumb;
            if (_bottomLeftThumb != null)
            {
                _bottomLeftThumb.DragDelta += (o, e) => LeftDragDelta(e);
                _bottomLeftThumb.DragDelta += (o, e) => BottomDragDelta(e);
            }

            _contentBorder = Template.FindName("PART_ContentBorder", this) as Border;
        }

        private void LeftDragDelta(DragDeltaEventArgs e)
        {
            if (_parentWindow == null)
            {
                return;
            }

            var newWidth = _parentWindow.ActualWidth - e.HorizontalChange;

            if (newWidth >= _parentWindow.MinWidth &&
                newWidth <= _parentWindow.MaxWidth)
            {
                _parentWindow.Width = newWidth;
                _parentWindow.Left = _parentWindow.Left + e.HorizontalChange;
            }
        }

        private void RightDragDelta(DragDeltaEventArgs e)
        {
            if (_parentWindow == null)
            {
                return;
            }

            var newWidth = _parentWindow.ActualWidth + e.HorizontalChange;

            if (newWidth >= _parentWindow.MinWidth &&
                newWidth <= _parentWindow.MaxWidth)
            {
                _parentWindow.Width = newWidth;
            }
        }

        private void TopDragDelta(DragDeltaEventArgs e)
        {
            if (_parentWindow == null)
            {
                return;
            }

            var newHeight = _parentWindow.ActualHeight - e.VerticalChange;

            if (newHeight >= _parentWindow.MinHeight &&
                newHeight <= _parentWindow.MaxHeight)
            {
                _parentWindow.Height = newHeight;
                _parentWindow.Top = _parentWindow.Top + e.VerticalChange;
            }
        }

        private void BottomDragDelta(DragDeltaEventArgs e)
        {
            if (_parentWindow == null)
            {
                return;
            }

            var newHeight = _parentWindow.ActualHeight + e.VerticalChange;

            if (newHeight >= _parentWindow.MinHeight &&
                newHeight <= _parentWindow.MaxHeight)
            {
                _parentWindow.Height = newHeight;
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _parentWindow = this.FindVisualParent<Window>();
            if (_parentWindow != null)
            {
                Loaded -= OnLoaded;

                RefreshBorderVisibility();
                _parentWindow.StateChanged += OnParentWindowStateChanged;
            }
        }

        private void OnParentWindowStateChanged(object sender, EventArgs e)
        {
            RefreshBorderVisibility();
        }


        private void RefreshBorderVisibility()
        {
            if (_contentBorder == null)
            {
                return;
            }

            if (_parentWindow.WindowState == WindowState.Normal)
            {
                _contentBorder.BorderBrush = BorderBrush;

                _contentBorder.BorderThickness = BorderThickness;
                _contentBorder.CornerRadius = CornerRadius;

                SetThumbsVisibility(Visibility.Visible);
            }
            else
            {
                _contentBorder.BorderBrush = Brushes.Transparent;
                _contentBorder.CornerRadius = new CornerRadius(0);
                //if (_parentWindow.AllowsTransparency)
                //{
                //    _contentBorder.BorderThickness = new Thickness(7); //чтобы избежать вылезания окна за пределы экрана
                //}
                //else
                //{
                _contentBorder.BorderThickness = new Thickness(0);
                //}


                SetThumbsVisibility(Visibility.Collapsed);
            }
        }

        private void SetThumbsVisibility(Visibility visibility)
        {
            if (_topThumb != null)
            {
                _topThumb.Visibility = visibility;
            }
            if (_topLeftThumb != null)
            {
                _topLeftThumb.Visibility = visibility;
            }
            if (_topRightThumb != null)
            {
                _topRightThumb.Visibility = visibility;
            }
            if (_bottomThumb != null)
            {
                _bottomThumb.Visibility = visibility;
            }
            if (_bottomLeftThumb != null)
            {
                _bottomLeftThumb.Visibility = visibility;
            }
            if (_bottomRightThumb != null)
            {
                _bottomRightThumb.Visibility = visibility;
            }
            if (_leftThumb != null)
            {
                _leftThumb.Visibility = visibility;
            }
            if (_rightThumb != null)
            {
                _rightThumb.Visibility = visibility;
            }
        }
    }
}