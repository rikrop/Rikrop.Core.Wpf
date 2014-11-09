using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using Rikrop.Core.Framework;
using Rikrop.Core.Wpf.Controls.Helpers;

namespace Rikrop.Core.Wpf.Controls
{
    [TemplatePart(Name = "PART_EditArea", Type = typeof (TextBox))]
    public class RrcEditableTextBlock : ContentControl
    {
        public static readonly DependencyProperty TextBlockStyleProperty =
            DependencyProperty.Register("TextBlockStyle", typeof (Style), typeof (RrcEditableTextBlock),
                                        new PropertyMetadata(default(Style)));

        public static readonly DependencyProperty TextBoxStyleProperty =
            DependencyProperty.Register("TextBoxStyle", typeof (Style), typeof (RrcEditableTextBlock),
                                        new PropertyMetadata(default(Style)));

        public static readonly DependencyProperty CompleteTextEditCommandProperty =
            DependencyProperty.Register("CompleteTextEditCommand", typeof (ICommand),
                                        typeof (RrcEditableTextBlock),
                                        new PropertyMetadata(null));

        public static readonly DependencyProperty IsOwnerProperty =
            DependencyProperty.RegisterAttached("IsOwner", typeof (bool), typeof (RrcEditableTextBlock), new PropertyMetadata(false, IsOwnerPropertyChangedCallback));

        private static void IsOwnerPropertyChangedCallback(DependencyObject dobj, DependencyPropertyChangedEventArgs dargs)
        {
            var o = dobj as UIElement;
            if (o == null)
            {
                return;
            }
            if (GetIsOwner(o))
            {
                o.PreviewKeyDown += NewsbreakTreeItemOnPreviewKeyDown;
                o.PreviewMouseDown += EditableTextBlockOwnerOnPreviewMouseDown;
            }
            else
            {
                o.PreviewKeyDown -= NewsbreakTreeItemOnPreviewKeyDown;
                o.PreviewMouseDown -= EditableTextBlockOwnerOnPreviewMouseDown;
            }
        }

        public static bool IsStartEditKey(Key key)
        {
            //letters
            if (key >= Key.A && key <= Key.Z)
            {
                return true;
            }

            //numbers from keypad
            if (key >= Key.NumPad0 && key <= Key.NumPad9)
            {
                return true;
            }

            if (key >= Key.D0 && key <= Key.D9)
            {
                return true;
            }

            if (key == Key.Space || key == Key.Tab || key == Key.Left || key == Key.Right)
            {
                return true;
            }

            return false;
        }

        public static void NewsbreakTreeItemOnPreviewKeyDown(object sender, KeyEventArgs args)
        {
            if (!IsStartEditKey(args.Key))
            {
                return;
            }

            var tviSender = args.OriginalSource as DependencyObject;
            if (tviSender == null)
            {
                return;
            }

            var textBlock = tviSender.FindVisualChild<RrcEditableTextBlock>();
            if (textBlock != null)
            {
                if (textBlock.IsInEditMode)
                {
                    return;
                }
                textBlock.EnterEditMode();
            }
        }

        public static void EditableTextBlockOwnerOnPreviewMouseDown(object sender, MouseButtonEventArgs args)
        {
            if (args.ClickCount < 2)
            {
                return;
            }

            var tviSender = sender as DependencyObject;
            if (tviSender == null)
            {
                return;
            }

            var textBlock = tviSender.FindVisualChild<RrcEditableTextBlock>();
            if (textBlock != null)
            {
                textBlock.EnterEditMode();
                args.Handled = true;
            }
        }

        public static void SetIsOwner(UIElement element, bool value)
        {
            element.SetValue(IsOwnerProperty, value);
        }

        public static bool GetIsOwner(UIElement element)
        {
            return (bool) element.GetValue(IsOwnerProperty);
        }

        public static readonly DependencyProperty CancelTextEditCommandProperty = DependencyProperty.Register(
            "CancelTextEditCommand",
            typeof (ICommand),
            typeof (RrcEditableTextBlock),
            new PropertyMetadata(default(ICommand)));

        public static readonly DependencyProperty BeginTextEditCommandProperty = DependencyProperty.Register(
            "BeginTextEditCommand",
            typeof (ICommand),
            typeof (RrcEditableTextBlock),
            new PropertyMetadata(null));

        public static readonly DependencyProperty SelectAllOnBeginEditProperty = DependencyProperty.Register(
            "SelectAllOnBeginEdit",
            typeof (bool),
            typeof (RrcEditableTextBlock),
            new PropertyMetadata(false));

        public static readonly DependencyProperty CommitOnLostFocusProperty =
            DependencyProperty.Register("CommitOnLostFocus", typeof (bool), typeof (RrcEditableTextBlock),
                                        new PropertyMetadata(true));

        public static DependencyProperty IsInEditModeProperty =
            DependencyProperty.Register("IsInEditMode", typeof (Boolean), typeof (RrcEditableTextBlock),
                                        new PropertyMetadata(false, IsInEditModePropertyChangedCallback));

        private TextBox _textBox;
        private string _oldText;

        private TextBlock _textBlock;
        private bool _isCommitChanged;
        public event Action<FrameworkElement> TextEdited;

        public bool CommitOnLostFocus
        {
            get { return (bool) GetValue(CommitOnLostFocusProperty); }
            set { SetValue(CommitOnLostFocusProperty, value); }
        }

        public Style TextBlockStyle
        {
            get { return (Style) GetValue(TextBlockStyleProperty); }
            set { SetValue(TextBlockStyleProperty, value); }
        }

