using System.Windows;
using System.Windows.Controls;

namespace Rikrop.Core.Wpf.Controls
{
    public class RrcExpander : Expander
    {
        static RrcExpander()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RrcExpander), new FrameworkPropertyMetadata(typeof(RrcExpander)));
        }

        #region ExpandedHeader Property

        public static DependencyProperty ExpandedHeaderProperty = DependencyProperty.Register(
            "ExpandedHeader",
            typeof(object),
            typeof(RrcExpander),
            new PropertyMetadata(default(object)));

        public object ExpandedHeader
        {
            get { return GetValue(ExpandedHeaderProperty); }
            set { SetValue(ExpandedHeaderProperty, value); }
        }

        #endregion //ExpandedHeader Property

        #region ExpandedHeaderTemplate Property

        public static DependencyProperty ExpandedHeaderTemplateProperty = DependencyProperty.Register(
            "ExpandedHeaderTemplate",
            typeof(DataTemplate),
            typeof(RrcExpander),
            new PropertyMetadata(default(DataTemplate)));

        public DataTemplate ExpandedHeaderTemplate
        {
            get { return (DataTemplate)GetValue(ExpandedHeaderTemplateProperty); }
            set { SetValue(ExpandedHeaderTemplateProperty, value); }
        }

        #endregion //ExpandedHeaderTemplate Property

        #region CollapsedHeader Property

        public static DependencyProperty CollapsedHeaderProperty = DependencyProperty.Register(
            "CollapsedHeader",
            typeof(object),
            typeof(RrcExpander),
            new PropertyMetadata(default(object)));

        public object CollapsedHeader
        {
            get { return GetValue(CollapsedHeaderProperty); }
            set { SetValue(CollapsedHeaderProperty, value); }
        }

        #endregion //CollapsedHeader Property

        #region CollapsedHeaderTemplate Property

        public static DependencyProperty CollapsedHeaderTemplateProperty = DependencyProperty.Register(
            "CollapsedHeaderTemplate",
            typeof(DataTemplate),
            typeof(RrcExpander),
            new PropertyMetadata(default(DataTemplate)));

        public DataTemplate CollapsedHeaderTemplate
        {
            get { return (DataTemplate)GetValue(CollapsedHeaderTemplateProperty); }
            set { SetValue(CollapsedHeaderTemplateProperty, value); }
        }

        #endregion //CollapsedHeaderTemplate Property

        private bool ExpandedOrCollapsedHeaderWasSetted { get; set; }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if(e.Property == ExpandedHeaderProperty || e.Property == ExpandedHeaderTemplateProperty)
            {
                ExpandedOrCollapsedHeaderWasSetted = true;
                UpdateHeader();
            }
            else if (e.Property == CollapsedHeaderProperty || e.Property == CollapsedHeaderTemplateProperty)
            {
                ExpandedOrCollapsedHeaderWasSetted = true;
                UpdateHeader();
            }
            else if(e.Property == IsExpandedProperty && ExpandedOrCollapsedHeaderWasSetted)
            {
                UpdateHeader();
            }
            else if(e.Property == ExpandDirectionProperty)
            {
                UpdateHeader();
            }
        }

        private void UpdateHeader()
        {
            if (IsExpanded)
            {
                Header = ExpandedHeader;
                HeaderTemplate = ExpandedHeaderTemplate;
            }
            else
            {
                Header = CollapsedHeader;
                HeaderTemplate = CollapsedHeaderTemplate;
            }
            //var headerContent = new ContentPresenter();
            //if (IsExpanded)
            //{
            //    headerContent.Content = ExpandedHeader;
            //    headerContent.ContentTemplate = ExpandedHeaderTemplate;
            //}
            //else
            //{
            //    headerContent.Content = CollapsedHeader;
            //    headerContent.ContentTemplate = CollapsedHeaderTemplate;
            //}

            //if (ExpandDirection == ExpandDirection.Left || ExpandDirection == ExpandDirection.Right)
            //{
            //    headerContent.LayoutTransform = new RotateTransform(-90);
            //}

            //Header = headerContent;
            //HeaderTemplate = null;
        }
    }
}
