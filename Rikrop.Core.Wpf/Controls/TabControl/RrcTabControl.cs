using System.Windows;

namespace Rikrop.Core.Wpf.Controls.TabControl
{
    public class RrcTabControl : System.Windows.Controls.TabControl
    {
        public static readonly DependencyProperty SelectedContainerProperty =
            DependencyProperty.Register("SelectedContainer", typeof(RrcTabItem), typeof(RrcTabControl));

        public static readonly DependencyProperty HeaderPanelProperty =
            DependencyProperty.Register("HeaderPanel", typeof(UIElement), typeof(RrcTabControl));

        public RrcTabItem SelectedContainer
        {
            get { return (RrcTabItem)GetValue(SelectedContainerProperty); }
            set { SetValue(SelectedContainerProperty, value); }
        }

        public UIElement HeaderPanel
        {
            get { return (UIElement)GetValue(HeaderPanelProperty); }
            set { SetValue(HeaderPanelProperty, value); }
        }

        static RrcTabControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RrcTabControl), new FrameworkPropertyMetadata(typeof(RrcTabControl)));
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new RrcTabItem();
        }
    }
}
