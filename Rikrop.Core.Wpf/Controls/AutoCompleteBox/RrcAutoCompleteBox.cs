//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Collections.Specialized;
//using System.ComponentModel;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;
//using System.Timers;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Controls.Primitives;
//using System.Windows.Data;
//using System.Windows.Input;
//using System.Xaml;
//using Rikrop.Core.Framework.Extensions;
//using Rikrop.Core.Wpf.Controls.Helpers;

//namespace Rikrop.Core.Wpf.Controls.AutoCompleteBox
//{
//    [TemplatePart(Name = "PART_EditableTextBox", Type = typeof(TextBox))]
//    [TemplatePart(Name = "PART_ItemsPanel", Type = typeof(Panel))]
//    [TemplatePart(Name = "PART_ClearButton", Type = typeof(Button))]
//    public class RrcAutoCompleteBox : ComboBox, IWeakEventListener
//    {
//        #region Dependency Properties

//        #region UpdateDelay Property

//        public static readonly DependencyProperty UpdateDelayProperty = DependencyProperty.Register(
//            "UpdateDelay",
//            typeof(double),
//            typeof(RrcAutoCompleteBox),
//            new PropertyMetadata((double)500, UpdateDelayChangedCallback)
//            );

//        public double UpdateDelay
//        {
//            get { return (double)GetValue(UpdateDelayProperty); }
//            set { SetValue(UpdateDelayProperty, value); }
//        }
//        private static void UpdateDelayChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
//        {
//            var ctrl = (RrcAutoCompleteBox)d;
//            if (ctrl != null && ctrl.Timer != null)
//            {
//                ctrl.Timer.Interval = (double)e.NewValue;
//            }
//        }

//        #endregion

//        #region SearchMode Property

//        public static readonly DependencyProperty SearchModeProperty = DependencyProperty.Register(
//            "SearchMode",
//            typeof(SearchMode),
//            typeof(RrcAutoCompleteBox),
//            new FrameworkPropertyMetadata(default(SearchMode))
//            );

//        public SearchMode SearchMode
//        {
//            get { return (SearchMode)GetValue(SearchModeProperty); }
//            set { SetValue(SearchModeProperty, value); }
//        }

//        #endregion

//        #region SearchMember Property

//        public static readonly DependencyProperty SearchMemberPathProperty = DependencyProperty.Register(
//            "SearchMember",
//            typeof(SearchMember),
//            typeof(RrcAutoCompleteBox),
//            new PropertyMetadata(null, SearchMemberPathChangedCallback)
//            );

//        public SearchMember SearchMember
//        {
//            get { return (SearchMember)GetValue(SearchMemberPathProperty); }
//            set { SetValue(SearchMemberPathProperty, value); }
//        }

//        private static void SearchMemberPathChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
//        {
//            var ctrl = (RrcAutoCompleteBox)d;
//            ctrl.SearchMembers.Clear();
//            var newValue = (SearchMember)e.NewValue;
//            if (newValue != null)
//            {
//                ctrl.SearchMembers.Add(newValue);
//            }
//        }
//        #endregion

//        #region CreateMemberPath Property

//        public static readonly DependencyProperty CreateMemberPathProperty = DependencyProperty.Register(
//            "CreateMemberPath",
//            typeof(string),
//            typeof(RrcAutoCompleteBox),
//            new PropertyMetadata(string.Empty, CreateMemberPathChangedCallback)
//            );

//        public string CreateMemberPath
//        {
//            get { return (string)GetValue(CreateMemberPathProperty); }
//            set { SetValue(CreateMemberPathProperty, value); }
//        }

//        private static void CreateMemberPathChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
//        {
//            var ctrl = (RrcAutoCompleteBox)d;
//            ctrl.CreateMemberPaths.Clear();
//            var newValue = (string)e.NewValue;
//            if (!string.IsNullOrEmpty(newValue))
//            {
//                ctrl.CreateMemberPaths.Add(newValue);
//            }
//        }

//        #endregion

//        #region SelectedItem Property

//        public static new readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
//            "SelectedItem",
//            typeof(object),
//            typeof(RrcAutoCompleteBox),
//            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, SelectedItemChangedCallback, null, false, UpdateSourceTrigger.PropertyChanged)
//            );

//        public new object SelectedItem
//        {
//            get { return GetValue(SelectedItemProperty); }
//            set { SetValue(SelectedItemProperty, value); }
//        }

//        private static void SelectedItemChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
//        {
//            var ctrl = (RrcAutoCompleteBox)d;
//            ((ComboBox)ctrl).SelectedItem = e.NewValue;
//            ctrl.UpdateSelectedItemText(e.NewValue);
//        }

//        #endregion

//        #region PropertiesDivider Property

