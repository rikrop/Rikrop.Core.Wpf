using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Rikrop.Core.Wpf.Controls.Helpers;

namespace Rikrop.Core.Wpf.Controls.TabControl
{
    public class RrcTabItem : TabItem
    {
        public static readonly DependencyProperty ValidationBindingProperty = DependencyProperty.Register(
            "ValidationBinding",
            typeof(object),
            typeof(RrcTabItem),
            new PropertyMetadata(default(object)));

        public static readonly DependencyProperty HighlightProperty =
            DependencyProperty.Register("Highlight", typeof(Brush), typeof(RrcTabItem));

        public object ValidationBinding
        {
            get { return GetValue(ValidationBindingProperty); }
            set { SetValue(ValidationBindingProperty, value); }
        }

        public Brush Highlight
        {
            get { return (Brush)GetValue(HighlightProperty); }
            set { SetValue(HighlightProperty, value); }
        }

        static RrcTabItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RrcTabItem), new FrameworkPropertyMetadata(typeof(RrcTabItem)));
            ContentProperty.OverrideMetadata(typeof(RrcTabItem), new FrameworkPropertyMetadata(null, null, CoerceContentProperty));
        }

        private static object CoerceContentProperty(DependencyObject d, object basevalue)
        {
            if (basevalue is UIElement)
            {
                return new AdornerDecorator { Child = (UIElement)basevalue };
            }
            return basevalue;
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property == IsSelectedProperty)
            {
                var ParentTabControl = Parent as RrcTabControl ?? this.FindVisualParent<RrcTabControl>();

                if (ParentTabControl != null)
                {
                    ParentTabControl.SelectedContainer = this;
                }
            }
        }
    }
}
