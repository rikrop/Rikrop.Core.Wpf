using System.Windows.Media;
using Rikrop.Core.Wpf.Controls.Helpers;
using Microsoft.Windows.Themes;

namespace Rikrop.Core.Wpf.Controls.Watermark
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;

    internal class WatermarkBehavior<T> : WatermarkBehavior
        where T : FrameworkElement
    {
        private AdornerLayer _adornerLayer;
        private readonly FrameworkElement _watermarkedElement;
        protected T WatermarkElement { get; set; }

        public WatermarkBehavior(T element, FrameworkElement watermarkedElement)
        {
            _watermarkedElement = watermarkedElement;
            WatermarkElement = element;

            element.Loaded += OnLoaded;
            element.IsVisibleChanged += UpdateWatermarkFromEvent;
            element.GotFocus += UpdateWatermarkFromEvent;
            element.LostFocus += UpdateWatermarkFromEvent;
            element.IsKeyboardFocusWithinChanged += UpdateWatermarkFromEvent;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _adornerLayer = AdornerLayer.GetAdornerLayer(_watermarkedElement);
            UpdateWatermark();
        }

        protected void UpdateWatermarkFromEvent(object sender, EventArgs e)
        {
            UpdateWatermark();
        }

        protected void UpdateWatermarkFromEvent(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateWatermark();
        }

        protected override bool CanShowWatermark()
        {
            return WatermarkElement.IsVisible &&
                   WatermarkElement.IsLoaded &&
                   (!WatermarkElement.IsKeyboardFocusWithin || !RrcWatermarkBehavior.GetHideOnFocusOrData(WatermarkElement)) &&
                   (!WatermarkElement.IsFocused || !RrcWatermarkBehavior.GetHideOnFocusOrData(WatermarkElement));
        }

        protected sealed override void ShowWatermark()
        {
            ClearWatermark();

            if (_adornerLayer != null)
            {
                _adornerLayer.Add(new WatermarkAdorner(_watermarkedElement, RrcWatermarkBehavior.GetWatermark(_watermarkedElement)));
            }

            OnShowWatermark();
        }

        protected sealed override void ClearWatermark()
        {
            if (_adornerLayer != null)
            {
                Adorner[] adorners = _adornerLayer.GetAdorners(_watermarkedElement);
                if (adorners == null)
                    return;

                var ourWatermarks = adorners.OfType<WatermarkAdorner>();
                foreach (var watermarkAdorner in ourWatermarks)
                {
                    watermarkAdorner.Visibility = Visibility.Hidden;
                    _adornerLayer.Remove(watermarkAdorner);
                    watermarkAdorner.Cleanup();
                }
            }

            OnClearWatermark();
        }

        protected virtual void OnShowWatermark()
        {

        }

        protected virtual void OnClearWatermark()
        {

        }
    }

    internal abstract class WatermarkBehavior
    {
        public static WatermarkBehavior Create(FrameworkElement element)
        {
            var box = element as TextBox;
            if (box != null)
            {
                return new TextBoxWatermarkBehavior(box, element);
            }

            if (element is PasswordBox)
            {
                return new PasswordBoxWatermarkBehavior(element as PasswordBox, element);
            }

            var chrome = element as ListBoxChrome;
            if (chrome != null)
            {
                var textBox = chrome.FindVisualParent<TextBox>();
                if (textBox != null)
                {
                    return new TextBoxWatermarkBehavior(textBox, element);
                }
            }

            return new WatermarkBehavior<FrameworkElement>(element, element);
        }

        public void UpdateWatermark()
        {
            if (CanShowWatermark())
            {
                ShowWatermark();
            }
            else
            {
                ClearWatermark();
            }
        }

        protected abstract bool CanShowWatermark();
        protected abstract void ShowWatermark();
        protected abstract void ClearWatermark();
    }
}