//        public static readonly DependencyProperty PropertiesDividerProperty = DependencyProperty.Register(
//            "PropertiesDivider",
//            typeof(string),
//            typeof(RrcAutoCompleteBox),
//            new PropertyMetadata(string.Empty, PropertiesDividerChangedCallback)
//            );

//        public string PropertiesDivider
//        {
//            get { return (string)GetValue(PropertiesDividerProperty); }
//            set { SetValue(PropertiesDividerProperty, value); }
//        }

//        private static void PropertiesDividerChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
//        {
//            var ctrl = (RrcAutoCompleteBox)d;
//            if (ctrl.PropertiesDividers.Count == 0)
//            {
//                ctrl.PropertiesDividers.Add((string)e.NewValue);
//            }
//            else
//            {
//                ctrl.PropertiesDividers[0] = (string)e.NewValue;
//            }
//        }

//        #endregion

//        #region AutoClearAfterSelect Property

//        public static readonly DependencyProperty AutoClearAfterSelectProperty = DependencyProperty.Register(
//            "AutoClearAfterSelect",
//            typeof(bool),
//            typeof(RrcAutoCompleteBox),
//            new PropertyMetadata(default(bool)));

//        public bool AutoClearAfterSelect
//        {
//            get { return (bool)GetValue(AutoClearAfterSelectProperty); }
//            set { SetValue(AutoClearAfterSelectProperty, value); }
//        }

//        #endregion //AutoClearAfterSelect Property

//        #region SearchMemberPathesCombineMode Property

//        public static readonly DependencyProperty SearchMemberPathesCombineModeProperty = DependencyProperty.Register(
//            "SearchMemberPathesCombineMode",
//            typeof(SearchMemberPathesCombineMode),
//            typeof(RrcAutoCompleteBox),
//            new PropertyMetadata(default(SearchMemberPathesCombineMode)));

//        public SearchMemberPathesCombineMode SearchMemberPathesCombineMode
//        {
//            get { return (SearchMemberPathesCombineMode)GetValue(SearchMemberPathesCombineModeProperty); }
//            set { SetValue(SearchMemberPathesCombineModeProperty, value); }
//        }

//        #endregion //SearchMemberPathesCombineMode Property

//        #region MessageText Property

//        public static readonly DependencyProperty MessageTextProperty = DependencyProperty.Register(
//            "MessageText",
//            typeof(string),
//            typeof(RrcAutoCompleteBox),
//            new PropertyMetadata(default(string))
//            {
//                DefaultValue = "найти..."
//            });

//        public string MessageText
//        {
//            get { return (string)GetValue(MessageTextProperty); }
//            set { SetValue(MessageTextProperty, value); }
//        }

//        #endregion //MessageText Property

//        public static readonly DependencyProperty MaxTextLengthProperty =
//            DependencyProperty.Register("MaxTextLength", typeof(int), typeof(RrcAutoCompleteBox), new PropertyMetadata(0)); // без ограничения

//        public int MaxTextLength
//        {
//            get { return (int)GetValue(MaxTextLengthProperty); }
//            set { SetValue(MaxTextLengthProperty, value); }
//        }

//        #endregion

//        #region Public Properties

//        public string CreateNewEntityText { get; set; }

//        public string EditEntityText { get; set; }

//        public string NothingIsFoundText
//        {
//            get { return "Ничего не найдено..."; }
//        }

//        public string PleaseWaitText
//        {
//            get { return "Пожалуйста, подождите..."; }
//        }

//        //public bool MoveFocusOnEnterKeyDown { get; set; }

//        public string ClearButtonToolTip { get; set; }

//        private Visibility _clearButtonVisibility;
//        public Visibility ClearButtonVisibility
//        {
//            get { return _clearButtonVisibility; }
//            set
//            {
//                if (_clearButtonVisibility != value)
//                {
//                    _clearButtonVisibility = value;
//                    if (ClearButton != null)
//                    {
//                        if (value != System.Windows.Visibility.Visible)
//                        {
//                            ClearButton.Visibility = value;
//                        }
//                        else
//                        {
//                            ClearButton.ClearValue(VisibilityProperty);
//                        }
//                    }
//                }
//            }
//        }

//        private BindingBase _displayMemberBinding;
//        public BindingBase DisplayMemberBinding
//        {
//            get { return _displayMemberBinding; }
//            set
//            {
//                if (_displayMemberBinding == value)
//                {
//                    return;
//                }
//                _displayMemberBinding = value;

//                BindingEvaluator = new BindingEvaluator<string>(value);
//                var textBlock = new FrameworkElementFactory(typeof(TextBlock));
//                textBlock.SetBinding(TextBlock.TextProperty, value);
//                ItemTemplate = new DataTemplate { VisualTree = textBlock };
//            }
//        }

