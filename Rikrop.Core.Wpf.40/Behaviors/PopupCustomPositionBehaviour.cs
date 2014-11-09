using System;
using System.Diagnostics.Contracts;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Rikrop.Core.Wpf.Behaviors
{
    public class PopupCustomPositionBehaviour : Freezable
    {
        public static readonly DependencyProperty AttachProperty =
            DependencyProperty.RegisterAttached(
                "Attach",
                typeof (PopupCustomPositionBehaviour),
                typeof (PopupCustomPositionBehaviour),
                new PropertyMetadata(null, AttachPropertyChangedCallback));

        public static readonly DependencyProperty PositionerProperty =
            DependencyProperty.Register("Positioner", typeof (IPopupCustomPositioner), typeof (PopupCustomPositionBehaviour), new PropertyMetadata(new TopLeftPositioner()));

        private Popup _popup;
        private PlacementMode _oldPlacement;
        private CustomPopupPlacementCallback _oldCallback;

        public IPopupCustomPositioner Positioner
        {
            get { return (IPopupCustomPositioner) GetValue(PositionerProperty); }
            set { SetValue(PositionerProperty, value); }
        }

        public static void SetAttach(UIElement element, PopupCustomPositionBehaviour value)
        {
            element.SetValue(AttachProperty, value);
        }

        public static PopupCustomPositionBehaviour GetAttach(UIElement element)
        {
            return (PopupCustomPositionBehaviour) element.GetValue(AttachProperty);
        }

        private static void AttachPropertyChangedCallback(DependencyObject dobj, DependencyPropertyChangedEventArgs dargs)
        {
            Contract.Assume(dobj is Popup);

            var ob = dargs.OldValue as PopupCustomPositionBehaviour;
            if (ob != null)
            {
                ob.Detach();
            }

            var nb = dargs.NewValue as PopupCustomPositionBehaviour;
            if (nb != null)
            {
                nb.AttachTo(dobj as Popup);
            }
        }

        protected override Freezable CreateInstanceCore()
        {
            return new PopupCustomPositionBehaviour();
        }

        private void Detach()
        {
            Contract.Assume(_popup != null);

            _popup.Placement = _oldPlacement;
            _popup.CustomPopupPlacementCallback = _oldCallback;

            _popup = null;
        }

        private void AttachTo(Popup popup)
        {
            Contract.Requires<ArgumentNullException>(popup != null);
            Contract.Assume(_popup == null);

            _popup = popup;
            _oldPlacement = _popup.Placement;
            _oldCallback = _popup.CustomPopupPlacementCallback;

            _popup.Placement = PlacementMode.Custom;
            _popup.CustomPopupPlacementCallback = CustomPopupPlacementCallback;
        }

        private CustomPopupPlacement[] CustomPopupPlacementCallback(Size popupSize, Size targetSize, Point offset)
        {
            Contract.Assume(Positioner != null);
            return Positioner.GetPosition(popupSize, targetSize, offset);
        }
    }

    public interface IPopupCustomPositioner
    {
        CustomPopupPlacement[] GetPosition(Size popupSize, Size targetSize, Point offset);
    }

    public class TopLeftPositioner : IPopupCustomPositioner
    {
        public CustomPopupPlacement[] GetPosition(Size popupSize, Size targetSize, Point offset)
        {
            var placement = new CustomPopupPlacement(new Point(0, 0), PopupPrimaryAxis.Vertical);
            return new[] {placement};
        }
    }

    public class CoverPositioner : IPopupCustomPositioner
    {
        public CustomPopupPlacement[] GetPosition(Size popupSize, Size targetSize, Point offset)
        {
            var placement = new CustomPopupPlacement(new Point(targetSize.Width/2 - popupSize.Width/2, 0), PopupPrimaryAxis.Vertical);
            return new[] {placement};
        }
    }

    public class RightToLeftPositioner : IPopupCustomPositioner
    {
        public CustomPopupPlacement[] GetPosition(Size popupSize, Size targetSize, Point offset)
        {
            var placement = new CustomPopupPlacement(new Point(targetSize.Width - popupSize.Width, targetSize.Height), PopupPrimaryAxis.Vertical);
            return new[] {placement};
        }
    }

    public class LeftToRightPositioner : IPopupCustomPositioner
    {
        public CustomPopupPlacement[] GetPosition(Size popupSize, Size targetSize, Point offset)
        {
            var placement = new CustomPopupPlacement(new Point(0, targetSize.Height), PopupPrimaryAxis.Vertical);
            return new[] {placement};
        }
    }
}