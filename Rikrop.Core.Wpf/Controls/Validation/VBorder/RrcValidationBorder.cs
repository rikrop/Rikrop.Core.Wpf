using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Rikrop.Core.Wpf.Controls.Validation.VBorder
{
    public class RrcValidationBorder : Border
    {
        public static readonly DependencyProperty ValidationBindingProperty = DependencyProperty.Register(
            "ValidationBinding",
            typeof (object),
            typeof (RrcValidationBorder),
            new PropertyMetadata(null));

        public object ValidationBinding
        {
            get { return GetValue(ValidationBindingProperty); }
            set { SetValue(ValidationBindingProperty, value); }
        }

        public static readonly DependencyProperty ValidationErrorBrushProperty =
            DependencyProperty.Register("ValidationErrorBrush", typeof(Brush), typeof(RrcValidationBorder), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(0xFF, 0xFA, 0xCb, 0xD1))));

        public Brush ValidationErrorBrush
        {
            get { return (Brush) GetValue(ValidationErrorBrushProperty); }
            set { SetValue(ValidationErrorBrushProperty, value); }
        }

        static RrcValidationBorder()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof (RrcValidationBorder),
                new FrameworkPropertyMetadata(typeof (RrcValidationBorder)));
        }

        public static readonly DependencyProperty ErrorInToolTipTemplateProperty =
            DependencyProperty.Register("ErrorInToolTipTemplate", typeof (DataTemplate), typeof (RrcValidationBorder), new PropertyMetadata(default(DataTemplate)));

        public DataTemplate ErrorInToolTipTemplate
        {
            get { return (DataTemplate) GetValue(ErrorInToolTipTemplateProperty); }
            set { SetValue(ErrorInToolTipTemplateProperty, value); }
        }
    }
}