//        private readonly List<string> _createMemberPathes = new List<string>();
//        public List<string> CreateMemberPaths
//        {
//            get { return _createMemberPathes; }
//        }

//        private readonly List<SearchMember> _searchMembers = new List<SearchMember>();
//        public List<SearchMember> SearchMembers
//        {
//            get { return _searchMembers; }
//        }

//        private readonly List<string> _propertiesDividers = new List<string>();
//        public List<string> PropertiesDividers
//        {
//            get { return _propertiesDividers; }
//        }

//        #endregion

//        #region Internal Properties

//        protected CollectionFilterBase CollectionFilter { get; private set; }
//        private TextBox EditableTextBox { get; set; }
//        private Panel ItemsHost { get; set; }
//        private Button ClearButton { get; set; }
//        private Timer Timer { get; set; }
//        protected bool IsFilterApplied { get; set; }
//        private bool IsKeyboardKeyPressed { get; set; }
//        private int SelectionStart { get; set; }
//        private int SelectionLength { get; set; }

//        private bool CanCommitSelection { get; set; }
//        private bool IsInternalUpdateSelectedItem { get; set; }

//        private BindingEvaluator<string> _bindingEvaluator;
//        private BindingEvaluator<string> BindingEvaluator
//        {
//            get { return _bindingEvaluator; }
//            set
//            {
//                if (_bindingEvaluator == value)
//                {
//                    return;
//                }

//                _bindingEvaluator = value;
//                if (SelectedItem != null)
//                {
//                    Text = _bindingEvaluator.GetDynamicValue(SelectedItem);
//                }
//            }
//        }


//        #endregion

//        #region Static Methods

//        static RrcAutoCompleteBox()
//        {
//            DefaultStyleKeyProperty.OverrideMetadata(typeof(RrcAutoCompleteBox), new FrameworkPropertyMetadata(typeof(RrcAutoCompleteBox)));

//            //ComboBox может очистить выбранное значение, если коллекции ItemsSource посылается сообщение Reset.
//            //Нам это мешает, по-этому при обновлении коллекции запрещаем ему менять выбранное значение.
//            Selector.SelectedItemProperty.OverrideMetadata(typeof(RrcAutoCompleteBox), new FrameworkPropertyMetadata(null, null, SelectedItemCoerceCallback));
//            TextProperty.OverrideMetadata(typeof(RrcAutoCompleteBox), new FrameworkPropertyMetadata(string.Empty, TextChangedCallback, TextCoerceCallback));

//            //Запрещаем менять свойства ComboBox'a на которых основана работа RrcAutoCompleteBox'a
//            IsEditableProperty.OverrideMetadata(typeof(RrcAutoCompleteBox), new FrameworkPropertyMetadata(true, null, IsEditableCoerceCallback));
//            IsTextSearchEnabledProperty.OverrideMetadata(typeof(RrcAutoCompleteBox), new FrameworkPropertyMetadata(false, null, IsTextSearchEnabledCoerceCallback));
//            StaysOpenOnEditProperty.OverrideMetadata(typeof(RrcAutoCompleteBox), new FrameworkPropertyMetadata(true, null, StaysOpenOnEditCoerceCallback));
//            ItemsSourceProperty.OverrideMetadata(typeof(RrcAutoCompleteBox), new FrameworkPropertyMetadata(null, ItemsSourcePropertyChanged, ComboBoxItemsSourceCoerceCallback));
//        }

//        private static void ItemsSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
//        {
//            var comboBox = (RrcAutoCompleteBox)d;
//            comboBox.OnItemsSourcePropertyChanged((IEnumerable)e.OldValue, (IEnumerable)e.NewValue);
//        }

//        private static object IsTextSearchEnabledCoerceCallback(DependencyObject d, object basevalue)
//        {
//            return false;
//        }

//        private static object IsEditableCoerceCallback(DependencyObject d, object basevalue)
//        {
//            return true;
//        }

//        private static object StaysOpenOnEditCoerceCallback(DependencyObject d, object basevalue)
//        {
//            return true;
//        }

//        private static object SelectedItemCoerceCallback(DependencyObject d, object newValue)
//        {
//            var ctrl = (RrcAutoCompleteBox)d;
//            if (ctrl.IsFilterApplied)
//            {
//                return ((ComboBox)ctrl).SelectedItem;
//            }
//            return newValue;
//        }

//        private static object TextCoerceCallback(DependencyObject d, object newValue)
//        {
//            var ctrl = (RrcAutoCompleteBox)d;
//            if (ctrl.IsFilterApplied)
//            {
//                return ctrl.Text;
//            }
//            return newValue;
//        }

