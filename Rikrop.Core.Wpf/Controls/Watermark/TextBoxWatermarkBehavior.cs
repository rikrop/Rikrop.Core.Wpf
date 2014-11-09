using System.Windows.Documents;

namespace Rikrop.Core.Wpf.Controls.Watermark
{
    using System.Windows;
    using System.Windows.Controls;

    internal class TextBoxWatermarkBehavior : WatermarkBehavior<TextBox>
    {
        private readonly FrameworkElement _watermarkedElement;

        #region WasToolTipChanged Property

        public static readonly DependencyProperty WasToolTipChangedProperty = DependencyProperty.RegisterAttached(
            "WasToolTipChanged",
            typeof(bool),
            typeof(TextBoxWatermarkBehavior),
            new PropertyMetadata(default(bool)));

        public static bool GetWasToolTipChanged(DependencyObject obj)
        {
            return (bool)obj.GetValue(WasToolTipChangedProperty);
        }

        public static void SetWasToolTipChanged(DependencyObject obj, bool value)
        {
            obj.SetValue(WasToolTipChangedProperty, value);
        }

        #endregion //WasToolTipChanged Property

        public TextBoxWatermarkBehavior(TextBox element, FrameworkElement watermarkedElement)
            : base(element, watermarkedElement)
        {
            _watermarkedElement = watermarkedElement;

            element.TextChanged += UpdateWatermarkFromEvent;
        }

        protected override bool CanShowWatermark()
        {
            return base.CanShowWatermark() &&
                   string.IsNullOrEmpty(WatermarkElement.Text);
        }

        protected override void OnShowWatermark()
        {
            if (RrcWatermarkBehavior.GetHideOnFocusOrData(_watermarkedElement) && GetWasToolTipChanged(_watermarkedElement))
            {
                WatermarkElement.ToolTip = null;
                SetWasToolTipChanged(_watermarkedElement, false);
            }
        }

        protected override void OnClearWatermark()
        {
            if (WatermarkElement.ToolTip == null)
            {
                var RrcTextBox = WatermarkElement as RrcTextBox;
                if (RrcTextBox != null && RrcTextBox.Watermark == null)
                {
                    return;
                }

                WatermarkElement.ToolTip = RrcWatermarkBehavior.GetWatermark(_watermarkedElement);
                SetWasToolTipChanged(_watermarkedElement, true);
            }
        }
    }
}
