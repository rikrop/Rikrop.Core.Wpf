using System.Windows;
using System.Windows.Controls;

namespace Rikrop.Core.Wpf.Controls
{
    public class RrcTreeView : TreeView
    {
        public static DependencyProperty SelectedItemTreeProperty = DependencyProperty.Register(
            "SelectedItemTree",
            typeof (object),
            typeof (RrcTreeView),
            new PropertyMetadata(default(object)));

        public object SelectedItemTree
        {
            get { return GetValue(SelectedItemTreeProperty); }
            set { SetValue(SelectedItemTreeProperty, value); }
        }

        static RrcTreeView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (RrcTreeView),
                                                     new FrameworkPropertyMetadata(typeof (RrcTreeView)));
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property == SelectedItemProperty)
            {
                SelectedItemTree = SelectedItem;
            }
        }
    }
}