//        private static void TextChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
//        {
//            var ctrl = (RrcAutoCompleteBox)d;
//            if (ctrl.TextChanged != null)
//                ctrl.TextChanged();
//        }

//        #endregion

//        #region Events

//        public event Action<object, ValidateNewItemEventArs> ValidateNewItem;
//        public event Action<object, CancelEventArgs> CanSelectItem;
//        public event Action TextChanged;

//        #endregion

//        public RrcAutoCompleteBox()
//        {
//            CreateNewEntityText = "Для создания, нажмите Enter или Tab";
//            EditEntityText = "Для редактирования, нажмите Ctrl + Enter";
//            ClearButtonToolTip = "Очистить";
//            Timer = new Timer(UpdateDelay);
//            Timer.Elapsed += TimerElapsed;

//            Loaded += OnLoaded;
//        }

//        private void OnLoaded(object sender, RoutedEventArgs e)
//        {
//            Loaded -= OnLoaded;
//            if (BindingEvaluator != null)
//            {
//                Text = BindingEvaluator.GetDynamicValue(SelectedItem);
//            }
//        }

//        #region Overrided Methods

//        public override void OnApplyTemplate()
//        {
//            base.OnApplyTemplate();

//            EditableTextBox = GetTemplateChild("PART_EditableTextBox") as TextBox;
//            if (EditableTextBox == null)
//            {
//                throw new XamlParseException("Error in RrcAutoCompleteBox template! \"PART_EditableTextBox\" element is not found!");
//            }

//            var b = new Binding(ExpressionHelper.GetName(o => MaxTextLength))
//            {
//                RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, GetType(), 1)
//            };
//            EditableTextBox.SetBinding(TextBox.MaxLengthProperty, b);

//            ItemsHost = GetTemplateChild("PART_ItemsPanel") as Panel;
//            if (ItemsHost == null)
//            {
//                throw new XamlParseException("Error in RrcAutoCompleteBox template! \"PART_ItemsPanel\" element is not found!");
//            }

//            ClearButton = GetTemplateChild("PART_ClearButton") as Button;
//            if (ClearButton == null)
//            {
//                throw new XamlParseException("Error in RrcAutoCompleteBox template! \"PART_ClearButton\" element is not found!");
//            }

//            EditableTextBox.TextChanged += OnTextChanged;
//            EditableTextBox.SelectionChanged += OnTextSelectionChanged;
//            ClearButton.Click += OnClearButtonClick;
//            ClearButton.ToolTip = ClearButtonToolTip;
//            if (ClearButtonVisibility != Visibility.Visible)
//            {
//                ClearButton.Visibility = ClearButtonVisibility;
//            }

//            RrcWatermarkBehavior.SetWatermark(this, new TextBlock
//            {
//                Text = MessageText,
//                Opacity = 0.5,
//                VerticalAlignment = VerticalAlignment.Center,
//                Margin = new Thickness(4, 0, 4, 0),
//                FontStyle = FontStyles.Italic,
//                FontWeight = FontWeights.Bold,
//            });

//            RrcWatermarkBehavior.SetSizeToContent(ItemsHost, SizeToContent.WidthAndHeight);
//        }

//        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
//        {
//            base.OnPropertyChanged(e);
//            if (e.Property == DisplayMemberPathProperty)
//            {
//                if (!string.IsNullOrEmpty(DisplayMemberPath))
//                {
//                    BindingEvaluator = new BindingEvaluator<string>(new Binding(DisplayMemberPath));
//                }
//            }
//        }

//        private void OnItemsSourcePropertyChanged(IEnumerable oldValue, IEnumerable newValue)
//        {
//            if (oldValue is INotifyCollectionChanged)
//            {
//                //((INotifyCollectionChanged)oldValue).CollectionChanged -= OnItemsSourceCollectionChanged;
//                CollectionChangedEventManager.RemoveListener((INotifyCollectionChanged)newValue, this);
//                if (oldValue is IRrcCollection)
//                {
//                    //((IRrcCollection)oldValue).PropertyChanged -= RrcCollection_PropertyChanged;
//                    IRrcCollectionWeakEventManager.RemoveListener((IRrcCollection)oldValue, this);
//                }
//            }

//            if (CollectionFilter != null)
//                CollectionFilter.ClearFilter();

//            if (newValue != null)
//            {
//                CollectionFilter = CollectionFilterBase.Create(newValue);

//                if (newValue is INotifyCollectionChanged)
//                {
//                    if (newValue is IRrcCollection)
//                    {
//                        //((IRrcCollection)newValue).PropertyChanged += RrcCollection_PropertyChanged;
//                        IRrcCollectionWeakEventManager.AddListener((IRrcCollection)newValue, this);
//                        ScrollViewer.SetIsDeferredScrollingEnabled(this, true);
//                    }

