namespace Rikrop.Core.Wpf.Controls.Watermark
{
    using System.Windows;

    public static class RrcWatermarkBehavior
    {
        #region Watermark Property

        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.RegisterAttached(
            "Watermark",
            typeof(object),
            typeof(RrcWatermarkBehavior),
            new PropertyMetadata(default(object), WatermarkChangedCallback));

        public static object GetWatermark(FrameworkElement obj)
        {
            return obj.GetValue(WatermarkProperty);
        }

        public static void SetWatermark(FrameworkElement obj, object value)
        {
            obj.SetValue(WatermarkProperty, value);
        }

        public static void WatermarkChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (FrameworkElement)d;
            WatermarkBehavior watermarkBehavior = GetWatermarkBehavior(ctrl);
            if (watermarkBehavior == null)
            {
                watermarkBehavior = WatermarkBehavior.Create(ctrl);
                SetWatermarkBehavior(ctrl, watermarkBehavior);
            }

            watermarkBehavior.UpdateWatermark();
        }

        #endregion //Watermark Property

        #region HideOnFocusOrData Property

        public static readonly DependencyProperty HideOnFocusOrDataProperty = DependencyProperty.RegisterAttached(
            "HideOnFocusOrData",
            typeof(bool),
            typeof(RrcWatermarkBehavior),
            new PropertyMetadata(true));

        public static bool GetHideOnFocusOrData(DependencyObject obj)
        {
            return (bool)obj.GetValue(HideOnFocusOrDataProperty);
        }

        public static void SetHideOnFocusOrData(DependencyObject obj, bool value)
        {
            obj.SetValue(HideOnFocusOrDataProperty, value);
        }

        #endregion //HideOnFocusOrData Property

        #region SizeToContent Property

        public static readonly DependencyProperty SizeToContentProperty = DependencyProperty.RegisterAttached(
            "SizeToContent",
            typeof(SizeToContent),
            typeof(RrcWatermarkBehavior),
            new PropertyMetadata(default(SizeToContent)));

        public static SizeToContent GetSizeToContent(DependencyObject obj)
        {
            return (SizeToContent)obj.GetValue(SizeToContentProperty);
        }

        public static void SetSizeToContent(DependencyObject obj, SizeToContent value)
        {
            obj.SetValue(SizeToContentProperty, value);
        }

        #endregion //SizeToContent Property

        #region AllowUseControlPadding Property

        public static readonly DependencyProperty AllowUseControlPaddingProperty = DependencyProperty.RegisterAttached(
            "AllowUseControlPadding",
            typeof(bool),
            typeof(RrcWatermarkBehavior),
            new PropertyMetadata(true));

        public static bool GetAllowUseControlPadding(DependencyObject obj)
        {
            return (bool)obj.GetValue(AllowUseControlPaddingProperty);
        }

        public static void SetAllowUseControlPadding(DependencyObject obj, bool value)
        {
            obj.SetValue(AllowUseControlPaddingProperty, value);
        }

        #endregion //AllowUseControlPadding Property

        #region WatermarkBehavior Property

        private static readonly DependencyProperty WatermarkBehaviorProperty = DependencyProperty.RegisterAttached(
            "WatermarkBehavior",
            typeof(WatermarkBehavior),
            typeof(RrcWatermarkBehavior),
            new PropertyMetadata(default(WatermarkBehavior)));

        private static WatermarkBehavior GetWatermarkBehavior(DependencyObject obj)
        {
            return (WatermarkBehavior)obj.GetValue(WatermarkBehaviorProperty);
        }

        private static void SetWatermarkBehavior(DependencyObject obj, WatermarkBehavior value)
        {
            obj.SetValue(WatermarkBehaviorProperty, value);
        }

        #endregion //WatermarkBehavior Property
    }
}
