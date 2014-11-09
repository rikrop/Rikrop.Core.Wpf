using System.Windows;
using System.Windows.Controls;

namespace Rikrop.Core.Wpf.Controls
{
    public class RrcCheckBox : CheckBox
    {
        static RrcCheckBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RrcCheckBox), new FrameworkPropertyMetadata(typeof(RrcCheckBox)));
            VerticalAlignmentProperty.OverrideMetadata(typeof(RrcCheckBox), new FrameworkPropertyMetadata(VerticalAlignment.Center));
            VerticalContentAlignmentProperty.OverrideMetadata(typeof(RrcCheckBox), new FrameworkPropertyMetadata(VerticalAlignment.Center));
        }

        //#region CheckBoxImageWidth Property

        //public static readonly DependencyProperty CheckBoxImageWidthProperty = DependencyProperty.Register(
        //    "CheckBoxImageWidth",
        //    typeof(double),
        //    typeof(RrcCheckBox),
        //    new PropertyMetadata(20d));

        //public double CheckBoxImageWidth
        //{
        //    get { return (double)GetValue(CheckBoxImageWidthProperty); }
        //    set { SetValue(CheckBoxImageWidthProperty, value); }
        //}

        //#endregion //CheckBoxImageWidth Property

        //#region CheckBoxImageHeight Property

        //public static readonly DependencyProperty CheckBoxImageHeightProperty = DependencyProperty.Register(
        //    "CheckBoxImageHeight",
        //    typeof(double),
        //    typeof(RrcCheckBox),
        //    new PropertyMetadata(20d));

        //public double CheckBoxImageHeight
        //{
        //    get { return (double)GetValue(CheckBoxImageHeightProperty); }
        //    set { SetValue(CheckBoxImageHeightProperty, value); }
        //}

        //#endregion //CheckBoxImageHeight Property

        #region CheckImageMargin Property

        public static readonly DependencyProperty CheckImageMarginProperty = DependencyProperty.Register(
            "CheckImageMargin",
            typeof(Thickness),
            typeof(RrcCheckBox),
            new PropertyMetadata(new Thickness(2)));

        public Thickness CheckImageMargin
        {
            get { return (Thickness)GetValue(CheckImageMarginProperty); }
            set { SetValue(CheckImageMarginProperty, value); }
        }

        #endregion //CheckImageMargin Property

        #region IsHorizontalReversed Property

        public static readonly DependencyProperty IsHorizontalReversedProperty = DependencyProperty.Register(
            "IsHorizontalReversed",
            typeof(bool),
            typeof(RrcCheckBox),
            new PropertyMetadata(default(bool)));

        public bool IsHorizontalReversed
        {
            get { return (bool)GetValue(IsHorizontalReversedProperty); }
            set { SetValue(IsHorizontalReversedProperty, value); }
        }

        #endregion //IsHorizontalReversed Property
    }
}