//                    //((INotifyCollectionChanged)newValue).CollectionChanged += OnItemsSourceCollectionChanged;
//                    CollectionChangedEventManager.AddListener((INotifyCollectionChanged)newValue, this);
//                }
//            }
//            else
//            {
//                CollectionFilter = null;
//            }
//        }

//        private void RrcCollection_PropertyChanged(object sender, PropertyChangedEventArgs e)
//        {
//            if (e.PropertyName == ExpressionHelper.GetName(x => ((IRrcCollection)ItemsSource).IsLoading))
//            {
//                UpdateUserHelpers();
//            }
//        }

//        private static object ComboBoxItemsSourceCoerceCallback(DependencyObject d, object basevalue)
//        {
//            //Увы, когда мы попадаем в OnItemsSourceChanged, ListCollectionView уже инициализирован
//            //и запросы будут выполнены.
//            //Единственный способ получить возможность проанализировать ItemsSource до его инициализации в WPF
//            //это во время Coerce, чем мы и пользуемся.
//            if (basevalue is IRrcCollection && !((ComboBox)d).IsDropDownOpen)
//            {
//                //((IRrcCollection)basevalue).DisableLoading = true;
//            }
//            return basevalue;
//        }

//        private void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
//        {
//            if (e.Action == NotifyCollectionChangedAction.Reset)
//            {
//                if (!Equals(SelectedItem, base.SelectedItem))
//                {
//                    base.SelectedItem = SelectedItem;
//                }
//            }
//        }

//        protected override void OnPreviewKeyDown(KeyEventArgs e)
//        {
//            IsKeyboardKeyPressed = true;
//            var isCtrlPressed = Keyboard.Modifiers.HasFlag(ModifierKeys.Control);
//            if (e.Key == Key.Down || e.Key == Key.Up)
//            {
//                if (!IsDropDownOpen)
//                {
//                    ApplyFilter();
//                }
//                else if (Items.Count > 0)
//                {
//                    //Это написано потому, что иногда при нажатии клавиш вверх или вниз ComboBox не переходит на элементы в списке
//                    //По-этому заставляем его это сделать кодом, чтобы пользователь мог клавишами выбрать нужный элемент
//                    if (base.SelectedItem == null)
//                    {
//                        base.SelectedItem = Items[0];
//                    }
//                    else
//                    {
//                        var item = ItemContainerGenerator.ContainerFromItem(base.SelectedItem) as ComboBoxItem;
//                        if (item != null && !item.IsHighlighted)
//                        {
//                            object tempItem = base.SelectedItem;
//                            base.SelectedItem = null;
//                            base.SelectedItem = tempItem;
//                        }
//                        else
//                        {
//                            base.OnPreviewKeyDown(e);
//                        }
//                    }
//                }
//                else
//                {
//                    base.OnPreviewKeyDown(e);
//                }
//            }
//            else if (e.Key == Key.Enter || e.Key == Key.Tab)
//            {
//                if (Timer.Enabled)
//                {
//                    ApplyFilter();
//                }
//                else if (IsDropDownOpen)
//                {
//                    CanCommitSelection = true;

//                    if (SearchMode != SearchMode.Search)
//                    {
//                        if (!string.IsNullOrEmpty(Text))
//                        {
//                            var si = CreateNewOrEditSelected(isCtrlPressed);
//                            OnItemCreatedOrEdited(si);
//                            base.SelectedItem = si;
//                            IsDropDownOpen = false;
//                        }
//                    }
//                    else
//                    {
//                        if (Items.Count == 1)
//                        {
//                            base.SelectedItem = Items[0];
//                            IsDropDownOpen = false;
//                        }
//                    }

//                    //Этот Handled очень важен, когда логический фокус стоит в другом месте, например на кнопке
//                    //Если убрать этот Handled, то кнопка будет активирована, хотя юзер совершает совсем другое действие
//                    e.Handled = true;

//                    //Т.к. e.Handled = true, базовый класс не получит этот евент.
//                    //Передаём его вручную, чтобы выбор элемента завершился.
//                    base.OnPreviewKeyDown(e);
//                }
//                else
//                {
//                    //if (MoveFocusOnEnterKeyDown)
//                    //{
//                    //    var elementWithFocus = Keyboard.FocusedElement as UIElement;
//                    //    if (elementWithFocus != null)
//                    //    {
//                    //        elementWithFocus.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
//                    //        e.Handled = true;
//                    //    }
//                    //}
//                }
//                IsKeyboardKeyPressed = false;
//            }
//            else
//            {
//                base.OnPreviewKeyDown(e);
//            }
//        }

