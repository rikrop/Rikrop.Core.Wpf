using System.Windows;
using System.Windows.Controls;
using Rikrop.Core.Wpf.Commands;

namespace Rikrop.Core.Wpf.Controls
{
    internal class RrcButtonVisualStates
    {
        public const string VisualStates = "VisualStates";
        public const string ImageOnly = "ImageOnly";
        public const string ContentOnly = "ContentOnly";
    }

    [TemplateVisualState(GroupName = RrcButtonVisualStates.VisualStates, Name = RrcButtonVisualStates.ContentOnly)]
    [TemplateVisualState(GroupName = RrcButtonVisualStates.VisualStates, Name = RrcButtonVisualStates.ImageOnly)]
    public class RrcButton : Button
    {
        static RrcButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RrcButton), new FrameworkPropertyMetadata(typeof(RrcButton)));
        }

        #region HideOnDisable Property

        public static DependencyProperty HideOnDisableProperty = DependencyProperty.Register(
            "HideOnDisable",
            typeof(bool),
            typeof(RrcButton),
            new PropertyMetadata(default(bool)));

        public bool HideOnDisable
        {
            get { return (bool)GetValue(HideOnDisableProperty); }
            set { SetValue(HideOnDisableProperty, value); }
        }

        #endregion //HideOnDisable Property

        #region ImageTemplate Property

        public static readonly DependencyProperty ImageTemplateProperty = DependencyProperty.Register(
            "ImageTemplate",
            typeof(DataTemplate),
            typeof(RrcButton),
            new PropertyMetadata(default(DataTemplate)));

        public DataTemplate ImageTemplate
        {
            get { return (DataTemplate)GetValue(ImageTemplateProperty); }
            set { SetValue(ImageTemplateProperty, value); }
        }

        #endregion //ImageTemplate Property

        #region MouseOverImageTemplate Property

        public static readonly DependencyProperty MouseOverImageTemplateProperty = DependencyProperty.Register(
            "MouseOverImageTemplate",
            typeof(DataTemplate),
            typeof(RrcButton),
            new PropertyMetadata(default(DataTemplate)));

        public DataTemplate MouseOverImageTemplate
        {
            get { return (DataTemplate)GetValue(MouseOverImageTemplateProperty); }
            set { SetValue(MouseOverImageTemplateProperty, value); }
        }

        #endregion //MouseOverImageTemplate Property

        #region DisabledImageTemplate Property

        public static readonly DependencyProperty DisabledImageTemplateProperty = DependencyProperty.Register(
            "DisabledImageTemplate",
            typeof(DataTemplate),
            typeof(RrcButton),
            new PropertyMetadata(default(DataTemplate)));

        public DataTemplate DisabledImageTemplate
        {
            get { return (DataTemplate)GetValue(DisabledImageTemplateProperty); }
            set { SetValue(DisabledImageTemplateProperty, value); }
        }

        #endregion //DisabledImageTemplate Property

        #region CornerRadius Property

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            "CornerRadius",
            typeof(CornerRadius),
            typeof(RrcButton),
            new PropertyMetadata(new CornerRadius(5)));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        #endregion //CornerRadius Property

        private ContentPresenter ContentPresenter { get; set; }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            ContentPresenter = (ContentPresenter)Template.FindName("ContentPresenter", this);
            UpdateVisualState();
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property == CommandProperty)
            {
                // Если привязанную команду требуется выполнять только при наличии прав и они отсутствуют,
                // скрываем кнопку
                var securityCommand = Command as ISecurityCommand;

                if (securityCommand != null && !securityCommand.EnoughRights)
                {
                    Visibility = Visibility.Collapsed;
                }
            }
            else if ((e.Property == IsEnabledProperty || e.Property == HideOnDisableProperty) && HideOnDisable)
            {
                Visibility = IsEnabled
                                 ? Visibility.Visible
                                 : Visibility.Collapsed;
            }
            else if(e.Property == ImageTemplateProperty || e.Property == ContentProperty)
            {
                UpdateVisualState();
            }
        }

        private void UpdateVisualState()
        {
            if(ContentPresenter == null)
            {
                return;
            }

            if (ImageTemplate == null)
            {
                VisualStateManager.GoToState(this, RrcButtonVisualStates.ContentOnly, true);
            }
            else if(Content == null)
            {
                VisualStateManager.GoToState(this, RrcButtonVisualStates.ImageOnly, true);
            }
        }
    }
}
