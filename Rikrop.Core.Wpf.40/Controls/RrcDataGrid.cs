using Rikrop.Core.Wpf.Controls.Watermark;

namespace Rikrop.Core.Wpf.Controls
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Threading;
    using Collections;
    using DataGrid;
    using Helpers;

    public class RrcDataGrid : System.Windows.Controls.DataGrid
    {
        static RrcDataGrid()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RrcDataGrid), new FrameworkPropertyMetadata(typeof(RrcDataGrid)));
            /*DataContextProperty.AddOwner(typeof(DataGridColumn));*/
            ItemsSourceProperty.OverrideMetadata(typeof(RrcDataGrid), new FrameworkPropertyMetadata(null));
            ColumnWidthProperty.OverrideMetadata(typeof(RrcDataGrid), new FrameworkPropertyMetadata(new DataGridLength(1, DataGridLengthUnitType.Star)));
        }

        #region Свойства зависимостей

        #region FocusCellContentOnEdit Property

        public static readonly DependencyProperty FocusCellContentOnEditProperty = DependencyProperty.Register(
            "FocusCellContentOnEdit",
            typeof(bool),
            typeof(RrcDataGrid),
            new PropertyMetadata(true));

        public bool FocusCellContentOnEdit
        {
            get { return (bool)GetValue(FocusCellContentOnEditProperty); }
            set { SetValue(FocusCellContentOnEditProperty, value); }
        }

        #endregion //FocusCellContentOnEdit Property

        #region DisplayAfter Property

        public static readonly DependencyProperty DisplayAfterProperty = DependencyProperty.Register(
            "DisplayAfter",
            typeof(TimeSpan),
            typeof(RrcDataGrid),
            new PropertyMetadata(TimeSpan.FromSeconds(0.1)));

        public TimeSpan DisplayAfter
        {
            get { return (TimeSpan)GetValue(DisplayAfterProperty); }
            set { SetValue(DisplayAfterProperty, value); }
        }

        #endregion //DisplayAfter Property

        #region IsEditing Property

        private static readonly DependencyPropertyKey IsEditingPropertyKey = DependencyProperty.RegisterReadOnly(
            "IsEditing", typeof(bool), typeof(RrcDataGrid), new PropertyMetadata(false));

        public static readonly DependencyProperty IsEditingProperty = IsEditingPropertyKey.DependencyProperty;

        public bool IsEditing
        {
            get { return (bool)GetValue(IsEditingProperty); }
            private set { SetValue(IsEditingPropertyKey, value); }
        }

        #endregion

        #region IsLoading Property

        public static readonly DependencyProperty IsLoadingProperty = DependencyProperty.Register(
            "IsLoading",
            typeof(bool),
            typeof(RrcDataGrid),
            new PropertyMetadata(default(bool), (o, args) => { }, CoerceIsLoading));

        private static object CoerceIsLoading(DependencyObject d, object basevalue)
        {
            var dataGrid = d as RrcDataGrid;
            return (bool)basevalue && dataGrid.ShowBusyIndicator;
        }

        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        #endregion //IsLoading Property

        public bool ShowBusyIndicator
        {
            get { return (bool)GetValue(ShowBusyIndicatorProperty); }
            set { SetValue(ShowBusyIndicatorProperty, value); }
        }
        public static readonly DependencyProperty ShowBusyIndicatorProperty =
            DependencyProperty.Register("ShowBusyIndicator", typeof(bool), typeof(RrcDataGrid), new PropertyMetadata(true));

        #endregion

        #region Events

        public event EventHandler<DataGridRowEditEndedEventArgs> RowEditEnded;
        public event Action<DataGridCell> CellMouseDoubleClick;
        #endregion

        public RrcDataGrid()
        {
            //Это необходимо потому, что в Child окнах без этого значение не присваивается
            ColumnWidth = new DataGridLength(1, DataGridLengthUnitType.Star);
            
            LoadingRow += dataGrid_LoadingRow;
        }