//        protected virtual void OnItemCreatedOrEdited(object item) { }

//        private void OnTextChanged(object sender, TextChangedEventArgs e)
//        {
//            if (IsKeyboardKeyPressed)
//            {
//                Timer.Stop();
//                Timer.Start();
//            }
//        }

//        private void OnTextSelectionChanged(object sender, RoutedEventArgs e)
//        {
//            if (IsKeyboardKeyPressed)
//            {
//                IsKeyboardKeyPressed = false;
//                SelectionStart = EditableTextBox.SelectionStart;
//                SelectionLength = EditableTextBox.SelectionLength;
//            }
//        }

//        protected override void OnKeyUp(KeyEventArgs e)
//        {
//            IsKeyboardKeyPressed = false;
//        }

//        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
//        {
//            IsKeyboardKeyPressed = false;

//            base.OnSelectionChanged(e);
//            UpdateSelectedItemText(base.SelectedItem);
//            RestoreUserSelection();
//        }

//        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
//        {
//            if (IsDropDownOpen)
//            {
//                base.OnMouseWheel(e);
//            }
//            else
//            {
//                e.Handled = true;
//                var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
//                {
//                    RoutedEvent = MouseWheelEvent,
//                    Source = this
//                };
//                var parent = this.FindVisualParent<UIElement>();
//                if (parent != null)
//                {
//                    parent.RaiseEvent(eventArg);
//                }
//            }

//        }

//        protected override void OnDropDownOpened(EventArgs e)
//        {
//            //if (ItemsSource.IsVirtualizedCollection())
//            //{
//            //    ((IVirtualizingCollection)ItemsSource).DisableLoading = false;
//            //}

//            CanCommitSelection = false;
//            //Если у нас эти 2 свойства рассинхронизированы
//            //значит скорей всего стоит биндинг на SelectedItem, а не на 
//            //FindedItem и это первое открытие списка
//            if (!Equals(base.SelectedItem, SelectedItem))
//            {
//                SetSelectedItem(base.SelectedItem);
//            }

//            base.OnDropDownOpened(e);

//            RestoreUserSelection();

//            //Если список открыли по кнопке, то показываем полный список элементов
//            if (!IsFilterApplied && CollectionFilter != null)
//            {
//                CollectionFilter.ClearFilter();
//            }

//            UpdateUserHelpers();
//        }

//        protected override void OnDropDownClosed(EventArgs e)
//        {
//            //if (ItemsSource.IsVirtualizedCollection())
//            //{
//            //    ((IVirtualizingCollection)ItemsSource).DisableLoading = true;
//            //}

//            base.OnDropDownClosed(e);
//            if (CanCommitSelection)
//            {
//                CommitSelection();
//            }
//            else
//            {
//                RollbackSelection();
//            }
//        }

//        protected override DependencyObject GetContainerForItemOverride()
//        {
//            var item = new RrcAutoCompleteBoxItem();
//            item.PreviewMouseLeftButtonUp += OnComboBoxItemMouseLeftButtonUp;
//            return item;
//        }

//        private void OnComboBoxItemMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
//        {
//            if (IsDropDownOpen)
//            {
//                CanCommitSelection = true;
//            }
//        }

//        #endregion

//        #region Help Methods

//        private void UpdateSelectedItemText(object selectedItem)
//        {
//            Text = GetSelectedItemText(selectedItem);
//        }

//        protected virtual string GetSelectedItemText(object selectedItem)
//        {
//            if (selectedItem == null)
//            {
//                return string.Empty;
//            }

//            if (BindingEvaluator != null)
//            {
//                return BindingEvaluator.GetDynamicValue(selectedItem);
//            }

//            return selectedItem.ToString();
//        }

//        protected void RestoreUserSelection()
//        {
//            if (EditableTextBox != null && !string.IsNullOrEmpty(Text))
//            {
//                //if (ItemSelectionMode == ItemSelectionBehavior.SelectTextAfter)
//                //{
//                //    EditableTextBox.SelectAll();
//                //}
//                //else
//                {
//                    EditableTextBox.SelectionLength = SelectionLength;
//                    EditableTextBox.SelectionStart = SelectionStart;
//                }
//            }
//        }

//        private void TimerElapsed(object sender, ElapsedEventArgs e)
//        {
//            Dispatcher.Invoke(new Action(ApplyFilter));
//        }

//        protected void ApplyFilter()
//        {
//            Timer.Stop();
//            if (CollectionFilter != null)
//            {
//                IsFilterApplied = true;
//                IsDropDownOpen = true;

//                CollectionFilter.FilterApplied += OnFilterApplied;

//                CollectionFilter.ApplyFilter(GetFilterValues());

//                UpdateUserHelpers();
//            }
//        }

