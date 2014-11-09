namespace Rikrop.Core.Wpf.Controls.Watermark
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Media;

    internal class WatermarkAdorner : Adorner
    {
        private readonly ContentPresenter _contentPresenter;
        private readonly SizeToContent _sizeToContent;

        public WatermarkAdorner(FrameworkElement adornedElement, object watermark) :
            base(adornedElement)
        {
            IsHitTestVisible = false;
            _sizeToContent = RrcWatermarkBehavior.GetSizeToContent(adornedElement);

            var margin = new Thickness(0);
            if (Control is Control)
            {
                var ctrl = (Control)Control;
                if (RrcWatermarkBehavior.GetAllowUseControlPadding(Control))
                {
                    margin = new Thickness(ctrl.Padding.Left + ctrl.BorderThickness.Left,
                                           ctrl.Padding.Top + ctrl.BorderThickness.Top,
                                           ctrl.Padding.Right + ctrl.BorderThickness.Right,
                                           ctrl.Padding.Bottom + ctrl.BorderThickness.Bottom);
                }
                else
                {
                    margin = new Thickness(ctrl.BorderThickness.Left,
                                           ctrl.BorderThickness.Top,
                                           ctrl.BorderThickness.Right,
                                           ctrl.BorderThickness.Bottom);
                }
            }

            _contentPresenter =
                new ContentPresenter
                {
                    Focusable = false,
                    Content = watermark,
                    Margin = margin,
                    VerticalAlignment = watermark is FrameworkElement
                                            ? ((FrameworkElement)watermark).VerticalAlignment
                                            : VerticalAlignment.Center,
                };
        }

        private FrameworkElement Control
        {
            get { return (FrameworkElement)AdornedElement; }
        }

        protected override Visual GetVisualChild(int index)
        {
            return _contentPresenter;
        }

        protected override int VisualChildrenCount
        {
            get { return 1; }
        }

        protected override Size MeasureOverride(Size constraint)
        {
            if (_sizeToContent != SizeToContent.Manual)
            {
                _contentPresenter.Measure(constraint);
                return AdornedElement.RenderSize;
            }
            _contentPresenter.Measure(AdornedElement.RenderSize);
            return AdornedElement.RenderSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            _contentPresenter.Arrange(new Rect(finalSize));
            if (_sizeToContent != SizeToContent.Manual &&
                _contentPresenter.ActualWidth > 0 && _contentPresenter.ActualHeight > 0)
            {
                double width = Control.ActualWidth;
                double height = Control.ActualHeight;
                if (_sizeToContent == SizeToContent.Width || _sizeToContent == SizeToContent.WidthAndHeight)
                {
                    width = _contentPresenter.ActualWidth + _contentPresenter.Margin.Left + _contentPresenter.Margin.Right;
                }

                if (_sizeToContent == SizeToContent.Height || _sizeToContent == SizeToContent.WidthAndHeight)
                {
                    height = _contentPresenter.ActualHeight + _contentPresenter.Margin.Top + _contentPresenter.Margin.Bottom;
                }

                if ((width != Control.ActualWidth &&
                     (_sizeToContent == SizeToContent.Width || _sizeToContent == SizeToContent.WidthAndHeight) ||
                     height != Control.ActualHeight &&
                     (_sizeToContent == SizeToContent.Height || _sizeToContent == SizeToContent.WidthAndHeight)) &&
                    width > 0 && height > 0)
                {
                    Control.Width = width;
                    Control.Height = height;

                    var size = new Size(width, height);
                    Control.Measure(size);
                    Control.Arrange(new Rect(size));
                    Control.UpdateLayout();
                }
            }
            return finalSize;
        }

        public void Cleanup()
        {
            _contentPresenter.Content = null;
        }
    }
}
