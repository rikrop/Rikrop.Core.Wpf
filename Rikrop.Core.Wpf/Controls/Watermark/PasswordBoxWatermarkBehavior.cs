using System.Windows;
using System.Windows.Controls;

namespace Rikrop.Core.Wpf.Controls.Watermark
{
    internal class PasswordBoxWatermarkBehavior : WatermarkBehavior<PasswordBox>
    {
        public static readonly DependencyProperty WasToolTipChangedProperty = DependencyProperty.RegisterAttached(
            "WasToolTipChanged",
            typeof (bool),
            typeof(PasswordBoxWatermarkBehavior),
            new PropertyMetadata(default(bool)));

        private readonly FrameworkElement _watermarkedElement;

        public PasswordBoxWatermarkBehavior(PasswordBox element, FrameworkElement watermarkedElement)
            : base(element, watermarkedElement)
        {
            _watermarkedElement = watermarkedElement;

            element.PasswordChanged += UpdateWatermarkFromEvent;
        }

        public static bool GetWasToolTipChanged(DependencyObject obj)
        {
            return (bool) obj.GetValue(WasToolTipChangedProperty);
        }

        public static void SetWasToolTipChanged(DependencyObject obj, bool value)
        {
            obj.SetValue(WasToolTipChangedProperty, value);
        }

        protected override bool CanShowWatermark()
        {
            return base.CanShowWatermark() && string.IsNullOrEmpty(WatermarkElement.Password);
        }

        protected override void OnShowWatermark()
        {
            if (RrcWatermarkBehavior.GetHideOnFocusOrData(_watermarkedElement) &&
                GetWasToolTipChanged(_watermarkedElement))
            {
                WatermarkElement.ToolTip = null;
                SetWasToolTipChanged(_watermarkedElement, false);
            }
        }

        protected override void OnClearWatermark()
        {
            if (WatermarkElement.ToolTip == null)
            {
                WatermarkElement.ToolTip = RrcWatermarkBehavior.GetWatermark(_watermarkedElement);
                SetWasToolTipChanged(_watermarkedElement, true);
            }
        }
    }
}