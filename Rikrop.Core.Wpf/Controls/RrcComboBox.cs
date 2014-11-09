using System;
using System.Windows;
using System.Windows.Controls;

namespace Rikrop.Core.Wpf.Controls
{
    public class RrcComboBox : ComboBox
    {
        static RrcComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RrcComboBox), new FrameworkPropertyMetadata(typeof(RrcComboBox)));
            VerticalAlignmentProperty.OverrideMetadata(typeof(RrcComboBox), new FrameworkPropertyMetadata(VerticalAlignment.Center));
        }

        #region EnumItemsSource Property

        public static readonly DependencyProperty EnumItemsSourceProperty = DependencyProperty.Register(
            "EnumItemsSource",
            typeof(Type),
            typeof(RrcComboBox),
            new PropertyMetadata(default(Type), EnumItemsSourceChangedCallback));

        public Type EnumItemsSource
        {
            get { return (Type)GetValue(EnumItemsSourceProperty); }
            set { SetValue(EnumItemsSourceProperty, value); }
        }

        public static void EnumItemsSourceChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (RrcComboBox)d;
            var enumType = (Type)e.NewValue;
            if (enumType != null)
            {
                ctrl.ItemsSource = Enum.GetValues(enumType);
            }
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if(e.Property == ItemsSourceProperty)
            {
                if (ItemsSourceChanged != null)
                    ItemsSourceChanged(this);
            }
        }

        public event Action<RrcComboBox> ItemsSourceChanged;

        #endregion //EnumItemsSource Property
    }
}
