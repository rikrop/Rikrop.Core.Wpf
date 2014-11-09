using System.Windows.Data;
using System.Windows.Input;

namespace Rikrop.Core.Wpf.Controls
{
    using System;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;

    public class RrcHyperlinkButton : RrcButton//Button
    {
        #region NavigateUri Property

        public static readonly DependencyProperty NavigateUriProperty = DependencyProperty.Register(
            "NavigateUri",
            typeof(Uri),
            typeof(RrcHyperlinkButton),
            new PropertyMetadata(null));

        public Uri NavigateUri
        {
            get { return (Uri)GetValue(NavigateUriProperty); }
            set { SetValue(NavigateUriProperty, value); }
        }

        #endregion //NavigateUri Property

        #region NavigateUriString Property

        public static readonly DependencyProperty NavigateUriStringProperty = DependencyProperty.Register(
            "NavigateUriString",
            typeof(string),
            typeof(RrcHyperlinkButton),
            new PropertyMetadata(null));

        public string NavigateUriString
        {
            get { return (string)GetValue(NavigateUriStringProperty); }
            set { SetValue(NavigateUriStringProperty, value); }
        }

        #endregion //NavigateUriString Property

        #region TextWrapping Property

        public static readonly DependencyProperty TextWrappingProperty = DependencyProperty.Register(
            "TextWrapping",
            typeof(TextWrapping),
            typeof(RrcHyperlinkButton),
            new PropertyMetadata(TextWrapping.NoWrap));

        public TextWrapping TextWrapping
        {
            get { return (TextWrapping)GetValue(TextWrappingProperty); }
            set { SetValue(TextWrappingProperty, value); }
        }

        #endregion //TextWrapping Property

        public static readonly DependencyProperty TextTrimmingProperty = DependencyProperty.Register(
            "TextTrimming",
            typeof(TextTrimming),
            typeof(RrcHyperlinkButton),
            new PropertyMetadata(TextTrimming.CharacterEllipsis));

        public TextTrimming TextTrimming
        {
            get { return (TextTrimming)GetValue(TextTrimmingProperty); }
            set { SetValue(TextTrimmingProperty, value); }
        }

        static RrcHyperlinkButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RrcHyperlinkButton),
                                                     new FrameworkPropertyMetadata(typeof(RrcHyperlinkButton)));
        }

        public RrcHyperlinkButton()
        {
            var textBlock = new RrcTextBlock { Text = NavigateUriString };
            var binding = new Binding { Source = this, Path = new PropertyPath("NavigateUriString") };
            textBlock.SetBinding(TextBlock.TextProperty, binding);
            ToolTip = textBlock;
            ContextMenu = new ContextMenu();
            var item = new RrcMenuItem { Header = "Копировать ссылку" };
            item.Click += item_Click;
            ContextMenu.Items.Add(item);
        }

        void item_Click(object sender, RoutedEventArgs e)
        {
            if (NavigateUriString != null)
            {
                SetData(DataFormats.UnicodeText, NavigateUriString);
            }
        }

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            if (!(e.ChangedButton == MouseButton.Left || e.ChangedButton == MouseButton.Middle))
            {
                return;
            }

            if (Command != null)
            {
                return;
            }
            if (NavigateUri != null)
            {
                Process.Start(NavigateUri.ToString());
                return;
            }

            if (!string.IsNullOrEmpty(NavigateUriString))
            {
                var uri = new Uri(NavigateUriString);
                Process.Start(uri.AbsoluteUri);
            }
        }

        private bool SetData(string format, object data, int retryTimes = 10, int millisecondsRetryDelay = 100)
        {
            for (int i = 0; i < retryTimes; i++)
            {
                try
                {
                    Clipboard.SetData(format, data);
                    return true;
                }
                catch { }
                System.Threading.Thread.Sleep(millisecondsRetryDelay);
            }

            return false;
        }
    }
}