//        private string FilterText
//        {
//            get { return EditableTextBox == null ? null : EditableTextBox.Text; }
//        }

//        private void OnFilterApplied()
//        {
//            if (CollectionFilter != null)
//            {
//                CollectionFilter.FilterApplied -= OnFilterApplied;
//            }

//            IsDropDownOpen = true;
//            IsFilterApplied = false;
//            UpdateUserHelpers();
//        }

//        private void UpdateUserHelpers()
//        {
//            if (ItemsHost == null)
//                return;

//            var count = Items.Count;
//            if (count < 0)
//            {
//                MessageText = PleaseWaitText;
//                return;
//            }

//            if (ItemsSource is IRrcCollection && ((IRrcCollection)ItemsSource).IsLoading)
//            {
//                MessageText = PleaseWaitText;
//            }
//            else
//            {
//                if (count > 0)
//                {
//                    //if (SearchMode != SearchMode.Search)
//                    //{
//                    //    MessageText = CreateNewEntityText;
//                    //}
//                    //else
//                    //{
//                    //    MessageText = string.Empty;
//                    //}
//                    MessageText = string.Empty;

//                    ItemsHost.Height = double.NaN;
//                    ItemsHost.Width = double.NaN;
//                }
//                else
//                {
//                    MessageText = GetNothingIsFoundMessage();
//                }
//            }
//        }

//        protected virtual string GetNothingIsFoundMessage()
//        {
//            StringBuilder text = SearchMode == SearchMode.Create
//                                     ? new StringBuilder(CreateNewEntityText)
//                                     : new StringBuilder(NothingIsFoundText);
//            if (SearchMode != SearchMode.Search && SearchMode != SearchMode.Create)
//            {
//                text.Append(Environment.NewLine);
//                switch (SearchMode)
//                {
//                    case SearchMode.SearchOrCreate:
//                        text = text.Append(CreateNewEntityText);
//                        break;
//                    case SearchMode.SearchOrCreateOrModify:
//                        text = SelectedItem == null
//                                   ? text.Append(CreateNewEntityText)
//                                   : text.Append(EditEntityText);
//                        break;
//                    case SearchMode.SearchOrCreateSlashModify:
//                        if (SelectedItem != null)
//                        {
//                            text = text.Append(CreateNewEntityText);
//                            text = text.Append(Environment.NewLine);
//                            text = text.Append(EditEntityText);
//                        }
//                        else
//                        {
//                            text = text.Append(CreateNewEntityText);
//                        }
//                        break;
//                }
//            }

//            return text.ToString();
//        }

//        protected virtual IEnumerable<FilterBase> GetFilterValues()
//        {
//            var filters = new List<FilterBase>();

//            if (string.IsNullOrEmpty(FilterText) || SearchMembers.Count == 0)
//            {
//                return filters;
//            }

//            if (SearchMemberPathesCombineMode == SearchMemberPathesCombineMode.Or)
//            {
//                filters.Add(
//                    new OrFilter(
//                        SearchMembers.Select(o => new TextFilter(o.Path) { Text = FilterText, FilterMode = o.TextFilterMode }).
//                            ToList()));
//            }
//            else
//            {
//                string[] values = PropertiesDividers.Count == 0
//                                      ? new[] { FilterText.Trim() }
//                                      : FilterText.Split(PropertiesDividers.ToArray(), StringSplitOptions.RemoveEmptyEntries);

//                for (int i = 0; i < values.Length; i++)
//                {
//                    if (i >= SearchMembers.Count)
//                    {
//                        break;
//                    }
//                    string value = values[i];
//                    SearchMember searchMember = SearchMembers[i];
//                    filters.Add(new TextFilter(searchMember.Path) { Text = value, FilterMode = searchMember.TextFilterMode });
//                }
//            }

//            return filters;
//        }

//        private bool TryValidateNewCreated(object o, out object validatedObject)
//        {
//            if (ValidateNewItem != null)
//            {
//                var args = new ValidateNewItemEventArs();
//                ValidateNewItem(o, args);

//                validatedObject = args.ValidatedItem;
//                return !args.Cancel;
//            }
//            validatedObject = null;
//            return false;
//        }


//        private static Type GetElementType(Type initialtype)
//        {
//            if (initialtype == null)
//            {
//                return null;
//            }

//            Type type = null;
//            if (initialtype.HasElementType)
//            {
//                type = initialtype.GetElementType();
//            }
//            else if (initialtype.IsGenericType)
//            {
//                type = initialtype.GetGenericArguments()[0];
//            }

//            if (type != null)
//            {
//                return type;
//            }
//            return GetElementType(initialtype.BaseType);
//        }

