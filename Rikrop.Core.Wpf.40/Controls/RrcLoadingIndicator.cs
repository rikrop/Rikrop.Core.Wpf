using System.Windows;
using System.Windows.Controls;

namespace Rikrop.Core.Wpf.Controls
{
    public class RrcLoadingIndicator : Control
    {
        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register("IsLoading", typeof (bool), typeof (RrcLoadingIndicator),
                                        new PropertyMetadata(false, IsLoadingChangedCallback));

        public static readonly DependencyProperty NotLoadingVisibilityProperty = DependencyProperty.Register(
            "NotLoadingVisibility",
            typeof (Visibility),
            typeof (RrcLoadingIndicator),
            new PropertyMetadata(Visibility.Collapsed, NotLoadingVisibilityChangedCallback));

        public static readonly DependencyProperty LoadingContentTemplateProperty = DependencyProperty.Register(
            "LoadingContentTemplate",
            typeof (DataTemplate),
            typeof (RrcLoadingIndicator),
            new PropertyMetadata(null));

        public static readonly DependencyProperty LoadingContentProperty =
            DependencyProperty.Register("LoadingContent", typeof (object), typeof (RrcLoadingIndicator), new PropertyMetadata(default(object)));

        public static readonly DependencyProperty LoadingContentTemplateSelectorProperty =
            DependencyProperty.Register("LoadingContentTemplateSelector", typeof (DataTemplateSelector), typeof (RrcLoadingIndicator), new PropertyMetadata(default(DataTemplateSelector)));

        public static readonly DependencyProperty LoadingContentStringFormatProperty =
            DependencyProperty.Register("LoadingContentStringFormat", typeof (string), typeof (RrcLoadingIndicator), new PropertyMetadata(default(string)));

        public object LoadingContent
        {
            get { return GetValue(LoadingContentProperty); }
            set { SetValue(LoadingContentProperty, value); }
        }

        public DataTemplateSelector LoadingContentTemplateSelector
        {
            get { return (DataTemplateSelector) GetValue(LoadingContentTemplateSelectorProperty); }
            set { SetValue(LoadingContentTemplateSelectorProperty, value); }
        }

        public string LoadingContentStringFormat
        {
            get { return (string) GetValue(LoadingContentStringFormatProperty); }
            set { SetValue(LoadingContentStringFormatProperty, value); }
        }

        public bool IsLoading
        {
            get { return (bool) GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        public Visibility NotLoadingVisibility
        {
            get { return (Visibility) GetValue(NotLoadingVisibilityProperty); }
            set { SetValue(NotLoadingVisibilityProperty, value); }
        }

        public DataTemplate LoadingContentTemplate
        {
            get { return (DataTemplate) GetValue(LoadingContentTemplateProperty); }
            set { SetValue(LoadingContentTemplateProperty, value); }
        }

        static RrcLoadingIndicator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (RrcLoadingIndicator),
                                                     new FrameworkPropertyMetadata(typeof (RrcLoadingIndicator)));
        }

        public RrcLoadingIndicator()
        {
            RefreshVisibility();
        }

        private static void IsLoadingChangedCallback(DependencyObject dependencyObject,
                                                     DependencyPropertyChangedEventArgs args)
        {
            var d = dependencyObject as RrcLoadingIndicator;
            if (d != null)
            {
                d.RefreshVisibility();
            }
        }

        private static void NotLoadingVisibilityChangedCallback(DependencyObject dependencyObject,
                                                                DependencyPropertyChangedEventArgs args)
        {
            var d = dependencyObject as RrcLoadingIndicator;
            if (d != null)
            {
                d.RefreshVisibility();
            }
        }

        private void RefreshVisibility()
        {
            Visibility = IsLoading
                             ? Visibility.Visible
                             : NotLoadingVisibility;
        }
    }
}