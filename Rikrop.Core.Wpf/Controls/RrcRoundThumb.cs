using System.Windows;
using System.Windows.Controls.Primitives;

namespace Rikrop.Core.Wpf.Controls
{
    public class RrcRoundThumb : Thumb
    {
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            "CornerRadius",
            typeof (CornerRadius),
            typeof (RrcRoundThumb),
            new PropertyMetadata(default(CornerRadius)));

        public static readonly DependencyProperty TopLeftRadiusProperty = DependencyProperty.Register(
            "TopLeftRadius",
            typeof (double),
            typeof (RrcRoundThumb),
            new PropertyMetadata(default(double), (o, e) => ((RrcRoundThumb) o).OnTopLeftRadiusChanged()));

        public static readonly DependencyProperty TopRightRadiusProperty = DependencyProperty.Register(
            "TopRightRadius",
            typeof (double),
            typeof (RrcRoundThumb),
            new PropertyMetadata(default(double), (o, e) => ((RrcRoundThumb) o).OnTopRightRadiusChanged()));

        public static readonly DependencyProperty BottomLeftRadiusProperty = DependencyProperty.Register(
            "BottomLeftRadius",
            typeof (double),
            typeof (RrcRoundThumb),
            new PropertyMetadata(default(double), (o, e) => ((RrcRoundThumb) o).OnBottomLeftRadiusChanged()));

        public static readonly DependencyProperty BottomRightRadiusProperty = DependencyProperty.Register(
            "BottomRightRadius",
            typeof (double),
            typeof (RrcRoundThumb),
            new PropertyMetadata(default(double), (o, e) => ((RrcRoundThumb) o).OnBottomRightRadiusChanged()));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius) GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public double TopLeftRadius
        {
            get { return (double) GetValue(TopLeftRadiusProperty); }
            set { SetValue(TopLeftRadiusProperty, value); }
        }

        public double TopRightRadius
        {
            get { return (double) GetValue(TopRightRadiusProperty); }
            set { SetValue(TopRightRadiusProperty, value); }
        }

        public double BottomLeftRadius
        {
            get { return (double) GetValue(BottomLeftRadiusProperty); }
            set { SetValue(BottomLeftRadiusProperty, value); }
        }

        public double BottomRightRadius
        {
            get { return (double) GetValue(BottomRightRadiusProperty); }
            set { SetValue(BottomRightRadiusProperty, value); }
        }

        static RrcRoundThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (RrcRoundThumb),
                                                     new FrameworkPropertyMetadata(typeof (RrcRoundThumb)));
        }

        private void OnTopLeftRadiusChanged()
        {
            CornerRadius = new CornerRadius(TopLeftRadius, CornerRadius.TopRight, CornerRadius.BottomRight,
                                            CornerRadius.BottomLeft);
        }

        private void OnTopRightRadiusChanged()
        {
            CornerRadius = new CornerRadius(CornerRadius.TopLeft, TopRightRadius, CornerRadius.BottomRight,
                                            CornerRadius.BottomLeft);
        }

        private void OnBottomLeftRadiusChanged()
        {
            CornerRadius = new CornerRadius(CornerRadius.TopLeft, CornerRadius.TopRight, CornerRadius.BottomRight,
                                            BottomLeftRadius);
        }

        private void OnBottomRightRadiusChanged()
        {
            CornerRadius = new CornerRadius(CornerRadius.TopLeft, CornerRadius.TopRight, BottomRightRadius,
                                            CornerRadius.BottomLeft);
        }
    }
}