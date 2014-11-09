using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Rikrop.Core.Wpf.Controls.ApplyCancelButtons
{
    public class ApplyCancelButtons : Control
    {
        public static readonly DependencyProperty ApplyCommandProperty =
            DependencyProperty.Register("ApplyCommand", typeof (ICommand), typeof (ApplyCancelButtons),
                                        new PropertyMetadata(default(ICommand)));

        public static readonly DependencyProperty CancelCommandProperty =
            DependencyProperty.Register("CancelCommand", typeof (ICommand), typeof (ApplyCancelButtons),
                                        new PropertyMetadata(default(ICommand)));

        public ICommand ApplyCommand
        {
            get { return (ICommand) GetValue(ApplyCommandProperty); }
            set { SetValue(ApplyCommandProperty, value); }
        }

        public ICommand CancelCommand
        {
            get { return (ICommand) GetValue(CancelCommandProperty); }
            set { SetValue(CancelCommandProperty, value); }
        }

        static ApplyCancelButtons()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (ApplyCancelButtons),
                                                     new FrameworkPropertyMetadata(typeof (ApplyCancelButtons)));
        }
    }
}