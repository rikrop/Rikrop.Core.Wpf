using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Rikrop.Core.Wpf.Controls
{
    public class RrcImageBrushButton : Button
    {
        public static readonly DependencyProperty ImageBrushProperty = DependencyProperty.Register(
            "ImageBrush",
            typeof(Brush),
            typeof (RrcImageBrushButton),
            new PropertyMetadata(default(Brush)));

        public static readonly DependencyProperty NormalImageBrushProperty = DependencyProperty.Register(
            "NormalImageBrush",
            typeof (Brush),
            typeof (RrcImageBrushButton),
            new PropertyMetadata(default(Brush)));

        public static readonly DependencyProperty MouseOverImageBrushProperty = DependencyProperty.Register(
            "MouseOverImageBrush",
            typeof (Brush),
            typeof (RrcImageBrushButton),
            new PropertyMetadata(default(Brush)));

        public static readonly DependencyProperty PressedImageBrushProperty = DependencyProperty.Register(
            "PressedImageBrush",
            typeof (Brush),
            typeof (RrcImageBrushButton),
            new PropertyMetadata(default(Brush)));

        public static readonly DependencyProperty DisabledImageBrushProperty = DependencyProperty.Register(
            "DisabledImageBrush",
            typeof (Brush),
            typeof (RrcImageBrushButton),
            new PropertyMetadata(default(Brush)));

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            "CornerRadius",
            typeof(CornerRadius),
            typeof(RrcImageBrushButton),
            new PropertyMetadata(default(CornerRadius)));

        public Brush ImageBrush
        {
            get { return (Brush)GetValue(ImageBrushProperty); }
            set { SetValue(ImageBrushProperty, value); }
        }

        public Brush NormalImageBrush
        {
            get { return (Brush)GetValue(NormalImageBrushProperty); }
            set { SetValue(NormalImageBrushProperty, value); }
        }

        public Brush MouseOverImageBrush
        {
            get { return (Brush) GetValue(MouseOverImageBrushProperty); }
            set { SetValue(MouseOverImageBrushProperty, value); }
        }

        public Brush PressedImageBrush
        {
            get { return (Brush) GetValue(PressedImageBrushProperty); }
            set { SetValue(PressedImageBrushProperty, value); }
        }

        public Brush DisabledImageBrush
        {
            get { return (Brush) GetValue(DisabledImageBrushProperty); }
            set { SetValue(DisabledImageBrushProperty, value); }
        }

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        static RrcImageBrushButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (RrcImageBrushButton), new FrameworkPropertyMetadata(typeof (RrcImageBrushButton)));
        }
    }
}