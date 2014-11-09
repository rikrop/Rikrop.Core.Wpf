using System;
using System.Diagnostics.Contracts;
using System.Windows;
using System.Windows.Input;

namespace Rikrop.Core.Wpf.Behaviors
{
    public class UiElementHandleEventsBehaviour : Freezable
    {
        public static readonly DependencyProperty BehaviourProperty =
            DependencyProperty.RegisterAttached("Behaviour", typeof (UiElementHandleEventsBehaviour), typeof (UiElementHandleEventsBehaviour), new PropertyMetadata(null, BehaviourPropertyChangedCallback));

        public static readonly DependencyProperty HandleMouseDownProperty =
            DependencyProperty.Register("HandleMouseDown", typeof (bool), typeof (UiElementHandleEventsBehaviour), new PropertyMetadata(false, HandleMouseDownPropertyChangedCallback));

        private UIElement _ue;

        public bool HandleMouseDown
        {
            get { return (bool) GetValue(HandleMouseDownProperty); }
            set { SetValue(HandleMouseDownProperty, value); }
        }

        public static void SetBehaviour(UIElement element, UiElementHandleEventsBehaviour value)
        {
            element.SetValue(BehaviourProperty, value);
        }

        public static UiElementHandleEventsBehaviour GetBehaviour(UIElement element)
        {
            return (UiElementHandleEventsBehaviour) element.GetValue(BehaviourProperty);
        }

        private static void HandleMouseDownPropertyChangedCallback(DependencyObject dobj, DependencyPropertyChangedEventArgs dargs)
        {
            Contract.Assume(dobj is UiElementHandleEventsBehaviour);
            var b = (UiElementHandleEventsBehaviour) dobj;

            if ((bool) dargs.NewValue)
            {
                b.SubscribeHandleMouseDown();
            }
            else
            {
                b.UnSubscribeHandleMouseDown();
            }
        }

        private static void BehaviourPropertyChangedCallback(DependencyObject dobj, DependencyPropertyChangedEventArgs dargs)
        {
            Contract.Assume(dobj is UIElement);
            var ue = (UIElement) dobj;

            var ob = dargs.NewValue as UiElementHandleEventsBehaviour;
            if (ob != null)
            {
                ob.Detach();
            }

            var b = dargs.NewValue as UiElementHandleEventsBehaviour;
            if (b != null)
            {
                b.Attach(ue);
            }
        }

        protected override Freezable CreateInstanceCore()
        {
            return new UiElementHandleEventsBehaviour();
        }

        private void UnSubscribeHandleMouseDown()
        {
            if (_ue == null)
            {
                return;
            }
            // Unsupported in 4.0 WeakEventManager<UIElement, MouseButtonEventArgs>.RemoveHandler(_ue, "MouseDown", UeOnMouseDown);
        }

        private void SubscribeHandleMouseDown()
        {
            if (_ue == null)
            {
                return;
            }

            // Unsupported in 4.0 WeakEventManager<UIElement, MouseButtonEventArgs>.AddHandler(_ue, "MouseDown", UeOnMouseDown);
        }

        private void UeOnMouseDown(object sender, MouseButtonEventArgs args)
        {
            if (HandleMouseDown)
            {
                args.Handled = true;
            }
        }

        private void Attach(UIElement ue)
        {
            Contract.Requires<ArgumentNullException>(ue != null);
            _ue = ue;
            SubscribeHandleMouseDown();
        }

        private void Detach()
        {
            UnSubscribeHandleMouseDown();
            _ue = null;
        }
    }
}