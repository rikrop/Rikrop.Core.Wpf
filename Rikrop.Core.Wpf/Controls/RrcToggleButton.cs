using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using Rikrop.Core.Wpf.Commands;

namespace Rikrop.Core.Wpf.Controls
{
    [TemplateVisualState(GroupName = RrcButtonVisualStates.VisualStates, Name = RrcButtonVisualStates.ContentOnly)]
    [TemplateVisualState(GroupName = RrcButtonVisualStates.VisualStates, Name = RrcButtonVisualStates.ImageOnly)]
    public class RrcToggleButton : ToggleButton
    {
        static RrcToggleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RrcToggleButton), new FrameworkPropertyMetadata(typeof(RrcToggleButton)));
        }

        public bool CanUncheck
        {
            get { return (bool)GetValue(CanUncheckProperty); }
            set { SetValue(CanUncheckProperty, value); }
        }

        public static readonly DependencyProperty CanUncheckProperty =
            DependencyProperty.Register("CanUncheck", typeof(bool), typeof(RrcToggleButton), new PropertyMetadata(true));

        protected override void OnPreviewMouseDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);

            if (IsChecked.HasValue && IsChecked.Value && !CanUncheck)
            {
                e.Handled = true;
            }
        }

        #region ImageTemplate Property

        public static readonly DependencyProperty ImageTemplateProperty = DependencyProperty.Register(
            "ImageTemplate",
            typeof(DataTemplate),
            typeof(RrcToggleButton),
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
            typeof(RrcToggleButton),
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
            typeof(RrcToggleButton),
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
            typeof(RrcToggleButton),
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
            else if (e.Property == ImageTemplateProperty || e.Property == ContentProperty)
            {
                UpdateVisualState();
            }
        }

        private void UpdateVisualState()
        {
            if (ContentPresenter == null)
            {
                return;
            }

            if (ImageTemplate == null)
            {
                VisualStateManager.GoToState(this, RrcButtonVisualStates.ContentOnly, true);
            }
            else if (Content == null)
            {
                VisualStateManager.GoToState(this, RrcButtonVisualStates.ImageOnly, true);
            }
        }

    }
}
