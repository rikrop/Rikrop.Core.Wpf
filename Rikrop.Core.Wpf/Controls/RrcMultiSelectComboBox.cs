using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace Rikrop.Core.Wpf.Controls
{
    [TemplatePart(Name = "PART_Popup", Type = typeof (Popup))]
    public class RrcMultiSelectComboBox : ListBox
    {
        public static readonly DependencyProperty MaxDropDownHeightProperty =
            ComboBox.MaxDropDownHeightProperty.AddOwner(typeof (RrcMultiSelectComboBox));

        public static readonly DependencyProperty IsDropDownOpenProperty =
            DependencyProperty.Register("IsDropDownOpen", typeof (bool), typeof (RrcMultiSelectComboBox));

        public static readonly DependencyProperty SelectedItemTemplateProperty =
            DependencyProperty.Register("SelectedItemTemplate", typeof (DataTemplate), typeof (RrcMultiSelectComboBox), new PropertyMetadata(default(DataTemplate)));

        public static readonly DependencyProperty SelectedItemsItemsControlStyleProperty =
            DependencyProperty.Register("SelectedItemsItemsControlStyle", typeof (Style), typeof (RrcMultiSelectComboBox), new PropertyMetadata(default(Style)));

        public static readonly DependencyProperty CanUncheckLastItemProperty =
            DependencyProperty.Register("CanUncheckLastItem", typeof (bool), typeof (RrcMultiSelectComboBox), new PropertyMetadata(true));

        public new static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register("SelectedItems", typeof (IList), typeof (RrcMultiSelectComboBox),
                                        new FrameworkPropertyMetadata((d, e) =>
                                                                          {
                                                                              var mcb = d as RrcMultiSelectComboBox;
                                                                              if (mcb == null)
                                                                              {
                                                                                  return;
                                                                              }
                                                                              mcb.SetNewSelectedItems(e.NewValue as IList, e.OldValue as IList);
                                                                          }));

        public static readonly DependencyProperty PopupClosedCommandProperty =
            DependencyProperty.Register("PopupClosedCommand", typeof (ICommand), typeof (RrcMultiSelectComboBox), new PropertyMetadata(default(ICommand)));

        public static readonly DependencyProperty PopupFooterTemplateProperty =
            DependencyProperty.Register("PopupFooterTemplate", typeof (DataTemplate), typeof (RrcMultiSelectComboBox), new PropertyMetadata(default(DataTemplate)));

        public static readonly DependencyProperty SelectionItemsFollowerTemplateProperty =
            DependencyProperty.Register("SelectionItemsFollowerTemplate", typeof (DataTemplate), typeof (RrcMultiSelectComboBox), new PropertyMetadata(default(DataTemplate)));

        public static readonly DependencyProperty PopupHeaderTemplateProperty =
            DependencyProperty.Register("PopupHeaderTemplate", typeof (DataTemplate), typeof (RrcMultiSelectComboBox), new PropertyMetadata(default(DataTemplate)));

        private Popup _popup;

        public DataTemplate PopupHeaderTemplate
        {
            get { return (DataTemplate) GetValue(PopupHeaderTemplateProperty); }
            set { SetValue(PopupHeaderTemplateProperty, value); }
        }

        public DataTemplate SelectionItemsFollowerTemplate
        {
            get { return (DataTemplate) GetValue(SelectionItemsFollowerTemplateProperty); }
            set { SetValue(SelectionItemsFollowerTemplateProperty, value); }
        }

        public DataTemplate PopupFooterTemplate
        {
            get { return (DataTemplate) GetValue(PopupFooterTemplateProperty); }
            set { SetValue(PopupFooterTemplateProperty, value); }
        }

        public ICommand PopupClosedCommand
        {
            get { return (ICommand) GetValue(PopupClosedCommandProperty); }
            set { SetValue(PopupClosedCommandProperty, value); }
        }

        public double MaxDropDownHeight
        {
            get { return (double) GetValue(MaxDropDownHeightProperty); }
            set { SetValue(MaxDropDownHeightProperty, value); }
        }

        public bool IsDropDownOpen
        {
            get { return (bool) GetValue(IsDropDownOpenProperty); }
            set { SetValue(IsDropDownOpenProperty, value); }
        }

        public DataTemplate SelectedItemTemplate
        {
            get { return (DataTemplate) GetValue(SelectedItemTemplateProperty); }
            set { SetValue(SelectedItemTemplateProperty, value); }
        }

        public Style SelectedItemsItemsControlStyle
        {
            get { return (Style) GetValue(SelectedItemsItemsControlStyleProperty); }
            set { SetValue(SelectedItemsItemsControlStyleProperty, value); }
        }

        public bool CanUncheckLastItem
        {
            get { return (bool) GetValue(CanUncheckLastItemProperty); }
            set { SetValue(CanUncheckLastItemProperty, value); }
        }

        public new IList SelectedItems
        {
            get { return GetValue(SelectedItemsProperty) as IList; }
            set { SetValue(SelectedItemsProperty, value); }
        }

        static RrcMultiSelectComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (RrcMultiSelectComboBox), new FrameworkPropertyMetadata(typeof (RrcMultiSelectComboBox)));
        }

        public RrcMultiSelectComboBox()
        {
            SelectionChanged += OnSelectionChanged;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _popup = (Popup) Template.FindName("PART_Popup", this);
            if (_popup == null)
            {
                throw new Exception("Can not find part PART_Popup");
            }
            _popup.Closed += OnPopupClosed;
        }

        protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            if (SelectionMode == SelectionMode.Single && IsDropDownOpen)
            {
                foreach (var item in Items)
                {
                    var listBoxItem = (ListBoxItem) ItemContainerGenerator.ContainerFromItem(item);
                    if (listBoxItem != null && listBoxItem.IsMouseOver)
                    {
                        SelectedItem = item;
                        IsDropDownOpen = false;
                        break;
                    }
                }
            }
        }

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            if (!IsDropDownOpen)
            {
                return;
            }
            e.Handled = true;

            if (SelectionMode != SelectionMode.Single)
            {
                var items = Items
                    .OfType<Object>()
                    .Select(item => (ListBoxItem) ItemContainerGenerator.ContainerFromItem(item))
                    .Where(item => item != null)
                    .ToList();

                var checkedItems = items.Count(item => item.IsSelected);

                if (checkedItems == 1)
                {
                    var item = items.First(i => i.IsSelected);
                    if (item.IsMouseOver)
                    {
                        if (CanUncheckLastItem && item.IsSelected)
                        {
                            item.IsSelected = false;
                        }
                        else
                        {
                            item.IsSelected = true;
                        }
                        return;
                    }
                }

                foreach (var item in items.Where(item => item.IsMouseOver))
                {
                    item.IsSelected = !item.IsSelected;
                    return;
                }


                if (_popup.Child.IsMouseOver)
                {
                    e.Handled = false;
                }
            }
        }

        private void OnPopupClosed(object sender, EventArgs eventArgs)
        {
            if (PopupClosedCommand != null && PopupClosedCommand.CanExecute(null))
            {
                PopupClosedCommand.Execute(null);
            }
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedItems == null)
            {
                return;
            }

            try
            {
                RemoveSelectedItemsChangedHandler(SelectedItems);

                if (e.AddedItems != null)
                {
                    foreach (var o in from object o in e.AddedItems where !SelectedItems.Contains(o) select o)
                    {
                        SelectedItems.Add(o);
                    }
                }

                if (e.RemovedItems != null)
                {
                    foreach (var o in e.RemovedItems)
                    {
                        SelectedItems.Remove(o);
                    }
                }
            }
            finally
            {
                AddSelectedItemsChangedHandler(SelectedItems);
            }
        }

        private void SetNewSelectedItems(IList newSelectedItems, IList oldSelectedItems)
        {
            RemoveSelectedItemsChangedHandler(oldSelectedItems);
            SetInnerSelectedItems(newSelectedItems);
            AddSelectedItemsChangedHandler(newSelectedItems);
        }

        private void SelectedItemsChangedHandler(object sender, NotifyCollectionChangedEventArgs args)
        {
            SetInnerSelectedItems(SelectedItems);
        }

        private void AddSelectedItemsChangedHandler(IList collection)
        {
            if (collection == null)
            {
                return;
            }
            var cvs = CollectionViewSource.GetDefaultView(collection);
            CollectionChangedEventManager.AddHandler(cvs, SelectedItemsChangedHandler);
        }

        private void RemoveSelectedItemsChangedHandler(IList collection)
        {
            if (collection == null)
            {
                return;
            }
            var cvs = CollectionViewSource.GetDefaultView(collection);
            CollectionChangedEventManager.RemoveHandler(cvs, SelectedItemsChangedHandler);
        }

        private void SetInnerSelectedItems(IList collection)
        {
            SelectionChanged -= OnSelectionChanged;
            SetSelectedItems(collection);
            SelectionChanged += OnSelectionChanged;
        }
    }
}