using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Rikrop.Core.Wpf.Controls
{
    /// <summary>
    ///   A control to provide a visual indicator when an application is busy.
    /// </summary>
    [TemplateVisualState(Name = RrcBusyIndicatorVisualStates.StateIdle, GroupName = RrcBusyIndicatorVisualStates.GroupBusyStatus)]
    [TemplateVisualState(Name = RrcBusyIndicatorVisualStates.StateBusy, GroupName = RrcBusyIndicatorVisualStates.GroupBusyStatus)]
    [TemplateVisualState(Name = RrcBusyIndicatorVisualStates.StateVisible, GroupName = RrcBusyIndicatorVisualStates.GroupVisibility)]
    [TemplateVisualState(Name = RrcBusyIndicatorVisualStates.StateHidden, GroupName = RrcBusyIndicatorVisualStates.GroupVisibility)]
    [StyleTypedProperty(Property = "OverlayStyle", StyleTargetType = typeof (Rectangle))]
    [StyleTypedProperty(Property = "RrcLoadingIndicatorStyle", StyleTargetType = typeof (RrcLoadingIndicator))]
    public class RrcBusyIndicator : ContentControl
    {
        ///// <summary>
        /////   Identifies the IsBusy dependency property.
        ///// </summary>
        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.RegisterAttached(
            "IsBusy", 
            typeof (bool), 
            typeof (RrcBusyIndicator), 
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits, OnIsBusyChanged));

        public static void SetIsBusy(UIElement element, bool value)
        {
            element.SetValue(IsBusyProperty, value);
        }

        public static bool GetIsBusy(UIElement element)
        {
            return (bool) element.GetValue(IsBusyProperty);
        }

        /// <summary>
        ///   Identifies the BusyContent dependency property.
        /// </summary>
        public static readonly DependencyProperty BusyContentProperty = DependencyProperty.Register(
            "BusyContent",
            typeof (object),
            typeof (RrcBusyIndicator),
            new PropertyMetadata(null));

        /// <summary>
        ///   Identifies the BusyTemplate dependency property.
        /// </summary>
        public static readonly DependencyProperty BusyContentTemplateProperty = DependencyProperty.Register(
            "BusyContentTemplate",
            typeof (DataTemplate),
            typeof (RrcBusyIndicator),
            new PropertyMetadata(null));

        /// <summary>
        ///   Identifies the DisplayAfter dependency property.
        /// </summary>
        public static readonly DependencyProperty DisplayAfterProperty = DependencyProperty.Register(
            "DisplayAfter",
            typeof (TimeSpan),
            typeof (RrcBusyIndicator),
            new PropertyMetadata(TimeSpan.FromSeconds(0.1)));

        /// <summary>
        ///   Identifies the OverlayStyle dependency property.
        /// </summary>
        public static readonly DependencyProperty OverlayStyleProperty = DependencyProperty.Register(
            "OverlayStyle",
            typeof (Style),
            typeof (RrcBusyIndicator),
            new PropertyMetadata(null));

        /// <summary>
        ///   Identifies the RrcLoadingIndicatorStyle dependency property.
        /// </summary>
        public static readonly DependencyProperty RrcLoadingIndicatorStyleProperty = DependencyProperty.Register(
            "RrcLoadingIndicatorStyle",
            typeof (Style),
            typeof (RrcBusyIndicator),
            new PropertyMetadata(null));

        /// <summary>
        ///   Timer used to delay the initial display and avoid flickering.
        /// </summary>
        private readonly DispatcherTimer _displayAfterTimer = new DispatcherTimer();

        /// <summary>
        ///   Gets or sets a value indicating the busy content to display to the user.
        /// </summary>
        public object BusyContent
        {
            get { return GetValue(BusyContentProperty); }
            set { SetValue(BusyContentProperty, value); }
        }

        /// <summary>
        ///   Gets or sets a value indicating the template to use for displaying the busy content to the user.
        /// </summary>
        public DataTemplate BusyContentTemplate
        {
            get { return (DataTemplate) GetValue(BusyContentTemplateProperty); }
            set { SetValue(BusyContentTemplateProperty, value); }
        }

        /// <summary>
        ///   Gets or sets a value indicating how long to delay before displaying the busy content.
        /// </summary>
        public TimeSpan DisplayAfter
        {
            get { return (TimeSpan) GetValue(DisplayAfterProperty); }
            set { SetValue(DisplayAfterProperty, value); }
        }

        /// <summary>
        ///   Gets or sets a value indicating the style to use for the overlay.
        /// </summary>
        public Style OverlayStyle
        {
            get { return (Style) GetValue(OverlayStyleProperty); }
            set { SetValue(OverlayStyleProperty, value); }
        }

        /// <summary>
        ///   Gets or sets a value indicating the style to use for the RrcLoadingIndicator.
        /// </summary>
        public Style RrcLoadingIndicatorStyle
        {
            get { return (Style) GetValue(RrcLoadingIndicatorStyleProperty); }
            set { SetValue(RrcLoadingIndicatorStyleProperty, value); }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether the BusyContent is visible.
        /// </summary>
        protected bool IsContentVisible { get; set; }

        static RrcBusyIndicator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (RrcBusyIndicator), new FrameworkPropertyMetadata(typeof (RrcBusyIndicator)));
        }

        public RrcBusyIndicator()
        {
            _displayAfterTimer.Tick += DisplayAfterTimerElapsed;
        }

        /// <summary>
        ///   IsBusyProperty property changed handler.
        /// </summary>
        /// <param name="d"> RrcBusyIndicator that changed its IsBusy. </param>
        /// <param name="e"> Event arguments. </param>
        private static void OnIsBusyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var bi = d as RrcBusyIndicator;
            if (bi == null)
            {
                return;
            }
            bi.OnIsBusyChanged(e);
        }

        /// <summary>
        ///   Overrides the OnApplyTemplate method.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ChangeVisualState(false);
        }

        /// <summary>
        ///   IsBusyProperty property changed handler.
        /// </summary>
        /// <param name="e"> Event arguments. </param>
        protected virtual void OnIsBusyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (GetIsBusy(this))
            {
                if (DisplayAfter.Equals(TimeSpan.Zero))
                {
                    // Go visible now
                    IsContentVisible = true;
                }
                else
                {
                    // Set a timer to go visible
                    _displayAfterTimer.Interval = DisplayAfter;
                    _displayAfterTimer.Start();
                }
            }
            else
            {
                // No longer visible
                _displayAfterTimer.Stop();
                IsContentVisible = false;
            }

            ChangeVisualState(true);
        }

        /// <summary>
        ///   Changes the control's visual state(s).
        /// </summary>
        /// <param name="useTransitions"> True if state transitions should be used. </param>
        protected virtual void ChangeVisualState(bool useTransitions)
        {
            VisualStateManager.GoToState(this, GetIsBusy(this)
                                                   ? RrcBusyIndicatorVisualStates.StateBusy
                                                   : RrcBusyIndicatorVisualStates.StateIdle, useTransitions);
            VisualStateManager.GoToState(this, IsContentVisible
                                                   ? RrcBusyIndicatorVisualStates.StateVisible
                                                   : RrcBusyIndicatorVisualStates.StateHidden, useTransitions);
        }

        /// <summary>
        ///   Handler for the DisplayAfterTimer.
        /// </summary>
        /// <param name="sender"> Event sender. </param>
        /// <param name="e"> Event arguments. </param>
        private void DisplayAfterTimerElapsed(object sender, EventArgs e)
        {
            _displayAfterTimer.Stop();
            IsContentVisible = true;
            ChangeVisualState(true);
        }
    }
}