        public Style TextBoxStyle
        {
            get { return (Style) GetValue(TextBoxStyleProperty); }
            set { SetValue(TextBoxStyleProperty, value); }
        }

        public bool IsInEditMode
        {
            get { return (bool) GetValue(IsInEditModeProperty); }
            set { SetValue(IsInEditModeProperty, value); }
        }

        public ICommand CompleteTextEditCommand
        {
            get { return (ICommand) GetValue(CompleteTextEditCommandProperty); }
            set { SetValue(CompleteTextEditCommandProperty, value); }
        }

        public ICommand CancelTextEditCommand
        {
            get { return (ICommand) GetValue(CancelTextEditCommandProperty); }
            set { SetValue(CancelTextEditCommandProperty, value); }
        }

        public ICommand BeginTextEditCommand
        {
            get { return (ICommand) GetValue(BeginTextEditCommandProperty); }
            set { SetValue(BeginTextEditCommandProperty, value); }
        }

        public bool SelectAllOnBeginEdit
        {
            get { return (bool) GetValue(SelectAllOnBeginEditProperty); }
            set { SetValue(SelectAllOnBeginEditProperty, value); }
        }

        static RrcEditableTextBlock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (RrcEditableTextBlock),
                                                     new FrameworkPropertyMetadata(typeof (RrcEditableTextBlock)));
        }

        private static void IsInEditModePropertyChangedCallback(DependencyObject dobj,
                                                                DependencyPropertyChangedEventArgs args)
        {
            var d = dobj as RrcEditableTextBlock;
            if (d != null)
            {
                d.UpdateOnIsEditModeChanged();
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _textBox = (TextBox) Template.FindName("PART_EditArea", this);
            if (_textBox == null)
            {
                throw new XamlParseException("PART_EditArea");
            }
            _textBox.PreviewLostKeyboardFocus += TextBoxLostFocus;

            var btbx = new Binding(ExpressionHelper.GetName<RrcEditableTextBlock>(o => o.TextBoxStyle))
                {
                    RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, GetType(), 1)
                };
            _textBox.SetBinding(StyleProperty, btbx);

            _textBlock = (TextBlock) Template.FindName("PART_TextBlock", this);
            if (_textBlock == null)
            {
                throw new XamlParseException("PART_TextBlock");
            }

            var btbl = new Binding(ExpressionHelper.GetName<RrcEditableTextBlock>(o => o.TextBlockStyle))
                {
                    RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, GetType(), 1)
                };
            _textBlock.SetBinding(StyleProperty, btbl);

            if (IsInEditMode)
            {
                EnterEditModeInternal();
            }
            else
            {
                SetVisual();                
            }
        }

        public void EnterEditMode()
        {
            if (IsInEditMode || !IsEnabled)
            {
                return;
            }

            IsInEditMode = true;
            EnterEditModeInternal();
        }

        public void CancelEditMode()
        {
            if (!IsInEditMode)
            {
                return;
            }
            IsInEditMode = false;
            CancelEditModeInternal();
        }

        public void CommitEditMode()
        {
            if (!IsInEditMode)
            {
                return;
            }
            _isCommitChanged = true;
            try
            {
                IsInEditMode = false;
            }
            finally
            {
                _isCommitChanged = false;
            }

            CommitEditModeInternal();
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            if (IsInEditMode)
            {
                if (e.Key == Key.Enter || e.Key == Key.Tab)
                {
                    CommitEditMode();
                    e.Handled = true;
                }
                else if (e.Key == Key.Escape)
                {
                    CancelEditMode();
                    e.Handled = true;
                }
            }
        }

        protected override void OnPreviewMouseDoubleClick(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDoubleClick(e);
            EnterEditMode();
            e.Handled = true;
        }

        private void UpdateOnIsEditModeChanged()
        {
            if (IsInEditMode)
            {
                EnterEditModeInternal();
            }
            else if (!_isCommitChanged)
            {
                CancelEditModeInternal();
            }
        }

        private void EnterEditModeInternal()
        {
            SetVisual();
            if (_textBox == null)
            {
                return;
            }
            _textBox.Focus();
            _textBox.SelectionLength = _textBox.Text.Length;

            if (BeginTextEditCommand != null)
            {
                if (BeginTextEditCommand.CanExecute(null))
                {
                    BeginTextEditCommand.Execute(null);
                }
            }
            else
            {
                _oldText = Content as string;
            }
        }

        private void CancelEditModeInternal()
        {
            SetVisual();
            if (CancelTextEditCommand != null)
            {
                if (CancelTextEditCommand.CanExecute(null))
                {
                    CancelTextEditCommand.Execute(null);
                }
            }
            else
            {
                Content = _oldText;
            }
        }

        private void CommitEditModeInternal()
        {
            SetVisual();
            if (TextEdited != null)
            {
                TextEdited(this);
            }
            if (CompleteTextEditCommand != null && CompleteTextEditCommand.CanExecute(null))
            {
                CompleteTextEditCommand.Execute(null);
            }
        }

        private void SetVisual()
        {
            if (_textBox != null)
            {
                _textBox.Visibility = IsInEditMode
                                          ? Visibility.Visible
                                          : Visibility.Collapsed;
            }

            if (_textBlock != null)
            {
                _textBlock.Visibility = IsInEditMode
                                            ? Visibility.Collapsed
                                            : Visibility.Visible;
            }
        }

        private void TextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if (IsInEditMode)
            {
                if (CommitOnLostFocus)
                {
                    CommitEditMode();
                }
                else
                {
                    CancelEditMode();
                }
            }
        }
    }
}