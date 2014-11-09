using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Rikrop.Core.Wpf.Controls
{
    public class RrcTextBox : TextBox
    {
        public static readonly DependencyProperty ShadowIfReadOnlyProperty =
            DependencyProperty.Register("ShadowIfReadOnly", typeof (bool), typeof (RrcTextBox),
                                        new PropertyMetadata(false));

        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register(
            "Watermark",
            typeof (object),
            typeof (RrcTextBox),
            new PropertyMetadata(default(object)));

        public static readonly DependencyProperty WatermarkTemplateProperty = DependencyProperty.Register(
            "WatermarkTemplate",
            typeof (DataTemplate),
            typeof (RrcTextBox),
            new PropertyMetadata(default(DataTemplate)));

        public bool ShadowIfReadOnly
        {
            get { return (bool) GetValue(ShadowIfReadOnlyProperty); }
            set { SetValue(ShadowIfReadOnlyProperty, value); }
        }

        public object Watermark
        {
            get { return GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }

        public DataTemplate WatermarkTemplate
        {
            get { return (DataTemplate) GetValue(WatermarkTemplateProperty); }
            set { SetValue(WatermarkTemplateProperty, value); }
        }

        static RrcTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (RrcTextBox),
                                                     new FrameworkPropertyMetadata(typeof (RrcTextBox)));
            TextProperty.OverrideMetadata(typeof (RrcTextBox),
                                          new FrameworkPropertyMetadata(string.Empty,
                                                                        FrameworkPropertyMetadataOptions.Journal |
                                                                        FrameworkPropertyMetadataOptions.
                                                                            BindsTwoWayByDefault, null, null, true,
                                                                        UpdateSourceTrigger.PropertyChanged));
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property == AcceptsReturnProperty && AcceptsReturn)
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                TextWrapping = TextWrapping.Wrap;
            }
        }
    }
}