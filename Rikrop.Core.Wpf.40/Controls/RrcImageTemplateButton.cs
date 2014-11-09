using System.Windows;
using System.Windows.Controls;

namespace Rikrop.Core.Wpf.Controls
{
    public class RrcImageTemplateButton : Button
    {
        public static readonly DependencyProperty MouseOverImageTemplateProperty = DependencyProperty.Register(
            "MouseOverImageTemplate",
            typeof (DataTemplate),
            typeof (RrcImageTemplateButton),
            new PropertyMetadata(default(DataTemplate)));

        public static readonly DependencyProperty PressedImageTemplateProperty = DependencyProperty.Register(
            "PressedImageTemplate",
            typeof (DataTemplate),
            typeof (RrcImageTemplateButton),
            new PropertyMetadata(default(DataTemplate)));

        public static readonly DependencyProperty DisabledImageTemplateProperty = DependencyProperty.Register(
            "DisabledImageTemplate",
            typeof (DataTemplate),
            typeof (RrcImageTemplateButton),
            new PropertyMetadata(default(DataTemplate)));

        public static readonly DependencyProperty ImageTemplateProperty = DependencyProperty.Register(
            "ImageTemplate",
            typeof (DataTemplate),
            typeof (RrcImageTemplateButton),
            new PropertyMetadata(default(DataTemplate)));

        public DataTemplate MouseOverImageTemplate
        {
            get { return (DataTemplate) GetValue(MouseOverImageTemplateProperty); }
            set { SetValue(MouseOverImageTemplateProperty, value); }
        }

        public DataTemplate PressedImageTemplate
        {
            get { return (DataTemplate) GetValue(PressedImageTemplateProperty); }
            set { SetValue(PressedImageTemplateProperty, value); }
        }

        public DataTemplate DisabledImageTemplate
        {
            get { return (DataTemplate) GetValue(DisabledImageTemplateProperty); }
            set { SetValue(DisabledImageTemplateProperty, value); }
        }

        public DataTemplate ImageTemplate
        {
            get { return (DataTemplate) GetValue(ImageTemplateProperty); }
            set { SetValue(ImageTemplateProperty, value); }
        }

        static RrcImageTemplateButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (RrcImageTemplateButton), new FrameworkPropertyMetadata(typeof (RrcImageTemplateButton)));
        }
    }
}