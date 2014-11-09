using System;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Rikrop.Core.Wpf.Commands;

namespace Rikrop.Core.Wpf.Controls
{
    public class RrcMenuItem : MenuItem
    {
        public static readonly DependencyProperty CanOpenInNewWindowProperty =
            DependencyProperty.Register("CanOpenInNewWindow", typeof (bool), typeof (RrcMenuItem), new FrameworkPropertyMetadata(true));

        public static readonly DependencyProperty VisibilityOverrideProperty =
            DependencyProperty.Register("VisibilityOverride", typeof (Visibility?), typeof (RrcMenuItem),
                                        new FrameworkPropertyMetadata(null, (o, e) => ((RrcMenuItem) o).OnVisibilityOverrideChanged(e)));

        private bool _isSecurityEnabled = true;
        public event Action VisibilityChanged;

        public bool IsSecurityEnabled
        {
            get { return _isSecurityEnabled; }
            private set
            {
                if (_isSecurityEnabled == value)
                {
                    return;
                }

                _isSecurityEnabled = value;

                //if(IsSecurityEnabledChanged != null)
                //{
                //    IsSecurityEnabledChanged();
                //}
            }
        }

        public bool CanOpenInNewWindow
        {
            get { return (bool) GetValue(CanOpenInNewWindowProperty); }
            set { SetValue(CanOpenInNewWindowProperty, value); }
        }

        public Visibility? VisibilityOverride
        {
            get { return (Visibility?) GetValue(VisibilityOverrideProperty); }
            set { SetValue(VisibilityOverrideProperty, value); }
        }

        static RrcMenuItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (RrcMenuItem), new FrameworkPropertyMetadata(typeof (RrcMenuItem)));
        }

        public void SetVisibility(Visibility visibility)
        {
            if (!VisibilityOverride.HasValue)
            {
                Visibility = visibility;
            }
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);

            RrcMenuItem menuItem;

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems != null)
                    {
                        foreach (var newItem in e.NewItems)
                        {
                            menuItem = newItem as RrcMenuItem;
                            if (menuItem != null)
                            {
                                menuItem.VisibilityChanged += OnSubItemVisibilityChanged;
                            }
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Reset:
                    if (e.OldItems != null)
                    {
                        foreach (var newItem in e.OldItems)
                        {
                            menuItem = newItem as RrcMenuItem;
                            if (menuItem != null)
                            {
                                menuItem.VisibilityChanged += OnSubItemVisibilityChanged;
                            }
                        }
                    }

                    foreach (var item in Items)
                    {
                        menuItem = item as RrcMenuItem;
                        if (menuItem != null)
                        {
                            menuItem.VisibilityChanged += OnSubItemVisibilityChanged;
                        }
                    }
                    break;
            }

            RefreshVisibility();
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new RrcMenuItem();
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property == CommandProperty)
            {
                // Если привязанную команду требуется выполнять только при наличии прав и они отсутствуют,
                // скрываем кнопку
                var securityCommand = Command as ISecurityCommand;

                if (securityCommand != null && !securityCommand.EnoughRights)
                {
                    IsSecurityEnabled = false;
                    SetVisibility(Visibility.Collapsed);
                }
                //else
                //{
                //    IsSecurityEnabled = true;
                //    SetVisibility(Visibility.Visible);
                //}
            }
            else if (e.Property == IsEnabledProperty)
            {
                SetVisibility(IsEnabled
                                  ? Visibility.Visible
                                  : Visibility.Collapsed);
            }
            else if (e.Property == RoleProperty)
            {
            }
            else if (e.Property == VisibilityProperty)
            {
                if (VisibilityChanged != null)
                {
                    VisibilityChanged();
                }
            }
        }

        private void OnSubItemVisibilityChanged()
        {
            RefreshVisibility();
        }

        private void RefreshVisibility()
        {
            if (Items.OfType<object>().All(i => i is RrcMenuItem))
            {
                SetVisibility(Items.OfType<RrcMenuItem>().Any(o => o.Visibility == Visibility.Visible)
                                  ? Visibility.Visible
                                  : Visibility.Collapsed);
            }
            else if (Items.Count > 0)
            {
                SetVisibility(Visibility.Visible);
            }
        }

        private void OnVisibilityOverrideChanged(DependencyPropertyChangedEventArgs e)
        {
            Visibility = (Visibility) e.NewValue;
        }
    }
}