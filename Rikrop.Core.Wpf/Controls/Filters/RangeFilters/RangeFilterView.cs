using System.Windows;
using System.Windows.Controls;

namespace Rikrop.Core.Wpf.Controls.Filters.RangeFilters
{
    public class RangeFilterView : Control
    {
        static RangeFilterView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RangeFilterView), new FrameworkPropertyMetadata(typeof(RangeFilterView)));
        }

        #region FromContent Property

        public static readonly DependencyProperty FromContentProperty = DependencyProperty.Register(
            "FromContent",
            typeof(FrameworkElement),
            typeof(RangeFilterView),
            new PropertyMetadata(default(FrameworkElement)));

        public FrameworkElement FromContent
        {
            get { return (FrameworkElement)GetValue(FromContentProperty); }
            set { SetValue(FromContentProperty, value); }
        }

        #endregion //FromContent Property

        #region ToContent Property

        public static readonly DependencyProperty ToContentProperty = DependencyProperty.Register(
            "ToContent",
            typeof(FrameworkElement),
            typeof(RangeFilterView),
            new PropertyMetadata(default(FrameworkElement)));

        public FrameworkElement ToContent
        {
            get { return (FrameworkElement)GetValue(ToContentProperty); }
            set { SetValue(ToContentProperty, value); }
        }

        #endregion //ToContent Property

        public RangeFilterView()
        {
            Loaded += OnLoaded;
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property == FromContentProperty)
            {
                if (FromContent != null)
                {
                    FromContent.IsEnabledChanged += delegate { FromContent.Focus(); };
                }
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (FromContent != null)
            {
                FromContent.Focus();
            }
        }
    }
}