/*        private void OnColumnsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (DataGridColumn column in e.NewItems)
                {
                    BindingOperations.SetBinding(column, DataContextProperty,
                                                 new Binding("DataContext") { Source = this, Mode = BindingMode.OneWay });
                }
            }#1#
        }*/

        protected override void OnRowEditEnding(DataGridRowEditEndingEventArgs e)
        {
            base.OnRowEditEnding(e);
            if (!e.Cancel)
            {
                Dispatcher.BeginInvoke(new DispatcherOperationCallback((param) =>
                {
                    OnRowEditEnded(new DataGridRowEditEndedEventArgs(e.Row));
                    return null;
                }), DispatcherPriority.Background, new object[] { null });
                //Dispatcher.BeginInvoke(new DispatcherOperationCallback((param) =>
                //{
                //    (DataContext as IGridListDictionary).Validate();
                //    return null;
                //}), DispatcherPriority.Background, new object[] { null });
            }
            IsEditing = false;
        }

        protected override void OnPreparingCellForEdit(DataGridPreparingCellForEditEventArgs e)
        {
            base.OnPreparingCellForEdit(e);
            if (FocusCellContentOnEdit &&
                e.Column is DataGridTemplateColumn &&
                e.EditingElement != null)
            {
                e.EditingElement.Loaded += OnEditingElementLoaded;
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            bool isValueKey = true;
            switch (e.Key)
            {
                case Key.Return:
                case Key.Tab:
                case Key.Back:
                case Key.Down:
                case Key.Up:
                case Key.Left:
                case Key.Right:
                case Key.Escape:
                case Key.LWin:
                case Key.RWin:
                case Key.Home:
                case Key.End:
                case Key.PageUp:
                case Key.PageDown:
                case Key.F1:
                case Key.F2:
                case Key.F3:
                case Key.F4:
                case Key.F5:
                case Key.F6:
                case Key.F7:
                case Key.F8:
                case Key.F9:
                case Key.F10:
                case Key.F11:
                case Key.F12:
                case Key.PrintScreen:
                case Key.Scroll:
                case Key.Pause:
                case Key.Insert:
                case Key.Delete:
                case Key.NumLock:
                case Key.CapsLock:
                case Key.LeftShift:
                case Key.LeftCtrl:
                case Key.LeftAlt:
                case Key.RightAlt:
                case Key.RightCtrl:
                case Key.RightShift:
                case Key.System:
                case Key.Apps:
                    isValueKey = false;
                    break;
            }

            if (isValueKey)
            {
                var cell = e.OriginalSource as DataGridCell;
                if (cell != null)
                {
                    //cell.Focus();
                    this.BeginEdit();

                    var textBox = RrcVisualTreeHelper.FindVisualChild<TextBox>(cell);
                    if (textBox != null)
                    {
                        textBox.Focus();
                    }
                }
            }

            base.OnPreviewKeyDown(e);
        }

        protected override void OnBeginningEdit(DataGridBeginningEditEventArgs e)
        {
            IsEditing = true;
            base.OnBeginningEdit(e);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            var cell = ((FrameworkElement)e.OriginalSource).FindVisualParent<DataGridCell>();
            if (cell == null)
            {
                if (SelectionMode == DataGridSelectionMode.Extended)
                {
                    SelectedItems.Clear();
                }
                else
                {
                    this.SelectedItem = null;
                }

                if (IsEditing)
                {
                    CommitEdit();
                    CommitEdit();
                }
            }
        }

        private static void OnEditingElementLoaded(object sender, RoutedEventArgs e)
        {
            ((FrameworkElement)sender).Loaded -= OnEditingElementLoaded;
            ((FrameworkElement)sender).MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
        }

        private void OnRowEditEnded(DataGridRowEditEndedEventArgs e)
        {
            var handler = RowEditEnded;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void OnCellMouseDoubleClick(DataGridCell cell)
        {
            var handler = CellMouseDoubleClick;
            if (handler != null)
            {
                handler(cell);
            }
        }

        protected override void OnSorting(DataGridSortingEventArgs eventArgs)
        {
            eventArgs.Handled = true;
            bool clearExistingSortDescriptions = !Keyboard.Modifiers.HasFlag(ModifierKeys.Shift);

            if (string.IsNullOrEmpty(eventArgs.Column.SortMemberPath))
            {
                return;
            }

            /*if (ItemsSource is IRrcCollection)
            {
                var collection = (IRrcCollection)ItemsSource;
                FillSortDescriptions(eventArgs.Column, collection.SortDescriptions, clearExistingSortDescriptions);
                collection.Refresh();
            }*/
            //else
            //{
                FillSortDescriptions(eventArgs.Column, Items.SortDescriptions, clearExistingSortDescriptions);
                Items.Refresh();
            //}
        }

        private static void FillSortDescriptions(DataGridColumn column, SortDescriptionCollection descriptions, bool clearExistingSortDescriptions)
        {
            column.SortDirection = column.SortDirection == null
                                       ? ListSortDirection.Ascending
                                       : column.SortDirection == ListSortDirection.Ascending
                                             ? ListSortDirection.Descending
                                             : (ListSortDirection?)null;

            int descriptionIndex = -1;
            if (clearExistingSortDescriptions)
            {
                descriptions.Clear();
            }
            else
            {
                for (int i = 0; i < descriptions.Count; i++)
                {
                    SortDescription description = descriptions[i];
                    if (string.Equals(description.PropertyName, column.SortMemberPath, StringComparison.Ordinal))
                    {
                        descriptionIndex = i;
                        break;
                    }
                }
            }
            if (column.SortDirection == null)
            {
                if (descriptionIndex > 0)
                {
                    descriptions.RemoveAt(descriptionIndex);
                }
            }
            else
            {
                var item = new SortDescription(column.SortMemberPath, column.SortDirection.Value);
                if (descriptionIndex >= 0)
                {
                    descriptions[descriptionIndex] = item;
                }
                else
                {
                    descriptions.Add(item);
                }
            }
        }

        private void dataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            if (e.Row.Item == CollectionView.NewItemPlaceholder)
            {
                RrcWatermarkBehavior.SetWatermark(e.Row, new RrcTextBlock()
                {
                    Text = "Для добавления, нажмите сюда...",
                    Margin = new Thickness(50, 1, 1, 1),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Style = FindResource("WatermarkTextBlockStyle") as Style
                });
            }
            else
            {
                RrcWatermarkBehavior.SetWatermark(e.Row, null);
            }
        }
    }

    public class DataGridRowEditEndedEventArgs : EventArgs
    {
        public DataGridRowEditEndedEventArgs(DataGridRow row)
        {
            _row = row;
        }

        public DataGridRow Row
        {
            get { return _row; }
        }

        private readonly DataGridRow _row;
    }
}