//        protected virtual object CreateNewOrEditSelected(bool force)
//        {
//            Type type = null;
//            object obj = null;
//            if (base.SelectedItem != null)
//            {
//                type = base.SelectedItem.GetType();
//                if (SearchMode == SearchMode.SearchOrCreateOrModify ||
//                    (SearchMode == SearchMode.SearchOrCreateSlashModify && force))
//                {
//                    obj = base.SelectedItem;
//                }
//            }
//            else
//            {
//                var coltype = Items.SourceCollection.GetType();
//                type = GetElementType(coltype);
//            }
//            if (type == null)
//            {
//                return null;
//            }
//            if (obj == null)
//            {
//                obj = Activator.CreateInstance(type);
//            }

//            string[] values = PropertiesDividers.Count == 0
//                                  ? new[] { Text.Trim() }
//                                  : Text.Split(PropertiesDividers.ToArray(), StringSplitOptions.RemoveEmptyEntries);

//            int i = 0;
//            bool createpart = false;
//            foreach (var value in values)
//            {
//                string propertyName = null;
//                if (!createpart)
//                {
//                    if (i >= SearchMembers.Count)
//                    {
//                        i = 0;
//                        createpart = true;
//                    }
//                    else
//                        propertyName = SearchMembers[i].Path;
//                }
//                if (createpart)
//                {
//                    if (i >= CreateMemberPaths.Count)
//                        break;
//                    propertyName = CreateMemberPaths[i];
//                }

//                string stringval = String.IsNullOrEmpty(value) ? "%" : value.Trim();

//                PropertyInfo property = obj.GetType().GetProperty(propertyName);
//                if (property != null)
//                {
//                    object val = property.PropertyType != typeof(string)
//                                     ? TypeDescriptor.GetConverter(property.PropertyType).ConvertFromString(stringval)
//                                     : stringval;
//                    property.SetValue(obj, val, null);
//                }
//                i++;
//            }
//            object validatedObject;
//            if (TryValidateNewCreated(obj, out validatedObject))
//            {
//                return validatedObject;
//            }
//            return null;
//        }


//        protected virtual void CommitSelection()
//        {
//            if (string.IsNullOrEmpty(Text))
//            {
//                base.SelectedItem = null;
//            }
//            else
//            {
//                UpdateSelectedItemText(base.SelectedItem);
//                RestoreUserSelection();
//            }

//            //Это и только это место будет корректным для установки SelectedItem'a
//            //Т.е. когда юзер ЗАКОНЧИЛ выбор элемента. А это происходит, когда список закрылся
//            if (!Equals(SelectedItem, base.SelectedItem))
//            {
//                var args = new CancelEventArgs();
//                if (CanSelectItem != null)
//                {
//                    CanSelectItem(base.SelectedItem, args);
//                }
//                if (!args.Cancel)
//                {
//                    SetSelectedItem(base.SelectedItem);
//                    if (AutoClearAfterSelect)
//                    {
//                        SetSelectedItem(null);
//                        Text = string.Empty;
//                    }
//                }
//                else
//                {
//                    RollbackSelection();
//                }
//            }
//        }

//        protected virtual void RollbackSelection()
//        {
//            base.SelectedItem = SelectedItem;
//            UpdateSelectedItemText(base.SelectedItem);
//            RestoreUserSelection();
//        }

//        private void SetSelectedItem(object item)
//        {
//            try
//            {
//                IsInternalUpdateSelectedItem = true;

//                SelectedItem = item;

//                OnSelectedItemSetted(SelectedItem);
//            }
//            finally
//            {
//                IsInternalUpdateSelectedItem = false;
//            }
//        }

//        protected virtual void OnSelectedItemSetted(object item)
//        {

//        }

//        private void OnClearButtonClick(object sender, RoutedEventArgs e)
//        {
//            base.SelectedItem = null;
//            CommitSelection();
//        }

//        #endregion

//        public bool IsNeedEnterKey
//        {
//            get
//            {
//                return IsDropDownOpen || Timer.Enabled;

//                //Если что-то сломается, дать мне знать. Будем разбираться. (Settler)
//                //В данном виде эта проверка запрещает применение Enter'a, если юзер в SSC
//                //ни разу не выбирал елемент (к примеру выбранный элемент забинжен на свойство)
//                //var ret = IsDropDownOpen || lastText != Text;

//                //return ret;
//            }
//        }

//        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
//        {
//            if (managerType == typeof(IRrcCollectionWeakEventManager))
//                RrcCollection_PropertyChanged(sender, (PropertyChangedEventArgs)e);
//            else if (managerType == typeof(CollectionChangedEventManager))
//                OnItemsSourceCollectionChanged(sender, (NotifyCollectionChangedEventArgs)e);

//            return true;
//        }
//    }
//}
