using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shapes;
using Rikrop.Core.Wpf.Controls.Helpers;

namespace Rikrop.Core.Wpf.Controls.RrcPopupDialog
{
    /// <summary>
    ///     A control to provide a popup dialog.
    /// </summary>
    [TemplateVisualState(Name = RrcPopupDialogVisualStates.StateVisible,
        GroupName = RrcPopupDialogVisualStates.GroupVisibility)]
    [TemplateVisualState(Name = RrcPopupDialogVisualStates.StateHidden,
        GroupName = RrcPopupDialogVisualStates.GroupVisibility)]
    [TemplateVisualState(Name = RrcPopupDialogVisualStates.StateSlideDown,
        GroupName = RrcPopupDialogVisualStates.GroupVisibility)]
    [StyleTypedProperty(Property = "OverlayRectangleStyle", StyleTargetType = typeof (Rectangle))]
    public class RrcPopupDialog : ContentControl
    {
        //PopupBorderEffect
        public static readonly DependencyProperty PopupBorderEffectProperty = DependencyProperty.RegisterAttached(
            "PopupBorderEffect",
            typeof(Effect),
            typeof(RrcPopupDialog),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(
            "IsOpen",
            typeof (bool),
            typeof (RrcPopupDialog),
            new FrameworkPropertyMetadata(default(bool), OnIsOpenChanged));

        public static readonly DependencyProperty PopupContentProperty = DependencyProperty.Register(
            "PopupContent",
            typeof (FrameworkElement),
            typeof (RrcPopupDialog),
            new FrameworkPropertyMetadata(null, OnPopupContentChanged));

        public static readonly DependencyProperty OverlayRectangleStyleProperty = DependencyProperty.RegisterAttached(
            "OverlayRectangleStyle",
            typeof (Style),
            typeof (RrcPopupDialog),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty PopupContentHorizontalAlignmentProperty = DependencyProperty.RegisterAttached(
            "PopupContentHorizontalAlignment",
            typeof (HorizontalAlignment),
            typeof (RrcPopupDialog),
            new FrameworkPropertyMetadata(HorizontalAlignment.Stretch, FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty PopupContentVerticalAlignmentProperty = DependencyProperty.RegisterAttached(
            "PopupContentVerticalAlignment",
            typeof (VerticalAlignment),
            typeof (RrcPopupDialog),
            new FrameworkPropertyMetadata(VerticalAlignment.Stretch, FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty PopupContentMarginProperty = DependencyProperty.RegisterAttached(
            "PopupContentMargin",
            typeof (Thickness),
            typeof (RrcPopupDialog),
            new FrameworkPropertyMetadata(new Thickness(0), FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty PopupContentMaxWidthProperty = DependencyProperty.RegisterAttached(
            "PopupContentMaxWidth",
            typeof (double),
            typeof (RrcPopupDialog),
            new FrameworkPropertyMetadata(double.PositiveInfinity, FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty PopupContentMaxHeightProperty = DependencyProperty.RegisterAttached(
            "PopupContentMaxHeight",
            typeof (double),
            typeof (RrcPopupDialog),
            new FrameworkPropertyMetadata(double.PositiveInfinity, FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty PopupContentPaddingProperty = DependencyProperty.RegisterAttached(
            "PopupContentPadding",
            typeof (Thickness),
            typeof (RrcPopupDialog),
            new FrameworkPropertyMetadata(new Thickness(0), FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty PopupContentBorderThicknessProperty = DependencyProperty.RegisterAttached(
            "PopupContentBorderThickness",
            typeof (Thickness),
            typeof (RrcPopupDialog),
            new FrameworkPropertyMetadata(new Thickness(0), FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty PopupContentBorderBrushProperty = DependencyProperty.RegisterAttached(
            "PopupContentBorderBrush",
            typeof (Brush),
            typeof (RrcPopupDialog),
            new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty PopupContentCornerRadiusProperty = DependencyProperty.RegisterAttached(
            "PopupContentCornerRadius",
            typeof (CornerRadius),
            typeof (RrcPopupDialog),
            new FrameworkPropertyMetadata(new CornerRadius(0), FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty PopupContentBackgroundProperty = DependencyProperty.RegisterAttached(
            "PopupContentBackground",
            typeof (Brush),
            typeof (RrcPopupDialog),
            new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty ClosePopupCommandProperty = DependencyProperty.RegisterAttached(
            "ClosePopupCommand",
            typeof (ICommand),
            typeof (RrcPopupDialog),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty ClosePopupCommandParameterProperty = DependencyProperty.RegisterAttached(
            "ClosePopupCommandParameter",
            typeof (object),
            typeof (RrcPopupDialog),
            new FrameworkPropertyMetadata(default(object), FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty AutoFocusProperty = DependencyProperty.RegisterAttached(
            "AutoFocus",
            typeof (bool),
            typeof (RrcPopupDialog),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty ClosePopupButtonVisibilityProperty = DependencyProperty.RegisterAttached(
            "ClosePopupButtonVisibility",
            typeof (Visibility),
            typeof (RrcPopupDialog),
            new FrameworkPropertyMetadata(Visibility.Visible, FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty ClosePopupButtonStyleProperty = DependencyProperty.RegisterAttached(
            "ClosePopupButtonStyle",
            typeof (Style),
            typeof (RrcPopupDialog),
            new FrameworkPropertyMetadata(default(Style), FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.RegisterAttached(
            "Header",
            typeof (object),
            typeof (RrcPopupDialog),
            new FrameworkPropertyMetadata(default(object), FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty HeaderTemplateSelectorProperty = DependencyProperty.RegisterAttached(
            "HeaderTemplateSelector",
            typeof (DataTemplateSelector),
            typeof (RrcPopupDialog),
            new FrameworkPropertyMetadata(default(DataTemplateSelector), FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.RegisterAttached(
            "HeaderTemplate",
            typeof (DataTemplate),
            typeof (RrcPopupDialog),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty HeaderBackgroundProperty = DependencyProperty.RegisterAttached(
            "HeaderBackground",
            typeof (Brush),
            typeof (RrcPopupDialog),
            new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty HeaderBorderStyleProperty = DependencyProperty.RegisterAttached(
            "HeaderBorderStyle",
            typeof (Style),
            typeof (RrcPopupDialog),
            new FrameworkPropertyMetadata(default(Style), FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty HeaderHorizontalAlignmentProperty = DependencyProperty.RegisterAttached(
            "HeaderHorizontalAlignment",
            typeof (HorizontalAlignment),
            typeof (RrcPopupDialog),
            new FrameworkPropertyMetadata(default(HorizontalAlignment), FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty HeaderVerticalAlignmentProperty = DependencyProperty.RegisterAttached(
            "HeaderVerticalAlignment",
            typeof (VerticalAlignment),
            typeof (RrcPopupDialog),
            new FrameworkPropertyMetadata(default(VerticalAlignment), FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty HeaderVisibilityProperty = DependencyProperty.RegisterAttached(
            "HeaderVisibility",
            typeof (Visibility),
            typeof (RrcPopupDialog),
            new FrameworkPropertyMetadata(default(Visibility), FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty StyleByPopupContentSelectorProperty = DependencyProperty.Register(
            "StyleByPopupContentSelector",
            typeof (StyleSelector),
            typeof (RrcPopupDialog),
            new FrameworkPropertyMetadata(default(StyleSelector), FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty StaysOpenProperty = DependencyProperty.RegisterAttached(
            "StaysOpen",
            typeof (bool),
            typeof (RrcPopupDialog),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty PopupAnimationProperty = DependencyProperty.RegisterAttached(
            "PopupAnimation",
            typeof (RrcPopupDialogAnimation),
            typeof (RrcPopupDialog),
            new FrameworkPropertyMetadata(default(RrcPopupDialogAnimation), FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty PopupAnimationDurationProperty = DependencyProperty.Register(
            "PopupAnimationDuration",
            typeof (Duration),
            typeof (RrcPopupDialog),
            new FrameworkPropertyMetadata(new Duration(TimeSpan.FromMilliseconds(200))));

        public static readonly DependencyProperty HeaderContentMarginProperty = DependencyProperty.RegisterAttached(
            "HeaderContentMargin",
            typeof (Thickness),
            typeof (RrcPopupDialog),
            new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.Inherits));


        public event EventHandler IsOpened = delegate { };
        public event EventHandler IsClosed = delegate { };

        public Thickness HeaderContentMargin
        {
            get { return (Thickness)GetValue(HeaderContentMarginProperty); }
            set { SetValue(HeaderContentMarginProperty, value); }
        }

        public Duration PopupAnimationDuration
        {
            get { return (Duration) GetValue(PopupAnimationDurationProperty); }
            set { SetValue(PopupAnimationDurationProperty, value); }
        }

        public RrcPopupDialogAnimation PopupAnimation
        {
            get { return (RrcPopupDialogAnimation) GetValue(PopupAnimationProperty); }
            set { SetValue(PopupAnimationProperty, value); }
        }

        public Style ClosePopupButtonStyle
        {
            get { return (Style) GetValue(ClosePopupButtonStyleProperty); }
            set { SetValue(ClosePopupButtonStyleProperty, value); }
        }

        public object Header
        {
            get { return GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public DataTemplateSelector HeaderTemplateSelector
        {
            get { return (DataTemplateSelector) GetValue(HeaderTemplateSelectorProperty); }
            set { SetValue(HeaderTemplateSelectorProperty, value); }
        }

        public Visibility HeaderVisibility
        {
            get { return (Visibility) GetValue(HeaderVisibilityProperty); }
            set { SetValue(HeaderVisibilityProperty, value); }
        }

        public VerticalAlignment HeaderVerticalAlignment
        {
            get { return (VerticalAlignment) GetValue(HeaderVerticalAlignmentProperty); }
            set { SetValue(HeaderVerticalAlignmentProperty, value); }
        }

        public HorizontalAlignment HeaderHorizontalAlignment
        {
            get { return (HorizontalAlignment) GetValue(HeaderHorizontalAlignmentProperty); }
            set { SetValue(HeaderHorizontalAlignmentProperty, value); }
        }

        public Style HeaderBorderStyle
        {
            get { return (Style) GetValue(HeaderBorderStyleProperty); }
            set { SetValue(HeaderBorderStyleProperty, value); }
        }

        public bool AutoFocus
        {
            get { return (bool) GetValue(AutoFocusProperty); }
            set { SetValue(AutoFocusProperty, value); }
        }

        public bool IsOpen
        {
            get { return (bool) GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        public FrameworkElement PopupContent
        {
            get { return (FrameworkElement) GetValue(PopupContentProperty); }
            set { SetValue(PopupContentProperty, value); }
        }

        public Style OverlayRectangleStyle
        {
            get { return (Style) GetValue(OverlayRectangleStyleProperty); }
            set { SetValue(OverlayRectangleStyleProperty, value); }
        }

        public Effect PopupBorderEffect
        {
            get { return (Effect) GetValue(PopupBorderEffectProperty); }
            set { SetValue(PopupBorderEffectProperty, value); }
        }

        public HorizontalAlignment PopupContentHorizontalAlignment
        {
            get { return (HorizontalAlignment) GetValue(PopupContentHorizontalAlignmentProperty); }
            set { SetValue(PopupContentHorizontalAlignmentProperty, value); }
        }

        public VerticalAlignment PopupContentVerticalAlignment
        {
            get { return (VerticalAlignment) GetValue(PopupContentVerticalAlignmentProperty); }
            set { SetValue(PopupContentVerticalAlignmentProperty, value); }
        }

        public Thickness PopupContentMargin
        {
            get { return (Thickness) GetValue(PopupContentMarginProperty); }
            set { SetValue(PopupContentMarginProperty, value); }
        }

        public double PopupContentMaxWidth
        {
            get { return (double) GetValue(PopupContentMaxWidthProperty); }
            set { SetValue(PopupContentMaxWidthProperty, value); }
        }

        public double PopupContentMaxHeight
        {
            get { return (double) GetValue(PopupContentMaxHeightProperty); }
            set { SetValue(PopupContentMaxHeightProperty, value); }
        }

        public Thickness PopupContentPadding
        {
            get { return (Thickness) GetValue(PopupContentPaddingProperty); }
            set { SetValue(PopupContentPaddingProperty, value); }
        }

        public Thickness PopupContentBorderThickness
        {
            get { return (Thickness) GetValue(PopupContentBorderThicknessProperty); }
            set { SetValue(PopupContentBorderThicknessProperty, value); }
        }

        public Brush PopupContentBorderBrush
        {
            get { return (Brush) GetValue(PopupContentBorderBrushProperty); }
            set { SetValue(PopupContentBorderBrushProperty, value); }
        }

        public CornerRadius PopupContentCornerRadius
        {
            get { return (CornerRadius) GetValue(PopupContentCornerRadiusProperty); }
            set { SetValue(PopupContentCornerRadiusProperty, value); }
        }

        public Brush PopupContentBackground
        {
            get { return (Brush) GetValue(PopupContentBackgroundProperty); }
            set { SetValue(PopupContentBackgroundProperty, value); }
        }

        public ICommand ClosePopupCommand
        {
            get { return (ICommand) GetValue(ClosePopupCommandProperty); }
            set { SetValue(ClosePopupCommandProperty, value); }
        }

        public object ClosePopupCommandParameter
        {
            get { return GetValue(ClosePopupCommandParameterProperty); }
            set { SetValue(ClosePopupCommandParameterProperty, value); }
        }

        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate) GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        public Brush HeaderBackground
        {
            get { return (Brush) GetValue(HeaderBackgroundProperty); }
            set { SetValue(HeaderBackgroundProperty, value); }
        }

        public StyleSelector StyleByPopupContentSelector
        {
            get { return (StyleSelector) GetValue(StyleByPopupContentSelectorProperty); }
            set { SetValue(StyleByPopupContentSelectorProperty, value); }
        }

        public Visibility ClosePopupButtonVisibility
        {
            get { return (Visibility) GetValue(ClosePopupButtonVisibilityProperty); }
            set { SetValue(ClosePopupButtonVisibilityProperty, value); }
        }

        public bool StaysOpen
        {
            get { return (bool) GetValue(StaysOpenProperty); }
            set { SetValue(StaysOpenProperty, value); }
        }


        static RrcPopupDialog()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (RrcPopupDialog),
                                                     new FrameworkPropertyMetadata(typeof (RrcPopupDialog)));
        }

        public static Thickness GetHeaderContentMargin(FrameworkElement element)
        {
            return (Thickness)element.GetValue(HeaderContentMarginProperty);
        }

        public static void SetHeaderContentMargin(FrameworkElement element, Thickness value)
        {
            element.SetValue(HeaderContentMarginProperty, value);
        }

        public static RrcPopupDialogAnimation GetPopupAnimation(FrameworkElement element)
        {
            return (RrcPopupDialogAnimation) element.GetValue(PopupAnimationProperty);
        }

        public static void SetPopupAnimation(FrameworkElement element, RrcPopupDialogAnimation value)
        {
            element.SetValue(PopupAnimationProperty, value);
        }

        public static bool GetStaysOpen(FrameworkElement element)
        {
            return (bool) element.GetValue(StaysOpenProperty);
        }

        public static void SetStaysOpen(FrameworkElement element, bool value)
        {
            element.SetValue(StaysOpenProperty, value);
        }

        public static Style GetOverlayRectangleStyle(FrameworkElement element)
        {
            return (Style) element.GetValue(OverlayRectangleStyleProperty);
        }

        public static Effect GetPopupBorderEffect(FrameworkElement element)
        {
            return (Effect) element.GetValue(PopupBorderEffectProperty);
        }

        public static void SetOverlayRectangleStyle(FrameworkElement element, Style value)
        {
            element.SetValue(OverlayRectangleStyleProperty, value);
        }

        public static void SetPopupBorderEffect(FrameworkElement element, Effect value)
        {
            element.SetValue(PopupBorderEffectProperty, value);
        }

        public static HorizontalAlignment GetPopupContentHorizontalAlignment(FrameworkElement element)
        {
            return (HorizontalAlignment) element.GetValue(PopupContentHorizontalAlignmentProperty);
        }

        public static void SetPopupContentHorizontalAlignment(FrameworkElement element, HorizontalAlignment value)
        {
            element.SetValue(PopupContentHorizontalAlignmentProperty, value);
        }

        public static VerticalAlignment GetPopupContentVerticalAlignment(FrameworkElement element)
        {
            return (VerticalAlignment) element.GetValue(PopupContentVerticalAlignmentProperty);
        }

        public static void SetPopupContentVerticalAlignment(FrameworkElement element, VerticalAlignment value)
        {
            element.SetValue(PopupContentVerticalAlignmentProperty, value);
        }

        public static Thickness GetPopupContentMargin(FrameworkElement element)
        {
            return (Thickness) element.GetValue(PopupContentMarginProperty);
        }

        public static void SetPopupContentMargin(FrameworkElement element, Thickness value)
        {
            element.SetValue(PopupContentMarginProperty, value);
        }

        public static double GetPopupContentMaxWidth(FrameworkElement element)
        {
            return (double) element.GetValue(PopupContentMaxWidthProperty);
        }

        public static void SetPopupContentMaxWidth(FrameworkElement element, double value)
        {
            element.SetValue(PopupContentMaxWidthProperty, value);
        }

        public static double GetPopupContentMaxHeight(FrameworkElement element)
        {
            return (double) element.GetValue(PopupContentMaxHeightProperty);
        }

        public static void SetPopupContentMaxHeight(FrameworkElement element, double value)
        {
            element.SetValue(PopupContentMaxHeightProperty, value);
        }

        public static Thickness GetPopupContentPadding(FrameworkElement element)
        {
            return (Thickness) element.GetValue(PopupContentPaddingProperty);
        }

        public static void SetPopupContentPadding(FrameworkElement element, Thickness value)
        {
            element.SetValue(PopupContentPaddingProperty, value);
        }

        public static Thickness GetPopupContentBorderThickness(FrameworkElement element)
        {
            return (Thickness) element.GetValue(PopupContentBorderThicknessProperty);
        }

        public static void SetPopupContentBorderThickness(FrameworkElement element, Thickness value)
        {
            element.SetValue(PopupContentBorderThicknessProperty, value);
        }

        public static Brush GetPopupContentBorderBrush(FrameworkElement element)
        {
            return (Brush) element.GetValue(PopupContentBorderBrushProperty);
        }

        public static void SetPopupContentBorderBrush(FrameworkElement element, Brush value)
        {
            element.SetValue(PopupContentBorderBrushProperty, value);
        }

        public static CornerRadius GetPopupContentCornerRadius(FrameworkElement element)
        {
            return (CornerRadius) element.GetValue(PopupContentCornerRadiusProperty);
        }

        public static void SetPopupContentCornerRadius(FrameworkElement element, CornerRadius value)
        {
            element.SetValue(PopupContentCornerRadiusProperty, value);
        }

        public static Brush GetPopupContentBackground(FrameworkElement element)
        {
            return (Brush) element.GetValue(PopupContentBackgroundProperty);
        }

        public static void SetPopupContentBackground(FrameworkElement element, Brush value)
        {
            element.SetValue(PopupContentBackgroundProperty, value);
        }

        public static bool GetAutoFocus(FrameworkElement element)
        {
            return (bool) element.GetValue(AutoFocusProperty);
        }

        public static void SetAutoFocus(FrameworkElement element, bool value)
        {
            element.SetValue(AutoFocusProperty, value);
        }

        public static ICommand GetClosePopupCommand(FrameworkElement element)
        {
            return (ICommand) element.GetValue(ClosePopupCommandProperty);
        }

        public static void SetClosePopupCommand(FrameworkElement element, ICommand value)
        {
            element.SetValue(ClosePopupCommandProperty, value);
        }

        public static object GetClosePopupCommandParameter(FrameworkElement element)
        {
            return element.GetValue(ClosePopupCommandParameterProperty);
        }

        public static void SetClosePopupCommandParameter(FrameworkElement element, object value)
        {
            element.SetValue(ClosePopupCommandParameterProperty, value);
        }

        public static Style GetClosePopupButtonStyle(FrameworkElement element)
        {
            return (Style) element.GetValue(ClosePopupButtonStyleProperty);
        }

        public static void SetClosePopupButtonStyle(FrameworkElement element, Style value)
        {
            element.SetValue(ClosePopupButtonStyleProperty, value);
        }

        public static object GetHeader(FrameworkElement element)
        {
            return element.GetValue(HeaderProperty);
        }

        public static void SetHeader(FrameworkElement element, object value)
        {
            element.SetValue(HeaderProperty, value);
        }

        public static DataTemplateSelector GetHeaderTemplateSelector(FrameworkElement element)
        {
            return (DataTemplateSelector) element.GetValue(HeaderTemplateSelectorProperty);
        }

        public static void SetHeaderTemplateSelector(FrameworkElement element, DataTemplateSelector value)
        {
            element.SetValue(HeaderTemplateSelectorProperty, value);
        }

        public static DataTemplate GetHeaderTemplate(FrameworkElement element)
        {
            return (DataTemplate) element.GetValue(HeaderTemplateProperty);
        }

        public static void SetHeaderTemplate(FrameworkElement element, DataTemplate value)
        {
            element.SetValue(HeaderTemplateProperty, value);
        }

        public static Visibility GetHeaderVisibility(FrameworkElement element)
        {
            return (Visibility) element.GetValue(HeaderVisibilityProperty);
        }

        public static void SetHeaderVisibility(FrameworkElement element, Visibility value)
        {
            element.SetValue(HeaderVisibilityProperty, value);
        }

        public static VerticalAlignment GetHeaderVerticalAlignment(FrameworkElement element)
        {
            return (VerticalAlignment) element.GetValue(HeaderVerticalAlignmentProperty);
        }

        public static void SetHeaderVerticalAlignment(FrameworkElement element, VerticalAlignment value)
        {
            element.SetValue(HeaderVerticalAlignmentProperty, value);
        }

        public static HorizontalAlignment GetHeaderHorizontalAlignment(FrameworkElement element)
        {
            return (HorizontalAlignment) element.GetValue(HeaderHorizontalAlignmentProperty);
        }

        public static void SetHeaderHorizontalAlignment(FrameworkElement element, HorizontalAlignment value)
        {
            element.SetValue(HeaderHorizontalAlignmentProperty, value);
        }

        public static Style GetHeaderBorderStyle(FrameworkElement element)
        {
            return (Style) element.GetValue(HeaderBorderStyleProperty);
        }

        public static void SetHeaderBorderStyle(FrameworkElement element, Style value)
        {
            element.SetValue(HeaderBorderStyleProperty, value);
        }

        public static Brush GetHeaderBackground(FrameworkElement element)
        {
            return (Brush) element.GetValue(HeaderBackgroundProperty);
        }

        public static void SetHeaderBackground(FrameworkElement element, Brush value)
        {
            element.SetValue(HeaderBackgroundProperty, value);
        }

        public static Visibility GetClosePopupButtonVisibility(FrameworkElement element)
        {
            return (Visibility) element.GetValue(ClosePopupButtonVisibilityProperty);
        }

        public static void SetClosePopupButtonVisibility(FrameworkElement element, Visibility value)
        {
            element.SetValue(ClosePopupButtonVisibilityProperty, value);
        }

        private static void OnIsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((RrcPopupDialog) d).OnIsOpenChanged(e);
        }

        private static void OnPopupContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dialog = d as RrcPopupDialog;
            if (dialog == null)
            {
                return;
            }

            dialog.SetStyleIfPossible();

            if (dialog.AutoFocus && dialog.PopupContent != null)
            {
                dialog.PopupContent.Loaded += RrcPopupDialogLoaded;
            }
        }

        private static void RrcPopupDialogLoaded(object sender, RoutedEventArgs e)
        {
            var content = (FrameworkElement) sender;
            content.Loaded -= RrcPopupDialogLoaded;


            var focusableChild =
                content.FindVisualChild<FrameworkElement>(el => el.Focusable && KeyboardNavigation.GetIsTabStop(el));
            if (focusableChild != null)
            {
                focusableChild.Focus();
            }
        }

        /// <summary>
        ///     Overrides the OnApplyTemplate method.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            ChangeVisualState(false);

            var closeButton = (Button) Template.FindName("PART_CloseButton", this);
            if (closeButton != null)
            {
                closeButton.Click += (o, args) =>
                                         {
                                             if (((Button) o).Command == null)
                                             {
                                                 IsOpen = false;
                                             }
                                         };
            }

            var overlayRectangle = (Rectangle) Template.FindName("PART_OverlayRectangle", this);
            if (overlayRectangle != null)
            {
                overlayRectangle.MouseDown += delegate { OnOverlayRectangleMouseDown(); };
            }

            var popupConentBorder = (Border) Template.FindName("PART_PopupConentBorder", this);
            if (popupConentBorder != null)
            {
                popupConentBorder.IsVisibleChanged += OnPopupContentBorderVisibilityChanged;
            }

            SetStyleIfPossible();
        }

        /// <summary>
        ///     IsOpenProperty property changed handler.
        /// </summary>
        /// <param name="e"> Event arguments. </param>
        protected virtual void OnIsOpenChanged(DependencyPropertyChangedEventArgs e)
        {
            ChangeVisualState(true);
            if (IsOpen)
            {
                IsOpened(this, EventArgs.Empty);
            }
            else
            {
                IsClosed(this, EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Changes the control's visual state(s).
        /// </summary>
        /// <param name="useTransitions"> True if state transitions should be used. </param>
        protected virtual void ChangeVisualState(bool useTransitions)
        {
            var state = !IsOpen
                            ? RrcPopupDialogVisualStates.StateHidden
                            : RrcPopupDialogVisualStates.StateVisible;

            VisualStateManager.GoToState(this, state, useTransitions);
        }

        private void OnPopupContentBorderVisibilityChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsOpen)
            {
                var popupAnimation = PopupContent != null
                                         ? GetPopupAnimation(PopupContent)
                                         : PopupAnimation;

                switch (popupAnimation)
                {
                    case RrcPopupDialogAnimation.Fade:
                        VisualStateManager.GoToState(this, RrcPopupDialogVisualStates.StateFade, false);
                        break;
                    case RrcPopupDialogAnimation.SlideDown:
                        ((Border) sender).UpdateLayout();
                        VisualStateManager.GoToState(this, RrcPopupDialogVisualStates.StateSlideDown, false);
                        break;
                }
            }
        }

        private void OnOverlayRectangleMouseDown()
        {
            var staysOpen = PopupContent != null
                                ? GetStaysOpen(PopupContent)
                                : StaysOpen;
            if (staysOpen)
            {
                return;
            }

            var commandParameter = PopupContent != null
                                       ? GetClosePopupCommandParameter(PopupContent)
                                       : ClosePopupCommandParameter;
            var command = PopupContent != null
                              ? GetClosePopupCommand(PopupContent)
                              : ClosePopupCommand;

            if (command != null && command.CanExecute(commandParameter))
            {
                command.Execute(commandParameter);
            }
            else if (command == null)
            {
                IsOpen = false;
            }
        }

        private void SetStyleIfPossible()
        {
            if (StyleByPopupContentSelector == null)
            {
                return;
            }

            Style = StyleByPopupContentSelector.SelectStyle(PopupContent, this);
        }
    }
}