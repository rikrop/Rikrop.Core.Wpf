using System;
using System.Diagnostics.Contracts;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Rikrop.Core.Framework;

namespace Rikrop.Core.Wpf.Behaviors
{
    public static class TextBlockTrimmingToolTipBehaviour
    {
        public static readonly DependencyProperty EnableTrimmingTooltipProperty =
            DependencyProperty.RegisterAttached("EnableTrimmingTooltip", typeof (bool), typeof (TextBlockTrimmingToolTipBehaviour), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits, EnableTrimmingTooltipPropertyChangedCallback));

        public static void SetEnableTrimmingTooltip(UIElement element, bool value)
        {
            element.SetValue(EnableTrimmingTooltipProperty, value);
        }

        public static bool GetEnableTrimmingTooltip(UIElement element)
        {
            return (bool) element.GetValue(EnableTrimmingTooltipProperty);
        }

        private static void EnableTrimmingTooltipPropertyChangedCallback(DependencyObject dobj, DependencyPropertyChangedEventArgs dargs)
        {
            var tb = dobj as TextBlock;
            if (tb == null)
            {
                return;
            }

            if ((bool) dargs.NewValue)
            {
                EnableAutoTooltip(tb);
            }
            else
            {
                DisableAutoTooltip(tb);
            }
        }

        private static void DisableAutoTooltip(TextBlock tb)
        {
            Contract.Requires<ArgumentNullException>(tb != null);

            tb.Loaded -= TextBlockLoaded;
            tb.SizeChanged -= TextBlockSizeChanged;

            ToolTipService.SetToolTip(tb, null);
            tb.TextTrimming = TextTrimming.None;
        }

        private static void EnableAutoTooltip(TextBlock tb)
        {
            Contract.Requires<ArgumentNullException>(tb != null);
            if (!tb.IsLoaded)
            {
                tb.Loaded += TextBlockLoaded;
            }
            tb.SizeChanged += TextBlockSizeChanged;
            tb.TextTrimming = TextTrimming.CharacterEllipsis;

            SetToolTip(tb);
        }

        private static void TextBlockLoaded(object sender, RoutedEventArgs e)
        {
            ((TextBlock) sender).Loaded -= TextBlockLoaded;
            SetToolTip((TextBlock) sender);
        }

        private static void TextBlockSizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetToolTip((TextBlock) sender);
        }

        private static void SetToolTip(TextBlock tb)
        {
            tb.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));

            var width = tb.DesiredSize.Width - tb.Margin.Left - tb.Margin.Right;

            if (tb.ActualWidth < width)
            {
                var b = new Binding(ExpressionHelper.GetName<TextBlock>(block => block.Text))
                            {
                                RelativeSource = new RelativeSource(RelativeSourceMode.Self)
                            };

                tb.SetBinding(ToolTipService.ToolTipProperty, b);
            }
            else
            {
                ToolTipService.SetToolTip(tb, null);
            }
        }
    }
}