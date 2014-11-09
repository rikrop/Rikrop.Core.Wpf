using System.Windows;
using System.Windows.Controls;

namespace Rikrop.Core.Wpf.Controls
{
    public class RrcGridSplitter : GridSplitter
    {
        static RrcGridSplitter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RrcGridSplitter), new FrameworkPropertyMetadata(typeof(RrcGridSplitter)));
        }

        #region VisualPresentationVisibility Property

        public static readonly DependencyProperty VisualPresentationVisibilityProperty = DependencyProperty.Register(
            "VisualPresentationVisibility",
            typeof(Visibility),
            typeof(RrcGridSplitter),
            new PropertyMetadata(Visibility.Visible));

        public Visibility VisualPresentationVisibility
        {
            get { return (Visibility)GetValue(VisualPresentationVisibilityProperty); }
            set { SetValue(VisualPresentationVisibilityProperty, value); }
        }

        #endregion //VisualPresentationVisibility Property

        public RrcGridSplitter()
        {
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (ActualWidth == 0 && ActualHeight == 0)
            {
                return;
            }

            Loaded -= OnLoaded;

            if (ResizeDirection == GridResizeDirection.Auto)
                ResizeDirection = ActualWidth > ActualHeight
                                      ? GridResizeDirection.Rows
                                      : GridResizeDirection.Columns;
        }
    }
}
