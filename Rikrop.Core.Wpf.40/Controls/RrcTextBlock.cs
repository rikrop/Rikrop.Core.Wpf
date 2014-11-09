using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Rikrop.Core.Framework;

namespace Rikrop.Core.Wpf.Controls
{
    public class RrcTextBlock : TextBlock
    {
        public static readonly DependencyProperty EnableAutoTooltipProperty =
            DependencyProperty.Register("EnableAutoTooltip", typeof (bool), typeof (RrcTextBlock),
                                        new PropertyMetadata(true));

        public bool EnableAutoTooltip
        {
            get { return (bool) GetValue(EnableAutoTooltipProperty); }
            set { SetValue(EnableAutoTooltipProperty, value); }
        }

        static RrcTextBlock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (RrcTextBlock),
                                                     new FrameworkPropertyMetadata(typeof (RrcTextBlock)));
        }

        public RrcTextBlock()
        {
            Loaded += TextBlockLoaded;
            SizeChanged += TextBlockSizeChanged;
        }

        private static void TextBlockLoaded(object sender, RoutedEventArgs e)
        {
            SetToolTip(sender);
        }

        private static void TextBlockSizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetToolTip(sender);
        }

        private static void SetToolTip(object sender)
        {
            var textBlock = sender as RrcTextBlock;

            if (textBlock == null)
            {
                return;
            }

            textBlock.ComputeAutoTooltip();
        }

        /// <summary>
        ///     Assigns the ToolTip for the given TextBlock based on whether the text is trimmed
        /// </summary>
        private void ComputeAutoTooltip()
        {
            if (!EnableAutoTooltip)
            {
                return;
            }

            Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));

            var width = DesiredSize.Width - Margin.Left - Margin.Right;

            if (ActualWidth < width)
            {
                var b = new Binding(ExpressionHelper.GetName(() => Text))
                    {
                        RelativeSource = new RelativeSource(RelativeSourceMode.Self)
                    };

                SetBinding(ToolTipService.ToolTipProperty, b);
            }
            else
            {
                ToolTipService.SetToolTip(this, null);
            }
        }
    }
}